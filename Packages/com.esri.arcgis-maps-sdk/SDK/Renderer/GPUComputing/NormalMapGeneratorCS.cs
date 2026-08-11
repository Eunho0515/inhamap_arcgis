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
using Esri.ArcGISMapsSDK.Renderer.GPUResources;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.View;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUComputing
{
	internal class NormalMapGeneratorCS : INormalMapGenerator
	{
		private readonly ComputeShader globalViewModeShader = null;
		private readonly ComputeShader localViewModeShader = null;
		private readonly ArcGISView view = null;

		public NormalMapGeneratorCS(ArcGISView view)
		{
			globalViewModeShader = Resources.Load<ComputeShader>("Shaders/Utils/CS/GlobalViewModeComputeNormalsSimple");
			localViewModeShader = Resources.Load<ComputeShader>("Shaders/Utils/CS/LocalViewModeComputeNormals");
			this.view = view;
		}

		// Tile extent - Min point and size of a tile in geo/proj coordinates in view SR
		// Texture extent - x,y is min point and z,w is size of the rectangle patch covering
		// the input elevation texture. For example, top right would be vec4(0.5)
		public void Compute(IGPUResourceTexture2D inputElevation, Vector4 tileExtent, Vector4 textureExtent, IGPUResourceRenderTexture output)
		{
			var sr = view.SpatialReference;
			if (sr.IsGeographic)
			{
				var globeRadius = sr.SpheroidData.MajorSemiAxis;
				var circleLongitude = 2.0 * System.Math.PI * globeRadius;
				var minLatitude = tileExtent.y * MathUtils.DegreesToRadians;
				var latitudeAngleDelta = tileExtent.w / output.Height * MathUtils.DegreesToRadians;
				var longitudeArc = System.Math.Abs(tileExtent.z) / 360.0;

				var result = ArcGISGeometryEngine.DistanceGeodetic(
					new ArcGISPoint(tileExtent.x, tileExtent.y, sr),
					new ArcGISPoint(tileExtent.x, tileExtent.y + tileExtent.w, sr),
					null,
					null,
					ArcGISGeodeticCurveType.Geodesic);

				// Assumption - this is always positive, since it's named distance
				var latitudeLength = result.Distance;

				var kernelHandle = globalViewModeShader.FindKernel("CSMain");

				globalViewModeShader.GetKernelThreadGroupSizes(kernelHandle, out var x, out var y, out _);

				globalViewModeShader.SetTexture(kernelHandle, "Output", output.NativeRenderTexture);
				globalViewModeShader.SetTexture(kernelHandle, "Input", inputElevation.NativeTexture);
				globalViewModeShader.SetFloat("MinLatitude", (float)minLatitude);
				globalViewModeShader.SetFloat("LatitudeAngleDelta", (float)latitudeAngleDelta);
				globalViewModeShader.SetFloat("LongitudeArc", (float)longitudeArc);
				globalViewModeShader.SetFloat("LatitudeLength", (float)latitudeLength);
				globalViewModeShader.SetFloat("CircleLongitude", (float)circleLongitude);
				globalViewModeShader.SetFloat("EarthRadius", (float)globeRadius);
				globalViewModeShader.SetVector("InputOffsetAndScale", new Vector4(textureExtent.x, textureExtent.y, textureExtent.z, textureExtent.w));

				globalViewModeShader.Dispatch(kernelHandle, (int)System.Math.Ceiling(output.Width / (float)x), (int)System.Math.Ceiling(output.Height / (float)y), 1);
			}
			else
			{
				// Projected view SR
				var kernelHandle = localViewModeShader.FindKernel("CSMain");
				localViewModeShader.GetKernelThreadGroupSizes(kernelHandle, out var x, out var y, out _);

				localViewModeShader.SetTexture(kernelHandle, "Output", output.NativeRenderTexture);
				localViewModeShader.SetTexture(kernelHandle, "Input", inputElevation.NativeTexture);
				localViewModeShader.SetFloat("LatitudeLength", tileExtent.w);
				localViewModeShader.SetFloat("LongitudeLength", tileExtent.z);
				localViewModeShader.SetVector("InputOffsetAndScale", new Vector4(textureExtent.x, textureExtent.y, textureExtent.z, textureExtent.w));

				localViewModeShader.Dispatch(kernelHandle, (int)System.Math.Ceiling(output.Width / (float)x), (int)System.Math.Ceiling(output.Height / (float)y), 1);
			}
		}
	}
}
