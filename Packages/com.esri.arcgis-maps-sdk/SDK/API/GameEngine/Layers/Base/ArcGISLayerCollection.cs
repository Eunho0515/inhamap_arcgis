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

namespace Esri.GameEngine.Layers.Base
{
    /// <summary>
    /// Defines a mutable collection of <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    internal partial class ArcGISLayerCollection
    #pragma warning restore CS0661, CS0659
    {
        #region Constructors
        /// <summary>
        /// Creates a vector. This allocates memory that must be deleted.
        /// </summary>
        internal ArcGISLayerCollection()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GELayerCollection_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
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
                
                var localResult = PInvoke.RT_GELayerCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Add a value to the vector.
        /// </summary>
        /// <param name="value">The value that is to be added.</param>
        /// <returns>
        /// The position of the value. Max value of size_t if an error occurs.
        /// </returns>
        internal ulong Add(ArcGISLayer value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_GELayerCollection_add(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Appends a vector to a vector.
        /// </summary>
        /// <param name="vector2">The value that is to be added.</param>
        /// <returns>
        /// The new size of vector_1. Max value of size_t if an error occurs.
        /// </returns>
        internal ulong AddArray(Unity.ArcGISCollection<ArcGISLayer> vector2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localVector2 = vector2.Handle;
            
            var localResult = PInvoke.RT_GELayerCollection_addArray(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Get a value at a specific position.
        /// </summary>
        /// <remarks>
        /// Retrieves the value at the specified position.
        /// </remarks>
        /// <param name="position">The position which you want to get the value.</param>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>, at the position requested.
        /// </returns>
        internal ArcGISLayer At(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            var localResult = PInvoke.RT_GELayerCollection_at(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISLayer localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.Base.PInvoke.RT_GELayer_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGIS3DObjectSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGIS3DObjectSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISBuildingSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISBuildingSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGaussianSplatLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGaussianSplatLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGroupLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGroupLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISImageLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISImageLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISIntegratedMeshLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISIntegratedMeshLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISOGC3DTilesLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISOGC3DTilesLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointCloudLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointCloudLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnknownLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnknownLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnsupportedLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnsupportedLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISVectorTileLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISVectorTileLayer(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISLayer(localResult);
                        break;
                }
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
        internal bool Contains(ArcGISLayer value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_GELayerCollection_contains(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the first value in the vector.
        /// </summary>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>, at the position requested.
        /// </returns>
        internal ArcGISLayer First()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GELayerCollection_first(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISLayer localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.Base.PInvoke.RT_GELayer_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGIS3DObjectSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGIS3DObjectSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISBuildingSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISBuildingSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGaussianSplatLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGaussianSplatLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGroupLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGroupLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISImageLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISImageLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISIntegratedMeshLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISIntegratedMeshLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISOGC3DTilesLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISOGC3DTilesLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointCloudLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointCloudLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnknownLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnknownLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnsupportedLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnsupportedLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISVectorTileLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISVectorTileLayer(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISLayer(localResult);
                        break;
                }
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
        internal ulong IndexOf(ArcGISLayer value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_GELayerCollection_indexOf(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Insert a value at the specified position in the vector.
        /// </summary>
        /// <remarks>
        /// Insert a value at a specified position to the vector.
        /// </remarks>
        /// <param name="position">The position which you want to insert the value.</param>
        /// <param name="value">The value that is to be added.</param>
        internal void Insert(ulong position, ArcGISLayer value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            var localValue = value.Handle;
            
            PInvoke.RT_GELayerCollection_insert(Handle, localPosition, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
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
            
            var localResult = PInvoke.RT_GELayerCollection_isEmpty(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the last value in the vector.
        /// </summary>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>, at the position requested.
        /// </returns>
        internal ArcGISLayer Last()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GELayerCollection_last(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISLayer localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.Base.PInvoke.RT_GELayer_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGIS3DObjectSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGIS3DObjectSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISBuildingSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISBuildingSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGaussianSplatLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGaussianSplatLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGroupLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGroupLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISImageLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISImageLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISIntegratedMeshLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISIntegratedMeshLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISOGC3DTilesLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISOGC3DTilesLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointCloudLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointCloudLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnknownLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnknownLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnsupportedLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnsupportedLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISVectorTileLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISVectorTileLayer(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISLayer(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Move a value from the current position to a new position in the string vector.
        /// </summary>
        /// <remarks>
        /// Move a value from the current position to a new position in the vector.
        /// </remarks>
        /// <param name="oldPosition">The current position of the value.</param>
        /// <param name="newPosition">The position which you want to move the value to.</param>
        internal void Move(ulong oldPosition, ulong newPosition)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localOldPosition = new UIntPtr(oldPosition);
            var localNewPosition = new UIntPtr(newPosition);
            
            PInvoke.RT_GELayerCollection_move(Handle, localOldPosition, localNewPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
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
            
            var localResult = PInvoke.RT_GELayerCollection_npos(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Remove a value at a specific position in the vector.
        /// </summary>
        /// <param name="position">The position which you want to remove the value.</param>
        internal void Remove(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            PInvoke.RT_GELayerCollection_remove(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove all values from the vector.
        /// </summary>
        internal void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GELayerCollection_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISLayerCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISLayerCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GELayerCollection_destroy(Handle, errorHandler);
                
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
            
            var vector2 = obj as ArcGISLayerCollection;
            
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
            
            var localResult = PInvoke.RT_GELayerCollection_equals(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISLayerCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GELayerCollection_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GELayerCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GELayerCollection_add(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GELayerCollection_addArray(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GELayerCollection_at(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GELayerCollection_contains(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GELayerCollection_first(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GELayerCollection_indexOf(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GELayerCollection_insert(IntPtr handle, UIntPtr position, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GELayerCollection_isEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GELayerCollection_last(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GELayerCollection_move(IntPtr handle, UIntPtr oldPosition, UIntPtr newPosition, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GELayerCollection_npos(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GELayerCollection_remove(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GELayerCollection_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GELayerCollection_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GELayerCollection_equals(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}