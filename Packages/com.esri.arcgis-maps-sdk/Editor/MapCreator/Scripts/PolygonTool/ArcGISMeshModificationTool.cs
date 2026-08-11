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
using Esri.ArcGISMapsSDK.Editor.UI;
using Esri.ArcGISMapsSDK.Editor.Utils;
using Esri.GameEngine.Layers;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor
{
	class ArcGISMeshModificationTool
	{
		private Button clipButton;
		private Button maskButton;
		private Button replaceButton;

		private event Action<ArcGISMeshModificationsInstanceData> onMeshModificationsChanged;
		private event Action onToolClosed;
		private VisualElement overlayContent;

		private int selectedMeshModification = -1;
		private ArcGISSketchTool sketchTool;

		private ArcGISMeshModificationsInstanceData meshModificationsInstanceData;

		public ArcGISMeshModificationTool(Action onToolClosed, Action<ArcGISMeshModificationsInstanceData> onMeshModificationsChanged, IArcGISCoordinatesConverterInterface coordinatesConverter)
		{
			this.onToolClosed = onToolClosed;
			this.onMeshModificationsChanged = onMeshModificationsChanged;

			sketchTool = new ArcGISSketchTool(HandlePolygonAdded, HandlePolygonChanged, HandlePolygonDeleted, HandleSelectionChanged, HandleToolClosed, coordinatesConverter);

			var overlayTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/PolygonTool/ArcGISMeshModificationToolTemplate.uxml");

			overlayContent = new VisualElement
			{
				name = "ArcGISMeshModificationTool",
				pickingMode = PickingMode.Ignore
			};

			overlayTemplate.CloneTree(overlayContent);

			overlayContent.Insert(0, sketchTool.GetOverlayContent());

			clipButton = overlayContent.Q<Button>("clip");
			maskButton = overlayContent.Q<Button>("mask");
			replaceButton = overlayContent.Q<Button>("replace");

			clipButton.clicked += () =>
			{
				HandleTypeChanged(ArcGISMeshModificationType.Clip);
			};

			maskButton.clicked += () =>
			{
				HandleTypeChanged(ArcGISMeshModificationType.Mask);
			};

			replaceButton.clicked += () =>
			{
				HandleTypeChanged(ArcGISMeshModificationType.Replace);
			};
		}

		public void OnEnable(ArcGISMeshModificationsInstanceData meshModificationsInstanceData)
		{
			this.meshModificationsInstanceData = (ArcGISMeshModificationsInstanceData)meshModificationsInstanceData.Clone();

			var polygons = new List<ArcGISPolygonInstanceData>();

			foreach (var meshModification in this.meshModificationsInstanceData.MeshModifications)
			{
				polygons.Add(meshModification.Polygon);
			}

			sketchTool.OnEnable(polygons);

			UpdateSelectedType();

			SceneView.lastActiveSceneView.rootVisualElement.Add(overlayContent);
		}

		public void OnDisable()
		{
			sketchTool.OnDisable();

			if (overlayContent != null && SceneView.lastActiveSceneView.rootVisualElement.Contains(overlayContent))
			{
				SceneView.lastActiveSceneView.rootVisualElement.Remove(overlayContent);
			}
		}

		private void HandlePolygonAdded(ArcGISPolygonInstanceData polygonInstanceData)
		{
			var meshModification = new ArcGISMeshModificationInstanceData
			{
				Polygon = polygonInstanceData,
				Type = ArcGISMeshModificationType.Clip
			};

			meshModificationsInstanceData.MeshModifications.Add(meshModification);

			onMeshModificationsChanged?.Invoke(meshModificationsInstanceData);
		}

		private void HandlePolygonChanged(int changedPolygon, ArcGISPolygonInstanceData polygonInstanceData)
		{
			Assert.IsTrue(changedPolygon >= 0 && changedPolygon < meshModificationsInstanceData.MeshModifications.Count);

			meshModificationsInstanceData.MeshModifications[changedPolygon].Polygon = polygonInstanceData;

			onMeshModificationsChanged?.Invoke(meshModificationsInstanceData);
		}

		private void HandlePolygonDeleted(int deletedPolygon)
		{
			meshModificationsInstanceData.MeshModifications.RemoveAt(deletedPolygon);

			onMeshModificationsChanged?.Invoke(meshModificationsInstanceData);
		}

		private void HandleSelectionChanged(int selectedPolygon)
		{
			selectedMeshModification = selectedPolygon;

			UpdateSelectedType();
		}

		private void HandleToolClosed()
		{
			OnDisable();
			onToolClosed?.Invoke();
		}

		private void HandleTypeChanged(ArcGISMeshModificationType type)
		{
			if (selectedMeshModification == -1)
			{
				return;
			}

			var meshModification = meshModificationsInstanceData.MeshModifications[selectedMeshModification];

			if (meshModification.Type == type)
			{
				return;
			}

			meshModification.Type = type;

			UpdateSelectedType();

			onMeshModificationsChanged?.Invoke(meshModificationsInstanceData);
		}

		private void UpdateSelectedType()
		{
			clipButton.SetEnabled(selectedMeshModification != -1);
			maskButton.SetEnabled(selectedMeshModification != -1);
			replaceButton.SetEnabled(selectedMeshModification != -1);

			clipButton.RemoveFromClassList("selected");
			maskButton.RemoveFromClassList("selected");
			replaceButton.RemoveFromClassList("selected");

			if (selectedMeshModification == -1)
			{
				return;
			}

			var meshModification = meshModificationsInstanceData.MeshModifications[selectedMeshModification];

			if (meshModification.Type == ArcGISMeshModificationType.Clip)
			{
				clipButton.AddToClassList("selected");
			}
			else if (meshModification.Type == ArcGISMeshModificationType.Mask)
			{
				maskButton.AddToClassList("selected");
			}
			else if (meshModification.Type == ArcGISMeshModificationType.Replace)
			{
				replaceButton.AddToClassList("selected");
			}
		}
	}
}
