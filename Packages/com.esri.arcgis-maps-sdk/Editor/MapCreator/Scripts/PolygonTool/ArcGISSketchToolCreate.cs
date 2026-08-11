// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Attn: Contracts and Legal Department
// Environmental Systems Research Institute, Inc.
// 380 New York Street
// Redlands, California 92373
// USA
//
// email: legal@esri.com
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor
{
	public class ArcGISSketchToolCreate
	{
		private const int LeftMouseButton = 0;
		private const int RightMouseButton = 1;
		private const float SnappingThreshold = 1.25f;
		private const float VertexSizeAngleFactor = 0.5f;
		private const float VertexSnappingOutline = 0.7f;

		private Vector3 mouseHitPoint;
		private Action<List<Vector3>> onCreatePolygonFinished;
		private List<Vector3> polygonVertices = new();

		public ArcGISSketchToolCreate(Action<List<Vector3>> handleOnCreatePolygonFinished)
		{
			onCreatePolygonFinished = handleOnCreatePolygonFinished;
		}

		public void Reset()
		{
			polygonVertices.Clear();
		}

		public void Draw(SceneView sceneView)
		{
			if (polygonVertices.Count == 0)
			{
				ArcGISPolygonHelpers.DrawSolidDiscWithOutline(mouseHitPoint, ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, mouseHitPoint, VertexSizeAngleFactor), Color.white, Color.black);
			}

			for (var index = 0; index < polygonVertices.Count; ++index)
			{
				var point = polygonVertices[index];
				var next = polygonVertices[(index + 1) % polygonVertices.Count];

				if (index == polygonVertices.Count - 1)
				{
					next = mouseHitPoint;
					Handles.color = Color.yellow;

					// Snap to the first vertex if we are close enough
					if (polygonVertices.Count > 2 && Vector3.Distance(polygonVertices[0], mouseHitPoint) < ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, next, SnappingThreshold))
					{
						next = polygonVertices[0];
						Handles.DrawSolidDisc(next, Vector3.up, ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, next, VertexSnappingOutline));
					}
					else
					{
						next = mouseHitPoint;
						ArcGISPolygonHelpers.DrawSolidDiscWithOutline(next, ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, next, VertexSizeAngleFactor), Color.white, Color.black);
					}
				}

				Handles.color = Color.white;
				Handles.DrawDottedLine(point, next, 5.0f);
			}

			foreach (var vertex in polygonVertices)
			{
				Handles.color = ArcGISSketchTool.OrangeColor;
				Handles.DrawSolidDisc(vertex, Vector3.up, ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, vertex, VertexSizeAngleFactor));
			}
		}

		public void Update(SceneView sceneView)
		{
			var currentEvent = Event.current;

			TraceRay(currentEvent.mousePosition, out mouseHitPoint);

			if (currentEvent.button == LeftMouseButton && currentEvent.type == EventType.MouseUp)
			{
				if (TraceRay(currentEvent.mousePosition, out var intersection))
				{
					if (polygonVertices.Count > 2 && Vector3.Distance(polygonVertices[0], intersection) < ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, intersection, SnappingThreshold))
					{
						onCreatePolygonFinished?.Invoke(polygonVertices);
					}
					else
					{
						polygonVertices.Add(intersection);
					}

					currentEvent.Use();
				}
			}
			else if (currentEvent.button == RightMouseButton && currentEvent.type == EventType.MouseUp && currentEvent.clickCount == 2)
			{
				if (polygonVertices.Count > 2)
				{
					onCreatePolygonFinished?.Invoke(polygonVertices);

					currentEvent.Use();
				}
			}
		}

		internal bool TraceRay(in Vector2 mousePosition, out Vector3 intersection)
		{
			intersection = Vector3.zero;

			if (polygonVertices.Count > 0)
			{
				var plane = new Plane(Vector3.up, polygonVertices[0]);
				var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
				if (plane.Raycast(ray, out var distance))
				{
					intersection = ray.GetPoint(distance);
					return true;
				}
			}
			else
			{
				if (UnityEngine.Physics.Raycast(HandleUtility.GUIPointToWorldRay(mousePosition), out var hitInfo))
				{
					intersection = hitInfo.point;
					return true;
				}
			}

			return false;
		}
	}
}
