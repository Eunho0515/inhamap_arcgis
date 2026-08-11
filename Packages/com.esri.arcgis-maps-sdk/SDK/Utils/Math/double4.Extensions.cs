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
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Utils.Math
{
	[BurstCompile(CompileSynchronously = true)]
	public static class Double4Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static quaternion AsQuaternion(this double4 value)
		{
			return new quaternion((float)value.x, (float)value.y, (float)value.z, (float)value.w);
		}
	}
}

