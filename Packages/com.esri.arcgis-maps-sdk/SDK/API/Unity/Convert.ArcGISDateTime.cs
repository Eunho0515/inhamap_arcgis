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
		private static DateTime s_epochUTC = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		private static readonly double s_minMilliseconds = (DateTimeOffset.MinValue - new DateTimeOffset(s_epochUTC)).TotalMilliseconds;
		private static readonly double s_maxMilliseconds = (DateTimeOffset.MaxValue - new DateTimeOffset(s_epochUTC)).TotalMilliseconds;

		public static DateTime ToDateTime(long unixMilliseconds)
		{
			// Core dates are expressed as milliseconds-from-Unix-Epoch.
			// Values can potentially be as small as Int64.MinValue or as large as Int64.MaxValue.
			// These extremes exceed the range of values that .NET's DateTimeOffset can represent.
			// In such cases we will round dates up to MinValue, or down to MaxValue.
			if (unixMilliseconds < s_minMilliseconds)
			{
				return new DateTime(0, DateTimeKind.Utc);
			}
			else if (unixMilliseconds > s_maxMilliseconds)
			{
				return new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc);
			}
			else
			{
				var timespan = TimeSpan.FromMilliseconds(unixMilliseconds);
				return s_epochUTC.Add(timespan);
			}
		}

		internal static DateTimeOffset FromArcGISDateTime(IntPtr value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var milliseconds = RT_DateTime_toUnixMilliseconds(value, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			errorHandler = ArcGISErrorManager.CreateHandler();

			RT_DateTime_destroy(value, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return ToDateTime(milliseconds);
		}

		internal static ArcGISNativeHandle ToArcGISDateTime(DateTimeOffset value)
		{
			var milliseconds = value.ToUnixTimeMilliseconds();

			var errorHandler = ArcGISErrorManager.CreateHandler();

			var result = RT_DateTime_fromUnixMilliseconds(milliseconds, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return new ArcGISNativeHandle(result, (handle, destroyErrorHandler) => RT_DateTime_destroy(handle, destroyErrorHandler));
		}

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_DateTime_fromUnixMilliseconds(long time, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern long RT_DateTime_toUnixMilliseconds(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_DateTime_destroy(IntPtr handle, IntPtr errorHandle);
	}
}
