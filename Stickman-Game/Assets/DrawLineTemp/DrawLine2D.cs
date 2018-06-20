﻿using System.Collections;
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

	protected virtual void Awake ()
	{
		if ( m_LineRenderer == null )
		{
			Debug.LogWarning ( "DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer." );
			CreateDefaultLineRenderer ();
		}
		if ( m_EdgeCollider2D == null && m_AddCollider )
		{
			Debug.LogWarning ( "DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D." );
			CreateDefaultEdgeCollider2D ();
		}
		if ( m_Camera == null ) {
			m_Camera = Camera.main;
		}
		m_Points = new List<Vector2> ();
		colliderPoints = new List<Vector2>();

        m_EdgeCollider2D.enabled = false;
	}

	protected virtual void Update ()
	{
		if ( Input.GetMouseButtonDown ( 0 ) )
		{
			Reset ();
		}

		if ( Input.GetMouseButton ( 0 ) )
		{
			Vector2 mousePosition = m_Camera.ScreenToWorldPoint ( Input.mousePosition );
			Vector2 lineRendererPosition = m_LineRenderer.transform.position;
			Vector2 colliderPos = mousePosition - lineRendererPosition;

			if ( !m_Points.Contains ( mousePosition ) )
			{
				m_Points.Add ( mousePosition );
				m_LineRenderer.positionCount = m_Points.Count;
				m_LineRenderer.SetPosition ( m_LineRenderer.positionCount - 1, mousePosition);

				colliderPoints.Add(colliderPos);
				if ( m_EdgeCollider2D != null && m_AddCollider && m_Points.Count > 1 )
				{
					m_EdgeCollider2D.points = colliderPoints.ToArray ();
				}
			}
		}

        if (m_Points.Count > 2)
            m_EdgeCollider2D.enabled = true;
	}

	protected virtual void Reset ()
	{
		if ( m_LineRenderer != null )
		{
			m_LineRenderer.positionCount = 0;
		}
		if ( m_Points != null )
		{
			m_Points.Clear ();
		}
		if ( m_EdgeCollider2D != null && m_AddCollider )
		{
			m_EdgeCollider2D.Reset ();
			m_EdgeCollider2D.edgeRadius = 0.1f;
		}

		if(colliderPoints != null)
			colliderPoints.Clear();
	}

	protected virtual void CreateDefaultLineRenderer ()
	{
		m_LineRenderer = gameObject.AddComponent<LineRenderer> ();
		m_LineRenderer.positionCount = 0;
		m_LineRenderer.material = new Material ( Shader.Find ( "Particles/Additive" ) );
		m_LineRenderer.startColor = Color.white;
		m_LineRenderer.endColor = Color.white;
		m_LineRenderer.startWidth = 0.2f;
		m_LineRenderer.endWidth = 0.2f;
		m_LineRenderer.useWorldSpace = true;
	}

	protected virtual void CreateDefaultEdgeCollider2D ()
	{
		m_EdgeCollider2D = gameObject.AddComponent<EdgeCollider2D> ();
		m_EdgeCollider2D.edgeRadius = 0.1f;
	}

}