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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISDropdownField<T> : VisualElement, INotifyValueChanged<T>
	{
		private DropdownField dropdownField;

		private List<T> options;
		public List<T> choices
		{
			get
			{
				return options;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				options = value;

				dropdownField.choices = options.Select((option, index) => index.ToString()).ToList();
			}
		}

		public Func<T, string> formatSelectedValueCallback;
		public Func<T, string> formatListItemCallback;

		public T value
		{
			get
			{
				return options.ElementAt(Convert.ToInt32(dropdownField.value));
			}
			set
			{
				var index = options.IndexOf(value);

				if (index.ToString() == dropdownField.value)
				{
					return;
				}

				var previousValue = options.ElementAt(Convert.ToInt32(dropdownField.value));

				using (var changeEvent = ChangeEvent<T>.GetPooled(previousValue, value))
				{
					changeEvent.target = this;

					SetValueWithoutNotify(value);
					SendEvent(changeEvent);
				}
			}
		}

		public ArcGISDropdownField()
		{
			options = new List<T>();

			dropdownField = new DropdownField(new List<string>() { "" }, "", (string value) =>
			{
				if (options.Count == 0 || value == "")
				{
					return "";
				}

				var item = options.ElementAt(Convert.ToInt32(value));

				if (formatSelectedValueCallback != null)
				{
					return formatSelectedValueCallback(item);
				}

				return item.ToString();
			}, (string value) =>
			{
				var item = options.ElementAt(Convert.ToInt32(value));

				if (formatListItemCallback != null)
				{
					return formatListItemCallback(item);
				}

				return item.ToString();
			});

			dropdownField.RegisterValueChangedCallback(evnt =>
			{
				var previousValue = options.ElementAt(Convert.ToInt32(dropdownField.value));
				var newValue = options.ElementAt(Convert.ToInt32(evnt.newValue));

				using (var changeEvent = ChangeEvent<T>.GetPooled(previousValue, newValue))
				{
					changeEvent.target = this;

					SetValueWithoutNotify(newValue);
					SendEvent(changeEvent);
				}
			});

			Add(dropdownField);
		}

		public void SetValueWithoutNotify(T newValue)
		{
			var newStringValue = "";

			if (newValue != null)
			{
				newStringValue = options.IndexOf(newValue).ToString();
			}

			dropdownField.SetValueWithoutNotify(newStringValue);
		}
	}
#if UNITY_6000_0_OR_NEWER
	[UxmlElement]
#endif
	public partial class ArcGISBuildingAttributeFilterDropdownField : ArcGISDropdownField<ArcGISBuildingAttributeFilterInstanceData>
	{
#if !UNITY_6000_0_OR_NEWER
		public new class UxmlFactory : UxmlFactory<ArcGISBuildingAttributeFilterDropdownField> { }
#endif
	}
}
