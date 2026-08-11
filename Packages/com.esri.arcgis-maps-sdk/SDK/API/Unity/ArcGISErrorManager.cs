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
	internal struct ErrorInteropHelper
	{
		internal IntPtr Error;
	}

	internal struct ArcGISErrorHandler
	{
		/// The function pointer which will be called when an error occurs. ArcGISRuntimeEnvironmentErrorEvent
		internal GameEngine.ArcGISRuntimeEnvironmentErrorEventInternal Handler;

		/// This is a pointer to the error
		internal IntPtr UserData;
	}

	internal class ArcGISErrorManager
	{
		private static readonly GameEngine.ArcGISRuntimeEnvironmentErrorEventInternal handler = new(HandlerFunction);

		#region Methods
		public static IntPtr CreateHandler()
		{
			ErrorInteropHelper errorInteropHelper;

			errorInteropHelper.Error = IntPtr.Zero;

			var errorInteropHelperPtr = GCHandle.ToIntPtr(GCHandle.Alloc(errorInteropHelper, GCHandleType.Pinned));

			ArcGISErrorHandler errorHandler;

			errorHandler.Handler = handler;
			errorHandler.UserData = errorInteropHelperPtr;

			var errorHandlerPtr = Marshal.AllocHGlobal(Marshal.SizeOf(errorHandler));

			Marshal.StructureToPtr(errorHandler, errorHandlerPtr, false);

			return errorHandlerPtr;
		}

		[MonoPInvokeCallback(typeof(GameEngine.ArcGISRuntimeEnvironmentErrorEventInternal))]
		internal static void HandlerFunction(IntPtr userData, IntPtr error)
		{
			if (error == IntPtr.Zero)
			{
				return;
			}

			var errorInteropHelperHandle = GCHandle.FromIntPtr(userData);

			var errorInteropHelper = (ErrorInteropHelper)errorInteropHelperHandle.Target;

			errorInteropHelper.Error = error;

			errorInteropHelperHandle.Target = errorInteropHelper;
		}

		public static void CheckError(IntPtr errorHandlerPtr)
		{
			var errorHandler = Marshal.PtrToStructure<ArcGISErrorHandler>(errorHandlerPtr);

			var errorInteropHelperHandle = GCHandle.FromIntPtr(errorHandler.UserData);

			var errorInteropHelper = (ErrorInteropHelper)errorInteropHelperHandle.Target;

			var error = errorInteropHelper.Error;

			errorInteropHelperHandle.Free();

			Marshal.DestroyStructure<ArcGISErrorHandler>(errorHandlerPtr);

			Marshal.FreeHGlobal(errorHandlerPtr);

			if (error != IntPtr.Zero)
			{
				throw Convert.FromArcGISError(new Standard.ArcGISError(error));
			}
		}
		#endregion // Methods
	}
}
