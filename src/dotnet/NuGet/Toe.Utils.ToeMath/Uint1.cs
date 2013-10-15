using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint1: IEquatable<Uint1>
	{

		/// <summary>
		/// Constructor of the Uint1.
		/// </summary>
		public Uint1(uint scale)
		{
			this.X = scale;
		}

		/// <summary>
		/// Constructor of the Uint1.
		/// </summary>
		public Uint1(Uint1 a)
		{
			this.X = a.X;
		}

		/// <summary>
		/// The X component of the Uint1.
		/// </summary>
		[FieldOffset(0)]
		public uint X;

		public static readonly Uint1 UnitX = new Uint1(1);

		public static readonly Uint1 Zero = new Uint1(0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Uint1());

		public uint Length { get { return (uint)Math.Sqrt((this.X * this.X)); } }

		public uint LengthSquared { get { return (this.X * this.X); } }
		public void Normalize()
		{
			uint len = this.Length;
			X /= len;
		}

		public static Uint1 Multiply (Uint1 left, Uint1 right)
		{
			return new Uint1((left.X * right.X));
		}

		public static Uint1 Multiply (Uint1 left, uint right)
		{
			return new Uint1((left.X * right));
		}

		public static void Multiply (ref Uint1 left, ref Uint1 right, out Uint1 result)
		{
			result = new Uint1((left.X * right.X));
		}

		public static void Add (ref Uint1 left, ref Uint1 right, out Uint1 result)
		{
			result = new Uint1((left.X + right.X));
		}

		public static void Sub (ref Uint1 left, ref Uint1 right, out Uint1 result)
		{
			result = new Uint1((left.X - right.X));
		}

		public static void Multiply (ref Uint1 left, uint right, out Uint1 result)
		{
			result = new Uint1((left.X * right));
		}

		public static uint Dot (Uint1 left, Uint1 right)
		{
			return (left.X * right.X);
		}

		public static uint Dot (ref Uint1 left, ref Uint1 right)
		{
			return (left.X * right.X);
		}
		public static Uint1 Normalize(Uint1 vec)
		{
			uint len = vec.Length;
			vec.X /= len;
			return vec;
		}
		public static Uint1 operator -(Uint1 left, Uint1 right)
		{
			left.X -= right.X;
			return left;
		}
		public static Uint1 operator +(Uint1 left, Uint1 right)
		{
			left.X += right.X;
			return left;
		}
		public static Uint1 operator *(Uint1 left, uint scale)
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
		public static bool operator ==(Uint1 left, Uint1 right) { return left.Equals(right); }
		public static bool operator !=(Uint1 left, Uint1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint1 other)
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

		public static Uint1 Scale (Uint1 left, uint scale)
		{
			return new Uint1((left.X * scale));
		}

	}
}
