using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Double2x1: IEquatable<Double2x1>
	{

		/// <summary>
		/// Row of the Double2x1.
		/// </summary>
		[FieldOffset(0)]
		public Double1 Row0;

		/// <summary>
		/// Row 0, Column 0 of the Double2x1.
		/// </summary>
		[FieldOffset(0)]
		public double M00;

		/// <summary>
		/// Row of the Double2x1.
		/// </summary>
		[FieldOffset(8)]
		public Double1 Row1;

		/// <summary>
		/// Row 1, Column 0 of the Double2x1.
		/// </summary>
		[FieldOffset(8)]
		public double M10;
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
				hashCode = (hashCode * 397) ^ this.M10.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Double2x1 left, Double2x1 right) { return left.Equals(right); }
		public static bool operator !=(Double2x1 left, Double2x1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Double2x1 other)
		{
			return this.M00.Equals(other.M00) && this.M10.Equals(other.M10);
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

		public override string ToString() { return string.Format("({0}, {1})", this.M00, this.M10); }
	}
	public static partial class MathHelper
	{
	}
}
