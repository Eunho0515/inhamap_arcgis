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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Memory
{
	public class DefaultMemorySystemHandler : IMemorySystemHandler
	{
		protected IMemorySystem memorySystem;

		public DefaultMemorySystemHandler()
		{
			Application.lowMemory += OnLowMemoryCallback;
		}

		public MemoryQuotas GetMemoryQuotas()
		{
			return new MemoryQuotas
			{
#if UNITY_STANDALONE || UNITY_EDITOR
				SystemMemory = (long)Mathf.Max(500, SystemInfo.systemMemorySize * 0.8f),
				VideoMemory = (long)Mathf.Max(500, SystemInfo.graphicsMemorySize * 0.8f),
#else
				SystemMemory = (long)Mathf.Max(500, SystemInfo.systemMemorySize * 0.4f)
#endif
			};
		}

		public void Initialize(IMemorySystem memorySystem)
		{
			this.memorySystem = memorySystem;
		}

		private void OnLowMemoryCallback()
		{
			memorySystem?.NotifyLowMemoryWarning();
		}
	}
}
