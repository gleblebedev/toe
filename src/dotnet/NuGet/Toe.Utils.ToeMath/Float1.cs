using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Float1: IEquatable<Float1>
	{

		/// <summary>
		/// Constructor of the Float1.
		/// </summary>
		public Float1(float scale)
		{
			this.X = scale;
		}

		/// <summary>
		/// Constructor of the Float1.
		/// </summary>
		public Float1(Float1 a)
		{
			this.X = a.X;
		}

		/// <summary>
		/// The X component of the Float1.
		/// </summary>
		[FieldOffset(0)]
		public float X;

		public static readonly Float1 UnitX = new Float1(1);

		public static readonly Float1 Zero = new Float1(0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Float1());

		public float Length { get { return (float)Math.Sqrt((this.X * this.X)); } }

		public float LengthSquared { get { return (this.X * this.X); } }
		public void Normalize()
		{
			float scale = 1.0f/this.Length;
			X *= scale;
		}

		public static Float1 Multiply (Float1 left, Float1 right)
		{
			return new Float1((left.X * right.X));
		}

		public static Float1 Multiply (Float1 left, float right)
		{
			return new Float1((left.X * right));
		}

		public static void Multiply (ref Float1 left, ref Float1 right, out Float1 result)
		{
			result = new Float1((left.X * right.X));
		}

		public static void Add (ref Float1 left, ref Float1 right, out Float1 result)
		{
			result = new Float1((left.X + right.X));
		}

		public static void Sub (ref Float1 left, ref Float1 right, out Float1 result)
		{
			result = new Float1((left.X - right.X));
		}

		public static void Multiply (ref Float1 left, float right, out Float1 result)
		{
			result = new Float1((left.X * right));
		}

		public static float Dot (Float1 left, Float1 right)
		{
			return (left.X * right.X);
		}

		public static float Dot (ref Float1 left, ref Float1 right)
		{
			return (left.X * right.X);
		}
		public static Float1 Normalize(Float1 vec)
		{
			float scale = 1.0f/vec.Length;
			vec.X *= scale;
			return vec;
		}
		public static Float1 operator -(Float1 left, Float1 right)
		{
			left.X -= right.X;
			return left;
		}
		public static Float1 operator +(Float1 left, Float1 right)
		{
			left.X += right.X;
			return left;
		}
		public static Float1 operator *(Float1 left, float scale)
		{
			left.X *= scale;
			return left;
		}
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.X.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Float1 left, Float1 right) { return left.Equals(right); }
		public static bool operator !=(Float1 left, Float1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float1 other)
		{
			return this.X.Equals(other.X);
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is Float4 && Equals((Float4)obj);
		}

		public override string ToString() { return string.Format("({0})", this.X); }
	}
	public static partial class MathHelper
	{

		public static Float1 Scale (Float1 left, float scale)
		{
			return new Float1((left.X * scale));
		}

	}
}
