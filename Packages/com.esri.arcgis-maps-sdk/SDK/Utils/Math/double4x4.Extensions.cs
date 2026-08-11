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
	public static class Double4x4Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond ToQuaterniond(this double4x4 matrix)
		{
			return new quaterniond(matrix);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 TransformPoint(this double4x4 matrix, double3 point)
		{
			var result = matrix.c0 * point.x + matrix.c1 * point.y + matrix.c2 * point.z + matrix.c3;

			return new double3(result.x, result.y, result.z);
		}
	}
}
