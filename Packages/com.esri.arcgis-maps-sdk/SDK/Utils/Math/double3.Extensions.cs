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
	public static class Double3Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsValid(this double3 vector)
		{
			return !math.isnan(vector.x) && !math.isnan(vector.y) && !math.isnan(vector.z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float3 ToFloat3(this double3 value)
		{
			float3 vector;

			vector.x = (float)value.x;
			vector.y = (float)value.y;
			vector.z = (float)value.z;

			return vector;
		}
	}
}

