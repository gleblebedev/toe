using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toe.Utils.ToeMath
{
	public partial struct Float3
	{
		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		/// <param name="result">The cross product of the two inputs</param>
		public static void Cross(ref Float3 left, ref Float3 right, out Float3 result)
		{
			result = new Float3(left.Y * right.Z - left.Z * right.Y,
				left.Z * right.X - left.X * right.Z,
				left.X * right.Y - left.Y * right.X);
		}

		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		public static Float3 Cross(Float3 left, Float3 right)
		{
			Float3 result;
			Cross(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void QuaternionTransform(ref Float3 vec, ref Float4 quat, out Float3 result)
		{
			// Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
			// vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
			Float3 xyz = quat.Xyz, temp, temp2;
			Float3.Cross(ref xyz, ref vec, out temp);
			Float3.Multiply(ref vec, quat.W, out temp2);
			Float3.Add(ref temp, ref temp2, out temp);
			Float3.Cross(ref xyz, ref temp, out temp);
			Float3.Multiply(ref temp, 2, out temp);
			Float3.Add(ref vec, ref temp, out result);
		}

			/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		public static Float3 QuaternionTransform(Float3 vec, Float4 quat)
		{
			Float3 res;
			QuaternionTransform(ref vec, ref quat, out res);
			return res;
		}
	}

	public partial struct Float4
	{
		public static readonly Float4 QuaternionIdentity = new Float4(0, 0, 0, 1);

		/// <summary>
		/// Convert the current quaternion to axis angle representation
		/// </summary>
		/// <param name="axis">The resultant axis</param>
		/// <param name="angle">The resultant angle</param>
		public static void ToAxisAngle(Float4 quaternion, out Float3 axis, out float angle)
		{
			var q = quaternion;
			if (Math.Abs(q.W) > 1.0f)
				q.Normalize();

			angle = 2.0f * (float)System.Math.Acos(q.W); // angle
			float den = (float)System.Math.Sqrt(1.0 - q.W * q.W);
			if (den > 0.0001f)
			{
				axis = MathHelper.Scale(q.Xyz, 1.0f / den);
			}
			else
			{
				// This occurs when the angle is zero. 
				// Not a problem: just set an arbitrary normalized axis.
				axis = Float3.UnitX;
			}

		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>A new instance containing the result of the calculation.</returns>
		public static Float4 QuaternionMultiply(Float4 left, Float4 right)
		{
			Float4 result;
			Multiply(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <param name="result">A new instance containing the result of the calculation.</param>
		public static void QuaternionMultiply(ref Float4 left, ref Float4 right, out Float4 result)
		{
			result = new Float4(
				left.Xyz * right.W + right.Xyz * left.W + Float3.Cross(left.Xyz, right.Xyz),
				left.W * right.W - Float3.Dot(left.Xyz, right.Xyz));
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="quaternion">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <param name="result">A new instance containing the result of the calculation.</param>
		public static void QuaternionMultiply(ref Float4 quaternion, float scale, out Float4 result)
		{
			result = new Float4(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="quaternion">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>A new instance containing the result of the calculation.</returns>
		public static Float4 QuaternionMultiply(Float4 quaternion, float scale)
		{
			return new Float4(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}
	}

	public partial struct Float4x4
	{
		public static Float4x4 Rotate(Float4 quaternion)
		{
			Float3 axis;
			float angle;
			Float4.ToAxisAngle(quaternion, out axis, out angle);
			return CreateFromAxisAngle(axis, angle);
		}

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <param name="result">A matrix instance.</param>
		public static void CreateFromAxisAngle(Float3 axis, float angle, out Float4x4 result)
		{
			float cos = (float)System.Math.Cos(-angle);
			float sin = (float)System.Math.Sin(-angle);
			float t = 1.0f - cos;

			axis.Normalize();

			result = new Float4x4(t * axis.X * axis.X + cos, t * axis.X * axis.Y - sin * axis.Z, t * axis.X * axis.Z + sin * axis.Y, 0.0f,
								 t * axis.X * axis.Y + sin * axis.Z, t * axis.Y * axis.Y + cos, t * axis.Y * axis.Z - sin * axis.X, 0.0f,
								 t * axis.X * axis.Z - sin * axis.Y, t * axis.Y * axis.Z + sin * axis.X, t * axis.Z * axis.Z + cos, 0.0f,
								 0, 0, 0, 1);
		}

		/// <summary>
		/// Matrix multiplication
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
		/// <returns>A new Matrix44 which holds the result of the multiplication</returns>
		public static Float4x4 operator *(Float4x4 left, Float4x4 right)
		{
			return Float4x4.Mult(left, right);
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
		/// <returns>A new instance that is the result of the multiplication</returns>
		public static Float4x4 Mult(Float4x4 left, Float4x4 right)
		{
			Float4x4 result;
			Mult(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
		/// <param name="result">A new instance that is the result of the multiplication</param>
		public static void Mult(ref Float4x4 left, ref Float4x4 right, out Float4x4 result)
		{
			float lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z, lM14 = left.Row0.W,
				lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z, lM24 = left.Row1.W,
				lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z, lM34 = left.Row2.W,
				lM41 = left.Row3.X, lM42 = left.Row3.Y, lM43 = left.Row3.Z, lM44 = left.Row3.W,
				rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z, rM14 = right.Row0.W,
				rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z, rM24 = right.Row1.W,
				rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z, rM34 = right.Row2.W,
				rM41 = right.Row3.X, rM42 = right.Row3.Y, rM43 = right.Row3.Z, rM44 = right.Row3.W;

			result = new Float4x4(
			 (((lM11 * rM11) + (lM12 * rM21)) + (lM13 * rM31)) + (lM14 * rM41),
			 (((lM11 * rM12) + (lM12 * rM22)) + (lM13 * rM32)) + (lM14 * rM42),
			 (((lM11 * rM13) + (lM12 * rM23)) + (lM13 * rM33)) + (lM14 * rM43),
			 (((lM11 * rM14) + (lM12 * rM24)) + (lM13 * rM34)) + (lM14 * rM44),
			 (((lM21 * rM11) + (lM22 * rM21)) + (lM23 * rM31)) + (lM24 * rM41),
			 (((lM21 * rM12) + (lM22 * rM22)) + (lM23 * rM32)) + (lM24 * rM42),
			 (((lM21 * rM13) + (lM22 * rM23)) + (lM23 * rM33)) + (lM24 * rM43),
			 (((lM21 * rM14) + (lM22 * rM24)) + (lM23 * rM34)) + (lM24 * rM44),
			 (((lM31 * rM11) + (lM32 * rM21)) + (lM33 * rM31)) + (lM34 * rM41),
			 (((lM31 * rM12) + (lM32 * rM22)) + (lM33 * rM32)) + (lM34 * rM42),
			 (((lM31 * rM13) + (lM32 * rM23)) + (lM33 * rM33)) + (lM34 * rM43),
			 (((lM31 * rM14) + (lM32 * rM24)) + (lM33 * rM34)) + (lM34 * rM44),
			 (((lM41 * rM11) + (lM42 * rM21)) + (lM43 * rM31)) + (lM44 * rM41),
			 (((lM41 * rM12) + (lM42 * rM22)) + (lM43 * rM32)) + (lM44 * rM42),
			 (((lM41 * rM13) + (lM42 * rM23)) + (lM43 * rM33)) + (lM44 * rM43),
			 (((lM41 * rM14) + (lM42 * rM24)) + (lM43 * rM34)) + (lM44 * rM44));
		}

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <returns>A matrix instance.</returns>
		public static Float4x4 CreateFromAxisAngle(Float3 axis, float angle)
		{
			Float4x4 result;
			CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="vector">The translation vector.</param>
		/// <returns>The resulting Matrix4 instance.</returns>
		public static Float4x4 CreateTranslation(Float3 vector)
		{
			Float4x4 result;
			CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="vector">The translation vector.</param>
		/// <param name="result">The resulting Matrix4 instance.</param>
		public static void CreateTranslation(ref Float3 vector, out Float4x4 result)
		{
			result = Identity;
			result.Row3 = new Float4(vector.X, vector.Y, vector.Z, 1);
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="x">X translation.</param>
		/// <param name="y">Y translation.</param>
		/// <param name="z">Z translation.</param>
		/// <param name="result">The resulting Matrix4 instance.</param>
		public static void CreateTranslation(float x, float y, float z, out Float4x4 result)
		{
			result = Identity;
			result.Row3 = new Float4(x, y, z, 1);
		}
	}
}
