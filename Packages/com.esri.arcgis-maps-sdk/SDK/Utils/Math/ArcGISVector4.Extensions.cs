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

namespace Esri.ArcGISMapsSDK.SDK.Utils.Math
{
	static class ArcGISVector4Extensions
	{
		public static Vector4 AsVector4(this ArcGISVector4 vector)
		{
			return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
		}
	}
}
