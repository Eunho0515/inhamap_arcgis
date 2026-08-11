using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using Unity.Burst;
using UnityEngine;
using Unity.Mathematics;

namespace Esri.HPFramework
{
    [BurstCompile(CompileSynchronously = true)]
    public readonly struct DoublePlane :
        IEquatable<DoublePlane>,
        IFormattable
    {
        public double3 Normal { get; }

        public double Distance { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DoublePlane(double3 normal, double3 point)
        {
            Normal = math.normalizesafe(normal);
            Distance = -math.dot(Normal, point);
        }

        public DoublePlane(double3 normal, double distance) :
            this(normal, distance, true) { }

        public DoublePlane(double3 a, double3 b, double3 c) :
            this(math.cross(b - a, c - a), a) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private DoublePlane(double3 normal, double distance, bool normalize)
        {
            Normal = normalize ? math.normalizesafe(normal) : normal;
            Distance = distance;
        }

        public static explicit operator Plane(DoublePlane plane)
        {
            Plane result = default;

            result.distance = (float)plane.Distance;
            result.normal = plane.Normal.ToVector3();

            return result;
        }

        public static explicit operator DoublePlane(Plane plane)
        {
            Vector3 normal = plane.normal;
            return new DoublePlane(normal.ToDouble3(), plane.distance, false);
        }

        public static bool operator ==(DoublePlane lhs, DoublePlane rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(DoublePlane lhs, DoublePlane rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override bool Equals(object other)
        {
            return other is DoublePlane plane && Equals(plane);
        }

        public bool Equals(DoublePlane other)
        {
            return Normal.Equals(other.Normal)
                   && Distance.Equals(other.Distance);
        }

        public override int GetHashCode()
        {
            int hashNormal = Normal.GetHashCode() << 2;

            int hashDistance = Distance.GetHashCode();

            return hashDistance ^ hashNormal;
        }

        /// <summary>
        /// For a given point returns the closest point on the plane.
        /// </summary>
        /// <param name="point">The point to project onto the plane.</param>
        /// <returns>
        /// A point on the plane that is closest to point.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3 ClosestPointOnPlane(double3 point)
        {
            double num = math.dot(Normal, point) + Distance;
            return point - Normal * num;
        }

        /// <summary>
        /// Returns a copy of the plane that faces in the opposite direction.
        /// </summary>
        public DoublePlane Flipped
        {
            get { return new DoublePlane(-Normal, -Distance); }
        }

        /// <summary>
        /// Returns a signed distance from plane to point.
        /// </summary>
        /// <param name="point">Get the distance from this point.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetDistanceToPoint(double3 point)
        {
            return math.dot(Normal, point) + Distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetSide(double3 point)
        {
            double3 origin = -Distance * Normal;
            return math.dot(point - origin, Normal) >= 0.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3 Raycast(double3 p1, double3 p2)
        {
            double3 origin = -Distance * Normal;
            double proj1 = math.dot(p1 - origin, Normal);
            double proj2 = math.dot(p2 - origin, Normal);
            double k = proj1 / (proj1 - proj2);
            return math.lerp(p1, p2, k);
        }

        /// <summary>
        /// Are two points on the same side of the plane?
        /// </summary>
        /// <param name="point0">First point to evaluate.</param>
        /// <param name="point1">Get if this second point is on the same side as <paramref name="point0"/>.</param>
        public bool SameSide(double3 point0, double3 point1)
        {
            double distanceToPoint1 = GetDistanceToPoint(point0);
            double distanceToPoint2 = GetDistanceToPoint(point1);
            return distanceToPoint1 > 0.0 && distanceToPoint2 > 0.0 || distanceToPoint1 <= 0.0 && distanceToPoint2 <= 0.0;
        }

        /// <summary>
        /// Returns a copy of the plane that is moved in space by the given translation.
        /// </summary>
        /// <param name="translation">The offset in space to move the plane with.</param>
        /// <returns>
        /// The translated plane.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DoublePlane Translate(double3 translation)
        {
            return new DoublePlane(Normal, Distance + math.dot(Normal, translation));
        }


        /// <summary>
        ///   <para>Returns a formatted string for the plane.</para>
        /// </summary>
        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for the plane.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for the plane.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";

            return string.Format(
                CultureInfo.InvariantCulture.NumberFormat,
                "Normal: {0}, Distance: {1}",
                Normal.ToString(format, formatProvider),
                Distance.ToString(format, formatProvider));
        }
    }
}
