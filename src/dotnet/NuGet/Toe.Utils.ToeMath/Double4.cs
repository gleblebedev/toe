using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Double4: IEquatable<Double4>
	{

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(double scale)
		{
			this.X = scale;
			this.Y = scale;
			this.Z = scale;
			this.W = scale;
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(double X, double Y)
		{
			this.X = X;
			this.Y = Y;
			this.Z = default(double);
			this.W = default(double);
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(double X, double Y, double Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = default(double);
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(double X, double Y, double Z, double W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(Double1 a)
		{
			this.X = a.X;
			this.Y = default(double);
			this.Z = default(double);
			this.W = default(double);
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(Double2 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = default(double);
			this.W = default(double);
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(Double3 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = default(double);
		}

		/// <summary>
		/// Constructor of the Double4.
		/// </summary>
		public Double4(Double4 a)
		{
			this.X = a.X;
			this.Y = a.Y;
			this.Z = a.Z;
			this.W = a.W;
		}

		/// <summary>
		/// The Xy component of the Double4.
		/// </summary>
		public Double2 Xy {
			get { return new Double2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y;  }
		}

		/// <summary>
		/// The Xyz component of the Double4.
		/// </summary>
		public Double3 Xyz {
			get { return new Double3(this.X, this.Y, this.Z); }
			set { this.X = value.X; this.Y = value.Y; this.Z = value.Z;  }
		}

		/// <summary>
		/// The X component of the Double4.
		/// </summary>
		[FieldOffset(0)]
		public double X;

		/// <summary>
		/// The Y component of the Double4.
		/// </summary>
		[FieldOffset(8)]
		public double Y;

		/// <summary>
		/// The Z component of the Double4.
		/// </summary>
		[FieldOffset(16)]
		public double Z;

		/// <summary>
		/// The W component of the Double4.
		/// </summary>
		[FieldOffset(24)]
		public double W;

		public static readonly Double4 UnitX = new Double4(1, 0, 0, 0);

		public static readonly Double4 UnitY = new Double4(0, 1, 0, 0);

		public static readonly Double4 UnitZ = new Double4(0, 0, 1, 0);

		public static readonly Double4 UnitW = new Double4(0, 0, 0, 1);

		public static readonly Double4 Zero = new Double4(0, 0, 0, 0);

		public static readonly int SizeInBytes = Marshal.SizeOf(new Double4());

		public double Length { get { return (double)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)); } }
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
		public static bool operator ==(Double4 left, Double4 right) { return left.Equals(right); }
		public static bool operator !=(Double4 left, Double4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double4 other)
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

		public static Double4 Multiply (Double4 left, Double4 right)
		{
			return new Double4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static void Multiply (ref Double4 left, ref Double4 right, out Double4 result)
		{
			result = new Double4((left.X * right.X), (left.Y * right.Y), (left.Z * right.Z), (left.W * right.W));
		}

		public static Double4 Scale (Double4 left, double scale)
		{
			return new Double4((left.X * scale), (left.Y * scale), (left.Z * scale), (left.W * scale));
		}

		public static double Dot (Double4 left, Double4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		public static double Dot (ref Double4 left, ref Double4 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

	}
}
