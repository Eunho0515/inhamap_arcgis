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

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static Exception FromArcGISError(Standard.ArcGISError error)
		{
			Exception exception = null;

			if (error.Handle != IntPtr.Zero)
			{
				var additionalMessage = error.AdditionalMessage;

				switch (error.Code)
				{
					case (int)Standard.ArcGISErrorType.CommonNullPtr:
						exception = new ArgumentNullException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonInvalidArgument:
						exception = new ArgumentException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonNotImplemented:
						exception = new NotImplementedException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonOutOfRange:
						exception = new ArgumentOutOfRangeException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonTimeout:
						exception = new TimeoutException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonFile:
						exception = new System.IO.FileLoadException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonFileNotFound:
						exception = new System.IO.FileNotFoundException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonInvalidCall:
						exception = new InvalidOperationException(additionalMessage);
						break;
					case (int)Standard.ArcGISErrorType.CommonIO:
						exception = new System.IO.IOException(additionalMessage);
						break;
					default:
						exception = new Exception(error.Message + (string.IsNullOrWhiteSpace(additionalMessage) ? string.Empty : (": " + additionalMessage)));
						break;
				}
			}

			return exception;
		}
	}
}
