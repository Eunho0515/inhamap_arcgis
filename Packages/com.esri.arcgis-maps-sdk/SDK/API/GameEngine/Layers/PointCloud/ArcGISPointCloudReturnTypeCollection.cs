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

namespace Esri.GameEngine.Layers.PointCloud
{
    /// <summary>
    /// Defines an collection of <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see>.
    /// </summary>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    public partial class ArcGISPointCloudReturnTypeCollection
    #pragma warning restore CS0661, CS0659
    {
        #region Constructors
        /// <summary>
        /// Creates a vector. This allocates memory that must be deleted.
        /// </summary>
        /// <since>2.4.0</since>
        public ArcGISPointCloudReturnTypeCollection()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_PointCloudReturnTypeCollection_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The number of values in the vector.
        /// </summary>
        /// <remarks>
        /// If an error occurs, 0 will be returned.
        /// </remarks>
        /// <since>2.4.0</since>
        public ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudReturnTypeCollection_getSize(Handle, errorHandler);
                
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
        /// <since>2.4.0</since>
        public ulong Add(ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_add(Handle, value, errorHandler);
            
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
        /// <since>2.4.0</since>
        public ulong AddArray(Unity.ArcGISCollection<ArcGISPointCloudReturnType> vector2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localVector2 = vector2.Handle;
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_addArray(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Get a value at a specific position.
        /// </summary>
        /// <param name="position">The position which you want to get the value.</param>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see> that represents the value at the specified position.
        /// </returns>
        /// <since>2.4.0</since>
        public ArcGISPointCloudReturnType At(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_at(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Indicates if the vector contains the given value.
        /// </summary>
        /// <param name="value">The value you want to find.</param>
        /// <returns>
        /// True if the value is in the vector, otherwise false.
        /// </returns>
        /// <since>2.4.0</since>
        public bool Contains(ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_contains(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the first value in the vector.
        /// </summary>
        /// <returns>
        /// The value, <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see>, at the position requested.
        /// </returns>
        /// <since>2.4.0</since>
        public ArcGISPointCloudReturnType First()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_first(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Find the position of the given value in the vector.
        /// </summary>
        /// <param name="value">The value you want to find.</param>
        /// <returns>
        /// The position of the value in the vector, Max value of size_t otherwise.
        /// </returns>
        /// <since>2.4.0</since>
        public ulong IndexOf(ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_indexOf(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Insert the provided value at the specified position in the vector.
        /// </summary>
        /// <param name="position">The position at which to insert the given value.</param>
        /// <param name="value">The value that to add.</param>
        /// <since>2.4.0</since>
        public void Insert(ulong position, ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            PInvoke.RT_PointCloudReturnTypeCollection_insert(Handle, localPosition, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Determines if the vector is empty.
        /// </summary>
        /// <returns>
        /// True if the vector object contains no values, otherwise false. Returns true if an error occurs.
        /// </returns>
        /// <since>2.4.0</since>
        public bool IsEmpty()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_isEmpty(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Get the last value in the vector.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see> representing the value at the specified position.
        /// </returns>
        /// <since>2.4.0</since>
        public ArcGISPointCloudReturnType Last()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_last(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Move a value from the current position to a new position in the vector.
        /// </summary>
        /// <param name="oldPosition">The current position of the value.</param>
        /// <param name="newPosition">The position to which you want to move the value.</param>
        /// <since>2.4.0</since>
        public void Move(ulong oldPosition, ulong newPosition)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localOldPosition = new UIntPtr(oldPosition);
            var localNewPosition = new UIntPtr(newPosition);
            
            PInvoke.RT_PointCloudReturnTypeCollection_move(Handle, localOldPosition, localNewPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
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
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_npos(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Remove a value at a specific position in the vector.
        /// </summary>
        /// <param name="position">The position where you want to remove the value.</param>
        /// <since>2.4.0</since>
        public void Remove(ulong position)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = new UIntPtr(position);
            
            PInvoke.RT_PointCloudReturnTypeCollection_remove(Handle, localPosition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove all values from the vector.
        /// </summary>
        /// <since>2.4.0</since>
        public void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_PointCloudReturnTypeCollection_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISPointCloudReturnTypeCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudReturnTypeCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudReturnTypeCollection_destroy(Handle, errorHandler);
                
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
            
            var vector2 = obj as ArcGISPointCloudReturnTypeCollection;
            
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
            
            var localResult = PInvoke.RT_PointCloudReturnTypeCollection_equals(Handle, localVector2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISPointCloudReturnTypeCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudReturnTypeCollection_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudReturnTypeCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudReturnTypeCollection_add(IntPtr handle, ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudReturnTypeCollection_addArray(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudReturnType RT_PointCloudReturnTypeCollection_at(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudReturnTypeCollection_contains(IntPtr handle, ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudReturnType RT_PointCloudReturnTypeCollection_first(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudReturnTypeCollection_indexOf(IntPtr handle, ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnTypeCollection_insert(IntPtr handle, UIntPtr position, ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudReturnTypeCollection_isEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudReturnType RT_PointCloudReturnTypeCollection_last(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnTypeCollection_move(IntPtr handle, UIntPtr oldPosition, UIntPtr newPosition, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_PointCloudReturnTypeCollection_npos(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnTypeCollection_remove(IntPtr handle, UIntPtr position, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnTypeCollection_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnTypeCollection_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_PointCloudReturnTypeCollection_equals(IntPtr handle, IntPtr vector2, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}