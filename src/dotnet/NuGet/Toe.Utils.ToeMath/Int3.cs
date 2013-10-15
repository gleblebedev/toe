using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int3: IEquatable<Int3>
	{

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(int scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(Int2 vec, int Z)
		{
			this.X = vec.X;
			this.Y = vec.Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(int);
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(int X, int Y, int Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(Int1 a)
		{
			this.X = a.X;
			this.Y = default(int);
			this.Z = default(int);
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(Int2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(int);
		}

		/// <summary>
		/// Constructor of the Int3.
		/// </summary>
		public Int3(Int3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
		}

		/// <summary>
		/// The Xy component of the Int3.
		/// </summary>
		public Int2 Xy {
			get { return new Int2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The X component of the Int3.
		/// </summary>
		[FieldOffset(0)]
		public int X;

		/// <summary>
		/// The Y component of the Int3.
		/// </summary>
		[FieldOffset(4)]
		public int Y;

		/// <summary>
		/// The Z component of the Int3.
		/// </summary>
		[FieldOffset(8)]
		public int Z;

		public static readonly Int3 UnitX = new Int3(1, 0, 0);

		public static readonly Int3 UnitY = new Int3(0, 1, 0);

		public static readonly Int3 UnitZ = new Int3(0, 0, 1);

		public static readonly Int3 Zero = new Int3(0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Int3());

		public int Length { get { return (int)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); } }

		public int LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z); } }
		public void Normalize()
		{
			int len = this.Length;
			X /= len;
			Y /= len;
			Z /= len;
		}

		public static Int3 Multiply (Int3 left, Int3 right)
		{
			return new Int3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static Int3 Multiply (Int3 left, int right)
		{
			return new Int3((left.X * right), (left.Y * right), (left.Z * right));
		}

		public static void Multiply (ref Int3 left, ref Int3 right, out Int3 result)
		{
			result = new Int3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static void Add (ref Int3 left, ref Int3 right, out Int3 result)
		{
			result = new Int3((left.X + right.X), (left.Y + right.Y), (left.Z + right.Z));
		}

		public static void Sub (ref Int3 left, ref Int3 right, out Int3 result)
		{
			result = new Int3((left.X - right.X), (left.Y - right.Y), (left.Z - right.Z));
		}

		public static void Multiply (ref Int3 left, int right, out Int3 result)
		{
			result = new Int3((left.X * right), (left.Y * right), (left.Z * right));
		}

		public static int Dot (Int3 left, Int3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		public static int Dot (ref Int3 left, ref Int3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}
		public static Int3 Normalize(Int3 vec)
		{
			int len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			vec.Z /= len;
			return vec;
		}
		public static Int3 operator -(Int3 left, Int3 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}
		public static Int3 operator +(Int3 left, Int3 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}
		public static Int3 operator *(Int3 left, int scale)
		{
			left.X *= scale;
			left.Y *= scale;
			left.Z *= scale;
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
				return hashCode;
			}
		}
		public static bool operator ==(Int3 left, Int3 right) { return left.Equals(right); }
		public static bool operator !=(Int3 left, Int3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int3 other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
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

		public override string ToString() { return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z); }
	}
	public static partial class MathHelper
	{

		public static Int3 Scale (Int3 left, int scale)
		{
			return new Int3((left.X * scale), (left.Y * scale), (left.Z * scale));
		}

	}
}
