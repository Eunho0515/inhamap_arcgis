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
using Esri.ArcGISMapsSDK.Components;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	[CustomEditor(typeof(ArcGISCameraComponent))]
	public class ArcGISCameraComponentEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var cameraComponent = target as ArcGISCameraComponent;
			var camera = cameraComponent.GetComponent<Camera>();
			var cameraComponentPresent = camera != null;
			var updateClippingPlanes = cameraComponent.UpdateClippingPlanes;
			var useCameraViewportProperties = cameraComponent.UseCameraViewportProperties;
			var verticalFov = cameraComponent.verticalFov;
			var horizontalFov = cameraComponent.horizontalFov;
			var viewportSizeX = cameraComponent.viewportSizeX;
			var viewportSizeY = cameraComponent.viewportSizeY;
			var qualityScalingFactor = cameraComponent.qualityScalingFactor;

			if (Draw(cameraComponentPresent, ref updateClippingPlanes, ref useCameraViewportProperties, ref verticalFov, ref horizontalFov, ref viewportSizeX, ref viewportSizeY, ref qualityScalingFactor))
			{
				cameraComponent.UpdateClippingPlanes = updateClippingPlanes;
				cameraComponent.UseCameraViewportProperties = useCameraViewportProperties;
				cameraComponent.verticalFov = verticalFov;
				cameraComponent.horizontalFov = horizontalFov;
				cameraComponent.viewportSizeX = viewportSizeX;
				cameraComponent.viewportSizeY = viewportSizeY;
				cameraComponent.qualityScalingFactor = qualityScalingFactor;

				EditorUtility.SetDirty(cameraComponent);
			}
		}

		private static bool Draw(bool cameraComponentPresent, ref bool updateClippingPlanes, ref bool useCameraViewportProperties, ref float verticalFov, ref float horizontalFov, ref uint viewportSizeX, ref uint viewportSizeY, ref float qualityScalingFactor)
		{
			var result = false;
			var drawViewportProps = true;

			if (cameraComponentPresent)
			{
				var oldUpdateClippingPlanes = updateClippingPlanes;

				updateClippingPlanes = EditorGUILayout.Toggle("Update Clipping Planes", updateClippingPlanes, GUILayout.ExpandWidth(true));

				if (oldUpdateClippingPlanes != updateClippingPlanes)
				{
					result = true;
				}

				var oldQualityScalingFactor = qualityScalingFactor;

				var qualityTooltip = "Adjust this property to modify the quality of the content rendered from the ArcGIS system. A higher number indicates better quality and a higher impact on performance.";
				qualityScalingFactor = EditorGUILayout.FloatField(new GUIContent("Quality Scaling Factor", qualityTooltip), qualityScalingFactor, GUILayout.ExpandWidth(true));

				qualityScalingFactor = Mathf.Clamp(qualityScalingFactor, 0.1f, 10);

				if (oldQualityScalingFactor != qualityScalingFactor)
				{
					result = true;
				}

				var oldUseCameraViewportProperties = useCameraViewportProperties;

				useCameraViewportProperties = EditorGUILayout.Toggle("Use Camera Viewport Properties", useCameraViewportProperties, GUILayout.ExpandWidth(true));

				if (oldUseCameraViewportProperties != useCameraViewportProperties)
				{
					result = true;
				}

				if (useCameraViewportProperties)
				{
					drawViewportProps = false;
				}
			}

			if (drawViewportProps)
			{
				// v fov
				var oldVerticalFov = verticalFov;

				verticalFov = EditorGUILayout.FloatField("Vertical FOV", verticalFov, GUILayout.ExpandWidth(true));

				if (oldVerticalFov != verticalFov)
				{
					result = true;
				}

				// h fov
				var oldhorizontalFov = horizontalFov;

				horizontalFov = EditorGUILayout.FloatField("Horizontal FOV", horizontalFov, GUILayout.ExpandWidth(true));

				if (oldhorizontalFov != horizontalFov)
				{
					result = true;
				}

				// viewport x
				var oldViewportSizeX = viewportSizeX;

				viewportSizeX = (uint)EditorGUILayout.IntField("Viewport Size X", (int)viewportSizeX, GUILayout.ExpandWidth(true));

				if (oldViewportSizeX != viewportSizeX)
				{
					result = true;
				}

				// viewport x
				var oldViewportSizeY = viewportSizeY;

				viewportSizeY = (uint)EditorGUILayout.IntField("Viewport Size Y", (int)viewportSizeY, GUILayout.ExpandWidth(true));

				if (oldViewportSizeY != viewportSizeY)
				{
					result = true;
				}
			}

			return result;
		}
	}
}
