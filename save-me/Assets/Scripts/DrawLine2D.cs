using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine2D : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera cam;
    public bool addCollider;

    EdgeCollider2D edgeCollider2D;
    List<Vector2> points;
    List<Vector2> colliderPoints;

    public Color lineColor;
    public Color ghostLineColor;
    public float lineWidth;
    public float edgeRadius;

    public float lineLenLimit = 30f;
    private float lineLength = 0f;

    public bool slowMotion;

    private void Awake()
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

        points = new List<Vector2>();
        colliderPoints = new List<Vector2>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Reset();

        if (Input.GetMouseButton(0) && lineLength <= lineLenLimit)
            Draw();
        else
        {
            StopDrawing(true);
        }
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        if (slowMotion == false)
            Time.timeScale += Time.deltaTime * 1f;
        else
            Time.timeScale -= Time.deltaTime * 1.5f;

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0.5f, 1f);
    }

    private void Reset()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        if (points != null)
        {
            points.Clear();
        }
        if (edgeCollider2D != null && addCollider)
        {
            edgeCollider2D.Reset();
            edgeCollider2D.edgeRadius = 0.1f;
        }

        if (colliderPoints != null)
            colliderPoints.Clear();

        // Set line length to 0
        lineLength = 0f;

        lineRenderer.startColor = ghostLineColor;
        lineRenderer.endColor = ghostLineColor;

        if (edgeCollider2D != null)
            edgeCollider2D.isTrigger = true;

        slowMotion = true;
    }

    private void Draw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lineRendererPosition = lineRenderer.transform.position;
        Vector2 colliderPos = mousePosition - lineRendererPosition;

        if (points.Count > 0)
        {
            if (IsThereObstacle(points[points.Count - 1], mousePosition) == true)
                return;
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
                edgeCollider2D.points = colliderPoints.ToArray();
            }
        }
    }

    private void CreateDefaultLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = ghostLineColor;
        lineRenderer.endColor = ghostLineColor;
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

    private bool IsThereObstacle(Vector2 a, Vector2 b)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(a, b);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.name != "DrawLine" && hit.collider.tag != "Character")
            {
                return true;
            }
        }

        return false;
    }

    public void StopDrawing(bool enableCollider)
    {
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        edgeCollider2D.isTrigger = !enableCollider;

        slowMotion = false;
    }
}