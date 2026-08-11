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

namespace Esri.GameEngine.Attributes
{
    /// <summary>
    /// An immutable collection of <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see> objects.
    /// </summary>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    public partial class ArcGISPointCloudAttributeImmutableCollection
    #pragma warning restore CS0661, CS0659
    {
        #region Properties
        /// <summary>
        /// The number of values in the vector.
        /// </summary>
        /// <remarks>
        /// If an error occurs, 0 is returned.
        /// </remarks>
        /// <since>2.4.0</since>
        public ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Gets the value at the specified position.
        /// </summary>
        /// <param name="position">The position at which to get the value.</param>
        /// <returns>
        /// The <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see> at the specified position.
        /// </returns>
        /// <since>2.4.0</since>
        public GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute At(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_at(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Determines if the vector contains the specified value.
        /// </summary>
        /// <param name="value">The value to find.</param>
        /// <returns>
        /// True if the value is in the vector, otherwise false.
        /// </returns>
        /// <since>2.4.0</since>
        public bool Contains(GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_contains(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the first value in the vector.
        /// </summary>
        /// <returns>
        /// The <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see> at the specified position.
        /// </returns>
        /// <since>2.4.0</since>
        public GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute First()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_first(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieves the position of the given value in the vector.
        /// </summary>
        /// <param name="value">The value to find.</param>
        /// <returns>
        /// The position of the value in the vector, Max value of size_t otherwise.
        /// </returns>
        /// <since>2.4.0</since>
        public ulong IndexOf(GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_indexOf(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Determines if the vector contains values.
        /// </summary>
        /// <returns>
        /// True if the vector object contains no values, otherwise false. Returns true if an error occurs.
        /// </returns>
        /// <since>2.4.0</since>
        public bool IsEmpty()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_isEmpty(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the last value in the vector.
        /// </summary>
        /// <returns>
        /// The <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see> at the specified position.
        /// </returns>
        /// <since>2.4.0</since>
        public GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute Last()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_last(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns a value indicating a bad position within the vector.
        /// </summary>
        /// <returns>
        /// A size_t.
        /// </returns>
        /// <since>2.4.0</since>
        public static ulong Npos()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_npos(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISPointCloudAttributeImmutableCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudAttributeImmutableCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudAttributeImmutableCollection_destroy(Handle, errorHandler);
                
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
            
            var vector2 = obj as ArcGISPointCloudAttributeImmutableCollection;
            
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
            
            var localResult = PInvoke.RT_PointCloudAttributeImmutableCollection_equals(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISPointCloudAttributeImmutableCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudAttributeImmutableCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudAttributeImmutableCollection_at(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudAttributeImmutableCollection_contains(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudAttributeImmutableCollection_first(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudAttributeImmutableCollection_indexOf(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudAttributeImmutableCollection_isEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudAttributeImmutableCollection_last(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudAttributeImmutableCollection_npos(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudAttributeImmutableCollection_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudAttributeImmutableCollection_equals(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}