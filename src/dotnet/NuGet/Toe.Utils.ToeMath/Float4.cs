using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Float4: IEquatable<Float4>
	{

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(float scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
			this.W = scale;
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(float);
			this.W = default(float);
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = default(float);
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(float X, float Y, float Z, float W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(Float1 a)
		{
			this.X = a.X;
			this.Y = default(float);
			this.Z = default(float);
			this.W = default(float);
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(Float2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(float);
			this.W = default(float);
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(Float3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = default(float);
		}

		/// <summary>
		/// Constructor of the Float4.
		/// </summary>
		public Float4(Float4 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = a.W;
		}

		/// <summary>
		/// The Xy component of the Float4.
		/// </summary>
		public Float2 Xy {
			get { return new Float2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The Xyz component of the Float4.
		/// </summary>
		public Float3 Xyz {
			get { return new Float3(this.X, this.Y, this.Z); }
			set { this.X = value.X; this.Y = value.Y; this.Z = value.Z;  }
		}

		/// <summary>
		/// The X component of the Float4.
		/// </summary>
		[FieldOffset(0)]
		public float X;

		/// <summary>
		/// The Y component of the Float4.
		/// </summary>
		[FieldOffset(4)]
		public float Y;

		/// <summary>
		/// The Z component of the Float4.
		/// </summary>
		[FieldOffset(8)]
		public float Z;

		/// <summary>
		/// The W component of the Float4.
		/// </summary>
		[FieldOffset(12)]
		public float W;

		public static readonly Float4 UnitX = new Float4(1, 0, 0, 0);

		public static readonly Float4 UnitY = new Float4(0, 1, 0, 0);

		public static readonly Float4 UnitZ = new Float4(0, 0, 1, 0);

		public static readonly Float4 UnitW = new Float4(0, 0, 0, 1);

		public static readonly Float4 Zero = new Float4(0, 0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Float4());

		public float Length { get { return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); } }
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
		public static bool operator ==(Float4 left, Float4 right) { return left.Equals(right); }
		public static bool operator !=(Float4 left, Float4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float4 other)
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

		public static Float4 Multiply (Float4 left, Float4 right)
		{
			return new Float4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static void Multiply (ref Float4 left, ref Float4 right, out Float4 result)
		{
			result = new Float4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static Float4 Scale (Float4 left, float scale)
		{
			return new Float4((left.X * scale), (left.Y * scale), (left.Z * scale), (left.W * scale));
		}

		public static float Dot (Float4 left, Float4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		public static float Dot (ref Float4 left, ref Float4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

	}
}
