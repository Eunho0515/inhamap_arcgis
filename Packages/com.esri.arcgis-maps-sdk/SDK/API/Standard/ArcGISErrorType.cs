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
    /// The list of possible generic errors.
    /// </summary>
    /// <remarks>
    /// This is used in the <see cref="GameEngine.ArcGISRuntimeEnvironmentErrorEvent">ArcGISRuntimeEnvironmentErrorEvent</see> error handler function.
    /// </remarks>
    /// <seealso cref="GameEngine.ArcGISRuntimeEnvironmentErrorEvent">ArcGISRuntimeEnvironmentErrorEvent</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISErrorType
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        /// <remarks>
        /// The catch-all for unknown errors.
        /// </remarks>
        /// <since>1.0.0</since>
        Unknown = -1,
        
        /// <summary>
        /// Success.
        /// </summary>
        /// <remarks>
        /// Indicates success, not an error.
        /// </remarks>
        /// <since>1.0.0</since>
        Success = 0,
        
        /// <summary>
        /// A null pointer.
        /// </summary>
        /// <since>1.0.0</since>
        CommonNullPtr = 1,
        
        /// <summary>
        /// Invalid argument.
        /// </summary>
        /// <since>1.0.0</since>
        CommonInvalidArgument = 2,
        
        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <since>1.0.0</since>
        CommonNotImplemented = 3,
        
        /// <summary>
        /// Out of range.
        /// </summary>
        /// <since>1.0.0</since>
        CommonOutOfRange = 4,
        
        /// <summary>
        /// Invalid access.
        /// </summary>
        /// <since>1.0.0</since>
        CommonInvalidAccess = 5,
        
        /// <summary>
        /// Illegal state.
        /// </summary>
        /// <since>1.0.0</since>
        CommonIllegalState = 6,
        
        /// <summary>
        /// Not found.
        /// </summary>
        /// <since>1.0.0</since>
        CommonNotFound = 7,
        
        /// <summary>
        /// Entity exists.
        /// </summary>
        /// <since>1.0.0</since>
        CommonExists = 8,
        
        /// <summary>
        /// Timeout.
        /// </summary>
        /// <since>1.0.0</since>
        CommonTimeout = 9,
        
        /// <summary>
        /// Regular expression error.
        /// </summary>
        /// <since>1.0.0</since>
        CommonRegularExpression = 10,
        
        /// <summary>
        /// Property not supported.
        /// </summary>
        /// <since>1.0.0</since>
        CommonPropertyNotSupported = 11,
        
        /// <summary>
        /// No permission.
        /// </summary>
        /// <since>1.0.0</since>
        CommonNoPermission = 12,
        
        /// <summary>
        /// File error.
        /// </summary>
        /// <since>1.0.0</since>
        CommonFile = 13,
        
        /// <summary>
        /// File not found.
        /// </summary>
        /// <since>1.0.0</since>
        CommonFileNotFound = 14,
        
        /// <summary>
        /// Invalid call.
        /// </summary>
        /// <since>1.0.0</since>
        CommonInvalidCall = 15,
        
        /// <summary>
        /// IO error.
        /// </summary>
        /// <since>1.0.0</since>
        CommonIO = 16,
        
        /// <summary>
        /// User canceled.
        /// </summary>
        /// <since>1.0.0</since>
        CommonUserCanceled = 17,
        
        /// <summary>
        /// Internal error.
        /// </summary>
        /// <since>1.0.0</since>
        CommonInternalError = 18,
        
        /// <summary>
        /// Conversion failed.
        /// </summary>
        /// <since>1.0.0</since>
        CommonConversionFailed = 19,
        
        /// <summary>
        /// No data.
        /// </summary>
        /// <since>1.0.0</since>
        CommonNoData = 20,
        
        /// <summary>
        /// Invalid JSON.
        /// </summary>
        /// <since>1.0.0</since>
        CommonInvalidJSON = 21,
        
        /// <summary>
        /// Propagated error.
        /// </summary>
        /// <since>1.0.0</since>
        CommonUserDefinedFailure = 22,
        
        /// <summary>
        /// Invalid XML.
        /// </summary>
        /// <since>1.0.0</since>
        CommonBadXML = 23,
        
        /// <summary>
        /// Object is already owned.
        /// </summary>
        /// <since>1.0.0</since>
        CommonObjectAlreadyOwned = 24,
        
        /// <summary>
        /// The resource is past its expiry date.
        /// </summary>
        /// <since>1.0.0</since>
        CommonExpired = 26,
        
        /// <summary>
        /// Nullability violation.
        /// </summary>
        /// <remarks>
        /// A null was returned from a property or method which is
        /// expected to be non-nullable.
        /// </remarks>
        /// <since>1.0.0</since>
        CommonNullabilityViolation = 27,
        
        /// <summary>
        /// Invalid property.
        /// </summary>
        /// <remarks>
        /// The value of a property is invalid.
        /// </remarks>
        /// <since>1.0.0</since>
        CommonInvalidProperty = 28,
        
        /// <summary>
        /// Unknown geometry error.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryUnknownError = 2000,
        
        /// <summary>
        /// Corrupt geometry.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryCorruptedGeometry = 2001,
        
        /// <summary>
        /// Empty geometry.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryEmptyGeometry = 2002,
        
        /// <summary>
        /// Math singularity.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryMathSingularity = 2003,
        
        /// <summary>
        /// Geometry buffer too small.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryBufferIsTooSmall = 2004,
        
        /// <summary>
        /// Geometry invalid shape type.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryInvalidShapeType = 2005,
        
        /// <summary>
        /// Geometry projection out of supported range.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryProjectionOutOfSupportedRange = 2006,
        
        /// <summary>
        /// Non simple geometry.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryNonSimpleGeometry = 2007,
        
        /// <summary>
        /// Cannot calculate geodesic.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryCannotCalculateGeodesic = 2008,
        
        /// <summary>
        /// Geometry notation conversion.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryNotationConversion = 2009,
        
        /// <summary>
        /// Missing grid file.
        /// </summary>
        /// <since>1.0.0</since>
        GeometryMissingGridFile = 2010,
        
        /// <summary>
        /// Geocode unsupported file format.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeUnsupportedFileFormat = 4001,
        
        /// <summary>
        /// Geocode unsupported spatial reference.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeUnsupportedSpatialReference = 4002,
        
        /// <summary>
        /// Geocode unsupported projection transformation.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeUnsupportedProjectionTransformation = 4003,
        
        /// <summary>
        /// Geocoder creation error.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeGeocoderCreation = 4004,
        
        /// <summary>
        /// Geocode intersections not supported.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeIntersectionsNotSupported = 4005,
        
        /// <summary>
        /// Uninitialized geocode task.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeUninitializedGeocodeTask = 4006,
        
        /// <summary>
        /// Invalid geocode locator properties.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeInvalidLocatorProperties = 4007,
        
        /// <summary>
        /// Geocode required field missing.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeRequiredFieldMissing = 4008,
        
        /// <summary>
        /// Geocode cannot read address.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeCannotReadAddress = 4009,
        
        /// <summary>
        /// Geocoding not supported.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeReverseGeocodingNotSupported = 4010,
        
        /// <summary>
        /// Geocode geometry type not supported.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeGeometryTypeNotSupported = 4011,
        
        /// <summary>
        /// Geocode suggest address not supported.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeSuggestAddressNotSupported = 4012,
        
        /// <summary>
        /// Geocode suggest result corrupt.
        /// </summary>
        /// <since>1.0.0</since>
        GeocodeSuggestResultCorrupted = 4013,
        
        /// <summary>
        /// JSON parser invalid token.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidToken = 6001,
        
        /// <summary>
        /// JSON parser invalid character.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidCharacter = 6002,
        
        /// <summary>
        /// JSON parser invalid unicode.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidUnicode = 6003,
        
        /// <summary>
        /// JSON parser invalid start of JSON.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidJSONStart = 6004,
        
        /// <summary>
        /// JSON parser invalid end of pair.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidEndOfPair = 6005,
        
        /// <summary>
        /// JSON parser invalid end of element.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidEndOfElement = 6006,
        
        /// <summary>
        /// JSON parser invalid escape sequence.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidEscapeSequence = 6007,
        
        /// <summary>
        /// JSON parser invalid end of field name.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidEndOfFieldName = 6008,
        
        /// <summary>
        /// JSON parser invalid start of field name.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidStartOfFieldName = 6009,
        
        /// <summary>
        /// JSON parser invalid start of comment.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidStartOfComment = 6010,
        
        /// <summary>
        /// JSON parser invalid decimal digit.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidDecDigit = 6011,
        
        /// <summary>
        /// JSON parser invalid hex digit.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserInvalidHexDigit = 6012,
        
        /// <summary>
        /// JSON parser expecting null.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingNull = 6013,
        
        /// <summary>
        /// JSON parser expecting true.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingTrue = 6014,
        
        /// <summary>
        /// JSON parser expecting false.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingFalse = 6015,
        
        /// <summary>
        /// JSON parser expecting closing quote.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingClosingQuote = 6016,
        
        /// <summary>
        /// JSON parser expecting not a number.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingNan = 6017,
        
        /// <summary>
        /// JSON parser expecting end of comment.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserExpectingEndOfComment = 6018,
        
        /// <summary>
        /// JSON parser unexpected end of data.
        /// </summary>
        /// <since>1.0.0</since>
        JSONParserUnexpectedEndOfData = 6019,
        
        /// <summary>
        /// JSON object expecting start object.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingStartObject = 6020,
        
        /// <summary>
        /// JSON object expecting start array.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingStartArray = 6021,
        
        /// <summary>
        /// JSON object expecting value object.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingValueObject = 6022,
        
        /// <summary>
        /// JSON object expecting value array.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingValueArray = 6023,
        
        /// <summary>
        /// JSON object expecting value int32.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingValueInt32 = 6024,
        
        /// <summary>
        /// JSON object expecting integer type.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingIntegerType = 6025,
        
        /// <summary>
        /// JSON object expecting number type.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingNumberType = 6026,
        
        /// <summary>
        /// JSON object expecting value string.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingValueString = 6027,
        
        /// <summary>
        /// JSON object expecting value bool.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectExpectingValueBool = 6028,
        
        /// <summary>
        /// JSON object iterator not started.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectIteratorNotStarted = 6029,
        
        /// <summary>
        /// JSON object iterator is finished.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectIteratorIsFinished = 6030,
        
        /// <summary>
        /// JSON object key not found.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectKeyNotFound = 6031,
        
        /// <summary>
        /// JSON object index out of range.
        /// </summary>
        /// <since>1.0.0</since>
        JSONObjectIndexOutOfRange = 6032,
        
        /// <summary>
        /// JSON string writer JSON is complete.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterJSONIsComplete = 6033,
        
        /// <summary>
        /// JSON string writer invalid JSON input.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterInvalidJSONInput = 6034,
        
        /// <summary>
        /// JSON string writer expecting container.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterExpectingContainer = 6035,
        
        /// <summary>
        /// JSON string writer expecting key or end object.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterExpectingKeyOrEndObject = 6036,
        
        /// <summary>
        /// JSON string writer expecting value or end array.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterExpectingValueOrEndArray = 6037,
        
        /// <summary>
        /// JSON string writer expecting value.
        /// </summary>
        /// <since>1.0.0</since>
        JSONStringWriterExpectingValue = 6038,
        
        /// <summary>
        /// Spatial reference is missing.
        /// </summary>
        /// <since>1.0.0</since>
        MappingMissingSpatialReference = 7001,
        
        /// <summary>
        /// Initial viewpoint is missing.
        /// </summary>
        /// <since>1.0.0</since>
        MappingMissingInitialViewpoint = 7002,
        
        /// <summary>
        /// Invalid request response.
        /// </summary>
        /// <since>1.0.0</since>
        MappingInvalidResponse = 7003,
        
        /// <summary>
        /// Bing maps key is missing.
        /// </summary>
        /// <since>1.0.0</since>
        MappingMissingBingMapsKey = 7004,
        
        /// <summary>
        /// Layer type is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingUnsupportedLayerType = 7005,
        
        /// <summary>
        /// Sync not enabled.
        /// </summary>
        /// <since>1.0.0</since>
        MappingSyncNotEnabled = 7006,
        
        /// <summary>
        /// Tile export is not enabled.
        /// </summary>
        /// <since>1.0.0</since>
        MappingTileExportNotEnabled = 7007,
        
        /// <summary>
        /// Required item property is missing.
        /// </summary>
        /// <since>1.0.0</since>
        MappingMissingItemProperty = 7008,
        
        /// <summary>
        /// Web map version is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingWebmapNotSupported = 7009,
        
        /// <summary>
        /// Spatial reference invalid or incompatible.
        /// </summary>
        /// <since>1.0.0</since>
        MappingSpatialReferenceInvalid = 7011,
        
        /// <summary>
        /// Package needs to be unpacked before it can be used.
        /// </summary>
        /// <since>1.0.0</since>
        MappingPackageUnpackRequired = 7012,
        
        /// <summary>
        /// Elevation source data format is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingUnsupportedElevationFormat = 7013,
        
        /// <summary>
        /// Web scene version or viewing mode is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingWebsceneNotSupported = 7014,
        
        /// <summary>
        /// Loadable object is not loaded when it is expected to be loaded.
        /// </summary>
        /// <since>1.0.0</since>
        MappingNotLoaded = 7015,
        
        /// <summary>
        /// Update packages for an offline map area are not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingScheduledUpdatesNotSupported = 7016,
        
        /// <summary>
        /// Trace operation failed.
        /// </summary>
        /// <since>1.0.0</since>
        MappingUtilityNetworkTraceFailed = 7017,
        
        /// <summary>
        /// Arcade expression is invalid.
        /// </summary>
        /// <since>1.0.0</since>
        MappingInvalidArcadeExpression = 7018,
        
        /// <summary>
        /// Requested extent contains too many associations.
        /// </summary>
        /// <since>1.0.0</since>
        MappingUtilityNetworkTooManyAssociations = 7019,
        
        /// <summary>
        /// A layer has requested more features than the service maximum feature count.
        /// </summary>
        /// <since>1.0.0</since>
        MappingMaxFeatureCountExceeded = 7020,
        
        /// <summary>
        /// Feature service does not support branch versioning.
        /// </summary>
        /// <since>1.0.0</since>
        MappingBranchVersioningNotSupportedByService = 7021,
        
        /// <summary>
        /// Packaging of data for the offline map area is not complete and it is not ready for download.
        /// </summary>
        /// <since>1.0.0</since>
        MappingPackagingNotComplete = 7022,
        
        /// <summary>
        /// An upload sync direction is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingSyncDirectionUploadNotSupported = 7023,
        
        /// <summary>
        /// Tile export in .tpkx format is not supported.
        /// </summary>
        /// <since>1.0.0</since>
        MappingTileCacheCompactV2ExportNotEnabled = 7024,
        
        /// <summary>
        /// The specified layer does not intersect the area of interest.
        /// </summary>
        /// <since>1.0.0</since>
        MappingLayerDoesNotIntersectAreaOfInterest = 7025,
        
        /// <summary>
        /// Local edits must be sent to a service (using a sync direction of upload) before update packages can download a replacement geodatabase.
        /// </summary>
        /// <since>1.0.0</since>
        MappingScheduledUpdateUploadRequired = 7026,
        
        /// <summary>
        /// Portal user with no license.
        /// </summary>
        /// <since>1.0.0</since>
        LicensingPortalUserWithNoLicense = 8006,
        
        /// <summary>
        /// IO error.
        /// </summary>
        /// <since>1.0.0</since>
        StdIOSBaseFailure = 10001,
        
        /// <summary>
        /// Invalid array length.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadArrayNewLength = 10002,
        
        /// <summary>
        /// Arithmetic underflow.
        /// </summary>
        /// <since>1.0.0</since>
        StdUnderflowError = 10003,
        
        /// <summary>
        /// System error.
        /// </summary>
        /// <since>1.0.0</since>
        StdSystemError = 10004,
        
        /// <summary>
        /// Range error.
        /// </summary>
        /// <since>1.0.0</since>
        StdRangeError = 10005,
        
        /// <summary>
        /// Arithmetic overflow.
        /// </summary>
        /// <since>1.0.0</since>
        StdOverflowError = 10006,
        
        /// <summary>
        /// Out of range.
        /// </summary>
        /// <since>1.0.0</since>
        StdOutOfRange = 10007,
        
        /// <summary>
        /// Length error.
        /// </summary>
        /// <since>1.0.0</since>
        StdLengthError = 10008,
        
        /// <summary>
        /// Invalid argument.
        /// </summary>
        /// <since>1.0.0</since>
        StdInvalidArgument = 10009,
        
        /// <summary>
        /// Asynchronous error.
        /// </summary>
        /// <since>1.0.0</since>
        StdFutureError = 10010,
        
        /// <summary>
        /// Math domain error.
        /// </summary>
        /// <since>1.0.0</since>
        StdDomainError = 10011,
        
        /// <summary>
        /// Unknown error.
        /// </summary>
        /// <since>1.0.0</since>
        StdRuntimeError = 10012,
        
        /// <summary>
        /// Logic error.
        /// </summary>
        /// <since>1.0.0</since>
        StdLogicError = 10013,
        
        /// <summary>
        /// Invalid weak reference.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadWeakPtr = 10014,
        
        /// <summary>
        /// Invalid type Id.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadTypeId = 10015,
        
        /// <summary>
        /// Invalid function call.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadFunctionCall = 10016,
        
        /// <summary>
        /// Invalid error management.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadException = 10017,
        
        /// <summary>
        /// Invalid cast.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadCast = 10018,
        
        /// <summary>
        /// Out of memory.
        /// </summary>
        /// <since>1.0.0</since>
        StdBadAlloc = 10019,
        
        /// <summary>
        /// Unknown error.
        /// </summary>
        /// <since>1.0.0</since>
        StdException = 10020,
        
        /// <summary>
        /// Service doesn't support rerouting.
        /// </summary>
        /// <since>1.0.0</since>
        NavigationReroutingNotSupportedByService = 13001,
        
        /// <summary>
        /// HTTP client operation canceled.
        /// </summary>
        /// <since>1.0.0</since>
        HTTPClientOperationCanceled = 14001,
        
        /// <summary>
        /// HTTP client timed out.
        /// </summary>
        /// <since>1.0.0</since>
        HTTPClientTimedOut = 14002,
        
        /// <summary>
        /// HTTP client unsupported method.
        /// </summary>
        /// <since>1.0.0</since>
        HTTPClientUnsupportedMethod = 14003,
        
        /// <summary>
        /// HTTP client unsupported protocol scheme.
        /// </summary>
        /// <since>1.0.0</since>
        HTTPClientUnsupportedProtocolScheme = 14004,
        
        /// <summary>
        /// A problem was encountered with a <see cref="">GeotriggerFeed</see>.
        /// </summary>
        /// <remarks>
        /// An invalid <see cref="">GeotriggerFeed</see> indicates that a <see cref="">GeotriggerMonitor</see> is unable to
        /// perform checks. No <see cref="">GeotriggerNotificationInfo</see> events will be sent.
        /// </remarks>
        /// <since>1.0.0</since>
        GeotriggersFeedError = 16001,
        
        /// <summary>
        /// A problem was encountered with the <see cref="">FenceParameters</see> for a <see cref="">FenceGeotrigger</see>.
        /// </summary>
        /// <remarks>
        /// An invalid <see cref="">FenceParameters</see> indicates that a <see cref="">GeotriggerMonitor</see> is
        /// unable to perform checks. No <see cref="">GeotriggerNotificationInfo</see> events will be sent.
        /// </remarks>
        /// <since>1.0.0</since>
        GeotriggersFenceParametersError = 16002,
        
        /// <summary>
        /// A problem was encountered with the fence data for a <see cref="">Geotrigger</see>.
        /// </summary>
        /// <remarks>
        /// There is a problem with some of the fence data and these will not be checked by a
        /// <see cref="">GeotriggerMonitor</see>. However, other data is valid and so
        /// <see cref="">GeotriggerNotificationInfo</see> events can be sent.
        /// </remarks>
        /// <since>1.0.0</since>
        GeotriggersFenceDataWarning = 16003,
        
        /// <summary>
        /// Invalid credentials, unable to generate token.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationInvalidCredentials = 18001,
        
        /// <summary>
        /// Unable to determine generate token URL.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationUnableToDetermineTokenURL = 18002,
        
        /// <summary>
        /// Token has expired.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationTokenExpired = 18003,
        
        /// <summary>
        /// A token or API key is required.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationTokenRequired = 18004,
        
        /// <summary>
        /// Invalid API key.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationInvalidAPIKey = 18005,
        
        /// <summary>
        /// Invalid token.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationInvalidToken = 18006,
        
        /// <summary>
        /// You do not have permission to access the resource.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationForbidden = 18007,
        
        /// <summary>
        /// Secure socket layer required.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationSSLRequired = 18008,
        
        /// <summary>
        /// The credential cannot be shared with the request URL.
        /// </summary>
        /// <since>1.1.0</since>
        AuthenticationCredentialCannotBeShared = 18009,
        
        /// <summary>
        /// The authorization end point responded with a failure.
        /// </summary>
        /// <since>1.2.0</since>
        AuthenticationOAuthFailure = 18010,
        
        /// <summary>
        /// The authentication challenge was canceled.
        /// </summary>
        /// <since>1.3.0</since>
        AuthenticationChallengeCanceled = 18011,
        
        /// <summary>
        /// An error has occurred while creating an <see cref="">IAPCredential</see>.
        /// </summary>
        /// <since>2.1.0</since>
        AuthenticationIAPFailure = 18012,
        
        /// <summary>
        /// An <see cref="">IAPCredential</see> is required to access the resource.
        /// </summary>
        /// <since>2.1.0</since>
        AuthenticationIAPCredentialRequired = 18013,
        
        /// <summary>
        /// An <see cref="">IAPCredential</see> used to access the resource is no longer valid.
        /// </summary>
        /// <since>2.1.0</since>
        AuthenticationInvalidIAPCredential = 18014,
        
        /// <summary>
        /// The required credential for the federating portal is missing. It was not provided, and no stored credential could be found in the ArcGIS credential store.
        /// </summary>
        /// <since>2.3.0</since>
        AuthenticationPortalCredentialMissing = 18015,
        
        /// <summary>
        /// The specified service for generating a federated token is not federated with any portal.
        /// </summary>
        /// <since>2.3.0</since>
        AuthenticationServiceNotFederated = 18016,
        
        /// <summary>
        /// The number of attachments added exceeds the maximum specified by 
        /// <see cref="">AttachmentsFormElement.maxAttachmentCount</see>.
        /// </summary>
        /// <since>2.4.0</since>
        FeatureFormExceedsMaximumAttachmentCountError = 20010,
        
        /// <summary>
        /// The attachment exceeds the maximum duration specified by <see cref="">AudioFormInput.maxDuration</see> or
        /// <see cref="">VideoFormInput.maxDuration</see>.
        /// </summary>
        /// <since>2.4.0</since>
        FeatureFormExceedsMaximumAttachmentDurationError = 20011,
        
        /// <summary>
        /// The attachment exceeds the maximum size specified by <see cref="">DocumentFormInput.maxFileSize</see>.
        /// </summary>
        /// <since>2.4.0</since>
        FeatureFormExceedsMaximumAttachmentSizeError = 20012,
        
        /// <summary>
        /// The type of the <see cref="">FormAttachment</see> does not match an allowed <see cref="">AttachmentsFormInputType</see>.
        /// </summary>
        /// <since>2.4.0</since>
        FeatureFormIncorrectAttachmentTypeError = 20013,
        
        /// <summary>
        /// The number of attachments added is less than the minimum specified by 
        /// <see cref="">AttachmentsFormElement.minAttachmentCount</see>.
        /// </summary>
        /// <since>2.4.0</since>
        FeatureFormLessThanMinimumAttachmentCountError = 20014,
        
        /// <summary>
        /// A critical error occurred while rendering and the view is in an unusable state.
        /// </summary>
        /// <remarks>
        /// Restarting the application is recommended to potentially resolve this problem.
        /// </remarks>
        /// <since>2.3.0</since>
        CriticalRenderingError = 21000,
        
        /// <summary>
        /// The graphics driver does not meet the Maps SDK's minimum requirements.
        /// </summary>
        /// <remarks>
        /// The graphics driver must at least meet the minimum requirements.
        /// </remarks>
        /// <since>2.3.0</since>
        CriticalRenderingRequirementsNotMetError = 21001,
        
        /// <summary>
        /// An error has occurred while trying to load the map. Check the load error on the map for more information.
        /// </summary>
        /// <since>2.3.0</since>
        GeoModelLoadError = 22000,
        
        /// <summary>
        /// The map's viewing mode is not compatible with the view it is being used with.
        /// </summary>
        /// <since>2.3.0</since>
        GeoModelIncompatibleViewingModeError = 22001,
        
        /// <summary>
        /// The map has incompatible vertical and horizontal datums.
        /// </summary>
        /// <since>2.3.0</since>
        GeoModelIncompatibleVerticalAndHorizontalDatumsError = 22002,
        
        /// <summary>
        /// The map has an incompatible spatial reference: global mode requires a geographic coordinate system, while local mode requires a projected coordinate system.
        /// </summary>
        /// <since>2.3.0</since>
        GeoModelIncompatibleSpatialReferenceError = 22003,
        
        /// <summary>
        /// The spatial reference has no vertical coordinate system.
        /// </summary>
        /// <remarks>
        /// In this situation, the Maps SDKs will choose how to interpret the vertical units.
        /// </remarks>
        /// <since>2.3.0</since>
        EmptyVerticalCoordinateSystemWarning = 23000,
        
        /// <summary>
        /// The scene's clipping area cannot be projected so no clipping area is used.
        /// </summary>
        /// <since>2.3.0</since>
        ClippingAreaNotProjectableWarning = 23001,
        
        /// <summary>
        /// A warning has been received during rendering.
        /// </summary>
        /// <since>2.3.0</since>
        RenderingWarning = 23002
    };
}