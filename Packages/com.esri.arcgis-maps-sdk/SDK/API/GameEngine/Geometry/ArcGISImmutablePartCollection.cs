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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// Represents immutable collection of parts for a polygon or polyline geometry. Each part is a collection of segments.
    /// </summary>
    /// <remarks>
    /// Polygons and polyline can have multiple disjoint parts.
    /// Each part is represented by an <see cref="GameEngine.Geometry.ArcGISImmutablePart">ArcGISImmutablePart</see>.
    /// A part is composed of segments representing the edge of the polygon or polyline.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISImmutablePartCollection
    {
        #region Properties
        /// <summary>
        /// Indicates if the immutable part collection contains no parts.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePartCollection_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The number of parts in the immutable part collection.
        /// </summary>
        /// <since>1.0.0</since>
        public ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePartCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The spatial reference for the immutable part collection.
        /// </summary>
        /// <remarks>
        /// If the collection does not have a spatial reference null is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePartCollection_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Returns a part at the specified index in the immutable part collection.
        /// A part is represented by an immutable collection of segments.
        /// </summary>
        /// <param name="index">Position to retrieve the part.</param>
        /// <returns>
        /// the immutable part at the specified part index.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISImmutablePart GetPart(ulong index)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            
            var localResult = PInvoke.RT_ImmutablePartCollection_getPart(Handle, localIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISImmutablePart localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISImmutablePart(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieves the position of the given part in the immutable part collection.
        /// </summary>
        /// <param name="immutablePart">The part you want to find.</param>
        /// <returns>
        /// The position of the part in the collection, -1 otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong IndexOf(ArcGISImmutablePart immutablePart)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localImmutablePart = immutablePart.Handle;
            
            var localResult = PInvoke.RT_ImmutablePartCollection_indexOf(Handle, localImmutablePart, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISImmutablePartCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISImmutablePartCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ImmutablePartCollection_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISImmutablePartCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ImmutablePartCollection_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePartCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePartCollection_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePartCollection_getPart(IntPtr handle, UIntPtr index, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePartCollection_indexOf(IntPtr handle, IntPtr immutablePart, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ImmutablePartCollection_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}