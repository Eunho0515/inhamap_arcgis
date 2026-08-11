// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		// Keep material reference from being GC'd. Workaround for https://devtopia.esri.com/runtime/millennium-falcon/issues/1249
		private static Dictionary<IntPtr, UnityEngine.Material> keepAlive = new();

		internal static UnityEngine.Material FromArcGISMaterialReference(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				return null;
			}
			var gcHandle = GCHandle.FromIntPtr(value);
			return (UnityEngine.Material)gcHandle.Target;
		}

		internal static IntPtr ToArcGISMaterialReference(UnityEngine.Material value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			var ptr = GCHandle.ToIntPtr(GCHandle.Alloc(value));
			keepAlive[ptr] = value;
			return ptr;
		}
	}
}
