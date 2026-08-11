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
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Esri.ArcGISMapsSDK.Utils.Math
{
	[Serializable]
	public struct quaterniond
	{
		/// <summary>
		/// The quaternion component values.
		/// </summary>
		public double4 value;

		/// <summary>A quaternion representing the identity transform.</summary>
		public static readonly quaterniond identity = new(0.0, 0.0, 0.0, 1.0);

		/// <summary>Constructs a quaternion from four double values.</summary>
		/// <param name="x">The quaternion x component.</param>
		/// <param name="y">The quaternion y component.</param>
		/// <param name="z">The quaternion z component.</param>
		/// <param name="w">The quaternion w component.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaterniond(double x, double y, double z, double w) { value.x = x; value.y = y; value.z = z; value.w = w; }

		/// <summary>Constructs a quaternion from double4 vector.</summary>
		/// <param name="value">The quaternion xyzw component values.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaterniond(double4 value) { this.value = value; }

		/// <summary>Constructs a unit quaternion from a double3x3 rotation matrix. The matrix must be orthonormal.</summary>
		/// <param name="m">The double3x3 orthonormal rotation matrix.</param>
		public quaterniond(double3x3 m)
		{
			var x = System.Math.Sqrt(System.Math.Max(0, 1 + m.c0.x - m.c1.y - m.c2.z)) / 2;
			var y = System.Math.Sqrt(System.Math.Max(0, 1 - m.c0.x + m.c1.y - m.c2.z)) / 2;
			var z = System.Math.Sqrt(System.Math.Max(0, 1 - m.c0.x - m.c1.y + m.c2.z)) / 2;
			var w = System.Math.Sqrt(System.Math.Max(0, 1 + m.c0.x + m.c1.y + m.c2.z)) / 2;

			value.x = x * System.Math.Sign(x * (m.c1.z - m.c2.y));
			value.y = y * System.Math.Sign(y * (m.c2.x - m.c0.z));
			value.z = z * System.Math.Sign(z * (m.c0.y - m.c1.x));
			value.w = w;
		}

		/// <summary>Constructs a unit quaternion from an orthonormal double4x4 matrix.</summary>
		/// <param name="m">The double4x4 orthonormal rotation matrix.</param>
		public quaterniond(double4x4 m)
		{
			var x = System.Math.Sqrt(System.Math.Max(0, 1 + m.c0.x - m.c1.y - m.c2.z)) / 2;
			var y = System.Math.Sqrt(System.Math.Max(0, 1 - m.c0.x + m.c1.y - m.c2.z)) / 2;
			var z = System.Math.Sqrt(System.Math.Max(0, 1 - m.c0.x - m.c1.y + m.c2.z)) / 2;
			var w = System.Math.Sqrt(System.Math.Max(0, 1 + m.c0.x + m.c1.y + m.c2.z)) / 2;

			value.x = x * System.Math.Sign(x * (m.c1.z - m.c2.y));
			value.y = y * System.Math.Sign(y * (m.c2.x - m.c0.z));
			value.z = z * System.Math.Sign(z * (m.c0.y - m.c1.x));
			value.w = w;
		}

		/// <summary>
		/// Returns a quaternion representing a rotation around a unit axis by an angle in radians.
		/// The rotation direction is clockwise when looking along the rotation axis towards the origin.
		/// </summary>
		/// <param name="axis">The axis of rotation.</param>
		/// <param name="angle">The angle of rotation in radians.</param>
		/// <returns>The quaternion representing a rotation around an axis.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond AxisAngle(double3 axis, double angle)
		{
			sincos(0.5 * angle, out var sina, out var cosa);
			return new quaterniond(double4(axis * sina, cosa));
		}

		/// <summary>
		/// Returns a quaternion constructed by first performing a rotation around the z-axis, then the x-axis and finally the y-axis.
		/// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
		/// This is the default order rotation order in Unity.
		/// </summary>
		/// <param name="xyz">A double3 vector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
		/// <returns>The quaternion representing the Euler angle rotation in z-x-y order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond EulerZXY(double3 xyz)
		{
			// return mul(rotateY(xyz.y), mul(rotateX(xyz.x), rotateZ(xyz.z)));
			sincos(0.5 * xyz, out var s, out var c);
			return new quaterniond
			(
				// s.x * c.y * c.z + s.y * s.z * c.x,
				// s.y * c.x * c.z - s.x * s.z * c.y,
				// s.z * c.x * c.y - s.x * s.y * c.z,
				// c.x * c.y * c.z + s.y * s.z * s.x
				double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(1.0, -1.0, -1.0, 1.0)
			);
		}

		/// <summary>Returns a quaternion that rotates around the x-axis by a given number of radians.</summary>
		/// <param name="angle">The clockwise rotation angle when looking along the x-axis towards the origin in radians.</param>
		/// <returns>The quaternion representing a rotation around the x-axis.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond RotateX(double angle)
		{
			sincos(0.5 * angle, out var sina, out var cosa);
			return new quaterniond(sina, 0.0, 0.0, cosa);
		}

		/// <summary>Returns a quaternion that rotates around the y-axis by a given number of radians.</summary>
		/// <param name="angle">The clockwise rotation angle when looking along the y-axis towards the origin in radians.</param>
		/// <returns>The quaternion representing a rotation around the y-axis.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond RotateY(double angle)
		{
			sincos(0.5 * angle, out var sina, out var cosa);
			return new quaterniond(0.0, sina, 0.0, cosa);
		}

		/// <summary>Returns a quaternion that rotates around the z-axis by a given number of radians.</summary>
		/// <param name="angle">The clockwise rotation angle when looking along the z-axis towards the origin in radians.</param>
		/// <returns>The quaternion representing a rotation around the z-axis.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond RotateZ(double angle)
		{
			sincos(0.5 * angle, out var sina, out var cosa);
			return new quaterniond(0.0, 0.0, sina, cosa);
		}

		/// <summary>
		/// Returns a quaternion view rotation given a unit length forward vector and a unit length up vector.
		/// The two input vectors are assumed to be unit length and not collinear.
		/// If these assumptions are not met use double3x3.LookRotationSafe instead.
		/// </summary>
		/// <param name="forward">The view forward direction.</param>
		/// <param name="up">The view up direction.</param>
		/// <returns>The quaternion view rotation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond LookRotation(double3 forward, double3 up)
		{
			var t = math.normalize(cross(up, forward));
			return new quaterniond(double3x3(t, cross(forward, t), forward));
		}

		/// <summary>
		/// Returns a quaternion view rotation given a forward vector and an up vector.
		/// The two input vectors are not assumed to be unit length.
		/// If the magnitude of either of the vectors is so extreme that the calculation cannot be carried out reliably or the vectors are collinear,
		/// the identity will be returned instead.
		/// </summary>
		/// <param name="forward">The view forward direction.</param>
		/// <param name="up">The view up direction.</param>
		/// <returns>The quaternion view rotation or the identity quaternion.</returns>
		public static quaterniond LookRotationSafe(double3 forward, double3 up)
		{
			var forwardLengthSq = math.dot(forward, forward);
			var upLengthSq = math.dot(up, up);

			forward *= rsqrt(forwardLengthSq);
			up *= rsqrt(upLengthSq);

			var t = cross(up, forward);
			var tLengthSq = math.dot(t, t);
			t *= rsqrt(tLengthSq);

			var mn = min(min(forwardLengthSq, upLengthSq), tLengthSq);
			var mx = max(max(forwardLengthSq, upLengthSq), tLengthSq);

			var accept = mn > 1e-35 && mx < 1e35 && isfinite(forwardLengthSq) && isfinite(upLengthSq) && isfinite(tLengthSq);
			return new quaterniond(select(double4(0.0, 0.0, 0.0, 1.0), new quaterniond(double3x3(t, cross(forward, t), forward)).value, accept));
		}

		/// <summary>Returns true if the quaternion is equal to a given quaternion, false otherwise.</summary>
		/// <param name="x">The quaternion to compare with.</param>
		/// <returns>True if the quaternion is equal to the input, false otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(quaterniond x) { return value.x == x.value.x && value.y == x.value.y && value.z == x.value.z && value.w == x.value.w; }

		/// <summary>Returns whether true if the quaternion is equal to a given quaternion, false otherwise.</summary>
		/// <param name="x">The object to compare with.</param>
		/// <returns>True if the quaternion is equal to the input, false otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object x) { return x is quaterniond converted && Equals(converted); }

		/// <summary>Returns a hash code for the quaternion.</summary>
		/// <returns>The hash code of the quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() { return (int)hash(this); }

		/// <summary>Returns a string representation of the quaternion.</summary>
		/// <returns>The string representation of the quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("quaterniond({0}f, {1}f, {2}f, {3}f)", value.x, value.y, value.z, value.w);
		}

		/// <summary>Returns a string representation of the quaternion using a specified format and culture-specific format information.</summary>
		/// <param name="format">The format string.</param>
		/// <param name="formatProvider">The format provider to use during string formatting.</param>
		/// <returns>The formatted string representation of the quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("quaterniond({0}f, {1}f, {2}f, {3}f)", value.x.ToString(format, formatProvider), value.y.ToString(format, formatProvider), value.z.ToString(format, formatProvider), value.w.ToString(format, formatProvider));
		}

		/// <summary>Returns the conjugate of a quaternion value.</summary>
		/// <param name="q">The quaternion to conjugate.</param>
		/// <returns>The conjugate of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond conjugate(quaterniond q)
		{
			return new quaterniond(q.value * double4(-1.0, -1.0, -1.0, 1.0));
		}

		/// <summary>Returns the inverse of a quaternion value.</summary>
		/// <param name="q">The quaternion to invert.</param>
		/// <returns>The quaternion inverse of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond inverse(quaterniond q)
		{
			var x = q.value;
			return new quaterniond(rcp(math.dot(x, x)) * x * double4(-1.0, -1.0, -1.0, 1.0));
		}

		/// <summary>Returns the dot product of two quaternions.</summary>
		/// <param name="a">The first quaternion.</param>
		/// <param name="b">The second quaternion.</param>
		/// <returns>The dot product of two quaternions.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double dot(quaterniond a, quaterniond b)
		{
			return math.dot(a.value, b.value);
		}

		/// <summary>Returns the length of a quaternion.</summary>
		/// <param name="q">The input quaternion.</param>
		/// <returns>The length of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double length(quaterniond q)
		{
			return sqrt(math.dot(q.value, q.value));
		}

		/// <summary>Returns the squared length of a quaternion.</summary>
		/// <param name="q">The input quaternion.</param>
		/// <returns>The length squared of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lengthsq(quaterniond q)
		{
			return math.dot(q.value, q.value);
		}

		/// <summary>Returns a normalized version of a quaternion q by scaling it by 1 / length(q).</summary>
		/// <param name="q">The quaternion to normalize.</param>
		/// <returns>The normalized quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond normalize(quaterniond q)
		{
			var x = q.value;
			return new quaterniond(rsqrt(math.dot(x, x)) * x);
		}

		/// <summary>
		/// Returns a safe normalized version of the q by scaling it by 1 / length(q).
		/// Returns the identity when 1 / length(q) does not produce a finite number.
		/// </summary>
		/// <param name="q">The quaternion to normalize.</param>
		/// <returns>The normalized quaternion or the identity quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond normalizesafe(quaterniond q)
		{
			var x = q.value;
			var len = math.dot(x, x);
			return new quaterniond(select(identity.value, x * rsqrt(len), len > FLT_MIN_NORMAL));
		}

		/// <summary>
		/// Returns a safe normalized version of the q by scaling it by 1 / length(q).
		/// Returns the given default value when 1 / length(q) does not produce a finite number.
		/// </summary>
		/// <param name="q">The quaternion to normalize.</param>
		/// <param name="defaultvalue">The default value.</param>
		/// <returns>The normalized quaternion or the default value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond normalizesafe(quaternion q, quaternion defaultvalue)
		{
			double4 x = q.value;
			var len = math.dot(x, x);
			return new quaterniond(select(defaultvalue.value, x * rsqrt(len), len > FLT_MIN_NORMAL));
		}

		/// <summary>Returns the natural exponent of a quaternion. Assumes w is zero.</summary>
		/// <param name="q">The quaternion with w component equal to zero.</param>
		/// <returns>The natural exponent of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond unitexp(quaterniond q)
		{
			var v_rcp_len = rsqrt(math.dot(q.value.xyz, q.value.xyz));
			var v_len = rcp(v_rcp_len);
			sincos(v_len, out var sin_v_len, out var cos_v_len);
			return new quaterniond(double4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len));
		}

		/// <summary>Returns the natural exponent of a quaternion.</summary>
		/// <param name="q">The quaternion.</param>
		/// <returns>The natural exponent of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond exp(quaterniond q)
		{
			var v_rcp_len = rsqrt(math.dot(q.value.xyz, q.value.xyz));
			var v_len = rcp(v_rcp_len);
			sincos(v_len, out var sin_v_len, out var cos_v_len);
			return new quaterniond(double4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len) * math.exp(q.value.w));
		}

		/// <summary>Returns the natural logarithm of a unit length quaternion.</summary>
		/// <param name="q">The unit length quaternion.</param>
		/// <returns>The natural logarithm of the unit length quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond unitlog(quaterniond q)
		{
			var w = clamp(q.value.w, -1.0, 1.0);
			var s = acos(w) * rsqrt(1.0 - w * w);
			return new quaterniond(double4(q.value.xyz * s, 0.0));
		}

		/// <summary>Returns the natural logarithm of a quaternion.</summary>
		/// <param name="q">The quaternion.</param>
		/// <returns>The natural logarithm of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond log(quaterniond q)
		{
			var v_len_sq = math.dot(q.value.xyz, q.value.xyz);
			var q_len_sq = v_len_sq + q.value.w * q.value.w;

			var s = acos(clamp(q.value.w * rsqrt(q_len_sq), -1.0, 1.0)) * rsqrt(v_len_sq);
			return new quaterniond(double4(q.value.xyz * s, 0.5 * math.log(q_len_sq)));
		}

		/// <summary>Returns the result of transforming the quaternion b by the quaternion a.</summary>
		/// <param name="a">The quaternion on the left.</param>
		/// <param name="b">The quaternion on the right.</param>
		/// <returns>The result of transforming quaternion b by the quaternion a.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond mul(quaterniond a, quaterniond b)
		{
			return new quaterniond(a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy) * double4(1.0, 1.0, 1.0, -1.0) - a.value.zxyz * b.value.yzxz);
		}

		/// <summary>Returns the result of transforming a vector by a quaternion.</summary>
		/// <param name="q">The quaternion transformation.</param>
		/// <param name="v">The vector to transform.</param>
		/// <returns>The transformation of vector v by quaternion q.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(quaterniond q, double3 v)
		{
			var t = 2 * cross(q.value.xyz, v);
			return v + q.value.w * t + cross(q.value.xyz, t);
		}

		/// <summary>Returns the result of rotating a vector by a unit quaternion.</summary>
		/// <param name="q">The quaternion rotation.</param>
		/// <param name="v">The vector to rotate.</param>
		/// <returns>The rotation of vector v by quaternion q.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 rotate(quaterniond q, double3 v)
		{
			var t = 2 * cross(q.value.xyz, v);
			return v + q.value.w * t + cross(q.value.xyz, t);
		}

		/// <summary>Returns the result of a normalized linear interpolation between two quaternions q1 and a2 using an interpolation parameter t.</summary>
		/// <remarks>
		/// Prefer to use this over slerp() when you know the distance between q1 and q2 is small. This can be much
		/// higher performance due to avoiding trigonometric function evaluations that occur in slerp().
		/// </remarks>
		/// <param name="q1">The first quaternion.</param>
		/// <param name="q2">The second quaternion.</param>
		/// <param name="t">The interpolation parameter.</param>
		/// <returns>The normalized linear interpolation of two quaternions.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond nlerp(quaterniond q1, quaterniond q2, double t)
		{
			var dt = dot(q1, q2);
			if (dt < 0.0)
			{
				q2.value = -q2.value;
			}

			return normalize(new quaterniond(lerp(q1.value, q2.value, t)));
		}

		/// <summary>Returns the result of a spherical interpolation between two quaternions q1 and a2 using an interpolation parameter t.</summary>
		/// <param name="q1">The first quaternion.</param>
		/// <param name="q2">The second quaternion.</param>
		/// <param name="t">The interpolation parameter.</param>
		/// <returns>The spherical linear interpolation of two quaternions.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaterniond slerp(quaterniond q1, quaterniond q2, double t)
		{
			var dt = dot(q1, q2);
			if (dt < 0.0)
			{
				dt = -dt;
				q2.value = -q2.value;
			}

			if (dt < 0.9995)
			{
				var angle = acos(dt);
				var s = rsqrt(1.0 - dt * dt);    // 1.0 / sin(angle)
				var w1 = sin(angle * (1.0 - t)) * s;
				var w2 = sin(angle * t) * s;
				return new quaterniond(q1.value * w1 + q2.value * w2);
			}
			else
			{
				// if the angle is small, use linear interpolation
				return nlerp(q1, q2, t);
			}
		}

		/// <summary>Returns a uint hash code of a quaternion.</summary>
		/// <param name="q">The quaternion to hash.</param>
		/// <returns>The hash code for the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(quaterniond q)
		{
			return math.hash(q.value);
		}

		/// <summary>
		/// Returns a uint4 vector hash code of a quaternion.
		/// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
		/// that are only reduced to a narrow uint hash at the very end instead of at every step.
		/// </summary>
		/// <param name="q">The quaternion to hash.</param>
		/// <returns>The uint4 vector hash code of the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(quaterniond q)
		{
			return math.hashwide(q.value);
		}

		/// <summary>
		/// Transforms the forward vector by a quaternion.
		/// </summary>
		/// <param name="q">The quaternion transformation.</param>
		/// <returns>The forward vector transformed by the input quaternion.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 forward(quaterniond q) { return mul(q, double3(0, 0, 1)); }  // for compatibility
	}
}
