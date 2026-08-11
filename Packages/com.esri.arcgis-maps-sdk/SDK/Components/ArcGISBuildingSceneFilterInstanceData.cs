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
using Esri.ArcGISMapsSDK.SDK.Utils;
using Esri.GameEngine.Layers.BuildingScene;
using System;
using System.Collections.Generic;

namespace Esri.ArcGISMapsSDK.Components
{
	[Serializable]
	public class ArcGISBuildingSceneLayerFilterInstanceData : ICloneable
	{
		public int ActiveBuildingAttributeFilterIndex = -1;
		public List<ArcGISBuildingAttributeFilterInstanceData> BuildingAttributeFilters = new();

		// The use of filters can be enabled or disabled, regardless of active filters.
		public bool IsBuildingAttributeFilterEnabled = false;
		// The use of Disciplines & Categories can be enabled or disabled
		public bool IsBuildingDisciplinesCategoriesFilterEnabled = false;

		// Each category is represented by a unique sublayer IDs.
		public List<long> EnabledCategories = new();
		// The disciplines can be identified by attribute model name in the sublayers.
		public List<ArcGISBuildingSceneSublayerDiscipline> EnabledDisciplines = new();

		public object Clone()
		{
			var buildingSceneLayerFilterInstanceData = new ArcGISBuildingSceneLayerFilterInstanceData
			{
				ActiveBuildingAttributeFilterIndex = ActiveBuildingAttributeFilterIndex,
				BuildingAttributeFilters = BuildingAttributeFilters.Clone(),
				EnabledCategories = EnabledCategories.Clone(),
				EnabledDisciplines = EnabledDisciplines.Clone(),
				IsBuildingAttributeFilterEnabled = IsBuildingAttributeFilterEnabled,
				IsBuildingDisciplinesCategoriesFilterEnabled = IsBuildingDisciplinesCategoriesFilterEnabled
			};

			return buildingSceneLayerFilterInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISBuildingSceneLayerFilterInstanceData data &&
				   ActiveBuildingAttributeFilterIndex == data.ActiveBuildingAttributeFilterIndex &&
				   IEnumerableEqualityComparer<ArcGISBuildingAttributeFilterInstanceData>.Default.Equals(BuildingAttributeFilters, data.BuildingAttributeFilters) &&
				   IsBuildingAttributeFilterEnabled == data.IsBuildingAttributeFilterEnabled &&
				   IsBuildingDisciplinesCategoriesFilterEnabled == data.IsBuildingDisciplinesCategoriesFilterEnabled &&
				   IEnumerableEqualityComparer<long>.Default.Equals(EnabledCategories, data.EnabledCategories) &&
				   IEnumerableEqualityComparer<ArcGISBuildingSceneSublayerDiscipline>.Default.Equals(EnabledDisciplines, data.EnabledDisciplines);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ActiveBuildingAttributeFilterIndex, BuildingAttributeFilters, IsBuildingAttributeFilterEnabled, IsBuildingDisciplinesCategoriesFilterEnabled, EnabledCategories, EnabledDisciplines);
		}

		public static bool operator ==(ArcGISBuildingSceneLayerFilterInstanceData left, ArcGISBuildingSceneLayerFilterInstanceData right)
		{
			return ReferenceEquals(left, right);
		}

		public static bool operator !=(ArcGISBuildingSceneLayerFilterInstanceData left, ArcGISBuildingSceneLayerFilterInstanceData right)
		{
			return !(left == right);
		}

		public static implicit operator bool(ArcGISBuildingSceneLayerFilterInstanceData other)
		{
			return other != null;
		}
	}

	[Serializable]
	public class ArcGISBuildingAttributeFilterInstanceData : ICloneable
	{
		public string Description = string.Empty;
		public string FilterID = string.Empty;
		public string Name = string.Empty;

		// TODO: Account for user defined from the UI. Defaulting as userdefined for now.
		public ArcGISSolidBuildingFilterDefinitionInstanceData SolidFilterDefinition = new();

		public object Clone()
		{
			var buildingAttributeFilterInstanceData = new ArcGISBuildingAttributeFilterInstanceData
			{
				Description = Description,
				FilterID = FilterID,
				Name = Name,
				SolidFilterDefinition = SolidFilterDefinition != null ? (ArcGISSolidBuildingFilterDefinitionInstanceData)SolidFilterDefinition.Clone() : null
			};

			return buildingAttributeFilterInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISBuildingAttributeFilterInstanceData data &&
				   Description == data.Description &&
				   FilterID == data.FilterID &&
				   Name == data.Name &&
				   EqualityComparer<ArcGISSolidBuildingFilterDefinitionInstanceData>.Default.Equals(SolidFilterDefinition, data.SolidFilterDefinition);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Description, FilterID, Name, SolidFilterDefinition);
		}

		public static bool operator ==(ArcGISBuildingAttributeFilterInstanceData left, ArcGISBuildingAttributeFilterInstanceData right)
		{
			return ReferenceEquals(left, right);
		}

		public static bool operator !=(ArcGISBuildingAttributeFilterInstanceData left, ArcGISBuildingAttributeFilterInstanceData right)
		{
			return !(left == right);
		}

		public static implicit operator bool(ArcGISBuildingAttributeFilterInstanceData other)
		{
			return other != null;
		}
	}

	[Serializable]
	public class ArcGISBuildingSceneLayerAttributeStatisticsInstanceData : ICloneable
	{
		public string FieldName;
		public List<string> MostFrequentValues = new();

		public object Clone()
		{
			var buildingSceneLayerAttributeStatisticsInstanceData = new ArcGISBuildingSceneLayerAttributeStatisticsInstanceData
			{
				FieldName = FieldName,
				MostFrequentValues = MostFrequentValues.Clone()
			};

			return buildingSceneLayerAttributeStatisticsInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISBuildingSceneLayerAttributeStatisticsInstanceData data &&
				   FieldName == data.FieldName &&
				   IEnumerableEqualityComparer<string>.Default.Equals(MostFrequentValues, data.MostFrequentValues);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FieldName, MostFrequentValues);
		}

		public static bool operator ==(ArcGISBuildingSceneLayerAttributeStatisticsInstanceData left, ArcGISBuildingSceneLayerAttributeStatisticsInstanceData right)
		{
			return ReferenceEquals(left, right);
		}

		public static bool operator !=(ArcGISBuildingSceneLayerAttributeStatisticsInstanceData left, ArcGISBuildingSceneLayerAttributeStatisticsInstanceData right)
		{
			return !(left == right);
		}
	}

	[Serializable]
	public class ArcGISSolidBuildingFilterDefinitionInstanceData : ICloneable
	{
		public string Title = string.Empty;
		public string WhereClause = string.Empty;

		public List<ArcGISBuildingSceneLayerAttributeStatisticsInstanceData> EnabledStatistics = new();

		public virtual object Clone()
		{
			var solidBuildingFilterDefinitionInstanceData = new ArcGISSolidBuildingFilterDefinitionInstanceData
			{
				EnabledStatistics = EnabledStatistics?.Clone(),
				Title = Title,
				WhereClause = WhereClause
			};

			return solidBuildingFilterDefinitionInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISSolidBuildingFilterDefinitionInstanceData data &&
				   Title == data.Title &&
				   WhereClause == data.WhereClause &&
				   IEnumerableEqualityComparer<ArcGISBuildingSceneLayerAttributeStatisticsInstanceData>.Default.Equals(EnabledStatistics, data.EnabledStatistics);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Title, WhereClause, EnabledStatistics);
		}

		public static bool operator ==(ArcGISSolidBuildingFilterDefinitionInstanceData left, ArcGISSolidBuildingFilterDefinitionInstanceData right)
		{
			return ReferenceEquals(left, right);
		}

		public static bool operator !=(ArcGISSolidBuildingFilterDefinitionInstanceData left, ArcGISSolidBuildingFilterDefinitionInstanceData right)
		{
			return !(left == right);
		}

		public static implicit operator bool(ArcGISSolidBuildingFilterDefinitionInstanceData other)
		{
			return other != null;
		}
	}

	[Serializable]
	public class ArcGISBuildingSceneLayerSublayerInstanceData : ICloneable
	{
		public string Name;
		public long ID;
		public ArcGISBuildingSceneSublayerDiscipline Discipline;

		public object Clone()
		{
			var buildingSceneLayerSublayerInstanceData = new ArcGISBuildingSceneLayerSublayerInstanceData
			{
				Discipline = Discipline,
				ID = ID,
				Name = Name
			};

			return buildingSceneLayerSublayerInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISBuildingSceneLayerSublayerInstanceData data &&
				   Name == data.Name &&
				   ID == data.ID &&
				   Discipline == data.Discipline;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, ID, Discipline);
		}

		public static bool operator ==(ArcGISBuildingSceneLayerSublayerInstanceData left, ArcGISBuildingSceneLayerSublayerInstanceData right)
		{
			return ReferenceEquals(left, right);
		}

		public static bool operator !=(ArcGISBuildingSceneLayerSublayerInstanceData left, ArcGISBuildingSceneLayerSublayerInstanceData right)
		{
			return !(left == right);
		}

		public static implicit operator bool(ArcGISBuildingSceneLayerSublayerInstanceData other)
		{
			return other != null;
		}
	}
}
