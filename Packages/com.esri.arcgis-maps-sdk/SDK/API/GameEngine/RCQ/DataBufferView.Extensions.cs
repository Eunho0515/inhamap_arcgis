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
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Esri.GameEngine.RCQ
{
	internal static class DataBufferViewExtensions
	{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		static AtomicSafetyHandle atomicSafeHandle = AtomicSafetyHandle.Create();
#endif
		public static T[] ToArray<T>(this ArcGISDataBufferView dataBuffer)
		{
			var elementSize = Marshal.SizeOf(typeof(T));

			var length = dataBuffer.Size / (uint)elementSize;
			var array = new T[length];

			for (var i = 0; i < length; i++)
			{
				var unmanagedElement = new System.IntPtr(dataBuffer.Data.ToInt64() + (long)i * elementSize);
				array[i] = Marshal.PtrToStructure<T>(unmanagedElement);
			}

			return array;
		}

		public static NativeArray<T> ToNativeArray<T>(this ArcGISDataBufferView buffer) where T : struct
		{
			unsafe
			{
				var itemSize = Marshal.SizeOf(typeof(T));
				var itemCount = buffer.Size / itemSize;
				var nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(buffer.Data.ToPointer(), (int)itemCount, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
				NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref nativeArray, atomicSafeHandle);
#endif
				return nativeArray;
			}

		}
	}
}

