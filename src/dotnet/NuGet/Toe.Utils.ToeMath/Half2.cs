using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Half2: IEquatable<Half2>
	{

		/// <summary>
		/// Constructor of the Half2.
		/// </summary>
		public Half2(half scale)
		{
			this.X = scale;
			this.Y = scale;
		}

		/// <summary>
		/// Constructor of the Half2.
		/// </summary>
		public Half2(Half1 vec, half Y)
		{
			this.X = vec.X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Half2.
		/// </summary>
		public Half2(half X, half Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Half2.
		/// </summary>
		public Half2(Half1 a)
		{
			this.X = a.X;
			this.Y = default(half);
		}

		/// <summary>
		/// Constructor of the Half2.
		/// </summary>
		public Half2(Half2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
		}

		/// <summary>
		/// The X component of the Half2.
		/// </summary>
		[FieldOffset(0)]
		public half X;

		/// <summary>
		/// The Y component of the Half2.
		/// </summary>
		[FieldOffset(2)]
		public half Y;

		public static readonly Half2 UnitX = new Half2(1, 0);

		public static readonly Half2 UnitY = new Half2(0, 1);

		public static readonly Half2 Zero = new Half2(0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Half2());

		public half Length { get { return (half)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); } }

		public half LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y); } }
		public void Normalize()
		{
			half len = this.Length;
			X /= len;
			Y /= len;
		}

		public static Half2 Multiply (Half2 left, Half2 right)
		{
			return new Half2((left.X * right.X), (left.Y * right.Y));
		}

		public static Half2 Multiply (Half2 left, half right)
		{
			return new Half2((left.X * right), (left.Y * right));
		}

		public static void Multiply (ref Half2 left, ref Half2 right, out Half2 result)
		{
			result = new Half2((left.X * right.X), (left.Y * right.Y));
		}

		public static void Add (ref Half2 left, ref Half2 right, out Half2 result)
		{
			result = new Half2((left.X + right.X), (left.Y + right.Y));
		}

		public static void Sub (ref Half2 left, ref Half2 right, out Half2 result)
		{
			result = new Half2((left.X - right.X), (left.Y - right.Y));
		}

		public static void Multiply (ref Half2 left, half right, out Half2 result)
		{
			result = new Half2((left.X * right), (left.Y * right));
		}

		public static half Dot (Half2 left, Half2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

		public static half Dot (ref Half2 left, ref Half2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}
		public static Half2 Normalize(Half2 vec)
		{
			half len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			return vec;
		}
		public static Half2 operator -(Half2 left, Half2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		public static Half2 operator +(Half2 left, Half2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		public static Half2 operator *(Half2 left, half scale)
		{
			left.X *= scale;
			left.Y *= scale;
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
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Half2 left, Half2 right) { return left.Equals(right); }
		public static bool operator !=(Half2 left, Half2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half2 other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y);
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

		public override string ToString() { return string.Format("({0}, {1})", this.X, this.Y); }
	}
	public static partial class MathHelper
	{

		public static Half2 Scale (Half2 left, half scale)
		{
			return new Half2((left.X * scale), (left.Y * scale));
		}

	}
}
