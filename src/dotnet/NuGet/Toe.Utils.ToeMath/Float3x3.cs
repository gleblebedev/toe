using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Float3x3: IEquatable<Float3x3>
	{

		/// <summary>
		/// Row of the Float3x3.
		/// </summary>
		[FieldOffset(0)]
		public Float3 Row0;

		/// <summary>
		/// Row 0, Column 0 of the Float3x3.
		/// </summary>
		[FieldOffset(0)]
		public float M00;

		/// <summary>
		/// Row 0, Column 1 of the Float3x3.
		/// </summary>
		[FieldOffset(4)]
		public float M01;

		/// <summary>
		/// Row 0, Column 2 of the Float3x3.
		/// </summary>
		[FieldOffset(8)]
		public float M02;

		/// <summary>
		/// Row of the Float3x3.
		/// </summary>
		[FieldOffset(12)]
		public Float3 Row1;

		/// <summary>
		/// Row 1, Column 0 of the Float3x3.
		/// </summary>
		[FieldOffset(12)]
		public float M10;

		/// <summary>
		/// Row 1, Column 1 of the Float3x3.
		/// </summary>
		[FieldOffset(16)]
		public float M11;

		/// <summary>
		/// Row 1, Column 2 of the Float3x3.
		/// </summary>
		[FieldOffset(20)]
		public float M12;

		/// <summary>
		/// Row of the Float3x3.
		/// </summary>
		[FieldOffset(24)]
		public Float3 Row2;

		/// <summary>
		/// Row 2, Column 0 of the Float3x3.
		/// </summary>
		[FieldOffset(24)]
		public float M20;

		/// <summary>
		/// Row 2, Column 1 of the Float3x3.
		/// </summary>
		[FieldOffset(28)]
		public float M21;

		/// <summary>
		/// Row 2, Column 2 of the Float3x3.
		/// </summary>
		[FieldOffset(32)]
		public float M22;
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
				hashCode = (hashCode * 397) ^ this.M10.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M11.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M12.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M20.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M21.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M22.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Float3x3 left, Float3x3 right) { return left.Equals(right); }
		public static bool operator !=(Float3x3 left, Float3x3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float3x3 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M12.Equals(other.M12) && this.M20.Equals(other.M20) && this.M21.Equals(other.M21) && this.M22.Equals(other.M22);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", this.M00, this.M01, this.M02, this.M10, this.M11, this.M12, this.M20, this.M21, this.M22); }
	}
	public static partial class MathHelper
	{
	}
}
