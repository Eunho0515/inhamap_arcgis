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
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorCameraTool : ArcGISMapCreatorTool
	{
		private class CameraSerializedProperties : ScriptableObject
		{
#pragma warning disable CS0414
			[SerializeField]
			ArcGISPointInstanceData position = new();
			[SerializeField]
			ArcGISRotation rotation;
#pragma warning restore CS0414
		}

		private ArcGISCameraComponent selectedCamera;
		private ArcGISLocationComponent selectedLocationComponent;
		private ArcGISMapComponent mapComponent;
		private SerializedObject serializedObject;

		private Button createCamButton;
		private ArcGISButtonField alignCameraField;
		private VisualElement rootElement;

		public override void OnDeselected()
		{
			rootElement?.Unbind();
		}

		public override void OnEnable()
		{
			rootElement = new VisualElement
			{
				name = "ArcGISMapCreatorCameraTool"
			};

			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/CameraToolTemplate.uxml");
			template.CloneTree(rootElement);

			InitializeCreateCameraButton();

			InitializeAlignCameraField();
			InitAlignCameraToViewButton();

			FindCamera();
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			mapComponent = mapComponentInterface as ArcGISMapComponent;

			FindCamera();

			rootElement.Unbind();

			if (selectedLocationComponent)
			{
				serializedObject = new SerializedObject(selectedLocationComponent);
			}
			else
			{
				serializedObject = new SerializedObject(ScriptableObject.CreateInstance<CameraSerializedProperties>());
			}

			rootElement.Bind(serializedObject);
		}

		public override VisualElement GetContent()
		{
			return rootElement;
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/CameraToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Camera";
		}

		private void FindCamera()
		{
			selectedCamera = MapCreatorUtilities.CameraFromActiveMapComponent;

			if (selectedCamera)
			{
				if (!selectedCamera.transform.parent || !selectedCamera.transform.parent.GetComponent<ArcGISMapComponent>())
				{
					Debug.LogWarning("Parent the ArcGIS Camera game object to a game object with an ArcGIS Map component to use the Camera UI tool");
				}

				selectedLocationComponent = selectedCamera.GetComponent<ArcGISLocationComponent>();

				if (!selectedLocationComponent)
				{
					Debug.LogWarning("Attach an ArcGIS Location component to the ArcGIS Camera game object to use the full capability of the Camera UI tool");
				}
			}
			else
			{
				selectedLocationComponent = null;
			}

			createCamButton.SetEnabled(!selectedCamera || !selectedLocationComponent);
		}

		private void InitializeCreateCameraButton()
		{
			createCamButton = rootElement.Query<Button>(name: "button-create-camera");

			createCamButton.clickable.activators.Clear();
			createCamButton.RegisterCallback<MouseDownEvent>(CreateCamera);
		}

		private void InitializeAlignCameraField()
		{
			alignCameraField = rootElement.Q<ArcGISButtonField>(name: "button-transfer-to-camera-field");
			alignCameraField.label = "Editor Mode View";
			alignCameraField.value = "Align with View";
			alignCameraField.Button.name = "button-transfer-to-camera";
		}

		private void CreateCamera(MouseDownEvent evnt)
		{
			GameObject cameraComponentGameObject;

			if (Camera.main && !Camera.main.GetComponentInParent<ArcGISMapComponent>())
			{
				cameraComponentGameObject = Camera.main.gameObject;
				cameraComponentGameObject.name = "ArcGISCamera";
			}
			else
			{
				cameraComponentGameObject = new GameObject("ArcGISCamera");
				cameraComponentGameObject.AddComponent<Camera>();
				cameraComponentGameObject.tag = "MainCamera";
			}

			if (SceneView.lastActiveSceneView)
			{
				cameraComponentGameObject.transform.SetPositionAndRotation(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation);
			}

			if (mapComponent)
			{
				cameraComponentGameObject.transform.parent = mapComponent.transform;
			}

			selectedCamera = cameraComponentGameObject.AddComponent<ArcGISCameraComponent>();
			selectedLocationComponent = cameraComponentGameObject.AddComponent<ArcGISLocationComponent>();

			var newSerializedObject = new SerializedObject(selectedLocationComponent);

			newSerializedObject.CopyFromSerializedProperty(serializedObject.FindProperty("position"));
			newSerializedObject.CopyFromSerializedProperty(serializedObject.FindProperty("rotation"));

			newSerializedObject.ApplyModifiedProperties();

			Undo.RegisterCreatedObjectUndo(cameraComponentGameObject, "Create " + cameraComponentGameObject.name);

			Selection.activeGameObject = cameraComponentGameObject;

			OnSelected(mapComponent);
		}

		private void InitAlignCameraToViewButton()
		{
			var AlignCameraToViewButton = alignCameraField.Button;
			AlignCameraToViewButton.clickable.activators.Clear();
			AlignCameraToViewButton.RegisterCallback<MouseDownEvent>(evnt =>
			{
				if (Application.isPlaying || !selectedCamera || !selectedLocationComponent)
				{
					return;
				}

				var cameraTransform = selectedCamera.GetComponent<HPTransform>();
				var mapComponent = cameraTransform.GetComponentInParent<ArcGISMapComponent>();

				var sceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;

				cameraTransform.transform.SetPositionAndRotation(sceneViewCameraTransform.position, sceneViewCameraTransform.rotation);

				var worldPosition = math.inverse(mapComponent.WorldMatrix).HomogeneousTransformPoint(sceneViewCameraTransform.position.ToDouble3());
				var geoPosition = mapComponent.View.WorldToGeographic(worldPosition);

				geoPosition = GeoUtils.ProjectToSpatialReference(geoPosition, selectedLocationComponent.Position.SpatialReference);

				if (!geoPosition.IsValid)
				{
					return;
				}

				var worldRotation = mapComponent.UniverseRotation * sceneViewCameraTransform.rotation;
				var geoRotation = GeoUtils.FromCartesianRotation(worldPosition, quaternionExtensions.ToQuaterniond(worldRotation),
					selectedLocationComponent.Position.SpatialReference, mapComponent.MapType);

				Undo.RecordObject(selectedLocationComponent, "Align Camera To View");

				selectedLocationComponent.Position = geoPosition;
				selectedLocationComponent.Rotation = geoRotation;
			});
		}
	}
}
