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
using Esri.GameEngine.Math;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils.Math
{
	public static class Matrix4x4Extensions
	{
		internal static Matrix4x4 ToMatrix4x4(this ArcGISMatrix44 arcMatrix)
		{
			// An ArcGISMatrix44 coming through RCQ is row-major while Unity's Matrix4x4 is expected to be column-major.

			var newMatrix = new Matrix4x4
			{
				m00 = arcMatrix.M00,
				m10 = arcMatrix.M01,
				m20 = arcMatrix.M02,
				m30 = arcMatrix.M03,
				m01 = arcMatrix.M10,
				m11 = arcMatrix.M11,
				m21 = arcMatrix.M12,
				m31 = arcMatrix.M13,
				m02 = arcMatrix.M20,
				m12 = arcMatrix.M21,
				m22 = arcMatrix.M22,
				m32 = arcMatrix.M23,
				m03 = arcMatrix.M30,
				m13 = arcMatrix.M31,
				m23 = arcMatrix.M32,
				m33 = arcMatrix.M33
			};

			return newMatrix;
		}
	}
}
