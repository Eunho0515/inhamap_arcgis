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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils.Math
{
	[BurstCompile(CompileSynchronously = true)]
	public static class quaterniondExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 ToEulerZXY(this quaterniond quaternion)
		{
			var q = quaternion.value;

			var sqx = q.x * q.x;
			var sqw = q.w * q.w;
			var sqy = q.y * q.y;
			var sqz = q.z * q.z;
			var unit = sqx + sqy + sqz + sqw; // if normalized is one, otherwise is correction factor
			var test = q.x * q.w - q.y * q.z;

			double3 v;

			if (test > 0.4995 * unit) // singularity at north pole
			{
				v.y = 2 * System.Math.Atan2(q.y, q.x);
				v.x = -System.Math.PI / 2;
				v.z = 0;
			}
			else if (test < -0.4995 * unit) // singularity at south pole
			{
				v.y = -2 * System.Math.Atan2(q.y, q.x);
				v.x = System.Math.PI / 2;
				v.z = 0;
			}
			else
			{
				// Quaternion to Left-Handled rotation matrix
				// Given a quaternion q = qw + qx * i + qy * j + qz * k, we can create the following rotation matrix
				// Q = {{1 - 2*(qy*qy + qz*qz), 2*(qx*qy - qz*qw),		2*(qx*qz + qy*qw)},
				//		{2*(qx*qy + qz*qw),		1 - 2*(qx*qx + qz*qz),	2*(qy*qz - qx*qw)},
				//		{2*(qx*qz - qy*qw),		2*(qy*qz + qx*qw),		1 - 2*(qx*qx + qy*qy)}}

				// The following expresion is come from euler rotation matrix YXZ used by Unity and the above matrix.

				v.x = System.Math.Asin(-2 * (q.w * q.x - q.y * q.z));                                       // Pitch
				v.y = System.Math.Atan2(2 * q.w * q.y + 2 * q.z * q.x, 1 - 2 * (q.x * q.x + q.y * q.y));    // Heading
				v.z = System.Math.Atan2(2 * q.w * q.z + 2 * q.x * q.y, 1 - 2 * (q.z * q.z + q.x * q.x));    // Roll
			}

			return math.radians(MathUtils.NormalizeAngleDegrees(math.degrees(v)));
		}

		public static Quaternion ToQuaternion(this quaterniond value)
		{
			return new Quaternion((float)value.value.x, (float)value.value.y, (float)value.value.z, (float)value.value.w);
		}
	}
}
