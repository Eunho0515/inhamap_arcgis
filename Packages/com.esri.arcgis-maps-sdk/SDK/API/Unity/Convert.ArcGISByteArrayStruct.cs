// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com

using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static NativeArray<byte> FromArcGISByteArrayStruct(Standard.ArcGISIntermediateByteArrayStruct value)
		{
			unsafe
			{

				var nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(value.Data.ToPointer(), (int)value.Size, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
				NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref nativeArray, AtomicSafetyHandle.Create());
#endif
				return nativeArray;
			}
		}

		internal static NativeArray<byte> FromReadOnlyNativeArray(NativeArray<byte>.ReadOnly value)
		{
			unsafe
			{
				var pointer = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(value);

				var nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(pointer, value.Length, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
				NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref nativeArray, AtomicSafetyHandle.Create());
#endif
				return nativeArray;
			}
		}

		internal static Standard.ArcGISIntermediateByteArrayStruct ToArcGISByteArrayStruct(NativeArray<byte> value)
		{
			Standard.ArcGISIntermediateByteArrayStruct byteArrayStruct;

			unsafe
			{
				byteArrayStruct.Data = (System.IntPtr)NativeArrayUnsafeUtility.GetUnsafePtr(value);
				byteArrayStruct.Size = (ulong)value.Length;
			}

			return byteArrayStruct;
		}
	}
}
