using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLine2D : MonoBehaviour
{
    private GameController gameController;

    public LineRenderer lineRenderer;
    public Material lineMaterial;
    public Camera cam;
    public bool addCollider;

    EdgeCollider2D edgeCollider2D;
    List<Vector2> points;
    List<Vector2> colliderPoints;

    public Color lineColor;
    public float lineWidth;
    public float edgeRadius;

    public float lineLenLimit = 30f;
    private float lineLength = 0f;

    public bool slowMotion;

    private void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogWarning("DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer.");
            CreateDefaultLineRenderer();
        }

        if (edgeCollider2D == null && addCollider)
        {
            Debug.LogWarning("DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D.");
            CreateDefaultEdgeCollider2D();
        }

        if (cam == null)
        {
            cam = Camera.main;
        }

        gameController = GameController.Instance;

        points = new List<Vector2>();
        colliderPoints = new List<Vector2>();
    }

    private void Update()
    {
        // If mouse over UI Gameobject, don't draw anything.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if(gameController.gamePaused == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            slowMotion = true;
            Reset();
        }

        if (Input.GetMouseButton(0) && lineLength <= lineLenLimit)
            Draw();

        if (Input.GetMouseButtonUp(0))
            slowMotion = false;
        
        UpdateTimeScale();
    }

    private void Draw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lineRendererPosition = lineRenderer.transform.position;
        Vector2 colliderPos = mousePosition - lineRendererPosition;

        if (points.Count > 0)
        {
            mousePosition = GetProperEndPoint(points[points.Count - 1], mousePosition);
            colliderPos = mousePosition - lineRendererPosition;
        }

        if (!points.Contains(mousePosition))
        {
            if (points.Count > 0)
            {
                // Here, we measure line length
                lineLength += Vector2.Distance(points[points.Count - 1], mousePosition);
            }

            points.Add(mousePosition);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePosition);
            colliderPoints.Add(colliderPos);

            if (edgeCollider2D != null && addCollider && points.Count > 1)
            {
                edgeCollider2D.isTrigger = false;
                edgeCollider2D.points = colliderPoints.ToArray();
            }
        }
    }

    private void UpdateTimeScale()
    {
        float timeScale = Time.timeScale;
        if (slowMotion == false)
            timeScale += Time.deltaTime * 1f;
        else
            timeScale -= Time.deltaTime * 2f;

        Time.timeScale = Mathf.Clamp(timeScale, 0.4f, 1f);
    }

    public void Reset()
    {
        lineRenderer.positionCount = 0;
        edgeCollider2D.Reset();
        edgeCollider2D.edgeRadius = edgeRadius;
        colliderPoints.Clear();
        points.Clear();

        // Set line length to 0
        lineLength = 0f;
    }

    private void CreateDefaultLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
    }

    private void CreateDefaultEdgeCollider2D()
    {
        edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider2D.edgeRadius = edgeRadius;
        edgeCollider2D.isTrigger = true;
    }

    private Vector2 GetProperEndPoint(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.name != "DrawLine")
            {
                end = hit.point + hit.normal * edgeRadius;
                break;
            }
        }

        return end;
    }
}