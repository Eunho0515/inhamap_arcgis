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

using System;
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISDataBuffer<T> where T : struct
	{
		private GameEngine.ArcGISIntermediateDataBuffer<T> intermediateDataBuffer;

		internal IntPtr Handle
		{
			get
			{
				return intermediateDataBuffer.Handle;
			}
			set
			{
				intermediateDataBuffer.Handle = value;
			}
		}

		internal ulong ItemSize
		{
			get
			{
				return intermediateDataBuffer.ItemSize;
			}
		}

		internal ArcGISDataBuffer(IntPtr handle)
		{
			intermediateDataBuffer = new GameEngine.ArcGISIntermediateDataBuffer<T>(handle);
		}

		internal IntPtr Data
		{
			get
			{
				return intermediateDataBuffer.Data;
			}
		}

		public ulong SizeBytes
		{
			get
			{
				return intermediateDataBuffer.SizeBytes;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISDataBuffer<T> buffer &&
				   Handle.Equals(buffer.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		public static bool operator ==(ArcGISDataBuffer<T> left, ArcGISDataBuffer<T> right)
		{
			if (ReferenceEquals(left, right))
			{
				return true;
			}

			if (left is null || right is null)
			{
				return false;
			}

			return left.Equals(right);
		}

		public static bool operator !=(ArcGISDataBuffer<T> left, ArcGISDataBuffer<T> right)
		{
			return !(left == right);
		}
	}
}
