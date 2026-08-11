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

namespace Esri.Unity
{
	internal static partial class Interop
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		public const string Dll = "runtimecore";
#elif UNITY_IOS && !UNITY_EDITOR
		public const string Dll = "__Internal";
#elif UNITY_VISIONOS && !UNITY_EDITOR
		public const string Dll = "__Internal";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
		public const string Dll = "libruntimecore.so";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		public const string Dll = "libruntimecore";
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA
		public const string Dll = "runtimecore.dll";
#endif
	}
}
