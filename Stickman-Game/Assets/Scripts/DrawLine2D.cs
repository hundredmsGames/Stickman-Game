using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine2D : MonoBehaviour
{
    [SerializeField]
    protected LineRenderer m_LineRenderer;
    [SerializeField]
    protected bool m_AddCollider = false;
    [SerializeField]
    protected EdgeCollider2D m_EdgeCollider2D;
    [SerializeField]
    protected Camera m_Camera;
    protected List<Vector2> m_Points;
    List<Vector2> colliderPoints;

    public Color lineColor;
    public Color ghostLineColor;
    public float lineWidth;
    public float edgeRadius;

    public float lineLenLimit = 10f;
    private float lineLength = 0f;

    public virtual LineRenderer lineRenderer
    {
        get
        {
            return m_LineRenderer;
        }
    }

    public virtual bool addCollider
    {
        get
        {
            return m_AddCollider;
        }
    }

    public virtual EdgeCollider2D edgeCollider2D
    {
        get
        {
            return m_EdgeCollider2D;
        }
    }

    public virtual List<Vector2> points
    {
        get
        {
            return m_Points;
        }
    }

    protected virtual void Awake()
    {
        if (m_LineRenderer == null)
        {
            Debug.LogWarning("DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer.");
            CreateDefaultLineRenderer();
        }
        if (m_EdgeCollider2D == null && m_AddCollider)
        {
            Debug.LogWarning("DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D.");
            CreateDefaultEdgeCollider2D();
        }
        if (m_Camera == null)
        {
            m_Camera = Camera.main;
        }
        m_Points = new List<Vector2>();
        colliderPoints = new List<Vector2>();

        m_EdgeCollider2D.enabled = false;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Reset();
        }

        if(Input.GetMouseButtonUp(0))
        {
            m_LineRenderer.startColor = lineColor;
            m_LineRenderer.endColor = lineColor;

            m_EdgeCollider2D.enabled = true;
        }

        if (Input.GetMouseButton(0) && lineLength <= lineLenLimit)
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lineRendererPosition = m_LineRenderer.transform.position;
            Vector2 colliderPos = mousePosition - lineRendererPosition;

            if (m_Points.Count > 0)
            {
                if (IsThereObstacle(m_Points[m_Points.Count - 1], mousePosition) == true)
                    return;
            }

            if (!m_Points.Contains(mousePosition))
            {
                if (m_Points.Count > 0)
                {
                    // Here, we measure line length
                    lineLength += Vector2.Distance(m_Points[m_Points.Count - 1], mousePosition);
                }

                m_Points.Add(mousePosition);
                m_LineRenderer.positionCount = m_Points.Count;
                m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                colliderPoints.Add(colliderPos);

                if (m_EdgeCollider2D != null && m_AddCollider && m_Points.Count > 1)
                {
                    m_EdgeCollider2D.points = colliderPoints.ToArray();
                } 
            }
        }
    }

    private bool IsThereObstacle(Vector2 a, Vector2 b)
    {
        //Get the mouse position on the screen and send a raycast into the game world from that position.
        RaycastHit2D[] hits = Physics2D.LinecastAll(a, b);
        Debug.DrawLine(a, b, Color.red);

        foreach(RaycastHit2D hit in hits)
        {
            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider.name != "DrawLine")
            {
                return true;
            }
        }    

        return false;
    }

    protected virtual void Reset()
    {
        if (m_LineRenderer != null)
        {
            m_LineRenderer.positionCount = 0;
        }
        if (m_Points != null)
        {
            m_Points.Clear();
        }
        if (m_EdgeCollider2D != null && m_AddCollider)
        {
            m_EdgeCollider2D.Reset();
            m_EdgeCollider2D.edgeRadius = 0.1f;
        }

        if (colliderPoints != null)
            colliderPoints.Clear();

        // Set line length to 0
        lineLength = 0f;

        m_LineRenderer.startColor = ghostLineColor;
        m_LineRenderer.endColor = ghostLineColor;

        m_EdgeCollider2D.enabled = false;
    }

    protected virtual void CreateDefaultLineRenderer()
    {
        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.positionCount = 0;
        m_LineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        m_LineRenderer.startColor = ghostLineColor;
        m_LineRenderer.endColor = ghostLineColor;
        m_LineRenderer.startWidth = lineWidth;
        m_LineRenderer.endWidth = lineWidth;
        m_LineRenderer.useWorldSpace = true;
    }

    protected virtual void CreateDefaultEdgeCollider2D()
    {
        m_EdgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        m_EdgeCollider2D.edgeRadius = edgeRadius;
    }

}