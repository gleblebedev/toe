using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int4: IEquatable<Int4>
	{

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(int scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
			this.W = scale;
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(Int3 vec, int W)
		{
			this.X = vec.X;
			this.Y = vec.Y;
			this.Z = vec.Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(int);
			this.W = default(int);
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(int X, int Y, int Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = default(int);
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(int X, int Y, int Z, int W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(Int1 a)
		{
			this.X = a.X;
			this.Y = default(int);
			this.Z = default(int);
			this.W = default(int);
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(Int2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(int);
			this.W = default(int);
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(Int3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = default(int);
		}

		/// <summary>
		/// Constructor of the Int4.
		/// </summary>
		public Int4(Int4 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = a.W;
		}

		/// <summary>
		/// The Xy component of the Int4.
		/// </summary>
		public Int2 Xy {
			get { return new Int2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The Xyz component of the Int4.
		/// </summary>
		public Int3 Xyz {
			get { return new Int3(this.X, this.Y, this.Z); }
			set { this.X = value.X; this.Y = value.Y; this.Z = value.Z;  }
		}

		/// <summary>
		/// The X component of the Int4.
		/// </summary>
		[FieldOffset(0)]
		public int X;

		/// <summary>
		/// The Y component of the Int4.
		/// </summary>
		[FieldOffset(4)]
		public int Y;

		/// <summary>
		/// The Z component of the Int4.
		/// </summary>
		[FieldOffset(8)]
		public int Z;

		/// <summary>
		/// The W component of the Int4.
		/// </summary>
		[FieldOffset(12)]
		public int W;

		public static readonly Int4 UnitX = new Int4(1, 0, 0, 0);

		public static readonly Int4 UnitY = new Int4(0, 1, 0, 0);

		public static readonly Int4 UnitZ = new Int4(0, 0, 1, 0);

		public static readonly Int4 UnitW = new Int4(0, 0, 0, 1);

		public static readonly Int4 Zero = new Int4(0, 0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Int4());

		public int Length { get { return (int)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); } }

		public int LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W); } }
		public void Normalize()
		{
			int len = this.Length;
			X /= len;
			Y /= len;
			Z /= len;
			W /= len;
		}

		public static Int4 Multiply (Int4 left, Int4 right)
		{
			return new Int4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static Int4 Multiply (Int4 left, int right)
		{
			return new Int4((left.X * right), (left.Y * right), (left.Z * right), (left.W * right));
		}

		public static void Multiply (ref Int4 left, ref Int4 right, out Int4 result)
		{
			result = new Int4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static void Add (ref Int4 left, ref Int4 right, out Int4 result)
		{
			result = new Int4((left.X + right.X), (left.Y + right.Y), (left.Z + right.Z), (left.W + right.W));
		}

		public static void Sub (ref Int4 left, ref Int4 right, out Int4 result)
		{
			result = new Int4((left.X - right.X), (left.Y - right.Y), (left.Z - right.Z), (left.W - right.W));
		}

		public static void Multiply (ref Int4 left, int right, out Int4 result)
		{
			result = new Int4((left.X * right), (left.Y * right), (left.Z * right), (left.W * right));
		}

		public static int Dot (Int4 left, Int4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		public static int Dot (ref Int4 left, ref Int4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}
		public static Int4 Normalize(Int4 vec)
		{
			int len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			vec.Z /= len;
			vec.W /= len;
			return vec;
		}
		public static Int4 operator -(Int4 left, Int4 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}
		public static Int4 operator +(Int4 left, Int4 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}
		public static Int4 operator *(Int4 left, int scale)
		{
			left.X *= scale;
			left.Y *= scale;
			left.Z *= scale;
			left.W *= scale;
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
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				hashCode = (hashCode * 397) ^ this.W.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Int4 left, Int4 right) { return left.Equals(right); }
		public static bool operator !=(Int4 left, Int4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int4 other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z) && this.W.Equals(other.W);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3})", this.X, this.Y, this.Z, this.W); }
	}
	public static partial class MathHelper
	{

		public static Int4 Scale (Int4 left, int scale)
		{
			return new Int4((left.X * scale), (left.Y * scale), (left.Z * scale), (left.W * scale));
		}

	}
}
