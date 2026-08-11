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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISDictionaryBase
	{
		#region Internal Members
		internal ArcGISDictionaryBase(IntPtr handle) => Handle = handle;

		~ArcGISDictionaryBase()
		{
			if (Handle != IntPtr.Zero)
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_Dictionary_destroy(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		internal IntPtr Handle { get; set; }
		#endregion // Internal Members
	}

	/// <summary>
	/// Defines a dictionary object.
	/// </summary>
	/// <since>1.0.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISDictionary<TKey, TValue> : ArcGISDictionaryBase
	{
		#region Constructors
		/// <summary>
		/// Creates a dictionary.
		/// </summary>
		/// <since>1.0.0</since>
		public ArcGISDictionary() : base(IntPtr.Zero)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			Handle = PInvoke.RT_Dictionary_create(Convert.ToArcGISElementType<TKey>(), Convert.ToArcGISElementType<TValue>(), errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Determines the number of values in the dictionary.
		/// </summary>
		/// <remarks>
		/// The number of values in the dictionary. If an error occurs a 0 will be returned.
		/// </remarks>
		/// <since>1.0.0</since>
		public int Count
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_Dictionary_getSize(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				return (int)localResult.ToUInt64();
			}
		}

		/// <summary>
		/// An array containing all the keys in the dictionary.
		/// </summary>
		/// <since>1.0.0</since>
		public ArcGISImmutableArray<TKey> Keys
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_Dictionary_getKeys(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				ArcGISImmutableArray<TKey> localLocalResult = null;

				if (localResult != IntPtr.Zero)
				{
					localLocalResult = new ArcGISImmutableArray<TKey>(localResult);
				}

				return localLocalResult;
			}
		}

		/// <since>1.0.0</since>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <since>1.0.0</since>
		public TValue this[TKey key]
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localKey = Convert.ToArcGISElement(key);

				var localResult = PInvoke.RT_Dictionary_at(Handle, localKey.Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				Standard.ArcGISElement localLocalResult = null;

				if (localResult != IntPtr.Zero)
				{
					localLocalResult = new Standard.ArcGISElement(localResult);
				}

				return Convert.FromArcGISElement<TValue>(localLocalResult);
			}
			set
			{
				if (ContainsKey(key))
				{
					var errorHandler = ArcGISErrorManager.CreateHandler();

					var localKey = Convert.ToArcGISElement(key);
					var localNewValue = Convert.ToArcGISElement(value);

					PInvoke.RT_Dictionary_replace(Handle, localKey.Handle, localNewValue.Handle, errorHandler);

					ArcGISErrorManager.CheckError(errorHandler);
				}
				else
				{
					var errorHandler = ArcGISErrorManager.CreateHandler();

					var localKey = Convert.ToArcGISElement(key);
					var localValue = Convert.ToArcGISElement(value);

					PInvoke.RT_Dictionary_insert(Handle, localKey.Handle, localValue.Handle, errorHandler);

					ArcGISErrorManager.CheckError(errorHandler);
				}
			}
		}
		#endregion // Properties

		#region Methods
		/// <since>1.0.0</since>
		public void Add(TKey key, TValue value)
		{
			this[key] = value;
		}

		/// <since>1.0.0</since>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this[item.Key] = item.Value;
		}

		/// <summary>
		/// Remove all values from the dictionary.
		/// </summary>
		/// <since>1.0.0</since>
		public void Clear()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			PInvoke.RT_Dictionary_removeAll(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);
		}

		/// <since>1.0.0</since>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			var result = this[item.Key];

			return result != null && result.Equals(item.Value);
		}

		/// <summary>
		/// Does the dictionary contain a key.
		/// </summary>
		/// <remarks>
		/// Does the dictionary contain a specific key.
		/// </remarks>
		/// <param name="key">The key you want to find.</param>
		/// <returns>
		/// True if the key is in the dictionary otherwise false.
		/// </returns>
		/// <since>1.0.0</since>
		public bool ContainsKey(TKey key)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localKey = Convert.ToArcGISElement(key);

			var localResult = PInvoke.RT_Dictionary_contains(Handle, localKey.Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <since>1.0.0</since>
		public bool Remove(TKey key)
		{
			if (ContainsKey(key))
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localKey = Convert.ToArcGISElement(key);

				PInvoke.RT_Dictionary_remove(Handle, localKey.Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				return true;
			}

			return false;
		}

		/// <since>1.0.0</since>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			var result = this[item.Key];

			if (result != null && result.Equals(item.Value))
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localKey = Convert.ToArcGISElement(item.Key);

				PInvoke.RT_Dictionary_remove(Handle, localKey.Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}

			return false;
		}

		/// <since>1.0.0</since>
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (ContainsKey(key))
			{
				value = this[key];

				return true;
			}

			value = default;

			return false;
		}
		#endregion // Methods

		#region Internal Members
		internal ArcGISDictionary(IntPtr handle) : base(handle)
		{

		}
		#endregion // Internal Members
	}

	internal static partial class PInvoke
	{
		#region P-Invoke Declarations
		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Dictionary_create(Standard.ArcGISElementType keyType, Standard.ArcGISElementType valueType, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Dictionary_getKeys(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern UIntPtr RT_Dictionary_getSize(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Dictionary_at(IntPtr handle, IntPtr key, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Dictionary_contains(IntPtr handle, IntPtr key, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Dictionary_insert(IntPtr handle, IntPtr key, IntPtr value, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Dictionary_remove(IntPtr handle, IntPtr key, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Dictionary_removeAll(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Dictionary_replace(IntPtr handle, IntPtr key, IntPtr newValue, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Dictionary_destroy(IntPtr handle, IntPtr errorHandle);
		#endregion // P-Invoke Declarations
	}
}
