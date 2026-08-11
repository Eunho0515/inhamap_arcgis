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

namespace Esri.GameEngine.Layers
{
    /// <summary>
    /// Defines the modification to apply to a layer or terrain.
    /// </summary>
    /// <since>1.3.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMeshModification
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="GameEngine.Layers.ArcGISMeshModification">ArcGISMeshModification</see> using an <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and an <see cref="GameEngine.Layers.ArcGISMeshModificationType">ArcGISMeshModificationType</see>.
        /// </summary>
        /// <param name="polygon">The polygon that defines the area to modify.</param>
        /// <param name="type">The type of mesh modification to perform.</param>
        /// <seealso cref="GameEngine.Layers.ArcGISMeshModification.Polygon">ArcGISMeshModification.Polygon</seealso>
        /// <since>1.3.0</since>
        public ArcGISMeshModification(GameEngine.Geometry.ArcGISPolygon polygon, ArcGISMeshModificationType type)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolygon = polygon.Handle;
            
            Handle = PInvoke.RT_GEMeshModification_create(localPolygon, type, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The polygon that defines the area to modify.
        /// </summary>
        /// <remarks>
        /// If the polygon is multipart (see <see cref="GameEngine.Geometry.ArcGISMultipart.Parts">ArcGISMultipart.Parts</see>) it must also be simple (see <see cref="GameEngine.Geometry.ArcGISGeometryEngine.IsSimple">ArcGISGeometryEngine.IsSimple</see>).
        /// Multipart polygons with multiple exterior rings and holes must have parts ordered such that exterior
        /// rings are followed by their immediate holes. Simplifying a polygon will ensure its paths are in the 
        /// required order (see <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see>).
        /// The polygon may not contain curves (see <see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see>).
        /// The polygon must have a spatial reference (see <see cref="GameEngine.Geometry.ArcGISGeometry.SpatialReference">ArcGISGeometry.SpatialReference</see>).
        /// </remarks>
        /// <since>1.3.0</since>
        public GameEngine.Geometry.ArcGISPolygon Polygon
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMeshModification_getPolygon(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISPolygon localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISPolygon(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The type of mesh modification to perform.
        /// </summary>
        /// <since>1.3.0</since>
        public ArcGISMeshModificationType Type
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMeshModification_getType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISMeshModification(IntPtr handle) => Handle = handle;
        
        ~ArcGISMeshModification()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEMeshModification_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISMeshModification other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMeshModification_create(IntPtr polygon, ArcGISMeshModificationType type, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMeshModification_getPolygon(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISMeshModificationType RT_GEMeshModification_getType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMeshModification_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}