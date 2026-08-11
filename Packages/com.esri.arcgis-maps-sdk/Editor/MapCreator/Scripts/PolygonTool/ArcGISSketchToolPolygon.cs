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
using Esri.ArcGISMapsSDK.SDK.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Esri.ArcGISMapsSDK.Editor
{
	internal struct Triangle
	{
		public int A { get; private set; }
		public int B { get; private set; }
		public int C { get; private set; }

		public Triangle(int a, int b, int c)
		{
			A = a;
			B = b;
			C = c;
		}
	}

	public class SketchToolPolygon
	{
		private Vector3[] boundingRectangle;
		private List<Vector3> newVertices = new();
		private Matrix4x4 transform = Matrix4x4.identity;
		private List<Triangle> triangles;
		private List<Vector3> vertices;

		private const float VertexSizeAngleFactor = 0.5f;
		private const float VertexOutlineSizeAngleFactor = 0.65f;

		private Vector3 center;
		public Vector3 Center
		{
			get => center;
		}

		private bool selected;
		public bool Selected
		{
			get => selected;
			set => selected = value;
		}

		public Matrix4x4 Transform
		{
			get => transform;
			set
			{
				transform = value;
			}
		}

		public SketchToolPolygon(List<Vector3> polygonVertices)
		{
			vertices = polygonVertices.Clone();

			UpdateTransform();
			Triangulate();
			CalculateBoundingRectangle();
			CalculateNewVerticesHotspots();
		}

		public Vector3 GetNewVertex(int index)
		{
			return transform.MultiplyPoint3x4(newVertices[index]);
		}

		public Vector3 GetVertex(int index)
		{
			return transform.MultiplyPoint3x4(vertices[index]);
		}

		public List<Vector3> GetVertices()
		{
			var outVertices = new List<Vector3>();

			foreach (var vertex in vertices)
			{
				outVertices.Add(transform.MultiplyPoint3x4(vertex));
			}

			return outVertices;
		}

		public void InsertVertex(int index, Vector3 vertex)
		{
			vertices.Insert(index, Transform.inverse.MultiplyPoint3x4(vertex));

			UpdateTransform();
			Triangulate();
			CalculateBoundingRectangle();
			CalculateNewVerticesHotspots();
		}

		public void Draw(SceneView sceneView, Vector3 mouseHitPoint)
		{
			if (selected)
			{
				// Draw rectangle.
				Assert.IsTrue(boundingRectangle.Length == 4);

				var globalBoundingRectangle = new Vector3[4];

				for (var i = 0; i < boundingRectangle.Length; ++i)
				{
					globalBoundingRectangle[i] = transform.MultiplyPoint3x4(boundingRectangle[i]);
				}

				Handles.color = Color.white;
				Handles.DrawSolidRectangleWithOutline(globalBoundingRectangle, new Color(0.5f, 0.5f, 0.5f, 0.05f), ArcGISSketchTool.OrangeColor);

				// Draw lines.
				for (var index = 0; index < vertices.Count; index++)
				{
					var start = GetVertex(index);
					var next = GetVertex((index + 1) % vertices.Count);

					Handles.color = new Color(ArcGISSketchTool.OrangeColor.r, ArcGISSketchTool.OrangeColor.g, ArcGISSketchTool.OrangeColor.b, 0.6f);
					Handles.DrawLine(start, next, 5.0f);

					Handles.color = Color.white;
					Handles.DrawDottedLine(start, next, 2.5f);
				}

				// Draw points.
				for (var index = 0; index < vertices.Count; index++)
				{
					var point = GetVertex(index);
					var size = ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, point, VertexSizeAngleFactor);
					var outlineSize = ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, point, VertexOutlineSizeAngleFactor);

					if (ArcGISPolygonHelpers.GetNormalizedOpeningAngle(sceneView, point, mouseHitPoint) < VertexSizeAngleFactor)
					{
						ArcGISPolygonHelpers.DrawSolidDiscWithOutline(point, size, ArcGISSketchTool.OrangeColor, Color.white, outlineSize);
					}
					else
					{
						ArcGISPolygonHelpers.DrawSolidDiscWithOutline(point, size, ArcGISSketchTool.OrangeColor, Color.black, outlineSize);
					}
				}

				// Draw new vertices
				foreach (var newVertex in newVertices)
				{
					var point = transform.MultiplyPoint3x4(newVertex);
					var size = ArcGISPolygonHelpers.GetSizeFromOpeningAngle(sceneView, point, VertexSizeAngleFactor);

					if (ArcGISPolygonHelpers.GetNormalizedOpeningAngle(sceneView, point, mouseHitPoint) < VertexSizeAngleFactor)
					{
						ArcGISPolygonHelpers.DrawSolidDiscWithOutline(point, size, ArcGISSketchTool.OrangeColor, Color.white);
					}
					else
					{
						Handles.color = Color.grey;
						Handles.DrawSolidDisc(point, Vector3.up, size);
					}
				}
			}
			else
			{
				// Draw lines
				for (var index = 0; index < vertices.Count; ++index)
				{
					var point = GetVertex(index);
					var next = GetVertex((index + 1) % vertices.Count);

					Handles.color = ArcGISSketchTool.OrangeColor;
					Handles.DrawLine(point, next, 5.0f);
				}
			}
		}

		public bool IsMouseInBoundingRectangle(Vector3 mouseHitPoint)
		{
			var localMouseHitPoint = transform.inverse.MultiplyPoint3x4(mouseHitPoint);

			if (IsPointInTriangle(localMouseHitPoint, boundingRectangle[0], boundingRectangle[1], boundingRectangle[2]) ||
				IsPointInTriangle(localMouseHitPoint, boundingRectangle[2], boundingRectangle[3], boundingRectangle[0]))
			{
				return true;
			}

			return false;
		}

		public bool IsHitByRay(Ray ray)
		{
			foreach (var triangle in triangles)
			{
				if (IsTriangleHit(triangle, ray))
				{
					return true;
				}
			}

			return false;
		}

		private void CalculateBoundingRectangle()
		{
			var xmax = float.MinValue;
			var xmin = float.MaxValue;
			var zmax = float.MinValue;
			var zmin = float.MaxValue;

			foreach (var point in vertices)
			{
				xmax = Mathf.Max(xmax, point.x);
				xmin = Mathf.Min(xmin, point.x);

				zmax = Mathf.Max(zmax, point.z);
				zmin = Mathf.Min(zmin, point.z);
			}

			var height = vertices[0].y;
			// TODO: use opening angle to calculate offset
			var offset = 20.0f;

			boundingRectangle = new Vector3[]
			{
				new(xmax + offset, height, zmax + offset),
				new(xmin - offset, height, zmax + offset),
				new(xmin - offset, height, zmin - offset),
				new(xmax + offset, height, zmin - offset)
			};
		}

		private void CalculateNewVerticesHotspots()
		{
			newVertices.Clear();

			for (var index = 0; index < vertices.Count; index++)
			{
				var start = vertices[index];
				var end = vertices[(index + 1) % vertices.Count];

				newVertices.Add((start + end) / 2);
			}
		}

		public bool FindNewVertex(SceneView sceneView, Vector3 mouseHitPoint, out int hitNewVertex)
		{
			for (var index = 0; index < newVertices.Count; index++)
			{
				if (ArcGISPolygonHelpers.GetNormalizedOpeningAngle(sceneView, GetNewVertex(index), mouseHitPoint) < VertexSizeAngleFactor)
				{
					hitNewVertex = index;

					return true;
				}
			}

			hitNewVertex = -1;

			return false;
		}

		public bool IsMouseOverPolygon(Vector3 mouseHitPoint)
		{
			var localMouseHitPoint = transform.inverse.MultiplyPoint3x4(mouseHitPoint);

			foreach (var triangle in triangles)
			{
				if (IsPointInTriangle(localMouseHitPoint, vertices[triangle.A], vertices[triangle.B], vertices[triangle.C]))
				{
					return true;
				}
			}

			return false;
		}

		public int IsMouseOverPolygonVertex(SceneView sceneView, Vector3 mouseHitPoint)
		{
			for (var index = 0; index < vertices.Count; index++)
			{
				if (ArcGISPolygonHelpers.GetNormalizedOpeningAngle(sceneView, GetVertex(index), mouseHitPoint) < VertexSizeAngleFactor)
				{
					return index;
				}
			}

			return -1;
		}

		public void UpdateVertex(int index, Vector3 vertex)
		{
			vertices[index] = Transform.inverse.MultiplyPoint3x4(vertex);

			UpdateTransform();
			Triangulate();
			CalculateBoundingRectangle();
			CalculateNewVerticesHotspots();
		}

		private bool IsTriangleHit(Triangle triangle, Ray ray)
		{
			// Using Möller-Trumbore
			var vertex1 = GetVertex(triangle.A);

			var edge1 = GetVertex(triangle.B) - vertex1;
			var edge2 = GetVertex(triangle.C) - vertex1;

			var h = Vector3.Cross(ray.direction, edge2);
			var a = Vector3.Dot(edge1, h);

			if (a > -Mathf.Epsilon && a < Mathf.Epsilon)
			{
				// This ray is parallel to this triangle.
				return false;
			}

			var f = 1.0f / a;
			var s = ray.origin - vertex1;
			var u = f * Vector3.Dot(s, h);

			if (u < 0.0 || u > 1.0)
			{
				return false;
			}

			var q = Vector3.Cross(s, edge1);
			var v = f * Vector3.Dot(ray.direction, q);

			if (v < 0.0 || u + v > 1.0)
			{
				return false;
			}

			// At this stage we can compute t to find out where the intersection point is on the line.
			var t = f * Vector3.Dot(edge2, q);

			if (t > Mathf.Epsilon)
			{
				// ray intersection
				// hitPoint = ray.origin + ray.direction * t;
				return true;
			}
			else
			{
				// This means that there is a line intersection but not a ray intersection.
				return false;
			}
		}

		private void UpdateTransform()
		{
			for (var i = 0; i < vertices.Count; i++)
			{
				vertices[i] = Transform.MultiplyPoint3x4(vertices[i]);
			}

			center = CalculateCenter(vertices);

			Transform = Matrix4x4.Translate(center);

			var inverseTransform = Transform.inverse;

			for (var i = 0; i < vertices.Count; i++)
			{
				vertices[i] = inverseTransform.MultiplyPoint3x4(vertices[i]);
			}
		}

		private void Triangulate()
		{
			triangles = Triangulate(vertices);
		}

		internal static List<Triangle> Triangulate(List<Vector3> vertices)
		{
			var triangles = new List<Triangle>();

			if (vertices.Count < 3)
			{
				return triangles;
			}
			else if (vertices.Count == 3)
			{
				triangles.Add(new Triangle(0, 1, 2));

				return triangles;
			}

			var earIndices = new List<int>();

			for (var i = 0; i < vertices.Count; i++)
			{
				earIndices.Add(i);
			}

			Vector3 getVertex(int index)
			{
				return vertices[getVertexIndex(index)];
			}

			int getVertexIndex(int index)
			{
				return earIndices[((index % earIndices.Count) + earIndices.Count) % earIndices.Count];
			}

			var polygonNormal = CalculateNormal(vertices);

			var earIndex = 0;
			var earTestCount = 0;

			while (earIndices.Count >= 3)
			{
				var isEar = true;

				if (earIndices.Count > 3 && earTestCount < earIndices.Count)
				{
					var previousVertex = getVertex(earIndex - 1);
					var earVertex = getVertex(earIndex);
					var nextVertex = getVertex(earIndex + 1);

					if (Vector3.Dot(CalculateTriangleNormal(previousVertex, earVertex, nextVertex), polygonNormal) > 0)
					{
						var index = (earIndex + 2) % earIndices.Count;

						do
						{
							var testVertex = getVertex(index);

							if (IsPointInTriangle(testVertex, previousVertex, earVertex, nextVertex))
							{
								isEar = false;

								break;
							}

							index = (index + 1) % earIndices.Count;
						} while (index != (earIndex - 1 + earIndices.Count) % earIndices.Count);
					}
					else
					{
						isEar = false;
					}
				}

				if (isEar)
				{
					var previousEarVertexIndex = getVertexIndex(earIndex - 1);
					var earVertexIndex = getVertexIndex(earIndex);
					var nextEarVertexIndex = getVertexIndex(earIndex + 1);

					triangles.Add(new Triangle(previousEarVertexIndex, earVertexIndex, nextEarVertexIndex));

					earIndices.RemoveAt(earIndex);

					earIndex = (earIndex - 1 + earIndices.Count) % earIndices.Count;
					earTestCount = 0;
				}
				else
				{
					earIndex = (earIndex + 1) % earIndices.Count;
					earTestCount++;
				}
			}

			return triangles;
		}

		// Since our triangle is counterclockwise, we want to check if the point is on the left side.
		internal static bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
		{
			var ab = b - a;
			var bc = c - b;
			var ca = a - c;

			return Vector3.Dot(Vector3.Cross(ab, p - a), Vector3.Cross(ab, c - a)) >= 0 &&
				   Vector3.Dot(Vector3.Cross(bc, p - b), Vector3.Cross(bc, a - b)) >= 0 &&
				   Vector3.Dot(Vector3.Cross(ca, p - c), Vector3.Cross(ca, b - c)) >= 0;
		}

		private static Vector3 CalculateCenter(List<Vector3> vertices)
		{
			var center = new Vector3();

			foreach (var vertex in vertices)
			{
				center += vertex;
			}

			return center / vertices.Count;
		}

		private static Vector3 CalculateNormal(List<Vector3> vertices)
		{
			var normal = new Vector3();

			for (var i = 0; i < vertices.Count; i++)
			{
				var vertex1 = vertices[i];
				var vertex2 = vertices[(i + 1) % vertices.Count];

				normal.x += (vertex1.y - vertex2.y) * (vertex1.z + vertex2.z);
				normal.y += (vertex1.z - vertex2.z) * (vertex1.x + vertex2.x);
				normal.z += (vertex1.x - vertex2.x) * (vertex1.y + vertex2.y);
			}

			return normal.normalized;
		}

		private static Vector3 CalculateTriangleNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
		{
			var side1 = (vertex2 - vertex1).normalized;
			var side2 = (vertex3 - vertex1).normalized;

			return Vector3.Cross(side1, side2).normalized;
		}
	}
}
