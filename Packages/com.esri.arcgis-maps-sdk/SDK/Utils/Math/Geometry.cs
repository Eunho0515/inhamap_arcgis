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
using Esri.GameEngine.Geometry;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Utils.Math
{
	public static class Geometry
	{
		public static bool RayEllipsoidIntersection(ArcGISSpheroidData spheroidData, double3 rayOrig, double3 rayDir, double Altitude, out double t)
		{
			t = 0;
			var ellipsoidScaling = 1.0 / (1.0 - spheroidData.Flattening);

			// Scale the space of the ray in Y-axis to trait the ellipsoid as a Sphere
			var scaledRayOrig = new double3(rayOrig.x, rayOrig.y * ellipsoidScaling, rayOrig.z);
			var scaledRayDir = new double3(rayDir.x, rayDir.y * ellipsoidScaling, rayDir.z);

			var result = RaySphereIntersection(scaledRayOrig, scaledRayDir, double3.zero, spheroidData.MajorSemiAxis + Altitude, out var scaledLength);

			if (result)
			{
				// Back to non-scaled world coordinate and compute the real length between hitPoint and rayOrig
				var scaledHitPoint = scaledRayOrig + scaledLength * scaledRayDir;
				var hitPoint = new double3(scaledHitPoint.x, scaledHitPoint.y / ellipsoidScaling, scaledHitPoint.z);

				t = System.Math.Sign(scaledLength) * math.length(hitPoint - rayOrig);
			}

			return result;
		}

		private static bool RaySphereIntersection(double3 rayOrig, double3 rayDir, double3 sphereCenter, double sphereRadius, out double t)
		{
			var a = math.dot(rayDir, rayDir);
			var s0_r0 = rayOrig - sphereCenter;
			var b = 2.0 * math.dot(rayDir, s0_r0);
			var c = math.dot(s0_r0, s0_r0) - (sphereRadius * sphereRadius);

			if (b * b - 4.0 * a * c < 0.0)
			{
				t = 0;
				return false;
			}
			else
			{
				t = (-b - System.Math.Sqrt((b * b) - 4.0 * a * c)) / (2.0 * a);
				return true;
			}
		}

		public static bool RayPlaneIntersection(double3 rayOrig, double3 rayDir, double3 planePosition, double3 planeNormal, out double t)
		{
			var denominator = math.dot(rayDir, planeNormal);

			if (System.Math.Abs(denominator) > 0.00001)
			{
				t = math.dot(planePosition - rayOrig, planeNormal) / denominator;
				return true;
			}
			else
			{
				t = 0;
				return false;
			}
		}
	}
}
