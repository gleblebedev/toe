using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Uint4: IEquatable<Uint4>
	{

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(uint scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
			this.W = scale;
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(uint X, uint Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(uint);
			this.W = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(uint X, uint Y, uint Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(uint X, uint Y, uint Z, uint W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(Uint1 a)
		{
			this.X = a.X;
			this.Y = default(uint);
			this.Z = default(uint);
			this.W = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(Uint2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(uint);
			this.W = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(Uint3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint4.
		/// </summary>
		public Uint4(Uint4 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = a.W;
		}

		/// <summary>
		/// The Xy component of the Uint4.
		/// </summary>
		public Uint2 Xy {
			get { return new Uint2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The Xyz component of the Uint4.
		/// </summary>
		public Uint3 Xyz {
			get { return new Uint3(this.X, this.Y, this.Z); }
			set { this.X = value.X; this.Y = value.Y; this.Z = value.Z;  }
		}

		/// <summary>
		/// The X component of the Uint4.
		/// </summary>
		[FieldOffset(0)]
		public uint X;

		/// <summary>
		/// The Y component of the Uint4.
		/// </summary>
		[FieldOffset(4)]
		public uint Y;

		/// <summary>
		/// The Z component of the Uint4.
		/// </summary>
		[FieldOffset(8)]
		public uint Z;

		/// <summary>
		/// The W component of the Uint4.
		/// </summary>
		[FieldOffset(12)]
		public uint W;

		public static readonly Uint4 UnitX = new Uint4(1, 0, 0, 0);

		public static readonly Uint4 UnitY = new Uint4(0, 1, 0, 0);

		public static readonly Uint4 UnitZ = new Uint4(0, 0, 1, 0);

		public static readonly Uint4 UnitW = new Uint4(0, 0, 0, 1);

		public static readonly Uint4 Zero = new Uint4(0, 0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Uint4());

		public uint Length { get { return (uint)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); } }
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
		public static bool operator ==(Uint4 left, Uint4 right) { return left.Equals(right); }
		public static bool operator !=(Uint4 left, Uint4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint4 other)
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

		public static Uint4 Multiply (Uint4 left, Uint4 right)
		{
			return new Uint4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static void Multiply (ref Uint4 left, ref Uint4 right, out Uint4 result)
		{
			result = new Uint4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static Uint4 Scale (Uint4 left, uint scale)
		{
			return new Uint4((left.X * scale), (left.Y * scale), (left.Z * scale), (left.W * scale));
		}

		public static uint Dot (Uint4 left, Uint4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		public static uint Dot (ref Uint4 left, ref Uint4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

	}
}
