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
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor
{
	public static class ArcGISPolygonHelpers
	{
		public static void DrawSolidDiscWithOutline(Vector3 position, float size, Color fillColor, Color outlineColor, float outlineSize = 0.0f)
		{
			Handles.color = outlineColor;
			Handles.DrawSolidDisc(position, Vector3.up, outlineSize);
			Handles.color = fillColor;
			Handles.DrawSolidDisc(position, Vector3.up, size);
		}

		public static float GetSizeFromOpeningAngle(SceneView sceneView, Vector3 position, float angle)
		{
			var normalizedFovFactor = sceneView.camera.fieldOfView / 60.0f;
			var targetOpeningAngle = angle * normalizedFovFactor;

			var eyePosition = sceneView.camera.transform.position;
			var pointVector = position - eyePosition;
			var rotationPosition = eyePosition + Quaternion.AngleAxis(targetOpeningAngle, sceneView.camera.transform.up) * pointVector;

			var A = position - eyePosition;
			var B = rotationPosition - eyePosition;

			var actualOpeningAngle = Mathf.Abs(Vector3.Angle(A, B));

			return Vector3.Distance(position, rotationPosition) * (targetOpeningAngle / actualOpeningAngle);
		}

		public static float GetNormalizedOpeningAngle(SceneView sceneView, Vector3 position1, Vector3 position2)
		{
			var normalizedFovFactor = sceneView.camera.fieldOfView / 60.0f;
			var eyePosition = sceneView.camera.transform.position;
			var A = Vector3.Normalize(position1 - eyePosition);
			var B = Vector3.Normalize(position2 - eyePosition);

			return Mathf.Abs(Vector3.Angle(A, B)) / normalizedFovFactor;
		}
	}
}
