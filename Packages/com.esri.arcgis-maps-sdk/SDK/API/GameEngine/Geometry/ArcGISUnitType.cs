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
namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// The different types of unit.
    /// </summary>
    /// <remarks>
    /// Each of different supported unit types. Can get the type by calling <see cref="GameEngine.Geometry.ArcGISUnit.ObjectType">ArcGISUnit.ObjectType</see>.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISUnit.ObjectType">ArcGISUnit.ObjectType</seealso>
    public enum ArcGISUnitType
    {
        /// <summary>
        /// A linear unit of measure.
        /// </summary>
        LinearUnit = 0,
        
        /// <summary>
        /// An angular unit of measure.
        /// </summary>
        AngularUnit = 1,
        
        /// <summary>
        /// An area unit of measure.
        /// </summary>
        AreaUnit = 2,
        
        /// <summary>
        /// An unknown unit type.
        /// </summary>
        Unknown = -1
    };
}