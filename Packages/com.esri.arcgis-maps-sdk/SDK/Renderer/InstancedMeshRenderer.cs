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
using System;
using Esri.ArcGISMapsSDK.Renderer.Renderables;
using Esri.HPFramework;
using UnityEngine;

[ExecuteAlways]
[Obsolete("InstancedMeshRenderer is deprecated. It will be removed in a future release.", true)]
public class InstancedMeshRenderer : MonoBehaviour
{
	private GraphicsBuffer commandBuffer;
	private GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
	private MaterialPropertyBlock materialPropertyBlock;

	private ComputeBuffer instanceBuffer;
	internal ComputeBuffer InstanceBuffer
	{
		get => instanceBuffer;
		set
		{
			instanceBuffer = value;

			materialPropertyBlock?.SetBuffer("_InstanceBuffer", instanceBuffer);

			if (commandData != null && commandBuffer != null)
			{
				commandData[0].instanceCount = (uint)(instanceBuffer?.count ?? 0);

				commandBuffer.SetData(commandData);
			}
		}
	}

	private Material material;
	public Material Material
	{
		get => material;
		set
		{
			material = value;
		}
	}

	private Mesh mesh;
	public Mesh Mesh
	{
		get => mesh;
		set
		{
			mesh = value;

			if (commandData != null && commandBuffer != null)
			{
				commandData[0].indexCountPerInstance = mesh?.GetIndexCount(0) ?? 0;

				commandBuffer.SetData(commandData);
			}
		}
	}

	private OrientedBoundingBox orientedBoundingBox;
	internal OrientedBoundingBox OrientedBoundingBox
	{
		get => orientedBoundingBox;
		set
		{
			orientedBoundingBox = value;
		}
	}

	private void Awake()
	{
		commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[1];

		commandBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandData.Length, GraphicsBuffer.IndirectDrawIndexedArgs.size);

		materialPropertyBlock = new MaterialPropertyBlock();

		if (instanceBuffer != null)
		{
			commandData[0].instanceCount = (uint)instanceBuffer.count;
			materialPropertyBlock?.SetBuffer("_InstanceBuffer", instanceBuffer);
		}

		if (mesh != null)
		{
			commandData[0].indexCountPerInstance = mesh.GetIndexCount(0);
		}

		commandBuffer.SetData(commandData);
	}

	private void OnDestroy()
	{
		commandBuffer?.Release();
	}

	private void LateUpdate()
	{
		if (instanceBuffer == null || material == null || mesh == null)
		{
			return;
		}

		materialPropertyBlock.SetMatrix("_LocalToWorld", gameObject.transform.localToWorldMatrix);
		materialPropertyBlock.SetMatrix("_WorldToLocal", gameObject.transform.worldToLocalMatrix);

		var renderParams = new RenderParams(material)
		{
			matProps = materialPropertyBlock,
			worldBounds = new Bounds(transform.position, orientedBoundingBox.Extent.ToVector3() * 2)
		};

		Graphics.RenderMeshIndirect(renderParams, mesh, commandBuffer, commandData.Length);
	}
}
