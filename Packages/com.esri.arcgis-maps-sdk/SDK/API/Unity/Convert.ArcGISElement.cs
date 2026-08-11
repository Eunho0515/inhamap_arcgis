// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using System;
using System.Reflection;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static object FromArcGISElement(Standard.ArcGISElement element)
		{
			var type = element.ObjectType;

			object result;

			switch (type)
			{
				case Standard.ArcGISElementType.DatumTransformation:
					result = element.GetValueAsDatumTransformation();
					break;
				case Standard.ArcGISElementType.Dictionary:
					result = element.GetValueAsDictionary();
					break;
				case Standard.ArcGISElementType.Float64:
					result = element.GetValueAsFloat64();
					break;
				case Standard.ArcGISElementType.GEAttribute:
					result = element.GetValueAsGEAttribute();
					break;
				case Standard.ArcGISElementType.GEBuildingSceneLayerAttributeStatistics:
					result = element.GetValueAsGEBuildingSceneLayerAttributeStatistics();
					break;
				case Standard.ArcGISElementType.GeographicTransformationStep:
					result = element.GetValueAsGeographicTransformationStep();
					break;
				case Standard.ArcGISElementType.Geometry:
					result = element.GetValueAsGeometry();
					break;
				case Standard.ArcGISElementType.GEVisualizationAttribute:
					result = element.GetValueAsGEVisualizationAttribute();
					break;
				case Standard.ArcGISElementType.GEVisualizationAttributeDescription:
					result = element.GetValueAsGEVisualizationAttributeDescription();
					break;
				case Standard.ArcGISElementType.HorizontalVerticalTransformationStep:
					result = element.GetValueAsHorizontalVerticalTransformationStep();
					break;
				case Standard.ArcGISElementType.OAuthUserCredential:
					result = element.GetValueAsOAuthUserCredential();
					break;
				case Standard.ArcGISElementType.ArcGISCredential:
					result = element.GetValueAsArcGISCredential();
					break;
				case Standard.ArcGISElementType.OAuthUserTokenInfo:
					result = element.GetValueAsOAuthUserTokenInfo();
					break;
				case Standard.ArcGISElementType.ArcGISTokenInfo:
					result = element.GetValueAsArcGISTokenInfo();
					break;
				case Standard.ArcGISElementType.String:
					result = element.GetValueAsString();
					break;
				case Standard.ArcGISElementType.GUID:
					result = element.GetValueAsGUID();
					break;
				case Standard.ArcGISElementType.DateTime:
					result = element.GetValueAsDateTime();
					break;
				case Standard.ArcGISElementType.Float32:
					result = element.GetValueAsFloat32();
					break;
				case Standard.ArcGISElementType.Int16:
					result = element.GetValueAsInt16();
					break;
				case Standard.ArcGISElementType.Int32:
					result = element.GetValueAsInt32();
					break;
				case Standard.ArcGISElementType.Int64:
					result = element.GetValueAsInt64();
					break;
				case Standard.ArcGISElementType.UInt16:
					result = element.GetValueAsUInt16();
					break;
				case Standard.ArcGISElementType.UInt32:
					result = element.GetValueAsUInt32();
					break;
				case Standard.ArcGISElementType.UInt64:
					result = element.GetValueAsUInt64();
					break;
				case Standard.ArcGISElementType.IdentifyLayerResultImmutableCollection:
					result = element.GetValueAsIdentifyLayerResultImmutableCollection();
					break;
				case Standard.ArcGISElementType.IdentifyLayerResult:
					result = element.GetValueAsIdentifyLayerResult();
					break;
				case Standard.ArcGISElementType.Feature:
					result = element.GetValueAsFeature();
					break;
				default:
					throw new InvalidCastException();
			}

			return result;
		}

		internal static T FromArcGISElement<T>(Standard.ArcGISElement element)
		{
			if (element == null)
			{
				return default;
			}

			if (typeof(T).IsSubclassOf(typeof(ArcGISDictionaryBase)))
			{
				var dictionaryBase = FromArcGISElement<ArcGISDictionaryBase>(element);

				var dictionary = (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { dictionaryBase.Handle }, null);

				dictionaryBase.Handle = IntPtr.Zero;

				return dictionary;
			}
			else if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
			{
				return (T)System.Convert.ChangeType(FromArcGISElement(element), typeof(T));
			}
			else
			{
				// Direct cast is needed for objects like DateTime and Guid that don't implement IConvertible
				return (T)FromArcGISElement(element);
			}
		}

		internal static Standard.ArcGISElement ToArcGISElement<T>(T value)
		{
			Standard.ArcGISElement result;

			switch (value)
			{
				case ArcGISDictionaryBase converted:
					result = Standard.ArcGISElement.FromDictionary(converted);
					break;
				case GameEngine.Attributes.ArcGISAttribute converted:
					result = Standard.ArcGISElement.FromGEAttribute(converted);
					break;
				case GameEngine.Geometry.ArcGISDatumTransformation converted:
					result = Standard.ArcGISElement.FromDatumTransformation(converted);
					break;
				case double converted:
					result = Standard.ArcGISElement.FromFloat64(converted);
					break;
				case GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics converted:
					result = Standard.ArcGISElement.FromGEBuildingSceneLayerAttributeStatistics(converted);
					break;
				case GameEngine.Geometry.ArcGISGeographicTransformationStep converted:
					result = Standard.ArcGISElement.FromGeographicTransformationStep(converted);
					break;
				case GameEngine.Geometry.ArcGISGeometry converted:
					result = Standard.ArcGISElement.FromGeometry(converted);
					break;
				case GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep converted:
					result = Standard.ArcGISElement.FromHorizontalVerticalTransformationStep(converted);
					break;
				case GameEngine.Authentication.ArcGISOAuthUserCredential converted:
					result = Standard.ArcGISElement.FromOAuthUserCredential(converted);
					break;
				case GameEngine.Authentication.ArcGISOAuthUserTokenInfo converted:
					result = Standard.ArcGISElement.FromOAuthUserTokenInfo(converted);
					break;
				case GameEngine.Authentication.ArcGISTokenInfo converted:
					result = Standard.ArcGISElement.FromArcGISTokenInfo(converted);
					break;
				case GameEngine.Authentication.ArcGISCredential converted:
					result = Standard.ArcGISElement.FromArcGISCredential(converted);
					break;
				case string converted:
					result = Standard.ArcGISElement.FromString(converted);
					break;
				case GameEngine.Attributes.ArcGISVisualizationAttribute converted:
					result = Standard.ArcGISElement.FromGEVisualizationAttribute(converted);
					break;
				case GameEngine.Attributes.ArcGISVisualizationAttributeDescription converted:
					result = Standard.ArcGISElement.FromGEVisualizationAttributeDescription(converted);
					break;
				case Guid converted:
					result = Standard.ArcGISElement.FromGUID(converted);
					break;
				case DateTime converted:
					result = Standard.ArcGISElement.FromDateTime(converted);
					break;
				case Int16 converted:
					result = Standard.ArcGISElement.FromInt16(converted);
					break;
				case Int32 converted:
					result = Standard.ArcGISElement.FromInt32(converted);
					break;
				case Int64 converted:
					result = Standard.ArcGISElement.FromInt64(converted);
					break;
				case UInt16 converted:
					result = Standard.ArcGISElement.FromUInt16(converted);
					break;
				case UInt32 converted:
					result = Standard.ArcGISElement.FromUInt32(converted);
					break;
				case UInt64 converted:
					result = Standard.ArcGISElement.FromUInt64(converted);
					break;
				case ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> converted:
					result = Standard.ArcGISElement.FromIdentifyLayerResultImmutableCollection(converted);
					break;
				case GameEngine.MapView.ArcGISIdentifyLayerResult converted:
					result = Standard.ArcGISElement.FromIdentifyLayerResult(converted);
					break;
				case GameEngine.Data.Feature converted:
					result = Standard.ArcGISElement.FromFeature(converted);
					break;
				default:
					throw new InvalidCastException();
			}

			return result;
		}

		internal static Standard.ArcGISElementType ToArcGISElementType<T>()
		{
			if (typeof(T) == typeof(ArcGISDictionaryBase) ||
				typeof(T).IsSubclassOf(typeof(ArcGISDictionaryBase)))
			{
				return Standard.ArcGISElementType.Dictionary;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISAttribute))
			{
				return Standard.ArcGISElementType.GEAttribute;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISDatumTransformation))
			{
				return Standard.ArcGISElementType.DatumTransformation;
			}
			else if (typeof(T) == typeof(double))
			{
				return Standard.ArcGISElementType.Float64;
			}
			else if (typeof(T) == typeof(GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics))
			{
				return Standard.ArcGISElementType.GEBuildingSceneLayerAttributeStatistics;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISGeographicTransformationStep))
			{
				return Standard.ArcGISElementType.GeographicTransformationStep;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISGeometry) ||
					 typeof(T).IsSubclassOf(typeof(GameEngine.Geometry.ArcGISGeometry)))
			{
				return Standard.ArcGISElementType.Geometry;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep))
			{
				return Standard.ArcGISElementType.HorizontalVerticalTransformationStep;
			}
			else if (typeof(T) == typeof(string))
			{
				return Standard.ArcGISElementType.String;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISVisualizationAttribute))
			{
				return Standard.ArcGISElementType.GEVisualizationAttribute;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISVisualizationAttributeDescription))
			{
				return Standard.ArcGISElementType.GEVisualizationAttributeDescription;
			}
			else if (typeof(T) == typeof(Guid))
			{
				return Standard.ArcGISElementType.GUID;
			}
			else if (typeof(T) == typeof(DateTime))
			{
				return Standard.ArcGISElementType.DateTime;
			}
			else if (typeof(T) == typeof(float))
			{
				return Standard.ArcGISElementType.Float32;
			}
			else if (typeof(T) == typeof(Int16))
			{
				return Standard.ArcGISElementType.Int16;
			}
			else if (typeof(T) == typeof(Int32))
			{
				return Standard.ArcGISElementType.Int32;
			}
			else if (typeof(T) == typeof(Int64))
			{
				return Standard.ArcGISElementType.Int64;
			}
			else if (typeof(T) == typeof(UInt16))
			{
				return Standard.ArcGISElementType.UInt16;
			}
			else if (typeof(T) == typeof(UInt32))
			{
				return Standard.ArcGISElementType.UInt32;
			}
			else if (typeof(T) == typeof(UInt64))
			{
				return Standard.ArcGISElementType.UInt64;
			}
			else if (typeof(T) == typeof(ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult>))
			{
				return Standard.ArcGISElementType.IdentifyLayerResultImmutableCollection;
			}
			else if (typeof(T) == typeof(GameEngine.MapView.ArcGISIdentifyLayerResult))
			{
				return Standard.ArcGISElementType.IdentifyLayerResult;
			}
			else if (typeof(T) == typeof(GameEngine.Data.Feature))
			{
				return Standard.ArcGISElementType.Feature;
			}
			else
			{
				throw new InvalidCastException();
			}
		}
	}
}
