using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int2: IEquatable<Int2>
	{

		/// <summary>
		/// Constructor of the Int2.
		/// </summary>
		public Int2(int scale)
		{
			this.X = scale;
			this.Y = scale;
		}

		/// <summary>
		/// Constructor of the Int2.
		/// </summary>
		public Int2(Int1 vec, int Y)
		{
			this.X = vec.X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Int2.
		/// </summary>
		public Int2(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Int2.
		/// </summary>
		public Int2(Int1 a)
		{
			this.X = a.X;
			this.Y = default(int);
		}

		/// <summary>
		/// Constructor of the Int2.
		/// </summary>
		public Int2(Int2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
		}

		/// <summary>
		/// The X component of the Int2.
		/// </summary>
		[FieldOffset(0)]
		public int X;

		/// <summary>
		/// The Y component of the Int2.
		/// </summary>
		[FieldOffset(4)]
		public int Y;

		public static readonly Int2 UnitX = new Int2(1, 0);

		public static readonly Int2 UnitY = new Int2(0, 1);

		public static readonly Int2 Zero = new Int2(0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Int2());

		public int Length { get { return (int)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); } }

		public int LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y); } }
		public void Normalize()
		{
			int len = this.Length;
			X /= len;
			Y /= len;
		}

		public static Int2 Multiply (Int2 left, Int2 right)
		{
			return new Int2((left.X * right.X), (left.Y * right.Y));
		}

		public static Int2 Multiply (Int2 left, int right)
		{
			return new Int2((left.X * right), (left.Y * right));
		}

		public static void Multiply (ref Int2 left, ref Int2 right, out Int2 result)
		{
			result = new Int2((left.X * right.X), (left.Y * right.Y));
		}

		public static void Add (ref Int2 left, ref Int2 right, out Int2 result)
		{
			result = new Int2((left.X + right.X), (left.Y + right.Y));
		}

		public static void Sub (ref Int2 left, ref Int2 right, out Int2 result)
		{
			result = new Int2((left.X - right.X), (left.Y - right.Y));
		}

		public static void Multiply (ref Int2 left, int right, out Int2 result)
		{
			result = new Int2((left.X * right), (left.Y * right));
		}

		public static int Dot (Int2 left, Int2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

		public static int Dot (ref Int2 left, ref Int2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}
		public static Int2 Normalize(Int2 vec)
		{
			int len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			return vec;
		}
		public static Int2 operator -(Int2 left, Int2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		public static Int2 operator +(Int2 left, Int2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		public static Int2 operator *(Int2 left, int scale)
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
		public static bool operator ==(Int2 left, Int2 right) { return left.Equals(right); }
		public static bool operator !=(Int2 left, Int2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int2 other)
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

		public static Int2 Scale (Int2 left, int scale)
		{
			return new Int2((left.X * scale), (left.Y * scale));
		}

	}
}
