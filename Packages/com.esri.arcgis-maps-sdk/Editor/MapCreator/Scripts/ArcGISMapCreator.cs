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
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public abstract class ArcGISMapCreatorTool
	{
		public abstract VisualElement GetContent();
		public abstract Texture2D GetImage();
		public abstract string GetLabel();

		public virtual void OnDeselected() { }
		public virtual void OnDestroy() { }
		public virtual void OnEnable() { }
		public virtual void OnSelected(IArcGISMapComponentInterface mapComponentInterface) { }
	}

	public class ArcGISMapCreator : EditorWindow
	{
		private string activeTool;
		private string lastActiveTool;
		private bool hadEditorMapInLastSelection;
		private VisualElement activeToolContent;
		private VisualTreeAsset toolbarButtonTemplate;

		private Dictionary<string, Button> buttons = new();
		private Dictionary<string, ArcGISMapCreatorTool> tools = new();

		[MenuItem("Tools/ArcGIS Maps SDK/Map Creator", false, 1)]
		private static void CreateMapCreatorEditorWindow()
		{
			GetWindow<ArcGISMapCreator>().Show();
		}

		ArcGISMapCreator()
		{
			tools.Add("map-tool", new ArcGISMapCreatorMapTool());
			tools.Add("camera-tool", new ArcGISMapCreatorCameraTool());
			tools.Add("basemap-tool", new ArcGISMapCreatorBasemapTool());
			tools.Add("elevation-tool", new ArcGISMapCreatorElevationTool());
			tools.Add("layers-tool", new ArcGISMapCreatorLayerTool());
			tools.Add("auth-tool", new ArcGISMapCreatorAuthTool());
			tools.Add("settings-tool", new ArcGISMapCreatorSettingsTool());
			tools.Add("help-tool", new ArcGISMapCreatorHelpTool());
		}

		private void OnDisable()
		{
			EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChange;
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void OnEnable()
		{
			titleContent.text = "ArcGISMapsSDK";
			titleContent.image = MapCreatorUtilities.Assets.LoadImage("MapCreator/MapCreatorIcon.png");

			minSize = new Vector2(456, 100);

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			EditorSceneManager.activeSceneChangedInEditMode += OnSceneChange;

			foreach (var tool in tools)
			{
				tool.Value.OnEnable();
			}

			toolbarButtonTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/ToolbarButtonTemplate.uxml");

			hadEditorMapInLastSelection = HasEditorMap();
		}

		private void OnBecameVisible()
		{
			UpdateToolBarSelection();
		}

		private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
		{
			if (stateChange == PlayModeStateChange.ExitingEditMode)
			{
				lastActiveTool = activeTool;
			}
			else if (stateChange == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(lastActiveTool))
			{
				SetActiveTool(IsToolEnabled(lastActiveTool) ? lastActiveTool : "map-tool");
			}
		}

		private void OnSceneChange(Scene prev, Scene next)
		{
			if (activeTool == null || activeTool == "")
			{
				return;
			}

			SetActiveTool(IsToolEnabled(activeTool) ? activeTool : "map-tool");
		}

		private void OnSelectionChange()
		{
			UpdateToolBarSelection();
			hadEditorMapInLastSelection = HasEditorMap();
		}

		private void OnHierarchyChange()
		{
			UpdateToolbar();

			if (!IsToolEnabled(activeTool))
			{
				SetActiveTool("map-tool");
			}

			if (activeTool == "camera-tool")
			{
				tools[activeTool].OnSelected(MapCreatorUtilities.MapComponent);
			}
		}

		private VisualElement BuildButton(ArcGISMapCreatorTool tool)
		{
			var element = new VisualElement();

			toolbarButtonTemplate.CloneTree(element);

			var image = element.Q<Image>();
			var label = element.Q<Label>();

			image.image = tool.GetImage();
			label.text = tool.GetLabel();

			return element;
		}

		private VisualElement BuildToolbar()
		{
			var toolbarElement = new VisualElement();

			toolbarElement.AddToClassList("toolbar-box");

			foreach (var tool in tools)
			{
				var buttonElement = BuildButton(tool.Value);

				var button = buttonElement.Q<Button>();

				button.RegisterCallback<MouseUpEvent>(evnt =>
				{
					SetActiveTool(tool.Key);
				});

				buttons[tool.Key] = button;

				toolbarElement.Add(buttonElement);
			}

			return toolbarElement;
		}

		private void CreateGUI()
		{
			rootVisualElement.styleSheets.Add(MapCreatorUtilities.Assets.LoadStyleSheet("MapCreator/MapCreatorStyle.uss"));
			if (EditorGUIUtility.isProSkin)
			{
				rootVisualElement.AddToClassList("dark");
			}
			else
			{
				rootVisualElement.AddToClassList("light");
			}
			rootVisualElement.Add(BuildToolbar());

			UpdateToolbar();

			SetActiveTool(Application.isPlaying ? "help-tool" : "map-tool");
		}

		private bool IsToolEnabled(string tool)
		{
			var editorNoMap = tool == "map-tool" && !Application.isPlaying;
			var editorMap = HasEditorMap();

			// Always enable the help tool, even in play mode.
			return editorNoMap || editorMap || tool == "help-tool";
		}

		private static bool HasEditorMap()
		{
			return MapCreatorUtilities.MapComponent != null && !Application.isPlaying;
		}

		internal ArcGISMapCreatorTool GetActiveTool()
		{
			return tools[activeTool];
		}

		internal Button GetButton(string tool)
		{
			if (!buttons.ContainsKey(tool))
			{
				return null;
			}

			return buttons[tool];
		}

		private void OnDestroy()
		{
			if (!string.IsNullOrEmpty(activeTool))
			{
				tools[activeTool].OnDestroy();
			}
		}

		private void SetActiveTool(string tool)
		{
			if (!string.IsNullOrEmpty(activeTool))
			{
				GetButton(activeTool)?.RemoveFromClassList("button-selected");

				if (activeToolContent != null && rootVisualElement.Contains(activeToolContent))
				{
					rootVisualElement.Remove(activeToolContent);
				}

				tools[activeTool].OnDeselected();
			}

			activeTool = tool;

			tools[activeTool].OnSelected(MapCreatorUtilities.MapComponent);
			activeToolContent = tools[activeTool].GetContent();

			GetButton(activeTool)?.AddToClassList("button-selected");

			if (!rootVisualElement.Contains(activeToolContent))
			{
				rootVisualElement.Add(activeToolContent);
			}
		}

		private void UpdateToolbar()
		{
			if (buttons.Count > 0)
			{
				foreach (var tool in tools)
				{
					var button = buttons[tool.Key];

					button.SetEnabled(IsToolEnabled(tool.Key));
				}
			}
		}

		private void UpdateToolBarSelection()
		{
			UpdateToolbar();
			UpdateToolBarActiveTool();
		}

		private void UpdateToolBarActiveTool()
		{
			if (hadEditorMapInLastSelection)
			{
				lastActiveTool = activeTool;
			}

			if (Selection.activeGameObject != null)
			{
				if (IsCameraComponentSelected() && IsToolEnabled("camera-tool"))
				{
					SetActiveTool("camera-tool");
				}
				else if (HasEditorMap())
				{
					SetActiveTool(string.IsNullOrEmpty(lastActiveTool) ? activeTool : lastActiveTool);
				}
				else
				{
					NoMapSelection();
				}
			}
			else if (!HasEditorMap())
			{
				NoMapSelection();
			}
		}

		private void NoMapSelection()
		{
			if (activeTool != "help-tool")
			{
				SetActiveTool("map-tool");
			}
		}

		public bool IsCameraComponentSelected()
		{
			var selectedCamera = MapCreatorUtilities.CameraFromActiveMapComponent;
			return selectedCamera && Selection.activeGameObject == selectedCamera.gameObject;
		}
	}
}
