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

namespace Esri.GameEngine.Data
{
    /// <summary>
    /// A representation of a real-world geographic entity.
    /// </summary>
    /// <remarks>
    /// Features are composed of both a geometry (point, polyline, or polygon) and attributes. The geometry
    /// represents the location and shape of the real-world entity; the attributes (key-value pairs) represent
    /// the fields and values that describe the entity. Examples of features include roads, fire hydrants, and
    /// property boundaries. Applications can access features from a feature layer or a feature collection to
    /// visualize the feature's geographic and attribute information, execute spatial queries, perform analyses,
    /// or make edits to the feature's data directly. Feature attribute values can be changed, but attribute definitions cannot be added,
    /// deleted, or modified.
    /// 
    /// Features are typically persisted in a data source (such as a feature service, geodatabase, shapefile,
    /// GeoJSON file, or GeoPackage) and have a common attribute schema. Features can also be stored directly in
    /// a feature collection in a map or scene. A feature collection groups logically related tables of features
    /// that may have different schema, geometry types, and symbology. See <see cref="">FeatureCollectionLayer</see> for more
    /// information.
    /// 
    /// <see cref="GameEngine.Data.Feature">Feature</see> is the base class for <see cref="">ArcGISFeature</see>. ArcGIS features are stored in ArcGIS
    /// specific data sources such as <see cref="">GeodatabaseFeatureTable</see> and <see cref="">ServiceFeatureTable</see>.
    /// </remarks>
    /// <seealso cref="">ArcGISFeature</seealso>
    /// <seealso cref="">ArcGISFeatureTable</seealso>
    /// <seealso cref="">GeoPackageFeatureTable</seealso>
    /// <seealso cref="">OGCFeatureCollectionTable</seealso>
    /// <seealso cref="">ShapefileFeatureTable</seealso>
    /// <seealso cref="">WFSFeatureTable</seealso>
    /// <seealso cref="">FeatureCollectionTable</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class Feature :
        GameEngine.Map.ArcGISGeoElement
    #pragma warning restore CS0661, CS0660
    {
        #region Properties
        /// <summary>
        /// The attributes of the feature.
        /// </summary>
        /// <since>1.0.0</since>
        public Unity.ArcGISDictionary<string, object> Attributes
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Feature_getAttributes(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISDictionary<string, object> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISDictionary<string, object>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The object type of this object.
        /// </summary>
        internal FeatureObjectType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Feature_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region GameEngine.Map.ArcGISGeoElement
        #endregion // GameEngine.Map.ArcGISGeoElement
        
        #region Standard.ArcGISInstanceId
        internal ulong InstanceId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Feature_getInstanceId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        #endregion // Standard.ArcGISInstanceId
        
        #region Internal Members
        internal Feature(IntPtr handle) => Handle = handle;
        
        ~Feature()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Feature_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static bool operator ==(Feature left, Feature right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            
            if (left is null || right is null)
            {
                return false;
            }
            
            if (!left || !right)
            {
                return false;
            }
            
            return left.InstanceId == right.InstanceId;
        }
        
        public static bool operator !=(Feature left, Feature right)
        {
            return !(left == right);
        }
        
        public static implicit operator bool(Feature other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Feature_getAttributes(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern FeatureObjectType RT_Feature_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Feature_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.Map.ArcGISGeoElement P-Invoke Declarations
        #endregion // GameEngine.Map.ArcGISGeoElement P-Invoke Declarations
        
        #region Standard.ArcGISInstanceId P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_Feature_getInstanceId(IntPtr handle, IntPtr errorHandler);
        #endregion // Standard.ArcGISInstanceId P-Invoke Declarations
    }
}