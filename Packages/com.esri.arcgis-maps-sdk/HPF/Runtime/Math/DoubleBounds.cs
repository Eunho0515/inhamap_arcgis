using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Esri.HPFramework
{
    [BurstCompile(CompileSynchronously = true)]
    [Serializable]
    public struct DoubleBounds :
        IEquatable<DoubleBounds>,
        IFormattable
    {
        public DoubleBounds(double3 center, double3 size) :
            this(center, 0.5 * size, false)
        {
            Assert.IsTrue(size.x >= 0, $"{nameof(DoubleBounds)} size.x cannot be a negative value.");
            Assert.IsTrue(size.y >= 0, $"{nameof(DoubleBounds)} size.y cannot be a negative value.");
            Assert.IsTrue(size.z >= 0, $"{nameof(DoubleBounds)} size.z cannot be a negative value.");
        }

        private DoubleBounds(double3 center, double3 extents, bool isEmpty)
        {
            this.center = center;
            this.extents = extents;
            IsEmpty = isEmpty;
        }

        public static DoubleBounds Empty
        {
            get
            {
                return new DoubleBounds(
                    double3.zero,
                    new double3(-1),
                    true);
            }
        }

        [SerializeField]
        private double3 center;

        [SerializeField]
        private double3 extents;

        public double3 Center
        {
            get { return center; }
        }

        public double3 Extents
        {
            get { return extents; }
        }

        public bool IsEmpty { get; }

        public static bool operator ==(DoubleBounds lhs, DoubleBounds rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(DoubleBounds lhs, DoubleBounds rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override bool Equals(object other)
        {
            return other is DoubleBounds bounds && Equals(bounds);
        }

        public bool Equals(DoubleBounds other)
        {
            return Center.Equals(other.Center)
                   && Extents.Equals(other.Extents)
                   && IsEmpty == other.IsEmpty;
        }

        public override int GetHashCode()
        {
            int hashCenter = center.GetHashCode() << 2;

            int hashExtents = extents.GetHashCode() << 3;

            int hashEmpty = IsEmpty.GetHashCode();

            return hashEmpty ^ hashCenter ^ hashExtents;
        }

        public double3 Min
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return center - extents; }
        }

        public double3 Max
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return center + extents; }
        }

        /// <summary>
        /// The total size of the box. This is always twice as large as the extents.
        /// </summary>
        public double3 Size
        {
            get { return extents * 2; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(DoubleBounds bounds)
        {
            if (IsEmpty || bounds.IsEmpty)
                return false;

            double3 ourMin = center - extents;
            double3 ourMax = center + extents;

            double3 theirMin = bounds.center - bounds.extents;
            double3 theirMax = bounds.center + bounds.extents;

            return
                ourMin.x <= theirMax.x && theirMin.x <= ourMax.x &&
                ourMin.y <= theirMax.y && theirMin.y <= ourMax.y &&
                ourMin.z <= theirMax.z && theirMin.z <= ourMax.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(DoubleBounds bounds)
        {
            if (IsEmpty || bounds.IsEmpty)
                return false;

            double3 ourMin = center - extents;
            double3 ourMax = center + extents;

            double3 theirMin = bounds.center - bounds.extents;
            double3 theirMax = bounds.center + bounds.extents;

            return
                ourMin.x <= theirMin.x &&
                ourMin.y <= theirMin.y &&
                ourMin.z <= theirMin.z &&
                ourMax.x >= theirMax.x &&
                ourMax.y >= theirMax.y &&
                ourMax.z >= theirMax.z;
        }

        public static explicit operator DoubleBounds(Bounds bounds)
        {
            return new DoubleBounds(
                bounds.center.ToDouble3(),
                bounds.extents.ToDouble3(),
                false);
        }

        public static explicit operator Bounds(DoubleBounds bounds)
        {
            Bounds result = default;

            result.center = bounds.center.ToVector3();
            result.extents = bounds.extents.ToVector3();

            return result;
        }

        public static DoubleBounds Union(DoubleBounds a, DoubleBounds b)
        {
            if (!a.Intersects(b))
                return Empty;

            double3 min = math.max(a.Min, b.Min);
            double3 max = math.min(a.Max, b.Max);

            return new DoubleBounds(0.5 * (min + max), max - min);
        }

        public static DoubleBounds Transform3x4(DoubleBounds bounds, double4x4 transformationMatrix)
        {
            //
            //  FIXME - Implement optimized version of this
            //
            return Transform(bounds, transformationMatrix, default, false);
        }

        public static DoubleBounds Transform(DoubleBounds bounds, double4x4 transformMatrix, DoublePlane clipPlane)
        {
            return Transform(bounds, transformMatrix, clipPlane, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe DoubleBounds Transform(DoubleBounds bounds, double4x4 transformationMatrix, DoublePlane clipPlane, bool useClipPlane)
        {
            if (bounds.IsEmpty)
                return Empty;

            double3* universeVertices = stackalloc double3[8];

            double3 extents = bounds.extents;
            double3 center = bounds.center;

            double extentX = extents.x;
            double extentY = extents.y;
            double extentZ = extents.z;

            //
            //  Compute corner vertices
            //
            universeVertices[0] = center + new double3(-extentX, -extentY, -extentZ);
            universeVertices[1] = center + new double3(-extentX, -extentY, extentZ);
            universeVertices[2] = center + new double3(-extentX, extentY, -extentZ);
            universeVertices[3] = center + new double3(-extentX, extentY, extentZ);

            universeVertices[4] = center + new double3(extentX, -extentY, -extentZ);
            universeVertices[5] = center + new double3(extentX, -extentY, extentZ);
            universeVertices[6] = center + new double3(extentX, extentY, -extentZ);
            universeVertices[7] = center + new double3(extentX, extentY, extentZ);

            bool writeOne = false;
            double3 min = new double3(double.MaxValue, double.MaxValue, double.MaxValue);
            double3 max = new double3(double.MinValue, double.MinValue, double.MinValue);

            for (int i = 0; i < 8; i++)
            {
                double3 v = universeVertices[i];

                if (!useClipPlane || clipPlane.GetSide(v))
                {
                    double3 clip = transformationMatrix.HomogeneousTransformPoint(v);
                    min = math.min(min, clip);
                    max = math.max(max, clip);
                    writeOne = true;
                }
                else
                {
                    int a = i ^ 0x01;
                    int b = i ^ 0x02;
                    int c = i ^ 0x04;

                    if (clipPlane.GetSide(universeVertices[a]))
                    {
                        double3 raycast = clipPlane.Raycast(v, universeVertices[a]);
                        double3 clip = transformationMatrix.HomogeneousTransformPoint(raycast);
                        min = math.min(min, clip);
                        max = math.max(max, clip);
                        writeOne = true;
                    }

                    if (clipPlane.GetSide(universeVertices[b]))
                    {
                        double3 raycast = clipPlane.Raycast(v, universeVertices[b]);
                        double3 clip = transformationMatrix.HomogeneousTransformPoint(raycast);
                        min = math.min(min, clip);
                        max = math.max(max, clip);
                        writeOne = true;
                    }

                    if (clipPlane.GetSide(universeVertices[c]))
                    {
                        double3 raycast = clipPlane.Raycast(v, universeVertices[c]);
                        double3 clip = transformationMatrix.HomogeneousTransformPoint(raycast);
                        min = math.min(min, clip);
                        max = math.max(max, clip);
                        writeOne = true;
                    }
                }
            }

            return writeOne
                ? new DoubleBounds(0.5 * (min + max), max - min)
                : Empty;
        }

        /// <summary>
        ///   <para>Returns a formatted string for the bounds.</para>
        /// </summary>
        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for the bounds.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for the bounds.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";

            return string.Format(
                CultureInfo.InvariantCulture.NumberFormat,
                "Center: {0}, Extents: {1}",
                Center.ToString(format, formatProvider),
                Extents.ToString(format, formatProvider));
        }
    }
}
