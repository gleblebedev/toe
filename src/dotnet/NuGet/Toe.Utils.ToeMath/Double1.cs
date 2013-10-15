using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Double1: IEquatable<Double1>
	{

		/// <summary>
		/// Constructor of the Double1.
		/// </summary>
		public Double1(double scale)
		{
			this.X = scale;
		}

		/// <summary>
		/// Constructor of the Double1.
		/// </summary>
		public Double1(Double1 a)
		{
			this.X = a.X;
		}

		/// <summary>
		/// The X component of the Double1.
		/// </summary>
		[FieldOffset(0)]
		public double X;

		public static readonly Double1 UnitX = new Double1(1);

		public static readonly Double1 Zero = new Double1(0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Double1());

		public double Length { get { return (double)Math.Sqrt((this.X * this.X)); } }

		public double LengthSquared { get { return (this.X * this.X); } }
		public void Normalize()
		{
			double len = this.Length;
			X /= len;
		}

		public static Double1 Multiply (Double1 left, Double1 right)
		{
			return new Double1((left.X * right.X));
		}

		public static Double1 Multiply (Double1 left, double right)
		{
			return new Double1((left.X * right));
		}

		public static void Multiply (ref Double1 left, ref Double1 right, out Double1 result)
		{
			result = new Double1((left.X * right.X));
		}

		public static void Add (ref Double1 left, ref Double1 right, out Double1 result)
		{
			result = new Double1((left.X + right.X));
		}

		public static void Sub (ref Double1 left, ref Double1 right, out Double1 result)
		{
			result = new Double1((left.X - right.X));
		}

		public static void Multiply (ref Double1 left, double right, out Double1 result)
		{
			result = new Double1((left.X * right));
		}

		public static double Dot (Double1 left, Double1 right)
		{
			return (left.X * right.X);
		}

		public static double Dot (ref Double1 left, ref Double1 right)
		{
			return (left.X * right.X);
		}
		public static Double1 Normalize(Double1 vec)
		{
			double len = vec.Length;
			vec.X /= len;
			return vec;
		}
		public static Double1 operator -(Double1 left, Double1 right)
		{
			left.X -= right.X;
			return left;
		}
		public static Double1 operator +(Double1 left, Double1 right)
		{
			left.X += right.X;
			return left;
		}
		public static Double1 operator *(Double1 left, double scale)
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
		public static bool operator ==(Double1 left, Double1 right) { return left.Equals(right); }
		public static bool operator !=(Double1 left, Double1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double1 other)
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

		public static Double1 Scale (Double1 left, double scale)
		{
			return new Double1((left.X * scale));
		}

	}
}
