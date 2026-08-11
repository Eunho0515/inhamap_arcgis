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
		internal static Guid FromArcGISGUID(IntPtr interopGUID)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var coreString = RT_GUID_toString(interopGUID, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			var guidString = FromArcGISString(coreString);

			errorHandler = ArcGISErrorManager.CreateHandler();

			RT_GUID_destroy(interopGUID, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return Guid.Parse(guidString);
		}

		internal static ArcGISNativeHandle ToArcGISGUID(Guid value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var guidHandle = RT_GUID_createFromString(value.ToString(), errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return new ArcGISNativeHandle(guidHandle, (handle, destroyErrorHandler) => RT_GUID_destroy(handle, destroyErrorHandler));
		}

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_GUID_createFromString([MarshalAs(UnmanagedType.LPStr)] string value, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_GUID_destroy(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_GUID_toString(IntPtr handle, IntPtr errorHandler);
	}
}
