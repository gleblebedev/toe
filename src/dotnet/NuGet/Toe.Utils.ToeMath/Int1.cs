using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int1: IEquatable<Int1>
	{

		/// <summary>
		/// Constructor of the Int1.
		/// </summary>
		public Int1(int scale)
		{
			this.X = scale;
		}

		/// <summary>
		/// Constructor of the Int1.
		/// </summary>
		public Int1(Int1 a)
		{
			this.X = a.X;
		}

		/// <summary>
		/// The X component of the Int1.
		/// </summary>
		[FieldOffset(0)]
		public int X;

		public static readonly Int1 UnitX = new Int1(1);

		public static readonly Int1 Zero = new Int1(0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Int1());

		public int Length { get { return (int)Math.Sqrt((this.X * this.X)); } }

		public int LengthSquared { get { return (this.X * this.X); } }
		public void Normalize()
		{
			int len = this.Length;
			X /= len;
		}

		public static Int1 Multiply (Int1 left, Int1 right)
		{
			return new Int1((left.X * right.X));
		}

		public static Int1 Multiply (Int1 left, int right)
		{
			return new Int1((left.X * right));
		}

		public static void Multiply (ref Int1 left, ref Int1 right, out Int1 result)
		{
			result = new Int1((left.X * right.X));
		}

		public static void Add (ref Int1 left, ref Int1 right, out Int1 result)
		{
			result = new Int1((left.X + right.X));
		}

		public static void Sub (ref Int1 left, ref Int1 right, out Int1 result)
		{
			result = new Int1((left.X - right.X));
		}

		public static void Multiply (ref Int1 left, int right, out Int1 result)
		{
			result = new Int1((left.X * right));
		}

		public static int Dot (Int1 left, Int1 right)
		{
			return (left.X * right.X);
		}

		public static int Dot (ref Int1 left, ref Int1 right)
		{
			return (left.X * right.X);
		}
		public static Int1 Normalize(Int1 vec)
		{
			int len = vec.Length;
			vec.X /= len;
			return vec;
		}
		public static Int1 operator -(Int1 left, Int1 right)
		{
			left.X -= right.X;
			return left;
		}
		public static Int1 operator +(Int1 left, Int1 right)
		{
			left.X += right.X;
			return left;
		}
		public static Int1 operator *(Int1 left, int scale)
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
		public static bool operator ==(Int1 left, Int1 right) { return left.Equals(right); }
		public static bool operator !=(Int1 left, Int1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int1 other)
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

		public static Int1 Scale (Int1 left, int scale)
		{
			return new Int1((left.X * scale));
		}

	}
}
