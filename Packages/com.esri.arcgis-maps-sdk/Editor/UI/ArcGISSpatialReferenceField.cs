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
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISSpatialReferenceField : BaseField<ArcGISSpatialReferenceInstanceData>
	{
		public static readonly string fieldUssClassName = "arcgis-spatial-reference-field";

		private readonly IntegerField horizontalField;
		private readonly IntegerField verticalField;
		private bool suppressChildValueUpdates;

		public ArcGISSpatialReferenceField() : base(null, new VisualElement())
		{
			AddToClassList(fieldUssClassName);
			styleSheets.Add(MapCreatorUtilities.Assets.LoadStyleSheet("UI/ArcGISSpatialReferenceFieldStyle.uss"));

			var content = this.Q<VisualElement>(className: inputUssClassName);

			horizontalField = new IntegerField("Horizontal")
			{
				name = "wkid",
				isDelayed = true,
			};

			verticalField = new IntegerField("Vertical")
			{
				name = "verticalWKID",
				isDelayed = true,
			};

			horizontalField.RegisterValueChangedCallback(_ => UpdateValueFromInputs());
			verticalField.RegisterValueChangedCallback(_ => UpdateValueFromInputs());

			content.Add(horizontalField);
			content.Add(verticalField);

			SetValueWithoutNotify(new ArcGISSpatialReferenceInstanceData());
		}

		public void BindProperty(SerializedProperty property)
		{
			var wkidProperty = property.FindPropertyRelative("WKID");
			var verticalWkidProperty = property.FindPropertyRelative("VerticalWKID");

			if (wkidProperty == null || verticalWkidProperty == null)
			{
				return;
			}

			SetValueWithoutNotify(new ArcGISSpatialReferenceInstanceData
			{
				WKID = wkidProperty.intValue,
				VerticalWKID = verticalWkidProperty.intValue,
			});

			horizontalField.BindProperty(wkidProperty);
			verticalField.BindProperty(verticalWkidProperty);
		}

		public override void SetValueWithoutNotify(ArcGISSpatialReferenceInstanceData newValue)
		{
			var normalizedValue = newValue == null
				? new ArcGISSpatialReferenceInstanceData()
				: (ArcGISSpatialReferenceInstanceData)newValue.Clone();

			base.SetValueWithoutNotify(normalizedValue);

			suppressChildValueUpdates = true;
			horizontalField.SetValueWithoutNotify(normalizedValue.WKID);
			verticalField.SetValueWithoutNotify(normalizedValue.VerticalWKID);
			suppressChildValueUpdates = false;
		}

		private void UpdateValueFromInputs()
		{
			if (suppressChildValueUpdates)
			{
				return;
			}

			value = new ArcGISSpatialReferenceInstanceData
			{
				WKID = horizontalField.value,
				VerticalWKID = verticalField.value,
			};
		}
	}
}
