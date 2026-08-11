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
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static string FromArcGISString(IntPtr interopString)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var stringPtr = RT_String_cStr(interopString, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			var result = Marshal.PtrToStringAnsi(stringPtr);

			errorHandler = ArcGISErrorManager.CreateHandler();

			RT_String_destroy(interopString, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return result;
		}

		[DllImport(Interop.Dll)]
		private static extern IntPtr RT_String_cStr(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		private static extern void RT_String_destroy(IntPtr handle, IntPtr errorHandler);
	}
}
