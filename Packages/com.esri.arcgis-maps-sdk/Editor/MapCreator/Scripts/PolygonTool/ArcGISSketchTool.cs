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
using Esri.ArcGISMapsSDK.SDK.Utils;
using Esri.ArcGISMapsSDK.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor
{
	public class ArcGISSketchTool
	{
		internal enum Mode
		{
			Draw,
			Edit
		};

		private Button copyButton;
		private Button deleteButton;
		private Button drawButton;
		private Button editButton;
		private Button pasteButton;

		private IArcGISCoordinatesConverterInterface coordinatesConverter;
		private Mode mode = Mode.Edit;
		private VisualElement overlayContent;
		private List<ArcGISPolygonInstanceData> polygonsInstanceData;
		private ArcGISSketchToolCreate sketchToolCreate;
		private ArcGISSketchToolEdit sketchToolEdit;

		private const int Unselected = -1;
		private int selectedPolygon = Unselected;

		public static Color OrangeColor = new(1.0f, 0.4f, 0.0f);

		public event Action<ArcGISPolygonInstanceData> onPolygonAdded;
		public event Action<int, ArcGISPolygonInstanceData> onPolygonChanged;
		public event Action<int> onPolygonDeleted;
		public event Action<int> onSelectionChanged;
		public event Action onToolClosed;

		private Tool lastTool = Tool.None;

		public ArcGISSketchTool(Action<ArcGISPolygonInstanceData> onPolygonAdded, Action<int, ArcGISPolygonInstanceData> onPolygonChanged, Action<int> onPolygonDeleted, Action<int> onSelectionChanged, Action onToolClosed, IArcGISCoordinatesConverterInterface coordinatesConverter)
		{
			this.onPolygonAdded = onPolygonAdded;
			this.onPolygonChanged = onPolygonChanged;
			this.onPolygonDeleted = onPolygonDeleted;
			this.onSelectionChanged = onSelectionChanged;
			this.onToolClosed = onToolClosed;

			sketchToolCreate = new ArcGISSketchToolCreate(HandleOnPolygonCreated);
			sketchToolEdit = new ArcGISSketchToolEdit(HandleOnPolygonChanged, HandleOnSelectionChanged);

			this.coordinatesConverter = coordinatesConverter;

			var overlayTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/PolygonTool/ArcGISSketchToolTemplate.uxml");

			overlayContent = new VisualElement
			{
				name = "ArcGISSketchTool",
				pickingMode = PickingMode.Ignore
			};

			overlayTemplate.CloneTree(overlayContent);

			drawButton = overlayContent.Q<Button>("draw");
			editButton = overlayContent.Q<Button>("edit");
			copyButton = overlayContent.Q<Button>("copy");
			pasteButton = overlayContent.Q<Button>("paste");
			deleteButton = overlayContent.Q<Button>("delete");
			var closeButton = overlayContent.Q<Button>("close");

			drawButton.clicked += () =>
			{
				ChangeMode(Mode.Draw);
			};

			editButton.clicked += () =>
			{
				ChangeMode(Mode.Edit);
			};

			copyButton.clicked += () =>
			{
				HandleCopyButtonClicked();
			};

			pasteButton.clicked += () =>
			{
				HandlePasteButtonClicked();
			};

			deleteButton.clicked += () =>
			{
				HandleDeleteClicked();
			};

			closeButton.clicked += () =>
			{
				HandleClosedClicked();
			};

			coordinatesConverter.OnRootChanged += () =>
			{
				UpdateWorldPolygons();
			};
		}

		public void OnEnable(List<ArcGISPolygonInstanceData> polygonsInstanceData)
		{
			this.polygonsInstanceData = polygonsInstanceData.Clone();

			ArcGISProjectSettingsAsset.Instance.ForceCollisionEnabledInEditorWorld = true;

			selectedPolygon = Unselected;

			UpdateWorldPolygons();

			ChangeMode(Mode.Edit);

			lastTool = Tools.current;
			Tools.current = Tool.None;

			SceneView.duringSceneGui += OnSceneGui;
			Selection.selectionChanged += OnGameObjectSelectionChanged;
		}

		public void OnDisable()
		{
			Selection.selectionChanged -= OnGameObjectSelectionChanged;
			SceneView.duringSceneGui -= OnSceneGui;

			Tools.current = lastTool;

			ChangeMode(Mode.Edit);

			ArcGISProjectSettingsAsset.Instance.ForceCollisionEnabledInEditorWorld = false;
		}

		private void ChangeMode(Mode mode)
		{
			this.mode = mode;

			drawButton.RemoveFromClassList("selected");
			editButton.RemoveFromClassList("selected");

			switch (mode)
			{
				case Mode.Draw:
					drawButton.AddToClassList("selected");
					sketchToolCreate.Reset();
					break;
				case Mode.Edit:
					editButton.AddToClassList("selected");
					sketchToolEdit.Reset();
					break;
			}
		}

		private void HandleCopyButtonClicked()
		{
			var polygonInstanceData = polygonsInstanceData[selectedPolygon];

			var json = JsonUtility.ToJson(polygonInstanceData);
			GUIUtility.systemCopyBuffer = json;
		}

		private void HandlePasteButtonClicked()
		{
			var polygonInstanceData = TryParseClipboard();

			if (!polygonInstanceData)
			{
				return;
			}

			polygonsInstanceData.Add(polygonInstanceData);

			UpdateWorldPolygons();

			onPolygonAdded?.Invoke(polygonInstanceData);
		}

		public VisualElement GetOverlayContent()
		{
			return overlayContent;
		}

		private void HandleClosedClicked()
		{
			OnDisable();
			onToolClosed?.Invoke();
		}

		private void HandleDeleteClicked()
		{
			Assert.IsTrue(this.selectedPolygon != Unselected);
			Assert.IsTrue(mode == Mode.Edit);

			var selectedPolygon = this.selectedPolygon;

			polygonsInstanceData.RemoveAt(selectedPolygon);

			sketchToolEdit.DeletePolygon(selectedPolygon);

			onPolygonDeleted?.Invoke(selectedPolygon);
		}

		private void HandleOnSelectionChanged(int polygonSelected)
		{
			selectedPolygon = polygonSelected;

			onSelectionChanged?.Invoke(selectedPolygon);
		}

		private void OnGameObjectSelectionChanged()
		{
			if (Selection.activeGameObject != null)
			{
				sketchToolEdit.ClearSelection();
			}
		}

		private void OnSceneGui(SceneView sceneView)
		{
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

			if (Event.current.keyCode == KeyCode.Escape && Event.current.type == EventType.KeyUp)
			{
				if (mode == Mode.Draw || selectedPolygon != Unselected)
				{
					ChangeMode(Mode.Edit);
				}
				else
				{
					HandleClosedClicked();
				}

				Event.current.Use();
			}
			else if (Event.current.keyCode == KeyCode.Delete && selectedPolygon != Unselected)
			{
				HandleDeleteClicked();
				Event.current.Use();
			}
			else if (Event.current.control && Event.current.keyCode == KeyCode.C && Event.current.type == EventType.KeyUp)
			{
				if (mode == Mode.Edit && selectedPolygon != Unselected)
				{
					HandleCopyButtonClicked();
					Event.current.Use();
				}
			}
			else if (Event.current.control && Event.current.keyCode == KeyCode.V && Event.current.type == EventType.KeyUp)
			{
				if (mode == Mode.Edit && selectedPolygon == Unselected)
				{
					HandlePasteButtonClicked();
					Event.current.Use();
				}
			}

			if (mode == Mode.Draw)
			{
				sketchToolCreate.Update(sceneView);
				sketchToolCreate.Draw(sceneView);
			}
			else
			{
				sketchToolEdit.Update(sceneView);
				sketchToolEdit.Draw(sceneView);
			}

			UpdateButtonStates();
		}

		private void HandleOnPolygonCreated(List<Vector3> polygonVertices)
		{
			var polygonInstanceData = new ArcGISPolygonInstanceData();

			foreach (var vertex in polygonVertices)
			{
				polygonInstanceData.Points.Add(coordinatesConverter.EngineToGeographic(vertex).ToInstanceData());
			}

			polygonsInstanceData.Add(polygonInstanceData);

			UpdateWorldPolygons();

			ChangeMode(Mode.Edit);

			onPolygonAdded?.Invoke(polygonInstanceData);
		}

		private void HandleOnPolygonChanged(int polygonIndex, List<Vector3> polygonVertices)
		{
			Assert.IsTrue(selectedPolygon == polygonIndex);

			var polygonInstanceData = new ArcGISPolygonInstanceData();

			foreach (var vertex in polygonVertices)
			{
				polygonInstanceData.Points.Add(coordinatesConverter.EngineToGeographic(vertex).ToInstanceData());
			}

			polygonsInstanceData[polygonIndex] = polygonInstanceData;

			var polygons = new List<List<Vector3>>();
			foreach (var instanceData in polygonsInstanceData)
			{
				var polygon = new List<Vector3>();
				foreach (var vertex in instanceData.Points)
				{
					polygon.Add(coordinatesConverter.GeographicToEngine(vertex.FromInstanceData()));
				}

				polygons.Add(polygon);
			}

			onPolygonChanged?.Invoke(polygonIndex, polygonInstanceData);
		}

		private ArcGISPolygonInstanceData TryParseClipboard()
		{
			try
			{
				var polygonInstanceData = JsonUtility.FromJson<ArcGISPolygonInstanceData>(GUIUtility.systemCopyBuffer);

				if (polygonInstanceData != null && polygonInstanceData.Points.Count >= 3)
				{
					return polygonInstanceData;
				}
			}
			catch
			{
			}

			return null;
		}

		private void UpdateButtonStates()
		{
			copyButton.SetEnabled(selectedPolygon != Unselected);
			deleteButton.SetEnabled(selectedPolygon != Unselected);
			pasteButton.SetEnabled(mode == Mode.Edit && TryParseClipboard() != null);
		}

		private void UpdateWorldPolygons()
		{
			if (polygonsInstanceData == null)
			{
				sketchToolEdit.SetPolygons(null);

				return;
			}

			var polygons = new List<List<Vector3>>();

			foreach (var polygonInstanceData in polygonsInstanceData)
			{
				var polygon = new List<Vector3>();

				foreach (var vertex in polygonInstanceData.Points)
				{
					polygon.Add(coordinatesConverter.GeographicToEngine(vertex.FromInstanceData()));
				}

				polygons.Add(polygon);
			}

			sketchToolEdit.SetPolygons(polygons);
		}
	}
}
