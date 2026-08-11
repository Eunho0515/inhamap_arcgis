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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Memory
{
	public class AndroidDefaultMemorySystemHandler : DefaultMemorySystemHandler
	{
		private AndroidJavaObject activityManager;

		public new MemoryQuotas GetMemoryQuotas()
		{
			var systemMemory = GetMemoryQuotasFromActivityManager();

			if (!systemMemory.HasValue)
			{
				Debug.LogWarning("Unable to get memory quotas from the ActivityManager. Using SystemInfo.systemMemorySize * 0.4");

				systemMemory = (long)(SystemInfo.systemMemorySize * 0.4);
			}

			return new MemoryQuotas
			{
				SystemMemory = (long)Mathf.Max(500, systemMemory.Value)
			};
		}

		private long? GetMemoryQuotasFromActivityManager()
		{
			if (activityManager == null)
			{
				AndroidJavaObject currentActivity = null;
				var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

				try
				{
					// When running on desktop while set to build Android, this throws
					currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
				}
				catch (Exception)
				{
					// Swallow exception
				}

				if (currentActivity != null)
				{
					activityManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "activity");
				}
			}

			if (activityManager == null)
			{
				return null;
			}

			var memoryInfo = new AndroidJavaObject("android.app.ActivityManager$MemoryInfo");
			activityManager.Call("getMemoryInfo", memoryInfo);

			var availableMemory = memoryInfo.Get<long>("availMem");
			var lowMemoryThreshold = memoryInfo.Get<long>("threshold");

			// Treat the difference between available and threshold as "actually available" memory
			return (long)((availableMemory - lowMemoryThreshold) / 1024.0f / 1024.0f);
		}
	}
}
