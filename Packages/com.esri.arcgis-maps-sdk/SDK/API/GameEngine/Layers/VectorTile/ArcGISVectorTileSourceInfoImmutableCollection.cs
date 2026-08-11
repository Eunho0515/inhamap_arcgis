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
using System;

namespace Esri.GameEngine.Layers.VectorTile
{
    /// <summary>
    /// Defines an immutable collection of <see cref="GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo">ArcGISVectorTileSourceInfo</see>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    internal partial class ArcGISVectorTileSourceInfoImmutableCollection
    #pragma warning restore CS0661, CS0659
    {
        #region Properties
        /// <summary>
        /// Determines the number of values in the vector.
        /// </summary>
        /// <remarks>
        /// The number of values in the vector. If an error occurs a 0 will be returned.
        /// </remarks>
        internal ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
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
        /// The value, <see cref="GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo">ArcGISVectorTileSourceInfo</see>, at the position requested.
        /// </returns>
        internal ArcGISVectorTileSourceInfo At(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_at(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISVectorTileSourceInfo localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISVectorTileSourceInfo(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Does the vector contain the given value.
        /// </summary>
        /// <remarks>
        /// Does the vector contain a specific value.
        /// </remarks>
        /// <param name="value">The value you want to find.</param>
        /// <returns>
        /// True if the value is in the vector otherwise false.
        /// </returns>
        internal bool Contains(ArcGISVectorTileSourceInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_contains(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the first value in the vector.
        /// </summary>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo">ArcGISVectorTileSourceInfo</see>, at the position requested.
        /// </returns>
        internal ArcGISVectorTileSourceInfo First()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_first(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISVectorTileSourceInfo localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISVectorTileSourceInfo(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieves the position of the given value in the vector.
        /// </summary>
        /// <param name="value">The value you want to find.</param>
        /// <returns>
        /// The position of the value in the vector, Max value of size_t otherwise.
        /// </returns>
        internal ulong IndexOf(ArcGISVectorTileSourceInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_indexOf(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Determines if there are any values in the vector.
        /// </summary>
        /// <remarks>
        /// Check if the vector object has any values in it.
        /// </remarks>
        /// <returns>
        /// true if the  vector object contains no values otherwise false. Will return true if an error occurs.
        /// </returns>
        internal bool IsEmpty()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_isEmpty(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the last value in the vector.
        /// </summary>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo">ArcGISVectorTileSourceInfo</see>, at the position requested.
        /// </returns>
        internal ArcGISVectorTileSourceInfo Last()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_last(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISVectorTileSourceInfo localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISVectorTileSourceInfo(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns a value indicating a bad position within the vector.
        /// </summary>
        /// <returns>
        /// A size_t.
        /// </returns>
        internal static ulong Npos()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_npos(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISVectorTileSourceInfoImmutableCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISVectorTileSourceInfoImmutableCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_VectorTileSourceInfoImmutableCollection_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var vector2 = obj as ArcGISVectorTileSourceInfoImmutableCollection;
            
            if (vector2 == null)
            {
                return false;
            }
            
            var localVector2 = vector2.Handle;
            
            if (Handle == localVector2)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_VectorTileSourceInfoImmutableCollection_equals(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISVectorTileSourceInfoImmutableCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_VectorTileSourceInfoImmutableCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VectorTileSourceInfoImmutableCollection_at(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_VectorTileSourceInfoImmutableCollection_contains(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VectorTileSourceInfoImmutableCollection_first(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_VectorTileSourceInfoImmutableCollection_indexOf(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_VectorTileSourceInfoImmutableCollection_isEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VectorTileSourceInfoImmutableCollection_last(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_VectorTileSourceInfoImmutableCollection_npos(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_VectorTileSourceInfoImmutableCollection_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_VectorTileSourceInfoImmutableCollection_equals(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}