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

namespace Esri.Standard
{
    /// <summary>
    /// Defines an element that can be added to a container object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISElement
    {
        #region Properties
        /// <summary>
        /// The type that the element is holding.
        /// </summary>
        internal ArcGISElementType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Element_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates an element object from an ArcGIS credential.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromArcGISCredential(GameEngine.Authentication.ArcGISCredential value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromArcGISCredential(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from an ArcGIS token info.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromArcGISTokenInfo(GameEngine.Authentication.ArcGISTokenInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromArcGISTokenInfo(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from an array value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromArray(Unity.ArcGISImmutableArray<object> value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromArray(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a date time value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromDateTime(DateTimeOffset value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            using var localValue = Unity.Convert.ToArcGISDateTime(value);
            
            var localResult = PInvoke.RT_Element_fromDateTime(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromDatumTransformation(GameEngine.Geometry.ArcGISDatumTransformation value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromDatumTransformation(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a dictionary value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromDictionary(Unity.ArcGISDictionaryBase value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromDictionary(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a feature.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromFeature(GameEngine.Data.Feature value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromFeature(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a float32 value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromFloat32(float value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromFloat32(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a float64 value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromFloat64(double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromFloat64(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from an attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromGEAttribute(GameEngine.Attributes.ArcGISAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGEAttribute(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</see>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromGEBuildingSceneLayerAttributeStatistics(GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGEBuildingSceneLayerAttributeStatistics(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromGeographicTransformationStep(GameEngine.Geometry.ArcGISGeographicTransformationStep value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGeographicTransformationStep(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a geometry value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>
        /// </returns>
        internal static ArcGISElement FromGeometry(GameEngine.Geometry.ArcGISGeometry value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGeometry(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from a visualization attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromGEVisualizationAttribute(GameEngine.Attributes.ArcGISVisualizationAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGEVisualizationAttribute(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from a visualization attribute description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromGEVisualizationAttributeDescription(GameEngine.Attributes.ArcGISVisualizationAttributeDescription value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromGEVisualizationAttributeDescription(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a GUID value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>
        /// </returns>
        internal static ArcGISElement FromGUID(Guid value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            using var localValue = Unity.Convert.ToArcGISGUID(value);
            
            var localResult = PInvoke.RT_Element_fromGUID(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromHorizontalVerticalTransformationStep(GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromHorizontalVerticalTransformationStep(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from an identify result.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromIdentifyLayerResult(GameEngine.MapView.ArcGISIdentifyLayerResult value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromIdentifyLayerResult(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from a <see cref="GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection">ArcGISIdentifyLayerResultImmutableCollection</see>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromIdentifyLayerResultImmutableCollection(Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromIdentifyLayerResultImmutableCollection(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from an int16_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromInt16(short value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromInt16(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from an int32_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromInt32(int value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromInt32(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from an int64_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>
        /// </returns>
        internal static ArcGISElement FromInt64(long value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromInt64(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from an OAuth user credential.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromOAuthUserCredential(GameEngine.Authentication.ArcGISOAuthUserCredential value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromOAuthUserCredential(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element object from an OAuth user token info.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromOAuthUserTokenInfo(GameEngine.Authentication.ArcGISOAuthUserTokenInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromOAuthUserTokenInfo(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudAttribute(GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromPointCloudAttribute(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud color class break.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudColorClassBreak(GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromPointCloudColorClassBreak(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud color stop.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudColorStop(GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromPointCloudColorStop(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud color unique value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudColorUniqueValue(GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromPointCloudColorUniqueValue(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudFilter(GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            var localResult = PInvoke.RT_Element_fromPointCloudFilter(localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a point cloud returns type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromPointCloudReturnType(GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromPointCloudReturnType(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromString(string value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromString(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a uint16_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromUInt16(ushort value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromUInt16(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a uint32_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromUInt32(uint value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromUInt32(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an element from a uint64_t value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISElement">ArcGISElement</see>.
        /// </returns>
        internal static ArcGISElement FromUInt64(ulong value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_fromUInt64(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISElement localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISElement(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as an ArcGIS credential.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see>.
        /// </returns>
        internal GameEngine.Authentication.ArcGISCredential GetValueAsArcGISCredential()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsArcGISCredential(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Authentication.ArcGISCredential localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Authentication.PInvoke.RT_ArcGISCredential_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Authentication.ArcGISCredentialType.OAuthUserCredential:
                        localLocalResult = new GameEngine.Authentication.ArcGISOAuthUserCredential(localResult);
                        break;
                    default:
                        localLocalResult = new GameEngine.Authentication.ArcGISCredential(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as an ArcGIS token info.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Authentication.ArcGISTokenInfo">ArcGISTokenInfo</see>.
        /// </returns>
        internal GameEngine.Authentication.ArcGISTokenInfo GetValueAsArcGISTokenInfo()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsArcGISTokenInfo(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Authentication.ArcGISTokenInfo localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Authentication.ArcGISTokenInfo(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the element value as an array.
        /// </summary>
        /// <returns>
        /// An <see cref="Unity.ArcGISImmutableArray<T>">ArcGISImmutableArray<T></see>.
        /// </returns>
        internal Unity.ArcGISImmutableArray<object> GetValueAsArray()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsArray(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<object> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<object>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An <see cref="">DateTimeOffset</see>.
        /// </returns>
        internal DateTimeOffset GetValueAsDateTime()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsDateTime(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISDateTime(localResult);
        }
        
        /// <summary>
        /// Gets the value of the element as an <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see>.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see>.
        /// </returns>
        internal GameEngine.Geometry.ArcGISDatumTransformation GetValueAsDatumTransformation()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsDatumTransformation(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISDatumTransformation localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_DatumTransformation_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISDatumTransformationType.GeographicTransformation:
                        localLocalResult = new GameEngine.Geometry.ArcGISGeographicTransformation(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISDatumTransformationType.HorizontalVerticalTransformation:
                        localLocalResult = new GameEngine.Geometry.ArcGISHorizontalVerticalTransformation(localResult);
                        break;
                    default:
                        localLocalResult = new GameEngine.Geometry.ArcGISDatumTransformation(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the element value as a dictionary.
        /// </summary>
        /// <returns>
        /// A <see cref="Unity.ArcGISDictionary<Key, Value>">ArcGISDictionary<Key, Value></see>.
        /// </returns>
        internal Unity.ArcGISDictionaryBase GetValueAsDictionary()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsDictionary(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISDictionaryBase localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISDictionaryBase(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of an element as a feature.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Data.Feature">Feature</see>.
        /// </returns>
        internal GameEngine.Data.Feature GetValueAsFeature()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsFeature(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Data.Feature localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Data.Feature(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An float32.
        /// </returns>
        internal float GetValueAsFloat32()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsFloat32(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An float64.
        /// </returns>
        internal double GetValueAsFloat64()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsFloat64(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of an element as an attribute.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Attributes.ArcGISAttribute">ArcGISAttribute</see>.
        /// </returns>
        internal GameEngine.Attributes.ArcGISAttribute GetValueAsGEAttribute()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGEAttribute(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Attributes.ArcGISAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Attributes.ArcGISAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of an element as a <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</see>.
        /// </summary>
        /// <returns>
        /// a <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</see>.
        /// </returns>
        internal GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics GetValueAsGEBuildingSceneLayerAttributeStatistics()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGEBuildingSceneLayerAttributeStatistics(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element as an <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see>.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see>.
        /// </returns>
        internal GameEngine.Geometry.ArcGISGeographicTransformationStep GetValueAsGeographicTransformationStep()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGeographicTransformationStep(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISGeographicTransformationStep localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Geometry.ArcGISGeographicTransformationStep(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>.
        /// </returns>
        internal GameEngine.Geometry.ArcGISGeometry GetValueAsGeometry()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGeometry(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new GameEngine.Geometry.ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new GameEngine.Geometry.ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new GameEngine.Geometry.ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new GameEngine.Geometry.ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new GameEngine.Geometry.ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new GameEngine.Geometry.ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of an element as a visualization attribute.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Attributes.ArcGISVisualizationAttribute">ArcGISVisualizationAttribute</see>.
        /// </returns>
        internal GameEngine.Attributes.ArcGISVisualizationAttribute GetValueAsGEVisualizationAttribute()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGEVisualizationAttribute(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Attributes.ArcGISVisualizationAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Attributes.ArcGISVisualizationAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of an element as a visualization attribute description.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Attributes.ArcGISVisualizationAttributeDescription">ArcGISVisualizationAttributeDescription</see>.
        /// </returns>
        internal GameEngine.Attributes.ArcGISVisualizationAttributeDescription GetValueAsGEVisualizationAttributeDescription()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGEVisualizationAttributeDescription(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Attributes.ArcGISVisualizationAttributeDescription localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Attributes.ArcGISVisualizationAttributeDescription(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An <see cref="">Guid</see>.
        /// </returns>
        internal Guid GetValueAsGUID()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsGUID(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISGUID(localResult);
        }
        
        /// <summary>
        /// Gets the value of the element as an <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see>.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see>.
        /// </returns>
        internal GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep GetValueAsHorizontalVerticalTransformationStep()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsHorizontalVerticalTransformationStep(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as an identify result.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see>.
        /// </returns>
        internal GameEngine.MapView.ArcGISIdentifyLayerResult GetValueAsIdentifyLayerResult()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsIdentifyLayerResult(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.MapView.ArcGISIdentifyLayerResult localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.MapView.ArcGISIdentifyLayerResult(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of an element as a <see cref="GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection">ArcGISIdentifyLayerResultImmutableCollection</see>.
        /// </summary>
        /// <returns>
        /// a <see cref="GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection">ArcGISIdentifyLayerResultImmutableCollection</see>.
        /// </returns>
        internal Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> GetValueAsIdentifyLayerResultImmutableCollection()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsIdentifyLayerResultImmutableCollection(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An int16_t.
        /// </returns>
        internal short GetValueAsInt16()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsInt16(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An int32_t.
        /// </returns>
        internal int GetValueAsInt32()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsInt32(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An int64_t.
        /// </returns>
        internal long GetValueAsInt64()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsInt64(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Returns the element's value as an OAuth user credential.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>.
        /// </returns>
        internal GameEngine.Authentication.ArcGISOAuthUserCredential GetValueAsOAuthUserCredential()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsOAuthUserCredential(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Authentication.ArcGISOAuthUserCredential localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Authentication.ArcGISOAuthUserCredential(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as an OAuth user credential.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Authentication.ArcGISOAuthUserTokenInfo">ArcGISOAuthUserTokenInfo</see>.
        /// </returns>
        internal GameEngine.Authentication.ArcGISOAuthUserTokenInfo GetValueAsOAuthUserTokenInfo()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsOAuthUserTokenInfo(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Authentication.ArcGISOAuthUserTokenInfo localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Authentication.ArcGISOAuthUserTokenInfo(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see>.
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute GetValueAsPointCloudAttribute()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudAttribute(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as point cloud color class break.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak">ArcGISPointCloudColorClassBreak</see>.
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak GetValueAsPointCloudColorClassBreak()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudColorClassBreak(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as a point cloud color stop.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop">ArcGISPointCloudColorStop</see>
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop GetValueAsPointCloudColorStop()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudColorStop(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the element's value as point cloud color unique value.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue">ArcGISPointCloudColorUniqueValue</see>.
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue GetValueAsPointCloudColorUniqueValue()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudColorUniqueValue(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudFilter">ArcGISPointCloudFilter</see>.
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudFilter GetValueAsPointCloudFilter()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudFilter(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.PointCloud.ArcGISPointCloudFilter localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.PointCloud.PInvoke.RT_PointCloudFilter_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.PointCloud.ArcGISPointCloudFilterType.PointCloudBitfieldFilter:
                        localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudBitfieldFilter(localResult);
                        break;
                    case GameEngine.Layers.PointCloud.ArcGISPointCloudFilterType.PointCloudReturnFilter:
                        localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnFilter(localResult);
                        break;
                    case GameEngine.Layers.PointCloud.ArcGISPointCloudFilterType.PointCloudValueFilter:
                        localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudValueFilter(localResult);
                        break;
                    default:
                        localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilter(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see>.
        /// </returns>
        internal GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType GetValueAsPointCloudReturnType()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsPointCloudReturnType(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An <see cref="">string</see>.
        /// </returns>
        internal string GetValueAsString()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsString(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An uint16_t.
        /// </returns>
        internal ushort GetValueAsUInt16()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsUInt16(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An uint32_t.
        /// </returns>
        internal uint GetValueAsUInt32()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsUInt32(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Gets the value of the element.
        /// </summary>
        /// <returns>
        /// An uint64_t.
        /// </returns>
        internal ulong GetValueAsUInt64()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Element_getValueAsUInt64(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Sets the value of the element from an ArcGIS credential.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromArcGISCredential(GameEngine.Authentication.ArcGISCredential value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromArcGISCredential(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from an ArcGIS token info.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromArcGISTokenInfo(GameEngine.Authentication.ArcGISTokenInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromArcGISTokenInfo(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of this element to be an array.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromArray(Unity.ArcGISImmutableArray<object> value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromArray(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromDateTime(DateTimeOffset value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            using var localValue = Unity.Convert.ToArcGISDateTime(value);
            
            PInvoke.RT_Element_setValueFromDateTime(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element to an <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        internal void SetValueFromDatumTransformation(GameEngine.Geometry.ArcGISDatumTransformation value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromDatumTransformation(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of this element to be a dictionary.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromDictionary(Unity.ArcGISDictionaryBase value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromDictionary(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of an element to a feature.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromFeature(GameEngine.Data.Feature value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromFeature(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromFloat32(float value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromFloat32(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromFloat64(double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromFloat64(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from an attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromGEAttribute(GameEngine.Attributes.ArcGISAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromGEAttribute(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element to an <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        internal void SetValueFromGeographicTransformationStep(GameEngine.Geometry.ArcGISGeographicTransformationStep value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromGeographicTransformationStep(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromGeometry(GameEngine.Geometry.ArcGISGeometry value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromGeometry(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from a visualization attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromGEVisualizationAttribute(GameEngine.Attributes.ArcGISVisualizationAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromGEVisualizationAttribute(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from a visualization attribute description.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromGEVisualizationAttributeDescription(GameEngine.Attributes.ArcGISVisualizationAttributeDescription value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromGEVisualizationAttributeDescription(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromGUID(Guid value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            using var localValue = Unity.Convert.ToArcGISGUID(value);
            
            PInvoke.RT_Element_setValueFromGUID(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element to an <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> instance.
        /// </summary>
        /// <param name="value">The instance.</param>
        internal void SetValueFromHorizontalVerticalTransformationStep(GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromHorizontalVerticalTransformationStep(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from an identify result.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromIdentifyLayerResult(GameEngine.MapView.ArcGISIdentifyLayerResult value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromIdentifyLayerResult(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromInt16(short value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromInt16(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromInt32(int value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromInt32(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromInt64(long value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromInt64(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from an OAuth user credential.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromOAuthUserCredential(GameEngine.Authentication.ArcGISOAuthUserCredential value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromOAuthUserCredential(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from an OAuth user token info.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromOAuthUserTokenInfo(GameEngine.Authentication.ArcGISOAuthUserTokenInfo value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromOAuthUserTokenInfo(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudAttribute(GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromPointCloudAttribute(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from a point cloud color class breaks.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudColorClassBreak(GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromPointCloudColorClassBreak(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from a point cloud color stop.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudColorStop(GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromPointCloudColorStop(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element from a point cloud color unique value.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudColorUniqueValue(GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromPointCloudColorUniqueValue(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudFilter(GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValue = value.Handle;
            
            PInvoke.RT_Element_setValueFromPointCloudFilter(Handle, localValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromPointCloudReturnType(GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromPointCloudReturnType(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromString(string value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromString(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromUInt16(ushort value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromUInt16(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromUInt32(uint value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromUInt32(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the value of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValueFromUInt64(ulong value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_Element_setValueFromUInt64(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISElement(IntPtr handle) => Handle = handle;
        
        ~ArcGISElement()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Element_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISElement other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISElementType RT_Element_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromArcGISCredential(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromArcGISTokenInfo(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromArray(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromDateTime(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromDatumTransformation(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromDictionary(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromFeature(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromFloat32(float value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromFloat64(double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGEAttribute(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGEBuildingSceneLayerAttributeStatistics(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGeographicTransformationStep(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGeometry(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGEVisualizationAttribute(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGEVisualizationAttributeDescription(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromGUID(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromHorizontalVerticalTransformationStep(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromIdentifyLayerResult(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromIdentifyLayerResultImmutableCollection(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromInt16(short value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromInt32(int value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromInt64(long value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromOAuthUserCredential(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromOAuthUserTokenInfo(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudAttribute(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudColorClassBreak(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudColorStop(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudColorUniqueValue(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudFilter(IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromPointCloudReturnType(GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromString([MarshalAs(UnmanagedType.LPStr)]string value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromUInt16(ushort value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromUInt32(uint value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_fromUInt64(ulong value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsArcGISCredential(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsArcGISTokenInfo(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsArray(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsDateTime(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsDatumTransformation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsDictionary(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsFeature(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern float RT_Element_getValueAsFloat32(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Element_getValueAsFloat64(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGEAttribute(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGEBuildingSceneLayerAttributeStatistics(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGeographicTransformationStep(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGeometry(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGEVisualizationAttribute(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGEVisualizationAttributeDescription(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsGUID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsHorizontalVerticalTransformationStep(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsIdentifyLayerResult(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsIdentifyLayerResultImmutableCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern short RT_Element_getValueAsInt16(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_Element_getValueAsInt32(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern long RT_Element_getValueAsInt64(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsOAuthUserCredential(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsOAuthUserTokenInfo(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsPointCloudAttribute(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsPointCloudColorClassBreak(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsPointCloudColorStop(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsPointCloudColorUniqueValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsPointCloudFilter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType RT_Element_getValueAsPointCloudReturnType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Element_getValueAsString(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ushort RT_Element_getValueAsUInt16(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern uint RT_Element_getValueAsUInt32(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ulong RT_Element_getValueAsUInt64(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromArcGISCredential(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromArcGISTokenInfo(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromArray(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromDateTime(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromDatumTransformation(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromDictionary(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromFeature(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromFloat32(IntPtr handle, float value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromFloat64(IntPtr handle, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGEAttribute(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGeographicTransformationStep(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGeometry(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGEVisualizationAttribute(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGEVisualizationAttributeDescription(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromGUID(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromHorizontalVerticalTransformationStep(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromIdentifyLayerResult(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromInt16(IntPtr handle, short value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromInt32(IntPtr handle, int value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromInt64(IntPtr handle, long value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromOAuthUserCredential(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromOAuthUserTokenInfo(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudAttribute(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudColorClassBreak(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudColorStop(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudColorUniqueValue(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudFilter(IntPtr handle, IntPtr value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromPointCloudReturnType(IntPtr handle, GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromString(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromUInt16(IntPtr handle, ushort value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromUInt32(IntPtr handle, uint value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_setValueFromUInt64(IntPtr handle, ulong value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Element_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}