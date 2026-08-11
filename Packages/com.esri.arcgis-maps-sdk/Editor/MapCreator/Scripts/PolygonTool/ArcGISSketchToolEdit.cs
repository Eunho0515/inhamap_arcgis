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
using UnityEngine.Assertions;

namespace Esri.ArcGISMapsSDK.Editor
{
	public class ArcGISSketchToolEdit
	{
		private Vector3 mouseHitPoint;
		private List<SketchToolPolygon> polygons = new();
		private int selectedPolygon = Unselected;
		private int selectedVertex = Unselected;

		private event Action<int, List<Vector3>> onPolygonChanged;
		private event Action<int> onSelectionChanged;

		private const int LeftMouseButton = 0;
		private const int Unselected = -1;

		public ArcGISSketchToolEdit(Action<int, List<Vector3>> onPolygonChanged, Action<int> onSelectionChanged)
		{
			this.onPolygonChanged = onPolygonChanged;
			this.onSelectionChanged = onSelectionChanged;
		}

		public void ClearSelection()
		{
			selectedPolygon = Unselected;
		}

		public void Reset()
		{
			if (selectedPolygon != Unselected)
			{
				polygons[selectedPolygon].Selected = false;
			}

			selectedPolygon = Unselected;
			selectedVertex = Unselected;

			onSelectionChanged?.Invoke(selectedPolygon);
		}

		public void Draw(SceneView sceneView)
		{
			for (var index = 0; index < polygons.Count; index++)
			{
				polygons[index].Draw(sceneView, mouseHitPoint);
			}

			if (selectedPolygon == Unselected)
			{
				return;
			}

			var polygon = polygons[selectedPolygon];

			var transformPosition = polygon.Transform.GetPosition();
			var transformRotation = polygon.Transform.rotation;

			if (selectedVertex == Unselected)
			{
				Handles.TransformHandle(ref transformPosition, ref transformRotation);
			}

			// Discard the rotation on the right or forward axes
			var eulerAngles = transformRotation.eulerAngles;

			eulerAngles.x = 0;
			eulerAngles.z = 0;

			transformRotation.eulerAngles = eulerAngles;

			var newTransform = Matrix4x4.TRS(transformPosition, transformRotation, Vector3.one);

			if (polygon.Transform != newTransform)
			{
				polygon.Transform = newTransform;

				HandleOnPolygonChanged();
			}
		}

		public void DeletePolygon(int polygonIndex)
		{
			if (polygonIndex < 0 || polygonIndex >= polygons.Count)
			{
				return;
			}

			if (polygonIndex == selectedPolygon)
			{
				selectedPolygon = Unselected;

				onSelectionChanged?.Invoke(selectedPolygon);
			}

			polygons.RemoveAt(polygonIndex);
		}

		public void SetPolygons(List<List<Vector3>> polygonsVertices)
		{
			polygons.Clear();

			selectedPolygon = Unselected;

			if (polygonsVertices == null)
			{
				return;
			}

			foreach (var polygon in polygonsVertices)
			{
				polygons.Add(new SketchToolPolygon(polygon));
			}
		}

		public void Update(SceneView sceneView)
		{
			var currentEvent = Event.current;

			if (selectedPolygon != Unselected)
			{
				TraceRay(currentEvent.mousePosition, out mouseHitPoint);
			}

			if (currentEvent.button == LeftMouseButton && currentEvent.type == EventType.MouseDown)
			{
				var hitPolygon = FindPolygon(HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition));
				if (hitPolygon != selectedPolygon)
				{
					Assert.IsTrue(hitPolygon < polygons.Count);

					if (selectedPolygon == Unselected)
					{
						polygons[hitPolygon].Selected = true;
					}
					else if (!polygons[selectedPolygon].IsMouseOverPolygon(mouseHitPoint))
					{
						if (hitPolygon == Unselected)
						{
							polygons[selectedPolygon].Selected = false;
						}
						else
						{
							polygons[selectedPolygon].Selected = false;
							polygons[hitPolygon].Selected = true;
						}
					}

					selectedPolygon = hitPolygon;
					onSelectionChanged?.Invoke(selectedPolygon);
				}
				// else if here because we want the user to be able to translate the polygon only after it has been selected.
				else if (hitPolygon != Unselected)
				{
					var polygon = polygons[selectedPolygon];

					if (polygon.FindNewVertex(sceneView, mouseHitPoint, out var hitNewVertex))
					{
						polygon.InsertVertex(hitNewVertex + 1, polygon.GetNewVertex(hitNewVertex));
					}
					else if (polygon.IsMouseOverPolygonVertex(sceneView, mouseHitPoint) != Unselected)
					{
						selectedVertex = polygon.IsMouseOverPolygonVertex(sceneView, mouseHitPoint);
					}
				}
			}
			else if (currentEvent.button == LeftMouseButton && currentEvent.type == EventType.MouseDrag)
			{
				if (selectedVertex != Unselected)
				{
					var vertex = polygons[selectedPolygon].GetVertex(selectedVertex);

					if (vertex != mouseHitPoint)
					{
						polygons[selectedPolygon].UpdateVertex(selectedVertex, mouseHitPoint);

						HandleOnPolygonChanged();
					}
				}
			}
			else if (currentEvent.button == LeftMouseButton && currentEvent.type == EventType.MouseUp)
			{
				selectedVertex = Unselected;
			}
		}

		private void HandleOnPolygonChanged()
		{
			Assert.IsTrue(selectedPolygon != Unselected);

			onPolygonChanged?.Invoke(selectedPolygon, polygons[selectedPolygon].GetVertices());
		}

		private int FindPolygon(Ray ray)
		{
			var polygonIndex = 0;
			foreach (var polygon in polygons)
			{
				if (polygonIndex == selectedPolygon)
				{
					if (polygons[selectedPolygon].IsMouseInBoundingRectangle(mouseHitPoint))
					{
						return selectedPolygon;
					}
				}

				if (polygon.IsHitByRay(ray))
				{
					return polygonIndex;
				}

				polygonIndex++;
			}

			return Unselected;
		}

		internal bool TraceRay(in Vector2 mousePosition, out Vector3 intersection)
		{
			intersection = Vector3.zero;
			var plane = new Plane(Vector3.up, polygons[selectedPolygon].GetVertex(0));
			var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
			if (plane.Raycast(ray, out var distance))
			{
				intersection = ray.GetPoint(distance);
				return true;
			}

			return false;
		}
	}
}
