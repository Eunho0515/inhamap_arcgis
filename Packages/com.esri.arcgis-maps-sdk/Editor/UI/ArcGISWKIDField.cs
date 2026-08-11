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
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
#if UNITY_6000_0_OR_NEWER
	[UxmlElement]
#endif
	public partial class ArcGISWKIDField : BindableElement, INotifyValueChanged<int>
	{
		private TextField textField;
#if !UNITY_6000_0_OR_NEWER
		public new class UxmlFactory : UxmlFactory<ArcGISWKIDField, UxmlTraits> { }

		public new class UxmlTraits : BindableElement.UxmlTraits { }
#endif
		private int value_;
		public int value
		{
			get
			{
				return value_;
			}
			set
			{
				if (value == this.value)
				{
					return;
				}

				var previous = this.value;

				SetValueWithoutNotify(value);

				using (var evt = ChangeEvent<int>.GetPooled(previous, value))
				{
					evt.target = this;
					SendEvent(evt);
				}
			}
		}

		private string ConvertWKIDToString(int wkid)
		{
			if (wkid == 0)
			{
				return "<None>";
			}
			else if (wkid == SpatialReferenceWkid.WGS84)
			{
				return "WGS 84 (4326)";
			}
			else if (wkid == SpatialReferenceWkid.WebMercator)
			{
				return "Web Mercator (3857)";
			}
			else if (wkid == SpatialReferenceVerticalWkid.EGM1996)
			{
				return "EGM 96 (5773)";
			}
			else if (wkid == SpatialReferenceVerticalWkid.WGS84)
			{
				return "WGS 84 (115700)";
			}

			return wkid.ToString();
		}

		public ArcGISWKIDField() : base()
		{
			textField = new TextField()
			{
				isDelayed = true
			};

			textField.RegisterCallback<FocusInEvent>(evnt =>
			{
				textField.SetValueWithoutNotify(value_.ToString());
			});

			textField.RegisterCallback<FocusOutEvent>(evnt =>
			{
				if (int.TryParse(textField.text, out var intValue))
				{
					value = intValue;
				}

				textField.SetValueWithoutNotify(ConvertWKIDToString(value));
			});

			textField.RegisterCallback<KeyDownEvent>(evnt =>
			{
				if (evnt.keyCode != KeyCode.Return &&
					evnt.keyCode != KeyCode.KeypadEnter &&
					evnt.keyCode != KeyCode.Escape)
				{
					return;
				}

				evnt.StopImmediatePropagation();

				if (evnt.keyCode == KeyCode.Escape)
				{
					textField.value = value.ToString();
				}

				textField.schedule.Execute(() =>
				{
					textField.Blur();
				});
			});

			textField.SetValueWithoutNotify(ConvertWKIDToString(value_));

			Add(textField);
		}

		public void SetValueWithoutNotify(int newValue)
		{
			value_ = Mathf.Max(0, newValue);

			textField.SetValueWithoutNotify(ConvertWKIDToString(value_));
		}
	}
}
