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
	public static class float3Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 ToDouble3(this float3 value)
		{
			return math.double3(value.x, value.y, value.z);
		}
	}
}

