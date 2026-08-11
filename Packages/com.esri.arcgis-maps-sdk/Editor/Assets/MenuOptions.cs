using Esri.ArcGISMapsSDK.Components;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Esri.ArcGISMapsSDK.Editor.Assets
{
	static class MenuOptions
	{
		[MenuItem("GameObject/ArcGISMaps SDK/Camera", false, 10)]
		static void AddCamera(MenuCommand menuCommand)
		{
			var cameraComponentGameObject = new GameObject("ArcGISCamera");

			cameraComponentGameObject.AddComponent<Camera>();

			if (SceneView.lastActiveSceneView != null)
			{
				cameraComponentGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
				cameraComponentGameObject.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
			}

			if (!Camera.main)
			{
				cameraComponentGameObject.tag = "MainCamera";
			}

			AttachToMap(cameraComponentGameObject, menuCommand);

			cameraComponentGameObject.AddComponent<ArcGISCameraComponent>();

			Undo.RegisterCreatedObjectUndo(cameraComponentGameObject, "Create " + cameraComponentGameObject.name);

			Selection.activeGameObject = cameraComponentGameObject;
		}

		[MenuItem("GameObject/ArcGISMaps SDK/Map", false, 10)]
#pragma warning disable IDE0060 // Remove unused parameter
		static void AddMap(MenuCommand menuCommand)
#pragma warning restore IDE0060 // Remove unused parameter

		{
			CreateMapGameObject();
		}

		static void AttachToMap(GameObject element, MenuCommand menuCommand)
		{
			var parent = menuCommand.context as GameObject;

			if (parent == null)
			{
				parent = GetOrCreateMapGameObject();
			}

			SceneManager.MoveGameObjectToScene(element, parent.scene);

			if (element.transform.parent == null)
			{
				Undo.SetTransformParent(element.transform, parent.transform, true, "Parent " + element.name);
			}

			GameObjectUtility.EnsureUniqueNameForSibling(element);
		}

		static GameObject CreateMapGameObject()
		{
			var mapComponentGameObject = new GameObject("ArcGISMap");

			mapComponentGameObject.AddComponent<ArcGISMapComponent>();

			GameObjectUtility.EnsureUniqueNameForSibling(mapComponentGameObject);

			Undo.RegisterCreatedObjectUndo(mapComponentGameObject, "Create " + mapComponentGameObject.name);

			return mapComponentGameObject;
		}

		static GameObject GetOrCreateMapGameObject()
		{
			var selectedGo = Selection.activeGameObject;

			var mapComponent = selectedGo?.GetComponentInParent<ArcGISMapComponent>();

			if (IsValidMap(mapComponent))
			{
				return mapComponent.gameObject;
			}

			var mapComponents = StageUtility.GetCurrentStageHandle().FindComponentsOfType<ArcGISMapComponent>();

			for (var i = 0; i < mapComponents.Length; i++)
			{
				if (IsValidMap(mapComponents[i]))
				{
					return mapComponents[i].gameObject;
				}
			}

			return CreateMapGameObject();
		}

		static bool IsValidMap(ArcGISMapComponent mapComponent)
		{
			if (!mapComponent || !mapComponent.gameObject.activeInHierarchy)
			{
				return false;
			}

			return true;
		}
	}
}
