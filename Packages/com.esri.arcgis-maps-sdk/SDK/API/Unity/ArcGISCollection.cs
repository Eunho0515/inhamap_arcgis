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
    public class ArcGISCollection<T>
    {
        #region Constructors
        public ArcGISCollection()
        {
            if (typeof (T) == typeof(GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter))
            {
                var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(double))
            {
                var collection = new GameEngine.Attributes.ArcGISDoubleCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Elevation.Base.ArcGISElevationSource))
            {
                var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.Base.ArcGISLayer))
            {
                var collection = new GameEngine.Layers.Base.ArcGISLayerCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.ArcGISMeshModification))
            {
                var collection = new GameEngine.Layers.ArcGISMeshModificationCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak))
            {
                var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop))
            {
                var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue))
            {
                var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudFilter))
            {
                var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType))
            {
                var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(GameEngine.Geometry.ArcGISPolygon))
            {
                var collection = new GameEngine.Geometry.ArcGISPolygonCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(string))
            {
                var collection = new GameEngine.Attributes.ArcGISStringCollection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else if (typeof (T) == typeof(uint))
            {
                var collection = new GameEngine.Attributes.ArcGISUint32Collection();
                
                Handle = collection.Handle;
                
                collection.Handle = IntPtr.Zero;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion // Constructors
    
        #region Internal Members
        internal ArcGISCollection(IntPtr handle) => Handle = handle;
    
        ~ArcGISCollection()
        {
            if (typeof (T) == typeof(GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter))
            {
                new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(Handle);
            }
            else if (typeof (T) == typeof(double))
            {
                new GameEngine.Attributes.ArcGISDoubleCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Elevation.Base.ArcGISElevationSource))
            {
                new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.Base.ArcGISLayer))
            {
                new GameEngine.Layers.Base.ArcGISLayerCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.ArcGISMeshModification))
            {
                new GameEngine.Layers.ArcGISMeshModificationCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak))
            {
                new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop))
            {
                new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue))
            {
                new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudFilter))
            {
                new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType))
            {
                new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(Handle);
            }
            else if (typeof (T) == typeof(GameEngine.Geometry.ArcGISPolygon))
            {
                new GameEngine.Geometry.ArcGISPolygonCollection(Handle);
            }
            else if (typeof (T) == typeof(string))
            {
                new GameEngine.Attributes.ArcGISStringCollection(Handle);
            }
            else if (typeof (T) == typeof(uint))
            {
                new GameEngine.Attributes.ArcGISUint32Collection(Handle);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    
        internal IntPtr Handle { get; private set; }
        #endregion // Internal Members
    }
    
    public static class ArcGISCollectionSpecialization
    {
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> vector2)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter At(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ulong position)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> vector2)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter First(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ulong position, GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter value)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter Last(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	return GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self, ulong position)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> self)
        {
        	var collection = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilterCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<double> self, double value)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<double> self, ArcGISCollection<double> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static double At(this ArcGISCollection<double> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<double> self, double value)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<double> self, ArcGISCollection<double> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static double First(this ArcGISCollection<double> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<double> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<double> self, double value)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<double> self, ulong position, double value)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<double> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static double Last(this ArcGISCollection<double> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<double> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<double> self)
        {
        	return GameEngine.Attributes.ArcGISDoubleCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<double> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<double> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISDoubleCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, GameEngine.Elevation.Base.ArcGISElevationSource value)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> vector2)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Elevation.Base.ArcGISElevationSource At(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ulong position)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, GameEngine.Elevation.Base.ArcGISElevationSource value)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> vector2)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Elevation.Base.ArcGISElevationSource First(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, GameEngine.Elevation.Base.ArcGISElevationSource value)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ulong position, GameEngine.Elevation.Base.ArcGISElevationSource value)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Elevation.Base.ArcGISElevationSource Last(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	return GameEngine.Elevation.Base.ArcGISElevationSourceCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self, ulong position)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> self)
        {
        	var collection = new GameEngine.Elevation.Base.ArcGISElevationSourceCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, GameEngine.Layers.Base.ArcGISLayer value)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> vector2)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.Base.ArcGISLayer At(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ulong position)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, GameEngine.Layers.Base.ArcGISLayer value)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> vector2)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.Base.ArcGISLayer First(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, GameEngine.Layers.Base.ArcGISLayer value)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ulong position, GameEngine.Layers.Base.ArcGISLayer value)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.Base.ArcGISLayer Last(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	return GameEngine.Layers.Base.ArcGISLayerCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self, ulong position)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> self)
        {
        	var collection = new GameEngine.Layers.Base.ArcGISLayerCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, GameEngine.Layers.ArcGISMeshModification value)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> vector2)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.ArcGISMeshModification At(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ulong position)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, GameEngine.Layers.ArcGISMeshModification value)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> vector2)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.ArcGISMeshModification First(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, GameEngine.Layers.ArcGISMeshModification value)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ulong position, GameEngine.Layers.ArcGISMeshModification value)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.ArcGISMeshModification Last(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	return GameEngine.Layers.ArcGISMeshModificationCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self, ulong position)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> self)
        {
        	var collection = new GameEngine.Layers.ArcGISMeshModificationCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak At(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak First(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ulong position, GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak Last(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	return GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreak> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorClassBreakCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop At(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop First(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ulong position, GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop Last(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	return GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorStop> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorStopCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue At(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue First(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ulong position, GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue Last(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	return GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValueCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudFilter At(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudFilter First(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ulong position, GameEngine.Layers.PointCloud.ArcGISPointCloudFilter value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudFilter Last(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	return GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudFilterCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType At(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> vector2)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType First(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ulong position, GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType value)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType Last(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	return GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self, ulong position)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudReturnType> self)
        {
        	var collection = new GameEngine.Layers.PointCloud.ArcGISPointCloudReturnTypeCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, GameEngine.Geometry.ArcGISPolygon value)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> vector2)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Geometry.ArcGISPolygon At(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ulong position)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, GameEngine.Geometry.ArcGISPolygon value)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> vector2)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Geometry.ArcGISPolygon First(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, GameEngine.Geometry.ArcGISPolygon value)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ulong position, GameEngine.Geometry.ArcGISPolygon value)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static GameEngine.Geometry.ArcGISPolygon Last(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	return GameEngine.Geometry.ArcGISPolygonCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self, ulong position)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> self)
        {
        	var collection = new GameEngine.Geometry.ArcGISPolygonCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<string> self, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<string> self, ArcGISCollection<string> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static string At(this ArcGISCollection<string> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<string> self, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<string> self, ArcGISCollection<string> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static string First(this ArcGISCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<string> self, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<string> self, ulong position, string value)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static string Last(this ArcGISCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<string> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<string> self)
        {
        	return GameEngine.Attributes.ArcGISStringCollection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<string> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<string> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISStringCollection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Add(this ArcGISCollection<uint> self, uint value)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.Add(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong AddArray(this ArcGISCollection<uint> self, ArcGISCollection<uint> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.AddArray(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static uint At(this ArcGISCollection<uint> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.At(position);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Contains(this ArcGISCollection<uint> self, uint value)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.Contains(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static bool Equals(this ArcGISCollection<uint> self, ArcGISCollection<uint> vector2)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.Equals(vector2);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static uint First(this ArcGISCollection<uint> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.First();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong GetSize(this ArcGISCollection<uint> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.Size;
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static ulong IndexOf(this ArcGISCollection<uint> self, uint value)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.IndexOf(value);
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Insert(this ArcGISCollection<uint> self, ulong position, uint value)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	collection.Insert(position, value);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static bool IsEmpty(this ArcGISCollection<uint> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.IsEmpty();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static uint Last(this ArcGISCollection<uint> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	var result = collection.Last();
        
        	collection.Handle = IntPtr.Zero;
        
        	return result;
        }
        
        public static void Move(this ArcGISCollection<uint> self, ulong oldPosition, ulong newPosition)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	collection.Move(oldPosition, newPosition);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static ulong Npos(this ArcGISCollection<uint> self)
        {
        	return GameEngine.Attributes.ArcGISUint32Collection.Npos();
        }
        
        public static void Remove(this ArcGISCollection<uint> self, ulong position)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	collection.Remove(position);
        
        	collection.Handle = IntPtr.Zero;
        }
        
        public static void RemoveAll(this ArcGISCollection<uint> self)
        {
        	var collection = new GameEngine.Attributes.ArcGISUint32Collection(self.Handle);
        
        	collection.RemoveAll();
        
        	collection.Handle = IntPtr.Zero;
        }
    }
}