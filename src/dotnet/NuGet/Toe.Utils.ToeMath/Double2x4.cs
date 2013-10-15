using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Double2x4: IEquatable<Double2x4>
	{

		/// <summary>
		/// Constructor of the Double2x4.
		/// </summary>
		public Double2x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13 )
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M03 = m03;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
		}

		/// <summary>
		/// Row of the Double2x4.
		/// </summary>
		public Double4 Row0 { get { return new Double4(this.M00, this.M01, this.M02, this.M03); } set {this.M00 = value.X;this.M01 = value.Y;this.M02 = value.Z;this.M03 = value.W;} }

		/// <summary>
		/// Row 0, Column 0 of the Double2x4.
		/// </summary>
		[FieldOffset(0)]
		public double M00;

		/// <summary>
		/// Row 0, Column 1 of the Double2x4.
		/// </summary>
		[FieldOffset(8)]
		public double M01;

		/// <summary>
		/// Row 0, Column 2 of the Double2x4.
		/// </summary>
		[FieldOffset(16)]
		public double M02;

		/// <summary>
		/// Row 0, Column 3 of the Double2x4.
		/// </summary>
		[FieldOffset(24)]
		public double M03;

		/// <summary>
		/// Row of the Double2x4.
		/// </summary>
		public Double4 Row1 { get { return new Double4(this.M10, this.M11, this.M12, this.M13); } set {this.M10 = value.X;this.M11 = value.Y;this.M12 = value.Z;this.M13 = value.W;} }

		/// <summary>
		/// Row 1, Column 0 of the Double2x4.
		/// </summary>
		[FieldOffset(32)]
		public double M10;

		/// <summary>
		/// Row 1, Column 1 of the Double2x4.
		/// </summary>
		[FieldOffset(40)]
		public double M11;

		/// <summary>
		/// Row 1, Column 2 of the Double2x4.
		/// </summary>
		[FieldOffset(48)]
		public double M12;

		/// <summary>
		/// Row 1, Column 3 of the Double2x4.
		/// </summary>
		[FieldOffset(56)]
		public double M13;

		public static readonly Double2x4 Identity = new Double2x4(1, 0, 0, 0, 0, 1, 0, 0);
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
				int hashCode = this.M00.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M01.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M02.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M03.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M10.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M11.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M12.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M13.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Double2x4 left, Double2x4 right) { return left.Equals(right); }
		public static bool operator !=(Double2x4 left, Double2x4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double2x4 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M03.Equals(other.M03) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M12.Equals(other.M12) && this.M13.Equals(other.M13);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", this.M00, this.M01, this.M02, this.M03, this.M10, this.M11, this.M12, this.M13); }
	}
	public static partial class MathHelper
	{
	}
}
