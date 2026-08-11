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
using Esri.GameEngine.RCQ;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer
{
	class RenderCommandThrottler : IRenderCommandThrottler
	{
		public static readonly float MaxExecTimeMicroseconds = 0.005f;
		public static readonly ulong MaxResourceMemory = 8 * 1024 * 1024; // bytes
		public static readonly uint MaxComposeCommands = 100;
		public static readonly uint MaxTriangles = 25000;

		private struct Stats
		{
			public uint commandCount;
			public uint composeCount;
			public ulong resourceMemory; // bytes
			public uint triangles;
			public bool isExecutingGroup;
		}

		private Stats stats;
		private float startTime;

		public RenderCommandThrottler()
		{
			BeginFrame();
		}

		public void BeginFrame()
		{
			stats.commandCount = 0;
			stats.composeCount = 0;
			stats.resourceMemory = 0;
			stats.triangles = 0;
			stats.isExecutingGroup = false;
			startTime = Time.realtimeSinceStartup;
		}

		public bool DoThrottle(RenderCommand renderCommand)
		{
			// Update stats from selected commands
			++stats.commandCount;

			switch (renderCommand.Type)
			{
				case ArcGISRenderCommandType.MultipleCompose:
					{
						++stats.composeCount;
					}
					break;

				case ArcGISRenderCommandType.SetTexturePixelData:
					{
						var parameters = (ArcGISSetTexturePixelDataCommandParameters)renderCommand.CommandParameters;
						stats.resourceMemory += parameters.Pixels.Size;
					}
					break;

				case ArcGISRenderCommandType.SetMeshVertexBuffers:
					{
						var parameters = (ArcGISSetMeshVertexBuffersCommandParameters)renderCommand.CommandParameters;
						stats.resourceMemory += parameters.Colors.Size + parameters.FeatureIndices.Size + parameters.Normals.Size + parameters.Positions.Size + parameters.Tangents.Size +
																parameters.Triangles.Size + parameters.UvRegionIds.Size + parameters.Uvs.Size;
						stats.triangles = parameters.Triangles.Size / 12;
					}
					break;

				case ArcGISRenderCommandType.CommandGroupBegin:
					{
						stats.isExecutingGroup = true;
					}
					break;

				case ArcGISRenderCommandType.CommandGroupEnd:
					{
						stats.isExecutingGroup = false;
					}
					break;

				default:
					break;
			}

			var ElapsedTime = Time.realtimeSinceStartup - startTime;

			if (!stats.isExecutingGroup &&
				(stats.composeCount >= MaxComposeCommands || stats.resourceMemory >= MaxResourceMemory || stats.triangles >= MaxTriangles || ElapsedTime >= MaxExecTimeMicroseconds))
			{
				return true;
			}

			return false;
		}
	}
}
