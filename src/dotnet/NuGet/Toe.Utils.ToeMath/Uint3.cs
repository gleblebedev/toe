using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Uint3: IEquatable<Uint3>
	{

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(uint scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
		}

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(uint X, uint Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(uint X, uint Y, uint Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(Uint1 a)
		{
			this.X = a.X;
			this.Y = default(uint);
			this.Z = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(Uint2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(uint);
		}

		/// <summary>
		/// Constructor of the Uint3.
		/// </summary>
		public Uint3(Uint3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
		}

		/// <summary>
		/// The Xy component of the Uint3.
		/// </summary>
		public Uint2 Xy {
			get { return new Uint2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The X component of the Uint3.
		/// </summary>
		[FieldOffset(0)]
		public uint X;

		/// <summary>
		/// The Y component of the Uint3.
		/// </summary>
		[FieldOffset(4)]
		public uint Y;

		/// <summary>
		/// The Z component of the Uint3.
		/// </summary>
		[FieldOffset(8)]
		public uint Z;

		public static readonly Uint3 UnitX = new Uint3(1, 0, 0);

		public static readonly Uint3 UnitY = new Uint3(0, 1, 0);

		public static readonly Uint3 UnitZ = new Uint3(0, 0, 1);

		public static readonly Uint3 Zero = new Uint3(0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Uint3());

		public uint Length { get { return (uint)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); } }
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
		public static bool operator ==(Uint3 left, Uint3 right) { return left.Equals(right); }
		public static bool operator !=(Uint3 left, Uint3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint3 other)
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

		public static Uint3 Multiply (Uint3 left, Uint3 right)
		{
			return new Uint3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static void Multiply (ref Uint3 left, ref Uint3 right, out Uint3 result)
		{
			result = new Uint3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static Uint3 Scale (Uint3 left, uint scale)
		{
			return new Uint3((left.X * scale), (left.Y * scale), (left.Z * scale));
		}

		public static uint Dot (Uint3 left, Uint3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		public static uint Dot (ref Uint3 left, ref Uint3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

	}
}
