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
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
#if UNITY_6000_0_OR_NEWER
	[UxmlElement]
#endif
	public partial class ArcGISEditableLabel : BindableElement, INotifyValueChanged<string>
	{
		private Label label;
		private TextField textField;

#if !UNITY_6000_0_OR_NEWER
		public new class UxmlFactory : UxmlFactory<ArcGISEditableLabel, UxmlTraits> { }

		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				var arcGISEditableLabel = (ArcGISEditableLabel)ve;

				arcGISEditableLabel.textField.bindingPath = arcGISEditableLabel.bindingPath;

				arcGISEditableLabel.bindingPath = null;
			}
		}
#endif

		public string value
		{
			get
			{
				return textField.value;
			}
			set
			{
				SetValueWithoutNotify(value);
			}
		}

		public ArcGISEditableLabel() : base()
		{
			textField = new TextField();
			label = new Label();

#if UNITY_6000_0_OR_NEWER
			RegisterCallback<AttachToPanelEvent>(_ => SyncBindingPath());
#endif

			label.RegisterCallback<MouseDownEvent>(evnt =>
			{
				evnt.StopImmediatePropagation();

				Edit();
			});

			textField.style.display = DisplayStyle.None;

			textField.RegisterCallback<ChangeEvent<string>>((evt) =>
			{
				label.text = ApplyEllipsisToValue(evt.newValue);
			});

			textField.RegisterCallback<FocusInEvent>(evnt =>
			{
				textField.SelectAll();
			});

			textField.RegisterCallback<FocusOutEvent>(evnt =>
			{
				ExitQuit();
			});

			textField.RegisterCallback<KeyDownEvent>(evnt =>
			{
				evnt.StopImmediatePropagation();

				if (evnt.keyCode != KeyCode.Return)
				{
					return;
				}

				ExitQuit();
			});

			style.flexGrow = 1;

			label.style.marginLeft = 6;
			label.style.paddingBottom = 1;
			label.style.paddingTop = 1;

			textField.style.marginBottom = 0;
			textField.style.marginTop = 0;

			textField.ElementAt(0).style.paddingTop = 0;

			Add(label);
			Add(textField);
		}

		private void SyncBindingPath()
		{
			if (textField == null || string.IsNullOrEmpty(bindingPath))
			{
				return;
			}

			textField.bindingPath = bindingPath;
			bindingPath = null;
		}

		private string ApplyEllipsisToValue(string value)
		{
			if (value.Length > 26)
			{
				return value.Substring(0, 26) + " ...";
			}

			return value;
		}

		public void Edit()
		{
			label.style.display = DisplayStyle.None;
			textField.style.display = DisplayStyle.Flex;
			textField.Focus();
		}

		public void ExitQuit()
		{
			label.style.display = DisplayStyle.Flex;
			textField.style.display = DisplayStyle.None;
		}

		public void SetValueWithoutNotify(string newValue)
		{
			textField.SetValueWithoutNotify(newValue);
			label.text = ApplyEllipsisToValue(newValue);
		}
	}
}
