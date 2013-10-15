using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint2: IEquatable<Uint2>
	{

		/// <summary>
		/// Constructor of the Uint2.
		/// </summary>
		public Uint2(uint scale)
		{
			this.X = scale;
			this.Y = scale;
		}

		/// <summary>
		/// Constructor of the Uint2.
		/// </summary>
		public Uint2(Uint1 vec, uint Y)
		{
			this.X = vec.X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Uint2.
		/// </summary>
		public Uint2(uint X, uint Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Uint2.
		/// </summary>
		public Uint2(Uint1 a)
		{
			this.X = a.X;
			this.Y = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint2.
		/// </summary>
		public Uint2(Uint2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
		}

		/// <summary>
		/// The X component of the Uint2.
		/// </summary>
		[FieldOffset(0)]
		public uint X;

		/// <summary>
		/// The Y component of the Uint2.
		/// </summary>
		[FieldOffset(4)]
		public uint Y;

		public static readonly Uint2 UnitX = new Uint2(1, 0);

		public static readonly Uint2 UnitY = new Uint2(0, 1);

		public static readonly Uint2 Zero = new Uint2(0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Uint2());

		public uint Length { get { return (uint)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); } }

		public uint LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y); } }
		public void Normalize()
		{
			uint len = this.Length;
			X /= len;
			Y /= len;
		}

		public static Uint2 Multiply (Uint2 left, Uint2 right)
		{
			return new Uint2((left.X * right.X), (left.Y * right.Y));
		}

		public static Uint2 Multiply (Uint2 left, uint right)
		{
			return new Uint2((left.X * right), (left.Y * right));
		}

		public static void Multiply (ref Uint2 left, ref Uint2 right, out Uint2 result)
		{
			result = new Uint2((left.X * right.X), (left.Y * right.Y));
		}

		public static void Add (ref Uint2 left, ref Uint2 right, out Uint2 result)
		{
			result = new Uint2((left.X + right.X), (left.Y + right.Y));
		}

		public static void Sub (ref Uint2 left, ref Uint2 right, out Uint2 result)
		{
			result = new Uint2((left.X - right.X), (left.Y - right.Y));
		}

		public static void Multiply (ref Uint2 left, uint right, out Uint2 result)
		{
			result = new Uint2((left.X * right), (left.Y * right));
		}

		public static uint Dot (Uint2 left, Uint2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

		public static uint Dot (ref Uint2 left, ref Uint2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}
		public static Uint2 Normalize(Uint2 vec)
		{
			uint len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			return vec;
		}
		public static Uint2 operator -(Uint2 left, Uint2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		public static Uint2 operator +(Uint2 left, Uint2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		public static Uint2 operator *(Uint2 left, uint scale)
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
		public static bool operator ==(Uint2 left, Uint2 right) { return left.Equals(right); }
		public static bool operator !=(Uint2 left, Uint2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint2 other)
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

		public static Uint2 Scale (Uint2 left, uint scale)
		{
			return new Uint2((left.X * scale), (left.Y * scale));
		}

	}
}
