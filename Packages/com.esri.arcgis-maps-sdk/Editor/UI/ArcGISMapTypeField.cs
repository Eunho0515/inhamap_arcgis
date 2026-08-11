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
using Esri.GameEngine.Map;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
#if UNITY_6000_0_OR_NEWER
	[UxmlElement]
#endif
	public partial class ArcGISMapTypeField : BindableElement, INotifyValueChanged<int>
	{
		private Button globalButton;
		private Button localButton;

		int currentValue;
		public int value
		{
			get
			{
				return currentValue;
			}
			set
			{
				if (EqualityComparer<int>.Default.Equals(currentValue, value))
				{
					return;
				}

				var previousValue = currentValue;

				SetValueWithoutNotify(value);

				using (var evt = ChangeEvent<int>.GetPooled(previousValue, currentValue))
				{
					evt.target = this;
					SendEvent(evt);
				}
			}
		}
#if !UNITY_6000_0_OR_NEWER
		public new class UxmlFactory : UxmlFactory<ArcGISMapTypeField, UxmlTraits> { }
#endif

		public ArcGISMapTypeField() : base()
		{
			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("UI/ArcGISMapTypeFieldTemplate.uxml");

			template.CloneTree(this);

			globalButton = this.Q<Button>(name: "global-button");
			globalButton.clickable.clickedWithEventInfo += delegate (EventBase evt)
			{
				value = (int)ArcGISMapType.Global;
			};

			localButton = this.Q<Button>(name: "local-button");
			localButton.clickable.clickedWithEventInfo += delegate (EventBase evt)
			{
				value = (int)ArcGISMapType.Local;
			};

			UpdateButtons();
		}

		public void SetValueWithoutNotify(int newValue)
		{
			if (newValue != (int)ArcGISMapType.Global && newValue != (int)ArcGISMapType.Local)
			{
				throw new ArgumentNullException("newValue");
			}

			currentValue = newValue;

			UpdateButtons();
		}

		private void UpdateButtons()
		{
			globalButton.SetEnabled(value != (int)ArcGISMapType.Global);
			localButton.SetEnabled(value != (int)ArcGISMapType.Local);
		}
	}
}
