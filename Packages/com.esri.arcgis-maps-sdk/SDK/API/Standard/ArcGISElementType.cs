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
namespace Esri.Standard
{
    /// <summary>
    /// The different types that an element can hold.
    /// </summary>
    /// <remarks>
    /// Each of the different supported element types. Can get the type by calling <see cref="Standard.ArcGISElement.ObjectType">ArcGISElement.ObjectType</see>.
    /// </remarks>
    /// <seealso cref="Standard.ArcGISElement">ArcGISElement</seealso>
    /// <seealso cref="Standard.ArcGISElement.ObjectType">ArcGISElement.ObjectType</seealso>
    public enum ArcGISElementType
    {
        /// <summary>
        /// The element is currently not holding any value.
        /// </summary>
        None = -1,
        
        /// <summary>
        /// An array.
        /// </summary>
        Array = 4,
        
        /// <summary>
        /// A buffer value.
        /// </summary>
        Buffer = 10,
        
        /// <summary>
        /// A date time value.
        /// </summary>
        DateTime = 19,
        
        /// <summary>
        /// Element holds a dictionary.
        /// </summary>
        Dictionary = 20,
        
        /// <summary>
        /// A feature object.
        /// </summary>
        Feature = 33,
        
        /// <summary>
        /// A 32 bit float value.
        /// </summary>
        Float32 = 42,
        
        /// <summary>
        /// A 64 bit float value.
        /// </summary>
        Float64 = 43,
        
        /// <summary>
        /// A geometry value.
        /// </summary>
        Geometry = 50,
        
        /// <summary>
        /// A GUID value.
        /// </summary>
        GUID = 55,
        
        /// <summary>
        /// An object containing the results of an identify on a layer.
        /// </summary>
        IdentifyLayerResult = 57,
        
        /// <summary>
        /// A 16-bit integer value.
        /// </summary>
        Int16 = 61,
        
        /// <summary>
        /// A 32-bit integer value.
        /// </summary>
        Int32 = 62,
        
        /// <summary>
        /// A 64-bit integer value.
        /// </summary>
        Int64 = 63,
        
        /// <summary>
        /// A string value.
        /// </summary>
        String = 105,
        
        /// <summary>
        /// An unsigned 16-bit integer value.
        /// </summary>
        UInt16 = 118,
        
        /// <summary>
        /// An unsigned 32-bit integer value.
        /// </summary>
        UInt32 = 119,
        
        /// <summary>
        /// An unsigned 64-bit integer value.
        /// </summary>
        UInt64 = 120,
        
        /// <summary>
        /// An variant type.
        /// </summary>
        Variant = 123,
        
        /// <summary>
        /// Element holds a vector.
        /// </summary>
        Vector = 124,
        
        /// <summary>
        /// A <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> object.
        /// </summary>
        DatumTransformation = 212,
        
        /// <summary>
        /// A <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> object.
        /// </summary>
        GeographicTransformationStep = 213,
        
        /// <summary>
        /// A <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> object.
        /// </summary>
        HorizontalVerticalTransformationStep = 214,
        
        /// <summary>
        /// A <see cref="GameEngine.Attributes.ArcGISVisualizationAttributeDescription">ArcGISVisualizationAttributeDescription</see> object.
        /// </summary>
        GEVisualizationAttributeDescription = 257,
        
        /// <summary>
        /// A <see cref="GameEngine.Attributes.ArcGISVisualizationAttribute">ArcGISVisualizationAttribute</see> object.
        /// </summary>
        GEVisualizationAttribute = 258,
        
        /// <summary>
        /// A <see cref="GameEngine.Attributes.ArcGISAttribute">ArcGISAttribute</see> object.
        /// </summary>
        GEAttribute = 259,
        
        /// <summary>
        /// An <see cref="GameEngine.Authentication.ArcGISTokenInfo">ArcGISTokenInfo</see> object.
        /// </summary>
        ArcGISTokenInfo = 277,
        
        /// <summary>
        /// An <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see> object.
        /// </summary>
        ArcGISCredential = 283,
        
        /// <summary>
        /// An <see cref="GameEngine.Authentication.ArcGISOAuthUserTokenInfo">ArcGISOAuthUserTokenInfo</see> object.
        /// </summary>
        OAuthUserTokenInfo = 284,
        
        /// <summary>
        /// An <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see> object.
        /// </summary>
        OAuthUserCredential = 285,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</see> object.
        /// </summary>
        GEBuildingSceneLayerAttributeStatistics = 297,
        
        /// <summary>
        /// A <see cref="GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection">ArcGISIdentifyLayerResultImmutableCollection</see> object.
        /// </summary>
        IdentifyLayerResultImmutableCollection = 340,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute">ArcGISPointCloudAttribute</see> object.
        /// </summary>
        PointCloudAttribute = 365,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak">ArcGISPointCloudColorClassBreak</see> object.
        /// </summary>
        PointCloudColorClassBreak = 366,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop">ArcGISPointCloudColorStop</see> object.
        /// </summary>
        PointCloudColorStop = 367,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue">ArcGISPointCloudColorUniqueValue</see> object.
        /// </summary>
        PointCloudColorUniqueValue = 368,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudFilter">ArcGISPointCloudFilter</see> object.
        /// </summary>
        PointCloudFilter = 369,
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType">ArcGISPointCloudReturnType</see> object.
        /// </summary>
        PointCloudReturnType = 370
    };
}