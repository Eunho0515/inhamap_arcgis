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
using System;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
#if UNITY_6000_0_OR_NEWER
	[UxmlElement]
#endif
	public partial class ArcGISButtonField : BaseField<string>
	{
		public static readonly string fieldUssClassName = "arcgis-button-field";
		public static readonly string buttonUssClassName = "arcgis-button-field__button";

		private readonly Button button;

		public event Action Clicked;

		public Button Button => button;

#if !UNITY_6000_0_OR_NEWER
		public new class UxmlFactory : UxmlFactory<ArcGISButtonField, UxmlTraits> { }
#endif

		public ArcGISButtonField() : base(null, new VisualElement())
		{
			AddToClassList(alignedFieldUssClassName);
			AddToClassList(fieldUssClassName);
			styleSheets.Add(MapCreatorUtilities.Assets.LoadStyleSheet("UI/ArcGISButtonFieldStyle.uss"));

			var content = this.Q<VisualElement>(className: inputUssClassName);

			button = new Button(() => Clicked?.Invoke())
			{
				name = "button",
			};

			button.AddToClassList(buttonUssClassName);
			content.Add(button);

			SetValueWithoutNotify(string.Empty);
		}

		public override void SetValueWithoutNotify(string newValue)
		{
			base.SetValueWithoutNotify(newValue ?? string.Empty);
			button.text = value;
		}
	}
}
