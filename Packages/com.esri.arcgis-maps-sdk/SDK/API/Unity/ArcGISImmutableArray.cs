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
	/// <summary>
	/// Defines a immutable dynamic array.
	/// </summary>
	/// <since>1.0.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISImmutableArray<T>
	{
		#region Properties
		/// <summary>
		/// Determines the number of values in the array.
		/// </summary>
		/// <remarks>
		/// The number of values in the array. If an error occurs a 0 will be returned.
		/// </remarks>
		/// <since>1.0.0</since>
		public ulong Size
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_Array_getSize(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				return localResult.ToUInt64();
			}
		}
		#endregion // Properties

		#region Methods
		/// <summary>
		/// Get a value at a specific position.
		/// </summary>
		/// <remarks>
		/// Retrieves the value at the specified position.
		/// </remarks>
		/// <param name="position">The position which you want to get the value.</param>
		/// <returns>
		/// The value, <see cref="">Element</see>, at the position requested.
		/// </returns>
		/// <since>1.0.0</since>
		public T At(ulong position)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localPosition = new UIntPtr(position);

			var localResult = PInvoke.RT_Array_at(Handle, localPosition, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			Standard.ArcGISElement localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new Standard.ArcGISElement(localResult);
			}

			return Convert.FromArcGISElement<T>(localLocalResult);
		}

		/// <summary>
		/// Does the array contain the given value.
		/// </summary>
		/// <remarks>
		/// Does the array contain a specific value.
		/// </remarks>
		/// <param name="value">The value you want to find.</param>
		/// <returns>
		/// True if the value is in the array otherwise false.
		/// </returns>
		/// <since>1.0.0</since>
		public bool Contains(T value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localValue = Convert.ToArcGISElement(value);

			var localResult = PInvoke.RT_Array_contains(Handle, localValue.Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Creates a <see cref="Unity.ArcGISImmutableArrayBuilder<T>">ArcGISImmutableArrayBuilder<T></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="Unity.ArcGISImmutableArrayBuilder<T>">ArcGISImmutableArrayBuilder<T></see>
		/// </returns>
		/// <seealso cref="Unity.ArcGISImmutableArrayBuilder<T>">ArcGISImmutableArrayBuilder<T></seealso>
		/// <since>1.0.0</since>
		public static ArcGISImmutableArrayBuilder<T> CreateBuilder()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Array_createBuilder(Convert.ToArcGISElementType<T>(), errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			ArcGISImmutableArrayBuilder<T> localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new ArcGISImmutableArrayBuilder<T>(localResult);
			}

			return localLocalResult;
		}

		/// <summary>
		/// Returns true if the two arrays are equal, false otherwise.
		/// </summary>
		/// <param name="array2">The second array.</param>
		/// <returns>
		/// Returns true if the two arrays are equal, false otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public bool Equals(ArcGISImmutableArray<T> array2)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localArray2 = array2.Handle;

			var localResult = PInvoke.RT_Array_equals(Handle, localArray2, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Get the first value in the array.
		/// </summary>
		/// <returns>
		/// The value, <see cref="">Element</see>, at the position requested.
		/// </returns>
		/// <since>1.0.0</since>
		public T First()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Array_first(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			Standard.ArcGISElement localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new Standard.ArcGISElement(localResult);
			}

			return Convert.FromArcGISElement<T>(localLocalResult);
		}

		/// <summary>
		/// Retrieves the position of the given value in the array.
		/// </summary>
		/// <param name="value">The value you want to find.</param>
		/// <returns>
		/// The position of the value in the array, or the max value of size_t otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public ulong IndexOf(T value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localValue = Convert.ToArcGISElement(value);

			var localResult = PInvoke.RT_Array_indexOf(Handle, localValue.Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult.ToUInt64();
		}

		/// <summary>
		/// Determines if there are any values in the array.
		/// </summary>
		/// <remarks>
		/// Check if the array object has any values in it.
		/// </remarks>
		/// <returns>
		/// true if the  array object contains no values otherwise false. Will return true if an error occurs.
		/// </returns>
		/// <since>1.0.0</since>
		public bool IsEmpty()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Array_isEmpty(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Get the last value in the array.
		/// </summary>
		/// <returns>
		/// The value, <see cref="">Element</see>, at the position requested.
		/// </returns>
		/// <since>1.0.0</since>
		public T Last()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Array_last(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			Standard.ArcGISElement localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new Standard.ArcGISElement(localResult);
			}

			return Convert.FromArcGISElement<T>(localLocalResult);
		}
		#endregion // Methods

		#region Internal Members
		internal ArcGISImmutableArray(IntPtr handle) => Handle = handle;

		~ArcGISImmutableArray()
		{
			if (Handle != IntPtr.Zero)
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_Array_destroy(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		internal IntPtr Handle { get; set; }
		#endregion // Internal Members
	}

	internal static partial class PInvoke
	{
		#region P-Invoke Declarations
		[DllImport(Interop.Dll)]
		internal static extern UIntPtr RT_Array_getSize(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Array_at(IntPtr handle, UIntPtr position, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Array_contains(IntPtr handle, IntPtr value, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Array_createBuilder(Standard.ArcGISElementType valueType, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Array_equals(IntPtr handle, IntPtr array2, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Array_first(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern UIntPtr RT_Array_indexOf(IntPtr handle, IntPtr value, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Array_isEmpty(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Array_last(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Array_destroy(IntPtr handle, IntPtr errorHandle);
		#endregion // P-Invoke Declarations
	}
}
