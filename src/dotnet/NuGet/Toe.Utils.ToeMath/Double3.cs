using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Double3: IEquatable<Double3>
	{

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(double scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
		}

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(double X, double Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(double);
		}

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(double X, double Y, double Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(Double1 a)
		{
			this.X = a.X;
			this.Y = default(double);
			this.Z = default(double);
		}

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(Double2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(double);
		}

		/// <summary>
		/// Constructor of the Double3.
		/// </summary>
		public Double3(Double3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
		}

		/// <summary>
		/// The Xy component of the Double3.
		/// </summary>
		public Double2 Xy {
			get { return new Double2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The X component of the Double3.
		/// </summary>
		[FieldOffset(0)]
		public double X;

		/// <summary>
		/// The Y component of the Double3.
		/// </summary>
		[FieldOffset(8)]
		public double Y;

		/// <summary>
		/// The Z component of the Double3.
		/// </summary>
		[FieldOffset(16)]
		public double Z;

		public static readonly Double3 UnitX = new Double3(1, 0, 0);

		public static readonly Double3 UnitY = new Double3(0, 1, 0);

		public static readonly Double3 UnitZ = new Double3(0, 0, 1);

		public static readonly Double3 Zero = new Double3(0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Double3());

		public double Length { get { return (double)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); } }
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
		public static bool operator ==(Double3 left, Double3 right) { return left.Equals(right); }
		public static bool operator !=(Double3 left, Double3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double3 other)
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

		public static Double3 Multiply (Double3 left, Double3 right)
		{
			return new Double3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static void Multiply (ref Double3 left, ref Double3 right, out Double3 result)
		{
			result = new Double3((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z));
		}

		public static Double3 Scale (Double3 left, double scale)
		{
			return new Double3((left.X * scale), (left.Y * scale), (left.Z * scale));
		}

		public static double Dot (Double3 left, Double3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		public static double Dot (ref Double3 left, ref Double3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

	}
}
