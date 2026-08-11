using System.Collections.Generic;

using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Esri.HPFramework
{
    /// <summary>
    /// The HPRoot determines how the universe space will be converted into world
    /// space. It defines the coordinate in universe space which corresponds to
    /// the position of the GameObject, in world space.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("HPFramework/HPRoot")]
    public class HPRoot : MonoBehaviour, HPNode
    {
        [SerializeField]
        private double3 m_LocalPosition = double3.zero;

        [SerializeField]
        private quaternion m_LocalRotation = quaternion.identity;

        private List<HPTransform> m_Children = new List<HPTransform>();


        private bool m_CachedWorldMatrixIsValid;

        private double4x4 m_CachedWorldMatrix;

        private static ProfilerMarker s_LateUpdateMarker = new ProfilerMarker("HPRoot.LateUpdate");

        double3 HPNode.LocalPosition
        {
            get { return LocalPosition; }
        }

        internal double3 LocalPosition
        {
            get { return double3.zero; }
        }

        double3 HPNode.UniversePosition
        {
            get { return UniversePosition; }
        }

        internal double3 UniversePosition
        {
            get { return double3.zero; }
        }

        /// <summary>
        /// Set the position in universe space which corresponds to the HPRoot's
        /// position in the scene.
        /// </summary>
        public double3 RootUniversePosition
        {
            get { return m_LocalPosition; }
            set
            {
                m_LocalPosition = value;
                InvalidateWorldCache();
            }
        }

        /// <summary>
        /// Set the rotation of the universe space which corresponds to the HPRoot's
        /// rotation in the scene.
        /// </summary>
        public quaternion RootUniverseRotation
        {
            get { return m_LocalRotation; }
            set
            {
                m_LocalRotation = value;
                InvalidateWorldCache();
            }
        }

        /// <summary>
        /// Simultaneously set root universe position and root universe rotation
        /// in a single call, updating underlying transforms only once.
        /// </summary>
        /// <param name="position">The position in universe space which corresponds to the HPRoot's position in the scene</param>
        /// <param name="rotation">The position in universe space which corresponds to the HPRoot's rotation in the scene</param>
        public void SetRootTR(double3 position, quaternion rotation)
        {
            m_LocalPosition = position;
            m_LocalRotation = rotation;
            InvalidateWorldCache();
        }


        quaternion HPNode.LocalRotation
        {
            get { return LocalRotation; }
        }

        internal quaternion LocalRotation
        {
            get { return quaternion.identity; }
        }


        quaternion HPNode.UniverseRotation
        {
            get { return UniverseRotation; }
        }

        internal quaternion UniverseRotation
        {
            get { return quaternion.identity; }
        }

        float3 HPNode.LocalScale
        {
            get { return LocalScale; }
        }

        internal float3 LocalScale
        {
            get { return new float3(1F); }
        }

        double4x4 HPNode.LocalMatrix
        {
            get { return double4x4.identity; }
        }

        double4x4 HPNode.UniverseMatrix
        {
            get { return double4x4.identity; }
        }

        double4x4 HPNode.WorldMatrix
        {
            get { return WorldMatrix; }
        }

        public double4x4 WorldMatrix
        {
            get
            {
                if (!m_CachedWorldMatrixIsValid || transform.hasChanged)
                {
                    double4x4 worldFromRoot = transform.localToWorldMatrix.ToDouble4x4();
                    double4x4 universeFromRoot = HPMath.TRS(m_LocalPosition, m_LocalRotation, LocalScale);
                    m_CachedWorldMatrix = math.mul(worldFromRoot, math.inverse(universeFromRoot));
                    m_CachedWorldMatrixIsValid = true;
                    transform.hasChanged = false;
                }
                return m_CachedWorldMatrix;
            }
        }

        /// <summary>
        /// Transforms position from universe space to world space
        /// </summary>
        public double3 TransformPoint(double3 universePosition)
        {
            return WorldMatrix.HomogeneousTransformPoint(universePosition);
        }

        /// <summary>
        /// Transforms direction from universe space to world space, the returned vector is an unit vector
        /// </summary>
        public double3 TransformDirection(double3 universeDirection)
        {
            double3 worldDirection = WorldMatrix.HomogeneousTransformVector(universeDirection);
            return math.normalizesafe(worldDirection);
        }

        /// <summary>
        /// Transforms rotation from universe space to world space
        /// </summary>
        public quaternion TransformRotation(quaternion universeRotation)
        {
            return math.mul(WorldMatrix.GetRotation(), universeRotation);
        }

        /// <summary>
        /// Transforms position from world space to universe space
        /// </summary>
        public double3 InverseTransformPoint(double3 worldPosition)
        {
            return math.inverse(WorldMatrix).HomogeneousTransformPoint(worldPosition);
        }

        /// <summary>
        /// Trnasforms direction from world space to universe space, the returned vector is an unit vector
        /// </summary>
        public double3 InverseTransformDirection(double3 worldDirection)
        {
            double3 universeDirection = math.inverse(WorldMatrix).HomogeneousTransformVector(worldDirection);
            return math.normalizesafe(universeDirection);
        }

        /// <summary>
        /// Transforms rotation from world space to universe space
        /// </summary>
        public quaternion InverseTransformRotation(quaternion worldRotation)
        {
            quaternion worldRotationInverted = math.inverse(WorldMatrix.GetRotation());
            return math.mul(worldRotationInverted, worldRotation);
        }

        NodeType HPNode.Type
        {
            get { return NodeType.HPRoot; }
        }

        void HPNode.RegisterChild(HPTransform hpTransform)
        {
            m_Children.Add(hpTransform);
        }

        void HPNode.UnregisterChild(HPTransform hPTransform)
        {
            m_Children.Remove(hPTransform); 
        }

        private void UpdateTransforms()
        {
            foreach (var child in m_Children)
                child.UpdateFromParent();
        }

        private void InvalidateWorldCache()
        {
            m_CachedWorldMatrixIsValid = false;
            foreach (var child in m_Children)
                child.InvalidateWorldCacheRecursively();
        }

        private void OnEnable()
        {
            InvalidateWorldCache();
        }

        private void LateUpdate()
        {
            s_LateUpdateMarker.Begin();

            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                foreach (var child in m_Children)
                    child.ClearUnityTransformChange();
            }

            UpdateTransforms();

            s_LateUpdateMarker.End();
        }
    }
}
