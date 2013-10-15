using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Half4: IEquatable<Half4>
	{

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(half scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
			this.W = scale;
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(Half3 vec, half W)
		{
			this.X = vec.X;
			this.Y = vec.Y;
			this.Z = vec.Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(half X, half Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(half);
			this.W = default(half);
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(half X, half Y, half Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = default(half);
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(half X, half Y, half Z, half W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(Half1 a)
		{
			this.X = a.X;
			this.Y = default(half);
			this.Z = default(half);
			this.W = default(half);
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(Half2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(half);
			this.W = default(half);
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(Half3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = default(half);
		}

		/// <summary>
		/// Constructor of the Half4.
		/// </summary>
		public Half4(Half4 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = a.W;
		}

		/// <summary>
		/// The Xy component of the Half4.
		/// </summary>
		public Half2 Xy {
			get { return new Half2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The Xyz component of the Half4.
		/// </summary>
		public Half3 Xyz {
			get { return new Half3(this.X, this.Y, this.Z); }
			set { this.X = value.X; this.Y = value.Y; this.Z = value.Z;  }
		}

		/// <summary>
		/// The X component of the Half4.
		/// </summary>
		[FieldOffset(0)]
		public half X;

		/// <summary>
		/// The Y component of the Half4.
		/// </summary>
		[FieldOffset(2)]
		public half Y;

		/// <summary>
		/// The Z component of the Half4.
		/// </summary>
		[FieldOffset(4)]
		public half Z;

		/// <summary>
		/// The W component of the Half4.
		/// </summary>
		[FieldOffset(6)]
		public half W;

		public static readonly Half4 UnitX = new Half4(1, 0, 0, 0);

		public static readonly Half4 UnitY = new Half4(0, 1, 0, 0);

		public static readonly Half4 UnitZ = new Half4(0, 0, 1, 0);

		public static readonly Half4 UnitW = new Half4(0, 0, 0, 1);

		public static readonly Half4 Zero = new Half4(0, 0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Half4());

		public half Length { get { return (half)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); } }

		public half LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W); } }
		public void Normalize()
		{
			half len = this.Length;
			X /= len;
			Y /= len;
			Z /= len;
			W /= len;
		}

		public static Half4 Multiply (Half4 left, Half4 right)
		{
			return new Half4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static Half4 Multiply (Half4 left, half right)
		{
			return new Half4((left.X * right), (left.Y * right), (left.Z * right), (left.W * right));
		}

		public static void Multiply (ref Half4 left, ref Half4 right, out Half4 result)
		{
			result = new Half4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static void Add (ref Half4 left, ref Half4 right, out Half4 result)
		{
			result = new Half4((left.X + right.X), (left.Y + right.Y), (left.Z + right.Z), (left.W + right.W));
		}

		public static void Sub (ref Half4 left, ref Half4 right, out Half4 result)
		{
			result = new Half4((left.X - right.X), (left.Y - right.Y), (left.Z - right.Z), (left.W - right.W));
		}

		public static void Multiply (ref Half4 left, half right, out Half4 result)
		{
			result = new Half4((left.X * right), (left.Y * right), (left.Z * right), (left.W * right));
		}

		public static half Dot (Half4 left, Half4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		public static half Dot (ref Half4 left, ref Half4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}
		public static Half4 Normalize(Half4 vec)
		{
			half len = vec.Length;
			vec.X /= len;
			vec.Y /= len;
			vec.Z /= len;
			vec.W /= len;
			return vec;
		}
		public static Half4 operator -(Half4 left, Half4 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}
		public static Half4 operator +(Half4 left, Half4 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}
		public static Half4 operator *(Half4 left, half scale)
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
		public static bool operator ==(Half4 left, Half4 right) { return left.Equals(right); }
		public static bool operator !=(Half4 left, Half4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half4 other)
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

		public static Half4 Scale (Half4 left, half scale)
		{
			return new Half4((left.X * scale), (left.Y * scale), (left.Z * scale), (left.W * scale));
		}

	}
}
