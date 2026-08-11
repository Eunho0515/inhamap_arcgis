using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.Assertions;
using Unity.Mathematics;

namespace Esri.HPFramework
{
    //
    //  FIXME - Changing the scale of the root resets the HPTransform's values!!!
    //

    /// <summary>
    /// The HPTransform is the High-Precision Framework's primary class. It acts very
    /// similarly to an ordinary transform, however its position is in 64 bit precision
    /// rather than being in 32 bit precision. The HPTransform can be used with or without
    /// an HPRoot parent. However, in order to truly benefit from the 64 bit precision, it
    /// should be the child of an HPRoot.
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("HPFramework/HPTransform")]
    public class HPTransform : MonoBehaviour, HPNode
    {
        /// <summary>
        /// The type of scale that the HPNode's current configuration allows. When it is 
        /// a leaf node, it will accept non-uniform scales. Otherwise, it will only apply
        /// a uniform scale.
        /// </summary>
        public enum ScaleTypes
        {
            /// <summary>
            /// A non uniform scale, which has an x, y and z component
            /// </summary>
            Anisotropic,

            /// <summary>
            /// A uniform scale, where only the x component is considered
            /// </summary>
            Isotropic
        }

        

        [SerializeField]
        private bool m_IsInitialized;

        [SerializeField]
        private double3 m_LocalPosition = double3.zero;

        [SerializeField]
        private quaternion m_LocalRotation = quaternion.identity;

        [SerializeField]
        private float3 m_LocalScale = new float3(1F);


        private HPNode m_Parent;

        private readonly List<HPTransform> m_Children = new List<HPTransform>();

        // Cache Unity's transform to reduce overhead of retrieving it everytime
        private Transform m_CachedUnityTransform;

        private Transform CachedUnityTransform
        {
            get
            {
                if (m_CachedUnityTransform == null)
                    m_CachedUnityTransform = transform;

                return m_CachedUnityTransform;
            }
        }

        private bool m_CachedUniverseMatrixIsValid;

        private double4x4 m_CachedUniverseMatrix;

        private bool m_CachedUniverseRotationIsValid;

        private quaternion m_CachedUniverseRotation;

        private bool m_CachedWorldMatrixIsValid;

        private double4x4 m_CachedWorldMatrix;

        private bool m_CachedLocalMatrixIsValid;

        private double4x4 m_CachedLocalMatrix;

        private bool m_UnityTransformIsValid;

        private bool m_LocalHasChanged;


        /// <summary>
        /// The position of the HPTransform relative to its parent HPRoot or HPTransform
        /// </summary>
        public double3 LocalPosition
        {
            get { return m_LocalPosition; }
            set
            {
                AssertIsValid(value);

                m_LocalPosition = value;

                InvalidateLocalCache();
            }
        }

        /// <summary>
        /// The position of the HPTransform, in universe space.
        /// </summary>
        public double3 UniversePosition
        {
            get
            {
                return m_Parent == null
                    ? m_LocalPosition
                    : m_Parent.UniverseMatrix.HomogeneousTransformPoint(m_LocalPosition);
            }
            set
            {
                AssertIsValid(value);

                m_LocalPosition = m_Parent == null
                    ? value
                    : math.inverse(m_Parent.UniverseMatrix).HomogeneousTransformPoint(value);

                InvalidateLocalCache();
            }
        }

        /// <summary>
        /// The rotation of the HPTransform relative to its parent HPTransform or HPRoot
        /// </summary>
        public quaternion LocalRotation
        {
            get { return m_LocalRotation; }
            set
            {
                AssertIsValid(value);

                m_LocalRotation = value;

                InvalidateLocalCache();
            }
        }

        /// <summary>
        /// The rotation of the HPTransform, in universe space
        /// </summary>
        public quaternion UniverseRotation
        {
            get
            {
                if (!m_CachedUniverseRotationIsValid)
                {
                    m_CachedUniverseRotation = m_Parent == null
                        ? m_LocalRotation
                        : math.mul(m_Parent.UniverseRotation, m_LocalRotation);

                    m_CachedUniverseRotationIsValid = true;
                }
                return m_CachedUniverseRotation;
            }
            set
            {
                AssertIsValid(value);

                m_LocalRotation = m_Parent == null
                    ? value
                    : math.mul(math.inverse(m_Parent.UniverseRotation), value);

                InvalidateLocalCache();
            }
        }


        //
        //  TODO - When scale is set to zero, quaternion method is printing messages in the console
        //
        /// <summary>
        /// The scale of the HPTransform, relative to its parent HPTransform or HPRoot. If the HPTransform is
        /// not a leaf node (i.e. if it contains another HPTransform) only uniform scales will be possible. Under
        /// these circumstances, only the x component of the scale will count towards the uniform scale.
        /// </summary>
        public float3 LocalScale
        {
            get
            {
                if (ScaleType == ScaleTypes.Anisotropic)
                    return m_LocalScale;
                else
                {
                    float uniformScale = m_LocalScale.x;
                    return new float3(uniformScale, uniformScale, uniformScale);
                }
            }
            set
            {
                AssertIsValid(value);

                m_LocalScale = value;

                InvalidateLocalCache();
            }
        }

        /// <summary>
        /// Set or get the parent of the HPTransform. When changing the parent, the HPTransform's position in the world
        /// will be preserved.
        /// </summary>
        public Transform Parent
        {
            get { return CachedUnityTransform.parent; }
            set { CachedUnityTransform.parent = value; }
        }

        /// <summary>
        /// The HPTransform's forward vector, in universe space.
        /// </summary>
        public float3 Forward
        {
            get { return math.mul(UniverseRotation, math.forward()); }
        }

        /// <summary>
        /// The HPTransform's right vector, in universe space.
        /// </summary>
        public float3 Right
        {
            get { return math.mul(UniverseRotation, math.right()); }
        }

        /// <summary>
        /// The HPTransform's up vector, in universe space.
        /// </summary>
        public float3 Up
        {
            get { return math.mul(UniverseRotation, math.up()); }
        }

        /// <summary>
        /// The type of scale that is currently supported by the HPTransform. When in uniform scale,
        /// only the x component of the set scale will be considered for the uniform scale.
        /// </summary>
        public ScaleTypes ScaleType
        {
            get { return HasChildren
                            ? ScaleTypes.Isotropic
                            : ScaleTypes.Anisotropic; }
        }


        double4x4 HPNode.LocalMatrix
        {
            get { return LocalMatrix; }
        }

        internal double4x4 LocalMatrix
        {
            get
            {
                if (!m_CachedLocalMatrixIsValid)
                {
                    m_CachedLocalMatrix = HPMath.TRS(m_LocalPosition, m_LocalRotation, m_LocalScale);
                    m_CachedLocalMatrixIsValid = true;
                }
                return m_CachedLocalMatrix;
            }
        }


        double4x4 HPNode.UniverseMatrix
        {
            get { return UniverseMatrix; }
        }

        internal double4x4 UniverseMatrix
        {
            get
            {
                if (!m_CachedUniverseMatrixIsValid)
                {
                    if (m_Parent == null)
                        m_CachedUniverseMatrix = LocalMatrix;
                    else
                        m_CachedUniverseMatrix = math.mul(m_Parent.UniverseMatrix, LocalMatrix);

                    m_CachedUniverseMatrixIsValid = true;
                }
                return m_CachedUniverseMatrix;
            }
        }

        double4x4 HPNode.WorldMatrix
        {
            get { return WorldMatrix; }
        }

        internal double4x4 WorldMatrix
        {
            get
            {
                if (!m_CachedWorldMatrixIsValid)
                {
                    if (m_Parent == null)
                        m_CachedWorldMatrix = LocalMatrix;
                    else
                        m_CachedWorldMatrix = math.mul(m_Parent.WorldMatrix, LocalMatrix);

                    m_CachedWorldMatrixIsValid = true;
                }
                return m_CachedWorldMatrix;
            }
        }

        private bool HasChildren
        {
            get { return m_Children.Count > 0; }
        }

        NodeType HPNode.Type
        {
            get { return Type; }
        }

        internal NodeType Type
        {
            get { return NodeType.HPTransform; }
        }

        internal bool IsSceneEditable
        {
            get { return m_Children.Count == 0; }
        }

        /// <summary>
        /// This method is called when the component or the GameObject is enabled.
        /// </summary>
        private void OnEnable()
        {
            Assert.IsNull(m_Parent);
            UpdateParentRelation();

            //
            //  TODO - Find a way of initializing the HPTransform from the Unity Transform without
            //         using serialized variables.
            //
            if (!m_IsInitialized)
                UpdateHPTransformFromUnityTransform();

            InvalidateLocalCache();
        }

        /// <summary>
        /// This method is called when the component or the GameObject is disabled.
        /// </summary>
        private void OnDisable()
        {
            if (m_Parent != null)
            {
                m_Parent.UnregisterChild(this);
                m_Parent = null;
            }
        }

        /// <summary>
        /// This method is called whenever values in the inspector are changed by the user. By invalidating
        /// the local cache, the underlying Unity Transform is updated. This method also catches changes
        /// that do not necessarily go through the properties and directly to the private serialized fields.
        /// </summary>
        private void OnValidate()
        {
            InvalidateLocalCache();
        }

        /// <summary>
        /// This method is called when the component is reset in the inspector. By invalidating the local cache,
        /// the underlying Unity Transform is updated. This method is important since resetting a component 
        /// bypasses the properties and acts directly on the private serialized fields.
        /// </summary>
        private void Reset()
        {
            InvalidateLocalCache();
        }


        /// <summary>
        /// Updates m_HPRoot so that it points towards the corret HPRoot
        /// component. Returns true if it has changed and false otherwise.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the parenting relation has changed;
        /// <see langword="false"/> otherwise.
        /// </returns>
        private void UpdateParentRelation()
        {
            if (!isActiveAndEnabled)
                return;


            Transform parent = CachedUnityTransform.parent;
            
            HPNode hpNode = parent == null
                ? null
                : parent.GetComponentInParent<HPNode>();

            if (hpNode != m_Parent)
            {
                m_Parent?.UnregisterChild(this);
                m_Parent = hpNode;
                m_Parent?.RegisterChild(this);
            }
        }

        internal void UpdateUnityTransform()
        {
            if (HasChildren)
                UpdateUnityTransformIntermediateTransform();
            else
                UpdateUnityTransformWorldSpace();

            ClearUnityTransformChange();

            for (int i = 0; i < m_Children.Count; ++i)
                m_Children[i].UpdateUnityTransform();

            m_UnityTransformIsValid = true;
        }

        internal void ClearUnityTransformChange()
        {
            CachedUnityTransform.hasChanged = false;

            for (int i = 0; i < m_Children.Count; ++i)
                m_Children[i].ClearUnityTransformChange();
        }

        private void UpdateUnityTransformIntermediateTransform()
        {
            Transform t = CachedUnityTransform;

            if (t.localPosition != Vector3.zero)
                t.localPosition = Vector3.zero;

            if (t.localRotation != Quaternion.identity)
                t.localRotation = Quaternion.identity;

            Vector3 scale = LocalScale;
            if (t.localScale != scale)
                t.localScale = scale;
        }

        private void UpdateUnityTransformWorldSpace()
        {
            double4x4 worldMatrix = WorldMatrix;

            // if (double.IsNaN(worldMatrix.m00))
            // {
            //     Debug.LogError("Invalid root matrix");
            //     return;
            // }

            worldMatrix.GetTRS(
                out double3 translation,
                out quaternion rotation,
                out float3 scale);

            CachedUnityTransform.position = translation.ToVector3();
            CachedUnityTransform.rotation = rotation;

            // Since scale is impacted by translation and rotation in GetTRS(),
            // we do not directly use scale from GetTRS() to avoid loss of precision in values
            CachedUnityTransform.localScale = HPMath.CopyVector3Sign(scale, m_LocalScale);
        }

        private void UpdateHPTransformFromUnityTransform()
        {
            double4x4 parentFromWorld = m_Parent == null
                ? double4x4.identity
                : math.inverse(m_Parent.WorldMatrix);

            double4x4 worldFromObject = HPMath.TRS(
                CachedUnityTransform.position.ToDouble3(),
                CachedUnityTransform.rotation,
                CachedUnityTransform.lossyScale);

            double4x4 parentFromObject = math.mul(parentFromWorld, worldFromObject);

            //
            //  TODO - Optimize this, we don't use scale.
            //
            parentFromObject.GetTRS(
                out double3 position,
                out quaternion rotation,
                out float3 scale);

            m_LocalPosition = position;
            m_LocalRotation = rotation;

            // Since scale is impacted by translation and rotation in GetTRS(),
            // we do not directly use scale from GetTRS() to avoid loss of precision in values
            m_LocalScale = HPMath.CopyVector3Sign(scale, CachedUnityTransform.localScale);

            InvalidateLocalCache();
        }

        private void LateUpdate()
        {
            if (m_Parent == null)
                UpdateFromParent();
        }

        private void OnTransformParentChanged()
        {
            double4x4 worldFromLocal = WorldMatrix;

            UpdateParentRelation();
            CachedUnityTransform.hasChanged = false;

            double4x4 worldFromParent = m_Parent == null ? double4x4.identity : m_Parent.WorldMatrix;
            double4x4 parentFromWorld = math.inverse(worldFromParent);
            double4x4 parentFromLocal = math.mul(parentFromWorld, worldFromLocal);

            parentFromLocal.GetTRS(out double3 translation, out quaternion rotation, out float3 scale);

            m_LocalPosition = translation;
            m_LocalRotation = rotation;
            m_LocalScale = scale;

            InvalidateLocalCache();
        }
        
        internal void UpdateFromParent()
        {
            //
            //  Give priority to changes made directly to the HPTransform over changes
            //      made to the CachedUnityTransform. This facilitates initializations.
            //
            if (!m_LocalHasChanged && CachedUnityTransform.hasChanged)
            {
                UpdateHPTransformFromUnityTransform();
                ClearUnityTransformChange();
            }

            if (!m_UnityTransformIsValid)
                UpdateUnityTransform();

            for (int i = 0; i < m_Children.Count; ++i)
                m_Children[i].UpdateFromParent();

            m_LocalHasChanged = false;
        }

        void HPNode.RegisterChild(HPTransform child)
        {
            //
            //  When first child is added, scale type transitions from
            //      uniform to non-uniform.
            //
            if (m_Children.Count == 0)
                InvalidateLocalCache();

            m_Children.Add(child);
            UpdateUnityTransform();
        }

        void HPNode.UnregisterChild(HPTransform child)
        {
            m_Children.Remove(child);

            //
            //  When last child is removed, scale type transitions from
            //      uniform to non-uniform.
            //
            if (m_Children.Count == 0)
                InvalidateLocalCache();
        }

        private void InvalidateLocalCache()
        {
            m_IsInitialized = true;
            m_LocalHasChanged = true;
            m_CachedLocalMatrixIsValid = false;
            InvalidateUniverseCache();
        }

        internal void InvalidateUniverseCache()
        {
            m_CachedUniverseRotationIsValid = false;
            m_CachedUniverseMatrixIsValid = false;
            InvalidateWorldCache();

            for (int i = 0; i < m_Children.Count; ++i)
                m_Children[i].InvalidateUniverseCache();
        }

        internal void InvalidateWorldCacheRecursively()
        {
            InvalidateWorldCache();

            for (int i = 0; i < m_Children.Count; ++i)
                m_Children[i].InvalidateWorldCacheRecursively();
        }

        private void InvalidateWorldCache()
        {
            m_CachedWorldMatrixIsValid = false;
            m_UnityTransformIsValid = false;
        }

        /// <summary>
        /// Validate the given double3 does not have infinite or NaN values.
        /// </summary>
        /// <param name="value">Struct to evaluate its content.</param>
        [Conditional("DEBUG")]
        private static void AssertIsValid(double3 value)
        {
            Assert.IsFalse(double.IsInfinity(value.x) || double.IsNaN(value.x), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(double.IsInfinity(value.y) || double.IsNaN(value.y), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(double.IsInfinity(value.z) || double.IsNaN(value.z), "Cannot set HP Transform with Null or Infinite values");
        }

        /// <summary>
        /// Validate the given float3 does not have infinite or NaN values.
        /// </summary>
        /// <param name="value">Struct to evaluate its content.</param>
        [Conditional("DEBUG")]
        private static void AssertIsValid(float3 value)
        {
            Assert.IsFalse(float.IsInfinity(value.x) || float.IsNaN(value.x), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(float.IsInfinity(value.y) || float.IsNaN(value.y), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(float.IsInfinity(value.z) || float.IsNaN(value.z), "Cannot set HP Transform with Null or Infinite values");
        }

        /// <summary>
        /// Validate the given quaternion does not have infinite or NaN values.
        /// </summary>
        /// <param name="value">Struct to evaluate its content.</param>
        [Conditional("DEBUG")]
        private static void AssertIsValid(quaternion value)
        {
            Assert.IsFalse(float.IsInfinity(value.value.x) || float.IsNaN(value.value.x), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(float.IsInfinity(value.value.y) || float.IsNaN(value.value.y), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(float.IsInfinity(value.value.z) || float.IsNaN(value.value.z), "Cannot set HP Transform with Null or Infinite values");
            Assert.IsFalse(float.IsInfinity(value.value.w) || float.IsNaN(value.value.w), "Cannot set HP Transform with Null or Infinite values");
        }
    }
}
