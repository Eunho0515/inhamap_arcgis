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

namespace Esri.Unity
{
    [StructLayout(LayoutKind.Sequential)]
    public class ArcGISImmutableCollection<T>
    {
        #region Internal Members
        internal ArcGISImmutableCollection(IntPtr handle) => Handle = handle;
    
        ~ArcGISImmutableCollection()
        {
            if (typeof (T) == typeof(GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer))
            {
                new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Map.ArcGISGeoElement))
            {
                new GameEngine.View.ArcGISGeoElementImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.MapView.ArcGISIdentifyLayerResult))
            {
                new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(long))
            {
                new GameEngine.Attributes.ArcGISInt64ImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute))
            {
                new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(string))
            {
                new GameEngine.Attributes.ArcGISStringImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo))
            {
                new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(Handle);
            }
            else if (typeof (T) == typeof(int))
            {
                new GameEngine.Attributes.ArcGISint32ImmutableCollection(Handle);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    
        internal IntPtr Handle { get; private set; }
        #endregion // Internal Members
    }
    
    public static class ArcGISImmutableCollectionSpecialization
    {
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer At(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self, ulong position)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self, ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> vector2)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer First(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer Last(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> self)
        {
        	return GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection.Npos();
        }
        
        public static GameEngine.Map.ArcGISGeoElement At(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self, ulong position)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self, GameEngine.Map.ArcGISGeoElement value)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self, ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> vector2)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Map.ArcGISGeoElement First(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self, GameEngine.Map.ArcGISGeoElement value)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Map.ArcGISGeoElement Last(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self)
        {
        	var collection = new GameEngine.View.ArcGISGeoElementImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> self)
        {
        	return GameEngine.View.ArcGISGeoElementImmutableCollection.Npos();
        }
        
        public static GameEngine.MapView.ArcGISIdentifyLayerResult At(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self, ulong position)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self, GameEngine.MapView.ArcGISIdentifyLayerResult value)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self, ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> vector2)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.MapView.ArcGISIdentifyLayerResult First(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self, GameEngine.MapView.ArcGISIdentifyLayerResult value)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.MapView.ArcGISIdentifyLayerResult Last(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self)
        {
        	var collection = new GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult> self)
        {
        	return GameEngine.View.ArcGISIdentifyLayerResultImmutableCollection.Npos();
        }
        
        public static long At(this ArcGISImmutableCollection<long> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<long> self, long value)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<long> self, ArcGISImmutableCollection<long> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static long First(this ArcGISImmutableCollection<long> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<long> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<long> self, long value)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<long> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static long Last(this ArcGISImmutableCollection<long> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISInt64ImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<long> self)
        {
        	return GameEngine.Attributes.ArcGISInt64ImmutableCollection.Npos();
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute At(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self, GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self, ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute First(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self, GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute value)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute Last(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> self)
        {
        	return GameEngine.Attributes.ArcGISPointCloudAttributeImmutableCollection.Npos();
        }
        
        public static string At(this ArcGISImmutableCollection<string> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<string> self, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<string> self, ArcGISImmutableCollection<string> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static string First(this ArcGISImmutableCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<string> self, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static string Last(this ArcGISImmutableCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<string> self)
        {
        	return GameEngine.Attributes.ArcGISStringImmutableCollection.Npos();
        }
        
        public static GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo At(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self, ulong position)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self, GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo value)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self, ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> vector2)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo First(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self, GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo value)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo Last(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self)
        {
        	var collection = new GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfo> self)
        {
        	return GameEngine.Layers.VectorTile.ArcGISVectorTileSourceInfoImmutableCollection.Npos();
        }
        
        public static int At(this ArcGISImmutableCollection<int> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISImmutableCollection<int> self, int value)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISImmutableCollection<int> self, ArcGISImmutableCollection<int> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static int First(this ArcGISImmutableCollection<int> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISImmutableCollection<int> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISImmutableCollection<int> self, int value)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool IsEmpty(this ArcGISImmutableCollection<int> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static int Last(this ArcGISImmutableCollection<int> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISint32ImmutableCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong Npos(this ArcGISImmutableCollection<int> self)
        {
        	return GameEngine.Attributes.ArcGISint32ImmutableCollection.Npos();
        }
    }
}