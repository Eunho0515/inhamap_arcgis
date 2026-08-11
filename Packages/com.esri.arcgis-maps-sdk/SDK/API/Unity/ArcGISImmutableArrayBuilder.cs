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
	/// Use to create and populate <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> collections.
	/// </summary>
	/// <remarks>
	/// The <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see> provides a mechanism for creating and populating <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> objects.
	/// Because <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> objects cannot be created or populate directly (they are immutable
	/// objects) the <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see> provides an efficient means to overcome this.
	///
	/// Use <see cref="Unity.ArcGISImmutableArray.CreateBuilder">ArcGISImmutableArray.CreateBuilder</see> to create a <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see>.
	/// </remarks>
	/// <seealso cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</seealso>
	/// <seealso cref="Unity.ArcGISMutableArray">ArcGISMutableArray</seealso>
	/// <since>1.0.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public partial class ArcGISImmutableArrayBuilder<T>
	{
		#region Methods
		/// <summary>
		/// Add a value to the <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see>.
		/// </summary>
		/// <param name="value">The value that is to be added.</param>
		/// <since>1.0.0</since>
		public void Add(T value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localValue = Convert.ToArcGISElement(value);

			PInvoke.RT_ArrayBuilder_add(Handle, localValue.Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);
		}

		/// <summary>
		/// Creates a <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> containing the values added to this <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see>.
		/// </summary>
		/// <remarks>
		/// The order of the values in the returned <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> matches the order in which the
		/// values were added to this <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see>.
		///
		/// This call empties this <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see> of values, but leaves it in a valid
		/// (re-usable) state.
		/// </remarks>
		/// <returns>
		/// A <see cref="Unity.ArcGISImmutableArray">ArcGISImmutableArray</see> containing the values added to this <see cref="Unity.ArcGISImmutableArrayBuilder">ArcGISImmutableArrayBuilder</see>.
		/// </returns>
		/// <since>1.0.0</since>
		public ArcGISImmutableArray<T> MoveToArray()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_ArrayBuilder_moveToArray(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			ArcGISImmutableArray<T> localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new ArcGISImmutableArray<T>(localResult);
			}

			return localLocalResult;
		}
		#endregion // Methods

		#region Internal Members
		internal ArcGISImmutableArrayBuilder(IntPtr handle) => Handle = handle;

		~ArcGISImmutableArrayBuilder()
		{
			if (Handle != IntPtr.Zero)
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_ArrayBuilder_destroy(Handle, errorHandler);

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
		internal static extern void RT_ArrayBuilder_add(IntPtr handle, IntPtr value, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_ArrayBuilder_moveToArray(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_ArrayBuilder_destroy(IntPtr handle, IntPtr errorHandle);
		#endregion // P-Invoke Declarations
	}
}
