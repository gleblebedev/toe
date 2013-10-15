using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Float3: IEquatable<Float3>
	{

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(float scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(Float2 vec, float Z)
		{
			this.X = vec.X;
			this.Y = vec.Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(float);
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(Float1 a)
		{
			this.X = a.X;
			this.Y = default(float);
			this.Z = default(float);
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(Float2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(float);
		}

		/// <summary>
		/// Constructor of the Float3.
		/// </summary>
		public Float3(Float3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
		}

		/// <summary>
		/// The Xy component of the Float3.
		/// </summary>
		public Float2 Xy {
			get { return new Float2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The X component of the Float3.
		/// </summary>
		[FieldOffset(0)]
		public float X;

		/// <summary>
		/// The Y component of the Float3.
		/// </summary>
		[FieldOffset(4)]
		public float Y;

		/// <summary>
		/// The Z component of the Float3.
		/// </summary>
		[FieldOffset(8)]
		public float Z;

		public static readonly Float3 UnitX = new Float3(1, 0, 0);

		public static readonly Float3 UnitY = new Float3(0, 1, 0);

		public static readonly Float3 UnitZ = new Float3(0, 0, 1);

		public static readonly Float3 Zero = new Float3(0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Float3());

		public float Length { get { return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); } }

		public float LengthSquared { get { return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z); } }
		public void Normalize()
		{
			float scale = 1.0f/this.Length;
			X *= scale;
			Y *= scale;
			Z *= scale;
		}

		public static Float3 Multiply (Float3 left, Float3 right)
		{
			return new Float3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static Float3 Multiply (Float3 left, float right)
		{
			return new Float3((left.X * right), (left.Y * right), (left.Z * right));
		}

		public static void Multiply (ref Float3 left, ref Float3 right, out Float3 result)
		{
			result = new Float3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static void Add (ref Float3 left, ref Float3 right, out Float3 result)
		{
			result = new Float3((left.X + right.X), (left.Y + right.Y), (left.Z + right.Z));
		}

		public static void Sub (ref Float3 left, ref Float3 right, out Float3 result)
		{
			result = new Float3((left.X - right.X), (left.Y - right.Y), (left.Z - right.Z));
		}

		public static void Multiply (ref Float3 left, float right, out Float3 result)
		{
			result = new Float3((left.X * right), (left.Y * right), (left.Z * right));
		}

		public static float Dot (Float3 left, Float3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		public static float Dot (ref Float3 left, ref Float3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}
		public static Float3 Normalize(Float3 vec)
		{
			float scale = 1.0f/vec.Length;
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}
		public static Float3 operator -(Float3 left, Float3 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}
		public static Float3 operator +(Float3 left, Float3 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}
		public static Float3 operator *(Float3 left, float scale)
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
		public static bool operator ==(Float3 left, Float3 right) { return left.Equals(right); }
		public static bool operator !=(Float3 left, Float3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float3 other)
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

		public static Float3 Scale (Float3 left, float scale)
		{
			return new Float3((left.X * scale), (left.Y * scale), (left.Z * scale));
		}

	}
}
