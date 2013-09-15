using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Half3: IEquatable<Half3>
	{

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(half scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
		}

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(half X, half Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(half);
		}

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(half X, half Y, half Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(Half1 a)
		{
			this.X = a.X;
			this.Y = default(half);
			this.Z = default(half);
		}

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(Half2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(half);
		}

		/// <summary>
		/// Constructor of the Half3.
		/// </summary>
		public Half3(Half3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
		}

		/// <summary>
		/// The Xy component of the Half3.
		/// </summary>
		public Half2 Xy {
			get { return new Half2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The X component of the Half3.
		/// </summary>
		[FieldOffset(0)]
		public half X;

		/// <summary>
		/// The Y component of the Half3.
		/// </summary>
		[FieldOffset(2)]
		public half Y;

		/// <summary>
		/// The Z component of the Half3.
		/// </summary>
		[FieldOffset(4)]
		public half Z;

		public static readonly Half3 UnitX = new Half3(1, 0, 0);

		public static readonly Half3 UnitY = new Half3(0, 1, 0);

		public static readonly Half3 UnitZ = new Half3(0, 0, 1);

		public static readonly Half3 Zero = new Half3(0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Half3());

		public half Length { get { return (half)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); } }
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
		public static bool operator ==(Half3 left, Half3 right) { return left.Equals(right); }
		public static bool operator !=(Half3 left, Half3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half3 other)
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

		public static Half3 Multiply (Half3 left, Half3 right)
		{
			return new Half3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static void Multiply (ref Half3 left, ref Half3 right, out Half3 result)
		{
			result = new Half3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static Half3 Scale (Half3 left, half scale)
		{
			return new Half3((left.X * scale), (left.Y * scale), (left.Z * scale));
		}

		public static half Dot (Half3 left, Half3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		public static half Dot (ref Half3 left, ref Half3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

	}
}
