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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.SDK.Utils
{
	internal static class ListExtensions
	{
		internal class RequireStruct<T> where T : struct { }
		internal class RequireClonable<T> where T : ICloneable { }

#pragma warning disable IDE0060 // Remove unused parameter
		internal static List<T> Clone<T>(this List<T> list, RequireClonable<T> ignore = null) where T : ICloneable
#pragma warning restore IDE0060 // Remove unused parameter
		{
			return list.Select(item => (T)item.Clone()).ToList();
		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal static List<T> Clone<T>(this List<T> list, RequireStruct<T> ignore = null) where T : struct
#pragma warning restore IDE0060 // Remove unused parameter
		{
			return list.Select(item => (T)item).ToList();
		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal static Dictionary<T, List<string>> Clone<T>(this Dictionary<T, List<string>> dictionary, RequireClonable<T> ignore = null) where T : ICloneable
#pragma warning restore IDE0060 // Remove unused parameter
		{
			var clone = new Dictionary<T, List<string>>(dictionary.Count, dictionary.Comparer);

			foreach (var entry in dictionary)
			{
				clone.Add((T)entry.Key.Clone(), entry.Value.Clone());
			}

			return clone;
		}

		internal static List<object> Clone(this List<object> list)
		{
			return JsonUtility.FromJson<List<object>>(JsonUtility.ToJson(list, false));
		}
	}

	internal class IEnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
	{
		private static volatile IEnumerableEqualityComparer<T> defaultEqualityComparer;

		public static IEnumerableEqualityComparer<T> Default
		{
			get
			{
				if (defaultEqualityComparer == null)
				{
					Interlocked.CompareExchange(ref defaultEqualityComparer, new IEnumerableEqualityComparer<T>(), null);
				}

				return defaultEqualityComparer;
			}
		}

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			return ReferenceEquals(x, y) || (x != null && y != null && x.SequenceEqual(y));
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			var hash = new HashCode();

			foreach (var item in obj.Where(e => e != null))
			{
				hash.Add(item);
			}

			return hash.ToHashCode();
		}
	}
}
