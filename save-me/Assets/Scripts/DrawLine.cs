using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLine : MonoBehaviour
{
    #region Variables

    private Camera mainCamera;
    public GameObject squarePrefab;
    public GameObject circlePrefab;

    private List<GameObject> lineGoList;
    private List<GameObject> circleGoList;
    private List<Vector2> points;
    private Vector2 mousePosition;
    private Vector2 lastMousePos;

    private float lineWidth = 0.4f;
    private bool resetLine;

    // Cast all but exclude "Line" and "AreaEffector".
    private int layerMask = ~((1 << 8) | (1 << 9));

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        points = new List<Vector2>();
        lineGoList = new List<GameObject>();
        circleGoList = new List<GameObject>();
    }

    private void Update()
    {
        //// If mouse over UI Gameobject, don't draw anything.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            resetLine = true;
        }

        if (Input.GetMouseButton(0))
        {
            Draw();
        }

        lastMousePos = mousePosition;
    }

    private void Draw()
    {
        if(resetLine == true)
        {
            Collider2D[] cols = Physics2D.OverlapPointAll(mousePosition, layerMask);

            // If we started draw and also we are in a obstacle. In this situation,
            // we should not draw anything, just return.
            if (cols.Length > 0)
                return;

            // If resetLine is true, line would be drawn inside an obstacle,
            // so we need to find an appropriate start point.
            FindAppropriateStartPoint(lastMousePos, mousePosition);
        }
        // If resetLine is false, that means we are drawing continuously.
        // In this situation, we just need to look for an appropriate end point.
        else
        {  
            mousePosition = GetAppropriateEndPoint(points[points.Count - 1], mousePosition);
        }

        if (!points.Contains(mousePosition))
        {
            points.Add(mousePosition);

            // Draw line.
            if(points.Count > 1 && resetLine == false)
            {
                Vector2 p1 = points[points.Count - 2];
                Vector2 p2 = points[points.Count - 1];
                CreateLine(p1, p2);
            }

            // Put a circle end of the line.
            CreateCircle(points[points.Count - 1]);

            resetLine = false;
        }
    }

    public void ResetLine()
    {
        foreach (GameObject go in lineGoList)
            Destroy(go);

        foreach (GameObject go in circleGoList)
            Destroy(go);

        points.Clear();
    }

    private void CreateLine(Vector2 p1, Vector2 p2)
    {
        float distance = Vector2.Distance(p1, p2);
        float angle = GetAngle(p1, p2);

        GameObject line = Instantiate(squarePrefab, p1, Quaternion.identity);
        line.name = "line_" + (points.Count + 1);
        line.tag = "DrawLine";
        line.transform.SetParent(this.transform);
        line.transform.Rotate(0, 0, angle);
        line.transform.localScale = new Vector3(distance, lineWidth, 0);
        line.GetComponent<SpriteRenderer>().sortingLayerName = "Line";
        lineGoList.Add(line);
    }

    private void CreateCircle(Vector2 p)
    {
        GameObject circle = Instantiate(circlePrefab, p, Quaternion.identity);
        circle.name = "circle_" + (circleGoList.Count + 1);
        circle.tag = "DrawLine";
        circle.transform.SetParent(this.transform);
        circle.transform.localScale = new Vector3(lineWidth, lineWidth, 0);
        circle.GetComponent<SpriteRenderer>().sortingLayerName = "Line";
        circleGoList.Add(circle);
    }

    private void FindAppropriateStartPoint(Vector2 start, Vector2 end)
    {        
        // If start point inside an obstacle find appropriate
        // point to start line.
        Collider2D[] cols = Physics2D.OverlapPointAll(start, layerMask);
        foreach (Collider2D col in cols)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(end, start, layerMask);
            float lineRadius = lineWidth / 2;

            foreach (RaycastHit2D hit in hits)
            {
                // When we raycasting backward, we need to ensure that the object we hit should be
                // same with the object that we collided at start point.
                if (hit.collider == col)
                {
                    // Add new start point in here. I don't know how to do it else.
                    Vector2 newStart = hit.point + hit.normal * lineRadius;
                    points.Add(newStart);
                    CreateCircle(newStart);
                    resetLine = false;
                    mousePosition = GetAppropriateEndPoint(points[points.Count - 1], mousePosition);

                    return;
                }
            }  
        }
    }

    private Vector2 GetAppropriateEndPoint(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, layerMask);
        float lineRadius = lineWidth / 2;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag != "DrawLine")
            {
                end = hit.point + hit.normal * lineRadius;
                break;
            }
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(end, lineRadius, layerMask);
        Collider2D hitCollider = null;
        foreach (Collider2D col in cols)
        {
            if (col.tag != "DrawLine")
            {
                hitCollider = col;
                break;
            }
        }

        if (hitCollider != null)
        {
            RaycastHit2D closest = new RaycastHit2D();
            closest.distance = float.MaxValue;

            RaycastHit2D hit = new RaycastHit2D();
            hits = Physics2D.RaycastAll(end, Vector2.up, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.right, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.down, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.left, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.up + Vector2.right, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.down + Vector2.right, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.up + Vector2.left, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            hits = Physics2D.RaycastAll(end, Vector2.down + Vector2.left, lineRadius, layerMask);
            if (IsThereAnyHit(hits, ref hit) == true && hit.distance < closest.distance)
                closest = hit;

            // If our rays found the collision.
            if (closest.distance != float.MaxValue)
            {
                end = closest.point + closest.normal * lineRadius;
            }
        }

        return end;
    }

    private bool IsThereAnyHit(RaycastHit2D[] hits, ref RaycastHit2D hitRef)
    {
        if(hits.Length > 0)
        {
            // We just grab the first one.
            // We hope that there is only one :D
            hitRef = hits[0];
            return true;
        }

        return false;
    }

    private float GetAngle(Vector2 origin, Vector2 other)
    {
        return Mathf.Atan2(other.y - origin.y, other.x - origin.x) * Mathf.Rad2Deg;
    }
}