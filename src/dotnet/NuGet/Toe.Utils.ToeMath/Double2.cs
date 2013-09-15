using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Double2: IEquatable<Double2>
	{

		/// <summary>
		/// Constructor of the Double2.
		/// </summary>
		public Double2(double scale)
		{
			this.X = scale;
			this.Y = scale;
		}

		/// <summary>
		/// Constructor of the Double2.
		/// </summary>
		public Double2(double X, double Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// Constructor of the Double2.
		/// </summary>
		public Double2(Double1 a)
		{
			this.X = a.X;
			this.Y = default(double);
		}

		/// <summary>
		/// Constructor of the Double2.
		/// </summary>
		public Double2(Double2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
		}

		/// <summary>
		/// The X component of the Double2.
		/// </summary>
		[FieldOffset(0)]
		public double X;

		/// <summary>
		/// The Y component of the Double2.
		/// </summary>
		[FieldOffset(8)]
		public double Y;

		public static readonly Double2 UnitX = new Double2(1, 0);

		public static readonly Double2 UnitY = new Double2(0, 1);

		public static readonly Double2 Zero = new Double2(0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Double2());

		public double Length { get { return (double)Math.Sqrt((this.X * this.X) + (this.Y * this.Y)); } }
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
		public static bool operator ==(Double2 left, Double2 right) { return left.Equals(right); }
		public static bool operator !=(Double2 left, Double2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double2 other)
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

		public static Double2 Multiply (Double2 left, Double2 right)
		{
			return new Double2((left.X * right.X), (left.Y * right.Y));
		}

		public static void Multiply (ref Double2 left, ref Double2 right, out Double2 result)
		{
			result = new Double2((left.X * right.X), (left.Y * right.Y));
		}

		public static Double2 Scale (Double2 left, double scale)
		{
			return new Double2((left.X * scale), (left.Y * scale));
		}

		public static double Dot (Double2 left, Double2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

		public static double Dot (ref Double2 left, ref Double2 right)
		{
			return (left.X * right.X) + (left.Y * right.Y);
		}

	}
}
