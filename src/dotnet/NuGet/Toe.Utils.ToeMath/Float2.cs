using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Float2: IEquatable<Float2>
	{

		/// <summary>
		/// Constructor of the Float2.
		/// </summary>
		public Float2(float scale)
		{
			this.X = scale;
			this.Y = scale;
		}

		/// <summary>
		/// Constructor of the Float2.
		/// </summary>
		public Float2(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Float2.
		/// </summary>
		public Float2(Float1 a)
		{
			this.X = a.X;
			this.Y = default(float);
		}

		/// <summary>
		/// Constructor of the Float2.
		/// </summary>
		public Float2(Float2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
		}

		/// <summary>
		/// The X component of the Float2.
		/// </summary>
		[FieldOffset(0)]
		public float X;

		/// <summary>
		/// The Y component of the Float2.
		/// </summary>
		[FieldOffset(4)]
		public float Y;

		public static readonly Float2 UnitX = new Float2(1, 0);

		public static readonly Float2 UnitY = new Float2(0, 1);

		public static readonly Float2 Zero = new Float2(0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Float2());

		public float Length { get { return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); } }
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
		public static bool operator ==(Float2 left, Float2 right) { return left.Equals(right); }
		public static bool operator !=(Float2 left, Float2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float2 other)
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

		public static Float2 Multiply (Float2 left, Float2 right)
		{
			return new Float2((left.X * right.X), (left.Y * right.Y));
		}

		public static void Multiply (ref Float2 left, ref Float2 right, out Float2 result)
		{
			result = new Float2((left.X * right.X), (left.Y * right.Y));
		}

		public static Float2 Scale (Float2 left, float scale)
		{
			return new Float2((left.X * scale), (left.Y * scale));
		}

		public static float Dot (Float2 left, Float2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

		public static float Dot (ref Float2 left, ref Float2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

	}
}
