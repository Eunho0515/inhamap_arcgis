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
using UnityEditor;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	public sealed class PropertyClipboardManipulator : MouseManipulator
	{
		private readonly SerializedProperty serializedProperty;

		public PropertyClipboardManipulator(SerializedProperty serializedProperty)
		{
			this.serializedProperty = serializedProperty;

			activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.RightMouse,
			});
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<MouseUpEvent>(OnMouseUp);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
		}

		private void OnMouseUp(MouseUpEvent evt)
		{
			if (serializedProperty == null)
			{
				return;
			}

			if (!CanStartManipulation(evt))
			{
				return;
			}

			if (serializedProperty.ShowCopyPasteMenu())
			{
				evt.StopPropagation();
			}
		}
	}
}
