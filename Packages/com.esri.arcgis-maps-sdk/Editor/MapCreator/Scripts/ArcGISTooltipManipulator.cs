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
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISTooltipManipulator : Manipulator
	{
		private VisualElement element;
		private bool isMouseOverTarget = false;
		private bool isMouseOverTooltip = false;

		public ArcGISTooltipManipulator(VisualElement element)
		{
			this.element = element;

			element.RegisterCallback<MouseEnterEvent>(evnt =>
			{
				isMouseOverTooltip = true;
			});

			element.RegisterCallback<MouseLeaveEvent>(evnt =>
			{
				isMouseOverTooltip = false;

				target.schedule.Execute(() =>
				{
					if (!isMouseOverTooltip && !isMouseOverTarget)
					{
						element.style.visibility = Visibility.Hidden;
					}
				});
			});
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<MouseEnterEvent>(MouseIn);
			target.RegisterCallback<MouseOutEvent>(MouseOut);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<MouseEnterEvent>(MouseIn);
			target.UnregisterCallback<MouseOutEvent>(MouseOut);
		}

		private void MouseIn(MouseEnterEvent e)
		{
			var root = target.hierarchy.parent;

			while (root.hierarchy.parent != null && !root.name.Contains("rootVisualContainer"))
			{
				root = root.hierarchy.parent;
			}

			if (!root.Contains(element))
			{
				root.Add(element);
			}

			element.style.position = Position.Absolute;
			element.style.right = root.worldBound.width - target.worldBound.xMax;
			element.style.top = target.worldBound.yMax - root.worldBound.y;
			element.style.visibility = Visibility.Visible;

			element.BringToFront();

			isMouseOverTarget = true;
		}

		private void MouseOut(MouseOutEvent e)
		{
			isMouseOverTarget = false;

			target.schedule.Execute(() =>
			{
				if (!isMouseOverTooltip && !isMouseOverTarget)
				{
					element.style.visibility = Visibility.Hidden;
				}
			});
		}
	}
}
