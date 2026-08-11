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
using Esri.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	internal class BSLFiltersWindow : EditorWindow
	{
		private ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilterInstanceData;

		private event Action<ArcGISBuildingSceneLayerFilterInstanceData> onBuildingSceneLayerFilterChanged;

		private List<ArcGISBuildingSceneLayerAttributeStatisticsInstanceData> attributeStatistics;

		private int selectedTab = 0;

		private ArcGISBuildingSceneLayerAttributeStatisticsInstanceData selectedAttribute;
		private List<string> displayedAttributeValues;

		private const string searchPlaceHolderText = "Search filter definition values";

		private VisualElement filterAttributesTab;
		private VisualElement filterDefinitionTab;
		private ArcGISBuildingAttributeFilterDropdownField filtersDropdown;
		private ListView filtersListView;
		private ListView attributesListView;
		private ListView selectedAttributesListView;
		private ListView selectedValuesListView;
		private VisualElement newFilterTab;
		private Foldout selectedFilterFoldout;
		private VisualElement whereClauseTab;
		private TextField whereClauseTextField;
		private TextField valueSearchTextField;
		private Button generateClauseButton;
		private Toggle selectAllValuesToggle;
		private Toggle selectAllAttributesToggle;

		public void CreateGUI()
		{
			rootVisualElement.AddToClassList(EditorGUIUtility.isProSkin ? "dark" : "light");

			var bslFiltersWindowFilterItemTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("BSLFilters/BSLFiltersWindowFilterItemTemplate.uxml");
			var bslFiltersWindowAttributeItemTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("BSLFilters/BSLFiltersWindowAttributeItemTemplate.uxml");

			var bslFiltersWindowTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("BSLFilters/BSLFiltersWindowTemplate.uxml");
			bslFiltersWindowTemplate.CloneTree(rootVisualElement);

			var toolbarContainer = rootVisualElement.Q<IMGUIContainer>();

			toolbarContainer.onGUIHandler = () => IMGUICode();

			filterAttributesTab = rootVisualElement.Q<VisualElement>("filter-attributes-tab");
			filterDefinitionTab = rootVisualElement.Q<VisualElement>("filter-definition-tab");
			newFilterTab = rootVisualElement.Q<VisualElement>("new-filter-tab");
			whereClauseTab = rootVisualElement.Q<VisualElement>("where-clause-tab");

			filtersDropdown = rootVisualElement.Q<ArcGISBuildingAttributeFilterDropdownField>("filters-dropdown");
			filtersListView = rootVisualElement.Q<ListView>("filters-listview");
			selectedFilterFoldout = rootVisualElement.Q<Foldout>("selected-filter-foldout");
			whereClauseTextField = rootVisualElement.Q<TextField>("where-clause-textfield");
			valueSearchTextField = rootVisualElement.Q<TextField>("select-filter-definition-values-search-textfield");

			var addFilterButton = rootVisualElement.Q<VisualElement>("add-filter-button");
			var applyButton = rootVisualElement.Q<Button>("apply-button");
			var clearButton = rootVisualElement.Q<Button>("clear-button");
			generateClauseButton = rootVisualElement.Q<Button>("generate-clause-button");
			selectAllValuesToggle = rootVisualElement.Q<Toggle>("select-all-values-toggle");
			selectAllAttributesToggle = rootVisualElement.Q<Toggle>("select-all-attributes-toggle");

			attributesListView = rootVisualElement.Q<ListView>("filter-attributes-listview");
			selectedAttributesListView = rootVisualElement.Q<ListView>("selected-filter-attributes-listview");
			selectedValuesListView = rootVisualElement.Q<ListView>("select-filter-definition-values-listview");

			addFilterButton.RegisterCallback<MouseUpEvent>(evnt =>
			{
				buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.Add(new ArcGISBuildingAttributeFilterInstanceData() { Name = $"New Filter" });

				UpdateData();
			});

			applyButton.clicked += () =>
			{
				onBuildingSceneLayerFilterChanged?.Invoke((ArcGISBuildingSceneLayerFilterInstanceData)buildingSceneLayerFilterInstanceData.Clone());
			};

			clearButton.clicked += () =>
			{
				if (selectedTab == 0)
				{
					buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex = -1;

					UpdateData();
				}
				else if (selectedTab == 1)
				{
					GetActiveFilter()?.SolidFilterDefinition?.EnabledStatistics.Clear();

					UpdateData();
				}
				else if (selectedTab == 2)
				{
					selectedAttribute?.MostFrequentValues.Clear();

					UpdateData();
				}
				else if (selectedTab == 3)
				{
					if (GetActiveFilter()?.SolidFilterDefinition != null)
					{
						GetActiveFilter().SolidFilterDefinition.WhereClause = string.Empty;

						UpdateData();
					}
				}
			};

			generateClauseButton.clicked += () =>
			{
				var activeFilter = GetActiveFilter();

				if (activeFilter?.SolidFilterDefinition != null)
				{
					activeFilter.SolidFilterDefinition.WhereClause = GenerateWhereClause(activeFilter.SolidFilterDefinition);
				}

				whereClauseTextField.SetValueWithoutNotify(activeFilter?.SolidFilterDefinition?.WhereClause);

				UpdateData();
			};

			filtersDropdown.formatSelectedValueCallback = (ArcGISBuildingAttributeFilterInstanceData buildingAttributeFilterInstanceData) =>
			{
				var index = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.IndexOf(buildingAttributeFilterInstanceData);

				return $"{index}. {buildingAttributeFilterInstanceData.Name}";
			};

			filtersDropdown.formatListItemCallback = (ArcGISBuildingAttributeFilterInstanceData buildingAttributeFilterInstanceData) =>
			{
				var index = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.IndexOf(buildingAttributeFilterInstanceData);

				return $"{index}. {buildingAttributeFilterInstanceData.Name}";
			};

			filtersDropdown.RegisterValueChangedCallback(evnt =>
			{
				var index = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.IndexOf(evnt.newValue);

				buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex = index;

				UpdateData();
			});

			filtersListView.makeItem = () =>
			{
				var element = new VisualElement();

				bslFiltersWindowFilterItemTemplate.CloneTree(element);

				var editButtonImage = element.Query<Button>("edit").Children<Image>().First();
				var duplicateButtonImage = element.Query<Button>("duplicate").Children<Image>().First();
				var deleteButtonImage = element.Query<Button>("delete").Children<Image>().First();

				editButtonImage.image = EditorGUIUtility.IconContent("d_editicon.sml").image;
				duplicateButtonImage.image = EditorGUIUtility.IconContent("TreeEditor.Duplicate").image;
				deleteButtonImage.image = EditorGUIUtility.IconContent("TreeEditor.Trash").image;

				var deleteButton = element.Q<Button>("delete");
				var duplicateButton = element.Q<Button>("duplicate");
				var editButton = element.Q<Button>("edit");
				var toggle = element.Q<Toggle>();

				Label nameLabel = element.Query<Label>();
				TextField nameField = element.Query<TextField>();

				editButton.clicked += () =>
				{
					selectedTab = 3;

					UpdateTabContent();
				};

				toggle.RegisterValueChangedCallback(evnt =>
				{
					var index = (int)element.userData;

					buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex = evnt.newValue ? index : -1;

					UpdateData();
				});

				duplicateButton.clicked += () =>
				{
					var index = (int)element.userData;

					var buildingAttributeFilter = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[index];

					buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.Add(new ArcGISBuildingAttributeFilterInstanceData()
					{
						Name = $"{buildingAttributeFilter.Name} - Duplicate",
						Description = buildingAttributeFilter.Description,
						SolidFilterDefinition = new ArcGISSolidBuildingFilterDefinitionInstanceData()
						{
							Title = buildingAttributeFilter.SolidFilterDefinition.Title,
							WhereClause = buildingAttributeFilter.SolidFilterDefinition.WhereClause,
						},
					});

					UpdateData();
				};

				deleteButton.clicked += () =>
				{
					var index = (int)element.userData;

					buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.RemoveAt(index);

					if (buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex == index)
					{
						buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex = -1;
					}

					UpdateData();
				};

				nameLabel.RegisterCallback<MouseDownEvent>(evnt =>
				{
					evnt.StopImmediatePropagation();
					var index = (int)element.userData;

					var buildingAttributeFilter = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[index];

					nameLabel.style.display = DisplayStyle.None;

					nameField.style.display = DisplayStyle.Flex;
					nameField.SetValueWithoutNotify(buildingAttributeFilter.Name);
					nameField.Focus();
				});

				nameField.RegisterCallback<FocusInEvent>(evnt =>
				{
					nameField.SelectAll();
				});

				nameField.RegisterCallback<FocusOutEvent>(evnt =>
				{
					var index = (int)element.userData;

					buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[index].Name = nameField.text;

					nameLabel.style.display = DisplayStyle.Flex;
					nameLabel.text = nameField.text;

					nameField.style.display = DisplayStyle.None;
				});

				nameField.RegisterCallback<KeyDownEvent>(evnt =>
				{
					if (evnt.keyCode != KeyCode.Return)
					{
						return;
					}

					evnt.StopImmediatePropagation();

					var index = (int)element.userData;

					buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[index].Name = nameField.text;

					nameLabel.style.display = DisplayStyle.Flex;
					nameLabel.text = nameField.text;

					nameField.style.display = DisplayStyle.None;
				});

				nameField.style.display = DisplayStyle.None;

				return element;
			};

			filtersListView.bindItem = (element, index) =>
			{
				element.userData = index;

				var label = element.Q<Label>();
				var toggle = element.Q<Toggle>();

				var buildingAttributeFilter = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[index];

				label.text = buildingAttributeFilter.Name;

				toggle.SetValueWithoutNotify(buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex == index);
			};

			filtersListView.unbindItem = (element, index) =>
			{
				element.userData = null;
			};

			attributesListView.makeItem = () =>
			{
				var element = new VisualElement();

				bslFiltersWindowAttributeItemTemplate.CloneTree(element);

				var toggle = element.Q<Toggle>();

				toggle.RegisterValueChangedCallback(evnt =>
				{
					var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

					if (solidFilterDefinition != null)
					{
						var index = (int)element.userData;
						var attribute = attributeStatistics.ElementAt(index);

						var enabledStatistic = solidFilterDefinition.EnabledStatistics.Find(enabledStatistic => enabledStatistic.FieldName == attribute.FieldName);

						if (evnt.newValue && enabledStatistic == null)
						{
							solidFilterDefinition.EnabledStatistics.Add(new ArcGISBuildingSceneLayerAttributeStatisticsInstanceData
							{
								FieldName = attribute.FieldName
							});
						}
						else
						{
							solidFilterDefinition.EnabledStatistics.Remove(enabledStatistic);
						}

						solidFilterDefinition.EnabledStatistics.Sort(delegate (ArcGISBuildingSceneLayerAttributeStatisticsInstanceData x, ArcGISBuildingSceneLayerAttributeStatisticsInstanceData y)
						{
							return string.Compare(x.FieldName, y.FieldName);
						});

						selectAllAttributesToggle.SetValueWithoutNotify(solidFilterDefinition.EnabledStatistics.Count == attributeStatistics.Count);

						UpdateData();
					}
				});

				return element;
			};

			attributesListView.bindItem = (element, index) =>
			{
				element.userData = index;

				var label = element.Q<Label>();

				var attribute = attributeStatistics.ElementAt(index);

				label.text = attribute.FieldName;

				var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

				var toggle = element.Q<Toggle>();

				if (solidFilterDefinition != null)
				{
					var enabledStatistic = solidFilterDefinition.EnabledStatistics.Find(enabledStatistic => enabledStatistic.FieldName == attribute.FieldName);

					toggle.SetValueWithoutNotify(enabledStatistic != null);
				}
				else
				{
					toggle.SetValueWithoutNotify(false);
				}
			};

			attributesListView.unbindItem = (element, index) =>
			{
				element.userData = null;
			};

			selectedAttributesListView.makeItem = () =>
			{
				var element = new Label();

				return element;
			};

			selectedAttributesListView.bindItem = (element, index) =>
			{
				element.userData = index;

				var label = element.Q<Label>();

				var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

				var enabledStatistics = solidFilterDefinition.EnabledStatistics[index];

				var labelText = enabledStatistics.FieldName;

				if (enabledStatistics.MostFrequentValues.Count > 0)
				{
					labelText += $" ({enabledStatistics.MostFrequentValues.Count})";
				}

				label.text = labelText;
			};

			selectedAttributesListView.unbindItem = (element, index) =>
			{
				element.userData = null;
			};

#if UNITY_2022_2_OR_NEWER
			selectedAttributesListView.selectionChanged += (selection) =>
#else
			selectedAttributesListView.onSelectionChange += (selection) =>
#endif
			{
				if (selection.Count() > 0)
				{
					selectedAttribute = (ArcGISBuildingSceneLayerAttributeStatisticsInstanceData)selection.First();
				}
				else
				{
					selectedAttribute = null;
				}

				UpdateData();
			};

			selectedValuesListView.makeItem = () =>
			{
				var element = new VisualElement();

				bslFiltersWindowAttributeItemTemplate.CloneTree(element);

				var toggle = element.Q<Toggle>();

				toggle.RegisterValueChangedCallback(evnt =>
				{
					var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

					if (solidFilterDefinition != null)
					{
						var index = (int)element.userData;
						var frequentValue = displayedAttributeValues[index];

						if (evnt.newValue)
						{
							selectedAttribute.MostFrequentValues.Add(frequentValue);
						}
						else
						{
							selectedAttribute.MostFrequentValues.Remove(frequentValue);
						}

						selectedAttribute.MostFrequentValues = selectedAttribute.MostFrequentValues.Distinct().ToList();

						UpdateData();
					}
				});

				return element;
			};

			selectedValuesListView.bindItem = (element, index) =>
			{
				element.userData = index;

				var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

				if (solidFilterDefinition != null)
				{
					var label = element.Q<Label>();
					var toggle = element.Q<Toggle>();

					var frequentValue = displayedAttributeValues[index];

					label.text = frequentValue;

					toggle.SetValueWithoutNotify(selectedAttribute.MostFrequentValues.Contains(frequentValue));
				}
			};

			selectedValuesListView.unbindItem = (element, index) =>
			{
				element.userData = null;
			};

			valueSearchTextField.RegisterValueChangedCallback(evnt =>
			{
				UpdateData();
			});

			selectAllValuesToggle.RegisterValueChangedCallback(evnt =>
			{
				var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

				if (solidFilterDefinition == null || selectedAttribute == null)
				{
					return;
				}

				var enabledStatistic = solidFilterDefinition.EnabledStatistics.Find(enabledStatistic => enabledStatistic.FieldName == selectedAttribute.FieldName);

				if (evnt.newValue)
				{
					displayedAttributeValues.ForEach(v => enabledStatistic.MostFrequentValues.Add(v));
					enabledStatistic.MostFrequentValues = enabledStatistic.MostFrequentValues.Distinct().ToList();
				}
				else
				{
					enabledStatistic.MostFrequentValues.RemoveAll(v => displayedAttributeValues.Contains(v));
				}

				UpdateData();
			});

			selectAllAttributesToggle.RegisterValueChangedCallback(evnt =>
			{
				var solidFilterDefinition = GetActiveFilter()?.SolidFilterDefinition;

				if (solidFilterDefinition == null)
				{
					return;
				}

				foreach (var attribute in attributeStatistics)
				{
					var enabledStatistic = solidFilterDefinition.EnabledStatistics.Find(enabledStatistic => enabledStatistic.FieldName == attribute.FieldName);
					if (evnt.newValue)
					{
						if (enabledStatistic == null)
						{
							solidFilterDefinition.EnabledStatistics.Add(new ArcGISBuildingSceneLayerAttributeStatisticsInstanceData
							{
								FieldName = attribute.FieldName
							});
						}
					}
					else
					{
						if (enabledStatistic != null)
						{
							solidFilterDefinition.EnabledStatistics.Remove(enabledStatistic);
						}
					}
				}

				solidFilterDefinition.EnabledStatistics.Sort(delegate (ArcGISBuildingSceneLayerAttributeStatisticsInstanceData x, ArcGISBuildingSceneLayerAttributeStatisticsInstanceData y)
				{
					return string.Compare(x.FieldName, y.FieldName);
				});

				UpdateData();
			});

			// Placeholders are not supported until 2023.1
			EditorUtilities.SetPlaceholderText(valueSearchTextField, searchPlaceHolderText);

			whereClauseTextField.RegisterValueChangedCallback(evnt =>
			{
				var activeFilter = GetActiveFilter();

				if (activeFilter != null)
				{
					activeFilter.SolidFilterDefinition.WhereClause = evnt.newValue;
				}
			});

			UpdateTabContent();
		}

		private ArcGISBuildingAttributeFilterInstanceData GetActiveFilter()
		{
			if (buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex < 0 || buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex >= buildingSceneLayerFilterInstanceData.BuildingAttributeFilters.Count)
			{
				return null;
			}

			return buildingSceneLayerFilterInstanceData.BuildingAttributeFilters[buildingSceneLayerFilterInstanceData.ActiveBuildingAttributeFilterIndex];
		}

		private void IMGUICode()
		{
			selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "New Filter", "Filter Attributes", "Filter Definition", "WHERE Clause" });

			UpdateTabContent();
		}

		public void Init(ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilterInstanceData, Action<ArcGISBuildingSceneLayerFilterInstanceData> onBuildingSceneLayerFilterChanged, List<ArcGISBuildingSceneLayerAttributeStatisticsInstanceData> attributeStatistics)
		{
			attributesListView.itemsSource = attributeStatistics;

			// These will be set/updated by UpdateData
			filtersListView.itemsSource = null;
			selectedAttributesListView.itemsSource = null;
			selectedValuesListView.itemsSource = null;

			this.buildingSceneLayerFilterInstanceData = buildingSceneLayerFilterInstanceData;
			this.onBuildingSceneLayerFilterChanged = onBuildingSceneLayerFilterChanged;
			this.attributeStatistics = attributeStatistics;

			selectedTab = 0;
			selectedAttribute = null;

			UpdateData();
		}

		private void UpdateData()
		{
			var activeFilter = GetActiveFilter();

			filtersDropdown.choices = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters;
			filtersDropdown.SetValueWithoutNotify(activeFilter);

			filtersListView.itemsSource = buildingSceneLayerFilterInstanceData.BuildingAttributeFilters;
			filtersListView.RefreshItems();

			attributesListView.RefreshItems();

			var filterDefinition = activeFilter?.SolidFilterDefinition;

			if (filterDefinition != null)
			{
				selectedAttributesListView.itemsSource = filterDefinition.EnabledStatistics;
				selectedAttributesListView.RefreshItems();

				if (!filterDefinition.EnabledStatistics.Contains(selectedAttribute))
				{
					selectedAttribute = null;
					selectedValuesListView.itemsSource = null;
					selectedAttributesListView.ClearSelection();
					displayedAttributeValues?.Clear();
				}

				if (selectedAttribute != null)
				{
					displayedAttributeValues = attributeStatistics.Find(attributeStatistic => attributeStatistic.FieldName == selectedAttribute.FieldName)?.MostFrequentValues;

					if (displayedAttributeValues != null)
					{
						var searchTextFieldValue = EditorUtilities.GetTextFieldValue(valueSearchTextField).ToLowerInvariant();

						if (!string.IsNullOrEmpty(searchTextFieldValue))
						{
							displayedAttributeValues = displayedAttributeValues.Where(v => v.ToLowerInvariant().Contains(searchTextFieldValue)).ToList();
						}

						selectedValuesListView.itemsSource = displayedAttributeValues;
						selectedValuesListView.RefreshItems();
					}
				}
				else
				{
					selectedValuesListView.itemsSource = null;
				}

				if (selectedAttribute != null && GetActiveFilter()?.SolidFilterDefinition != null)
				{
					var enabledStatistic = GetActiveFilter().SolidFilterDefinition.EnabledStatistics.Find(enabledStatistic => enabledStatistic.FieldName == selectedAttribute.FieldName);

					var allSelected = !displayedAttributeValues.Any(v => !enabledStatistic.MostFrequentValues.Contains(v));
					selectAllValuesToggle.SetValueWithoutNotify(allSelected);
				}
			}
			else
			{
				selectedAttributesListView.itemsSource = null;
				selectedAttributesListView.RefreshItems();

				selectedValuesListView.itemsSource = null;
				selectedValuesListView.RefreshItems();

				selectAllValuesToggle.SetValueWithoutNotify(false);
			}

			whereClauseTextField.value = filterDefinition?.WhereClause ?? "";
		}

		private void UpdateTabContent()
		{
			selectedFilterFoldout.style.display = selectedTab > 0 ? DisplayStyle.Flex : DisplayStyle.None;
			newFilterTab.style.display = selectedTab == 0 ? DisplayStyle.Flex : DisplayStyle.None;
			filterAttributesTab.style.display = selectedTab == 1 && attributeStatistics != null ? DisplayStyle.Flex : DisplayStyle.None;
			filterDefinitionTab.style.display = selectedTab == 2 && attributeStatistics != null ? DisplayStyle.Flex : DisplayStyle.None;
			whereClauseTab.style.display = selectedTab == 3 ? DisplayStyle.Flex : DisplayStyle.None;
		}

		private static string GenerateWhereClause(ArcGISSolidBuildingFilterDefinitionInstanceData solidBuildingFilterDefinitionInstanceData)
		{
			var whereClause = string.Empty;

			foreach (var enabledStatistics in solidBuildingFilterDefinitionInstanceData.EnabledStatistics)
			{
				if (enabledStatistics.MostFrequentValues.Count == 0)
				{
					continue;
				}

				var values = string.Empty;

				foreach (var mostFrequentValue in enabledStatistics.MostFrequentValues)
				{
					if (values.Length > 0)
					{
						values += ", ";
					}

					values += $"'{mostFrequentValue}'";
				}

				if (whereClause.Length > 0)
				{
					whereClause += " and ";
				}

				whereClause += $"{enabledStatistics.FieldName} in ({values})";
			}

			return whereClause;
		}
	}
}
