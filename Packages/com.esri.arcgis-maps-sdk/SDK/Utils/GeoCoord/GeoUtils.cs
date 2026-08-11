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
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Map;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils.GeoCoord
{
	public static class SpatialReferenceWkid
	{
		public const int WebMercator = 3857;
		public const int WGS84 = 4326;
		public const int CGCS2000 = 4490;
	}

	public static class SpatialReferenceVerticalWkid
	{
		public const int EGM1996 = 5773;
		public const int EGM2008 = 3855;
		public const int WGS84 = 115700;
	}

	public static class GeoUtils
	{
		// This matrix switches the Y and Z axes
		// We use it to convert between ECEF <-> Unity spaces
		// The inverse of this matrix is the same matrix so we can use the same one to
		// go from ECEF to Unity and from Unity to ECEF
		internal static double4x4 ECEFToUnityAxes = new
		(
			math.double4(1, 0, 0, 0),
			math.double4(0, 0, 1, 0),
			math.double4(0, 1, 0, 0),
			math.double4(0, 0, 0, 1)
		);

		// Approx Mt Everest on Earth
		// TODO: What about other planets?
		internal const double MaxTerrainAltitude = 9000.0;

		public static double4x4 GetENUReferenceMatrix(double3 cartesianPosition, ArcGISSpheroidData spheroidData)
		{
			double3 north;
			double3 east;
			var up = GetGeodeticSurfaceNormal(cartesianPosition, spheroidData);

			if (System.Math.Abs(cartesianPosition.x) < MathUtils.Epsilond && System.Math.Abs(cartesianPosition.y) < MathUtils.Epsilond)
			{
				var sign = System.Math.Sign(cartesianPosition.z);

				// At the center, it should never happen
				if (System.Math.Abs(cartesianPosition.z) < MathUtils.Epsilond)
				{
					// Return same as north pole
					sign = 1;
				}

				north = sign * math.forward();
				east = sign * math.right();
			}
			else
			{
				east = math.normalize(new double3(-cartesianPosition.y, cartesianPosition.x, 0));
				north = math.normalize(math.cross(up, east));
			}

			// X in ECEF is Right -> East
			// Y in ECEF is Forward -> North
			// Z in ECEF is Up -> Up
			return new double4x4
			(
				math.double4(east, 0),
				math.double4(north, 0),
				math.double4(up, 0),
				math.double4(cartesianPosition, 1)
			);
		}

		public static double3 GetGeodeticSurfaceNormal(double3 cartesianPosition, ArcGISSpheroidData spheroidData)
		{
			var normal = new double3(cartesianPosition.x, cartesianPosition.y, cartesianPosition.z / (1.0 - spheroidData.SquaredEccentricity));
			return math.normalize(normal);
		}

		public static ArcGISPoint ProjectToSpatialReference(ArcGISPoint position, ArcGISSpatialReference toSpatialRef)
		{
			if (position.SpatialReference.Equals(toSpatialRef))
			{
				return position;
			}

			try
			{
				var projected = ArcGISGeometryEngine.Project(position, toSpatialRef);

				var projectedPoint = projected as ArcGISPoint;

				return projectedPoint;
			}
			catch (System.Exception ex)
			{
				Debug.LogError("ProjectToSpatialReference EXCEPTION THROWN: " + position.SpatialReference.ToString() + " could not be projected to " + toSpatialRef.ToString());
				throw ex;
			}
		}

		public static ArcGISRotation FromCartesianRotation(double3 cartesianPosition, quaterniond rotation, ArcGISSpatialReference spatialReference, ArcGISMapType? mapType)
		{
			var tangentToWorld = GetENUReference(cartesianPosition, spatialReference, mapType);
			var worldToTangent = quaterniond.inverse(tangentToWorld.ToQuaterniond());

			var localRotation = quaterniond.mul(worldToTangent, rotation);

			var eulerAngles = math.degrees(localRotation.ToEulerZXY());

			var pitch = 90 + eulerAngles.x;
			var heading = eulerAngles.y;
			var roll = eulerAngles.z;

			return ArcGISRotation.Normalize(new ArcGISRotation(heading, pitch, roll));
		}

		public static quaterniond ToCartesianRotation(double3 cartesianPosition, ArcGISRotation rotation, ArcGISSpatialReference spatialReference, ArcGISMapType? mapType)
		{
			var eulerAngles = new double3
			{
				x = 90 - rotation.Pitch,
				y = rotation.Heading,
				z = rotation.Roll
			};

			var tangentToWorld = GetENUReference(cartesianPosition, spatialReference, mapType);

			return quaterniond.mul(tangentToWorld.ToQuaterniond(), quaterniond.EulerZXY(math.radians(eulerAngles)));
		}

		public static double4x4 GetENUReference(double3 cartesianPosition, ArcGISSpatialReference spatialReference, ArcGISMapType? mapType)
		{
			if (!spatialReference || mapType == ArcGISMapType.Local)
			{
				return double4x4.identity;
			}

			return math.mul(math.mul(ECEFToUnityAxes, GetENUReferenceMatrix(ECEFToUnityAxes.TransformPoint(cartesianPosition), spatialReference.SpheroidData)), ECEFToUnityAxes);
		}
	}
}
