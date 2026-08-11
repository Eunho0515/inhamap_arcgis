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
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor
{
	class ArcGISSpatialFeatureFilterTool
	{
		private Button disjointButton;
		private Button containsButton;

		private event Action<ArcGISSpatialFeatureFilterInstanceData> onSpatialFeatureFilterChanged;
		private event Action onToolClosed;
		private VisualElement overlayContent;

		private ArcGISSketchTool sketchTool;

		private ArcGISSpatialFeatureFilterInstanceData spatialFeatureFilterInstanceData;

		public ArcGISSpatialFeatureFilterTool(Action onToolClosed, Action<ArcGISSpatialFeatureFilterInstanceData> onSpatialFeatureFilterChanged, IArcGISCoordinatesConverterInterface coordinatesConverter)
		{
			this.onSpatialFeatureFilterChanged = onSpatialFeatureFilterChanged;
			this.onToolClosed = onToolClosed;

			sketchTool = new ArcGISSketchTool(HandlePolygonAdded, HandlePolygonChanged, HandlePolygonDeleted, null, HandleToolClosed, coordinatesConverter);

			var overlayTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/PolygonTool/ArcGISSpatialFeatureFilterToolTemplate.uxml");

			overlayContent = new VisualElement
			{
				name = "ArcGISSpatialFeatureFilterTool",
				pickingMode = PickingMode.Ignore
			};

			overlayTemplate.CloneTree(overlayContent);

			overlayContent.Insert(0, sketchTool.GetOverlayContent());

			disjointButton = overlayContent.Q<Button>("disjoint");
			containsButton = overlayContent.Q<Button>("contains");

			disjointButton.clicked += () =>
			{
				HandleTypeChanged(ArcGISSpatialFeatureFilterSpatialRelationship.Disjoint);
			};

			containsButton.clicked += () =>
			{
				HandleTypeChanged(ArcGISSpatialFeatureFilterSpatialRelationship.Contains);
			};
		}

		public void OnEnable(ArcGISSpatialFeatureFilterInstanceData spatialFeatureFilterInstanceData)
		{
			this.spatialFeatureFilterInstanceData = (ArcGISSpatialFeatureFilterInstanceData)spatialFeatureFilterInstanceData.Clone();

			sketchTool.OnEnable(this.spatialFeatureFilterInstanceData.Polygons);

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
			spatialFeatureFilterInstanceData.Polygons.Add(polygonInstanceData);

			onSpatialFeatureFilterChanged?.Invoke(spatialFeatureFilterInstanceData);
		}

		private void HandlePolygonChanged(int changedPolygon, ArcGISPolygonInstanceData polygonInstanceData)
		{
			Assert.IsTrue(changedPolygon >= 0 && changedPolygon < spatialFeatureFilterInstanceData.Polygons.Count);

			spatialFeatureFilterInstanceData.Polygons[changedPolygon] = polygonInstanceData;

			onSpatialFeatureFilterChanged?.Invoke(spatialFeatureFilterInstanceData);
		}

		private void HandlePolygonDeleted(int deletedPolygon)
		{
			spatialFeatureFilterInstanceData.Polygons.RemoveAt(deletedPolygon);

			onSpatialFeatureFilterChanged?.Invoke(spatialFeatureFilterInstanceData);
		}

		private void HandleToolClosed()
		{
			OnDisable();

			onToolClosed?.Invoke();
		}

		private void HandleTypeChanged(ArcGISSpatialFeatureFilterSpatialRelationship type)
		{
			if (spatialFeatureFilterInstanceData.Type == type)
			{
				return;
			}

			spatialFeatureFilterInstanceData.Type = type;

			UpdateSelectedType();

			onSpatialFeatureFilterChanged?.Invoke(spatialFeatureFilterInstanceData);
		}

		private void UpdateSelectedType()
		{
			containsButton.RemoveFromClassList("selected");
			disjointButton.RemoveFromClassList("selected");

			if (spatialFeatureFilterInstanceData.Type == ArcGISSpatialFeatureFilterSpatialRelationship.Contains)
			{
				containsButton.AddToClassList("selected");
			}
			else if (spatialFeatureFilterInstanceData.Type == ArcGISSpatialFeatureFilterSpatialRelationship.Disjoint)
			{
				disjointButton.AddToClassList("selected");
			}
		}
	}
}
