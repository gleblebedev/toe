using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Half1: IEquatable<Half1>
	{

		/// <summary>
		/// Constructor of the Half1.
		/// </summary>
		public Half1(half scale)
		{
			this.X = scale;
		}

		/// <summary>
		/// Constructor of the Half1.
		/// </summary>
		public Half1(Half1 a)
		{
			this.X = a.X;
		}

		/// <summary>
		/// The X component of the Half1.
		/// </summary>
		[FieldOffset(0)]
		public half X;

		public static readonly Half1 UnitX = new Half1(1);

		public static readonly Half1 Zero = new Half1(0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Half1());

		public half Length { get { return (half)Math.Sqrt((this.X * this.X)); } }

		public half LengthSquared { get { return (this.X * this.X); } }
		public void Normalize()
		{
			half len = this.Length;
			X /= len;
		}

		public static Half1 Multiply (Half1 left, Half1 right)
		{
			return new Half1((left.X * right.X));
		}

		public static Half1 Multiply (Half1 left, half right)
		{
			return new Half1((left.X * right));
		}

		public static void Multiply (ref Half1 left, ref Half1 right, out Half1 result)
		{
			result = new Half1((left.X * right.X));
		}

		public static void Add (ref Half1 left, ref Half1 right, out Half1 result)
		{
			result = new Half1((left.X + right.X));
		}

		public static void Sub (ref Half1 left, ref Half1 right, out Half1 result)
		{
			result = new Half1((left.X - right.X));
		}

		public static void Multiply (ref Half1 left, half right, out Half1 result)
		{
			result = new Half1((left.X * right));
		}

		public static half Dot (Half1 left, Half1 right)
		{
			return (left.X * right.X);
		}

		public static half Dot (ref Half1 left, ref Half1 right)
		{
			return (left.X * right.X);
		}
		public static Half1 Normalize(Half1 vec)
		{
			half len = vec.Length;
			vec.X /= len;
			return vec;
		}
		public static Half1 operator -(Half1 left, Half1 right)
		{
			left.X -= right.X;
			return left;
		}
		public static Half1 operator +(Half1 left, Half1 right)
		{
			left.X += right.X;
			return left;
		}
		public static Half1 operator *(Half1 left, half scale)
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
		public static bool operator ==(Half1 left, Half1 right) { return left.Equals(right); }
		public static bool operator !=(Half1 left, Half1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half1 other)
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

		public static Half1 Scale (Half1 left, half scale)
		{
			return new Half1((left.X * scale));
		}

	}
}
