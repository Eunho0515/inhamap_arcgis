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
using Esri.GameEngine.Layers.BuildingScene;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	internal class BSLCategoriesWindow : EditorWindow
	{
		private ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilterInstanceData;

		private event Action<ArcGISBuildingSceneLayerFilterInstanceData> onBuildingSceneLayerFilterChanged;

		private List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayerInstances;

		private List<ArcGISBuildingSceneLayerSublayerInstanceData> architecturalSublayers = new();
		private List<ArcGISBuildingSceneLayerSublayerInstanceData> structuralSublayers = new();
		private List<ArcGISBuildingSceneLayerSublayerInstanceData> mechanicalSublayers = new();
		private List<ArcGISBuildingSceneLayerSublayerInstanceData> electricalSublayers = new();
		private List<ArcGISBuildingSceneLayerSublayerInstanceData> pipingSublayers = new();

		private ListView architecturalListView;
		private ListView structuralListView;
		private ListView mechanicalListView;
		private ListView electricalListView;
		private ListView pipingListView;

		private Toggle architecturalToggle;
		private Toggle structuralToggle;
		private Toggle mechanicalToggle;
		private Toggle electricalToggle;
		private Toggle pipingToggle;

		private VisualTreeAsset bslCategoriesWindowCategoryItemTemplate;

		public void CreateGUI()
		{
			bslCategoriesWindowCategoryItemTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("BSLFilters/BSLCategoriesWindowCategoryItemTemplate.uxml");

			var bslcategoriesWindowTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("BSLFilters/BSLCategoriesWindowTemplate.uxml");
			bslcategoriesWindowTemplate.CloneTree(rootVisualElement);

			architecturalListView = rootVisualElement.Q<ListView>("architectural-listview");
			structuralListView = rootVisualElement.Q<ListView>("structural-listview");
			mechanicalListView = rootVisualElement.Q<ListView>("mechanical-listview");
			electricalListView = rootVisualElement.Q<ListView>("electrical-listview");
			pipingListView = rootVisualElement.Q<ListView>("piping-listview");

			architecturalToggle = rootVisualElement.Q<Toggle>("architectural-toggle");
			structuralToggle = rootVisualElement.Q<Toggle>("structural-toggle");
			mechanicalToggle = rootVisualElement.Q<Toggle>("mechanical-toggle");
			electricalToggle = rootVisualElement.Q<Toggle>("electrical-toggle");
			pipingToggle = rootVisualElement.Q<Toggle>("piping-toggle");

			var selectAllButton = rootVisualElement.Q<Button>("select-all-button");
			var clearButton = rootVisualElement.Q<Button>("clear-button");

			selectAllButton.clicked += () =>
			{
				buildingSceneLayerFilterInstanceData.EnabledCategories = sublayerInstances.Select(sublayer => sublayer.ID).ToList();
				buildingSceneLayerFilterInstanceData.EnabledDisciplines = sublayerInstances.Select(sublayer => sublayer.Discipline).Distinct().ToList();
				UpdateData();
			};

			clearButton.clicked += () =>
			{
				buildingSceneLayerFilterInstanceData.EnabledCategories.Clear();
				buildingSceneLayerFilterInstanceData.EnabledDisciplines.Clear();
				UpdateData();
			};
		}

		private Func<VisualElement> MakeItem(List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayerList)
		{
			return () =>
			{
				var element = new VisualElement();

				bslCategoriesWindowCategoryItemTemplate.CloneTree(element);

				var toggle = element.Q<Toggle>();

				toggle.RegisterValueChangedCallback(evnt =>
				{
					if (element.userData is int index && index < sublayerList.Count)
					{
						index = (int)element.userData;

						if (evnt.newValue)
						{
							buildingSceneLayerFilterInstanceData.EnabledCategories.Add(sublayerList[index].ID);
						}
						else
						{
							buildingSceneLayerFilterInstanceData.EnabledCategories.Remove(sublayerList[index].ID);
						}

						UpdateData();
					}
				});

				return element;
			};
		}

		private Action<VisualElement, int> BindAction(List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayerList)
		{
			return (element, index) =>
			{
				element.userData = index;

				if (index < sublayerList.Count)
				{
					var label = element.Q<Label>();
					var toggle = element.Q<Toggle>();

					label.text = sublayerList[index].Name;
					label.SetEnabled(buildingSceneLayerFilterInstanceData.EnabledDisciplines.Contains(sublayerList[index].Discipline));
					toggle.SetValueWithoutNotify(buildingSceneLayerFilterInstanceData.EnabledCategories.Contains(sublayerList[index].ID));
				}
			};
		}

		private Action<VisualElement, int> UnbindAction()
		{
			return (element, index) =>
			{
				element.userData = null;
			};
		}

		public void Init(ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilterInstanceData, Action<ArcGISBuildingSceneLayerFilterInstanceData> onBuildingSceneLayerFilterChanged, List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayerInstances)
		{
			this.buildingSceneLayerFilterInstanceData = buildingSceneLayerFilterInstanceData;
			this.onBuildingSceneLayerFilterChanged = onBuildingSceneLayerFilterChanged;
			this.sublayerInstances = sublayerInstances;

			PopulateSublayers(ref architecturalSublayers, ArcGISBuildingSceneSublayerDiscipline.Architectural);
			InitDiscipline(architecturalSublayers, architecturalListView, architecturalToggle, "architectural-element", ArcGISBuildingSceneSublayerDiscipline.Architectural);

			PopulateSublayers(ref structuralSublayers, ArcGISBuildingSceneSublayerDiscipline.Structural);
			InitDiscipline(structuralSublayers, structuralListView, structuralToggle, "structural-element", ArcGISBuildingSceneSublayerDiscipline.Structural);

			PopulateSublayers(ref mechanicalSublayers, ArcGISBuildingSceneSublayerDiscipline.Mechanical);
			InitDiscipline(mechanicalSublayers, mechanicalListView, mechanicalToggle, "mechanical-element", ArcGISBuildingSceneSublayerDiscipline.Mechanical);

			PopulateSublayers(ref electricalSublayers, ArcGISBuildingSceneSublayerDiscipline.Electrical);
			InitDiscipline(electricalSublayers, electricalListView, electricalToggle, "electrical-element", ArcGISBuildingSceneSublayerDiscipline.Electrical);

			PopulateSublayers(ref pipingSublayers, ArcGISBuildingSceneSublayerDiscipline.Piping);
			InitDiscipline(pipingSublayers, pipingListView, pipingToggle, "piping-element", ArcGISBuildingSceneSublayerDiscipline.Piping);

			UpdateData();
		}

		private void PopulateSublayers(ref List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayers, ArcGISBuildingSceneSublayerDiscipline discipline)
		{
			sublayers = sublayerInstances.Where(sublayerInstance => sublayerInstance.Discipline == discipline).ToList();
			sublayers.Sort((x, y) => x.Name.CompareTo(y.Name));
		}

		private void InitDiscipline(List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayers, ListView listView, Toggle toggle, string containerName, ArcGISBuildingSceneSublayerDiscipline discipline)
		{
			listView.makeItem = MakeItem(sublayers);
			listView.bindItem = BindAction(sublayers);
			listView.unbindItem = UnbindAction();
			toggle.RegisterValueChangedCallback(evnt => RegisterDisciplineToggleCallback(evnt, discipline));

			rootVisualElement.Q<VisualElement>(containerName).style.display = sublayers.Any() ? DisplayStyle.Flex : DisplayStyle.None;
		}

		private void RegisterDisciplineToggleCallback(ChangeEvent<bool> evnt, ArcGISBuildingSceneSublayerDiscipline discipline)
		{
			if (evnt.newValue)
			{
				buildingSceneLayerFilterInstanceData.EnabledDisciplines.Add(discipline);
				buildingSceneLayerFilterInstanceData.EnabledDisciplines = buildingSceneLayerFilterInstanceData.EnabledDisciplines.Distinct().ToList();
			}
			else
			{
				buildingSceneLayerFilterInstanceData.EnabledDisciplines.Remove(discipline);
			}
			UpdateData();
		}

		private void UpdateData()
		{
			UpdateDiscipline(architecturalListView, architecturalSublayers, architecturalToggle, ArcGISBuildingSceneSublayerDiscipline.Architectural);
			UpdateDiscipline(structuralListView, structuralSublayers, structuralToggle, ArcGISBuildingSceneSublayerDiscipline.Structural);
			UpdateDiscipline(mechanicalListView, mechanicalSublayers, mechanicalToggle, ArcGISBuildingSceneSublayerDiscipline.Mechanical);
			UpdateDiscipline(electricalListView, electricalSublayers, electricalToggle, ArcGISBuildingSceneSublayerDiscipline.Electrical);
			UpdateDiscipline(pipingListView, pipingSublayers, pipingToggle, ArcGISBuildingSceneSublayerDiscipline.Piping);

			onBuildingSceneLayerFilterChanged?.Invoke((ArcGISBuildingSceneLayerFilterInstanceData)buildingSceneLayerFilterInstanceData.Clone());
		}

		private void UpdateDiscipline(ListView listView, List<ArcGISBuildingSceneLayerSublayerInstanceData> sublayers, Toggle toggle, ArcGISBuildingSceneSublayerDiscipline discipline)
		{
			listView.itemsSource = sublayers;
			listView.RefreshItems();

			toggle.SetValueWithoutNotify(buildingSceneLayerFilterInstanceData.EnabledDisciplines.Contains(discipline));
		}
	}
}
