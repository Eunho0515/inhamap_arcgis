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
    /// A collection object that can be changed containing parts for a geometry. Each part is represented by an <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>.
    /// </summary>
    /// <seealso cref="GameEngine.Geometry.ArcGISImmutablePartCollection">ArcGISImmutablePartCollection</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMutablePartCollection
    {
        #region Constructors
        /// <summary>
        /// Creates a mutable part collection with a specified spatial reference.
        /// </summary>
        /// <param name="spatialReference">A spatial reference object, can be null.</param>
        /// <since>1.0.0</since>
        public ArcGISMutablePartCollection(ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_MutablePartCollection_createWithSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Indicates if the mutable part collection contains no parts.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePartCollection_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The number of parts in the collection.
        /// </summary>
        /// <since>1.0.0</since>
        public ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePartCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The spatial reference for the mutable part collection.
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
                
                var localResult = PInvoke.RT_MutablePartCollection_getSpatialReference(Handle, errorHandler);
                
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
        /// Add a new part to the end of the mutable part collection.
        /// </summary>
        /// <param name="mutablePart">The part to add.</param>
        /// <returns>
        /// the index where the part was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPart(ArcGISMutablePart mutablePart)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localMutablePart = mutablePart.Handle;
            
            var localResult = PInvoke.RT_MutablePartCollection_addPart(Handle, localMutablePart, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Returns a part at the specified index in the collection.
        /// </summary>
        /// <param name="index">The position in the collection.</param>
        /// <returns>
        /// The part at the specified position in the collection.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISMutablePart GetPart(ulong index)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            
            var localResult = PInvoke.RT_MutablePartCollection_getPart(Handle, localIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISMutablePart localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISMutablePart(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieves the position of the segment in the mutable part collection.
        /// The first segment that is equal to the supplied segment is returned.
        /// </summary>
        /// <param name="mutablePart">The part to find.</param>
        /// <returns>
        /// The position of the segment in the collection, -1 otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong IndexOf(ArcGISMutablePart mutablePart)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localMutablePart = mutablePart.Handle;
            
            var localResult = PInvoke.RT_MutablePartCollection_indexOf(Handle, localMutablePart, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Inserts a part into the mutable part collection at the specified part index.
        /// </summary>
        /// <remarks>
        /// The part index can be equal to the part count and this is equivalent to adding a part to the end of the collection.
        /// </remarks>
        /// <param name="index">Zero-based index of the part.</param>
        /// <param name="mutablePart">The part to insert.</param>
        /// <since>1.0.0</since>
        public void InsertPart(ulong index, ArcGISMutablePart mutablePart)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            var localMutablePart = mutablePart.Handle;
            
            PInvoke.RT_MutablePartCollection_insertPart(Handle, localIndex, localMutablePart, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove all parts from the mutable part collection.
        /// </summary>
        /// <remarks>
        /// After calling this method the mutable part collection is empty.
        /// </remarks>
        /// <since>1.0.0</since>
        public void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_MutablePartCollection_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Removes a part from the mutable part collection.
        /// </summary>
        /// <param name="index">Zero-based index of the part.</param>
        /// <since>1.0.0</since>
        public void RemovePart(ulong index)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            
            PInvoke.RT_MutablePartCollection_removePart(Handle, localIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Replace a part in the mutable part collection at the specified part index.
        /// </summary>
        /// <param name="index">Zero-based index of the part.</param>
        /// <param name="mutablePart">Collection of segments representing the part.</param>
        /// <since>1.0.0</since>
        public void SetPart(ulong index, ArcGISMutablePart mutablePart)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            var localMutablePart = mutablePart.Handle;
            
            PInvoke.RT_MutablePartCollection_setPart(Handle, localIndex, localMutablePart, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISMutablePartCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISMutablePartCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_MutablePartCollection_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISMutablePartCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePartCollection_createWithSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_MutablePartCollection_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePartCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePartCollection_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePartCollection_addPart(IntPtr handle, IntPtr mutablePart, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePartCollection_getPart(IntPtr handle, UIntPtr index, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePartCollection_indexOf(IntPtr handle, IntPtr mutablePart, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePartCollection_insertPart(IntPtr handle, UIntPtr index, IntPtr mutablePart, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePartCollection_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePartCollection_removePart(IntPtr handle, UIntPtr index, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePartCollection_setPart(IntPtr handle, UIntPtr index, IntPtr mutablePart, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePartCollection_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}