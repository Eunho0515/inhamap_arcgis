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
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISShapeDimensionsField : BaseField<double2>
	{
		public static readonly string fieldUssClassName = "arcgis-shape-dimensions-field";

		private readonly DoubleField xField;
		private readonly DoubleField yField;
		private bool suppressChildValueUpdates;

		public ArcGISShapeDimensionsField() : base(null, new VisualElement())
		{
			AddToClassList(alignedFieldUssClassName);
			AddToClassList(fieldUssClassName);
			styleSheets.Add(MapCreatorUtilities.Assets.LoadStyleSheet("UI/ArcGISShapeDimensionsFieldStyle.uss"));

			var content = this.Q<VisualElement>(className: inputUssClassName);

			xField = new DoubleField("X")
			{
				name = "x",
				isDelayed = true,
			};

			yField = new DoubleField("Y")
			{
				name = "y",
				isDelayed = true,
			};

			xField.RegisterValueChangedCallback(_ => UpdateValueFromInputs());
			yField.RegisterValueChangedCallback(_ => UpdateValueFromInputs());

			content.Add(xField);
			content.Add(yField);

			SetValueWithoutNotify(new double2());
			ApplyShape(MapExtentShapes.Rectangle);
		}

		public void BindProperty(SerializedProperty property)
		{
			var xProperty = property.FindPropertyRelative("x");
			var yProperty = property.FindPropertyRelative("y");
			var extentShapeProperty = FindSiblingProperty(property, "ExtentShape");

			if (xProperty == null || yProperty == null)
			{
				return;
			}

			SetValueWithoutNotify(new double2(xProperty.doubleValue, yProperty.doubleValue));

			xField.BindProperty(xProperty);
			yField.BindProperty(yProperty);

			if (extentShapeProperty != null)
			{
				ApplyShape((MapExtentShapes)extentShapeProperty.intValue);
				this.TrackPropertyValue(extentShapeProperty, changedProperty => ApplyShape((MapExtentShapes)changedProperty.intValue));
			}
		}

		public override void SetValueWithoutNotify(double2 newValue)
		{
			base.SetValueWithoutNotify(newValue);

			suppressChildValueUpdates = true;
			xField.SetValueWithoutNotify(newValue.x);
			yField.SetValueWithoutNotify(newValue.y);
			suppressChildValueUpdates = false;
		}

		private static SerializedProperty FindSiblingProperty(SerializedProperty property, string siblingName)
		{
			var propertyPath = property.propertyPath;
			var separatorIndex = propertyPath.LastIndexOf('.');

			if (separatorIndex < 0)
			{
				return property.serializedObject.FindProperty(siblingName);
			}

			var parentPath = propertyPath.Substring(0, separatorIndex);
			return property.serializedObject.FindProperty($"{parentPath}.{siblingName}");
		}

		private void ApplyShape(MapExtentShapes shape)
		{
			yField.label = "Y";

			switch (shape)
			{
				case MapExtentShapes.Square:
					xField.label = "Length";
					yField.style.display = DisplayStyle.None;
					break;
				case MapExtentShapes.Circle:
					xField.label = "Radius";
					yField.style.display = DisplayStyle.None;
					break;
				default:
					xField.label = "X";
					yField.style.display = DisplayStyle.Flex;
					break;
			}
		}

		private void UpdateValueFromInputs()
		{
			if (suppressChildValueUpdates)
			{
				return;
			}

			value = new double2(xField.value, yField.value);
		}
	}
}
