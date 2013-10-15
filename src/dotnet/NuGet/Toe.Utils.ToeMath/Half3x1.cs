using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Half3x1: IEquatable<Half3x1>
	{

		/// <summary>
		/// Constructor of the Half3x1.
		/// </summary>
		public Half3x1(half m00, half m10, half m20 )
		{
			this.M00 = m00;
			this.M10 = m10;
			this.M20 = m20;
		}

		/// <summary>
		/// Row of the Half3x1.
		/// </summary>
		public Half1 Row0 { get { return new Half1(this.M00); } set {this.M00 = value.X;} }

		/// <summary>
		/// Row 0, Column 0 of the Half3x1.
		/// </summary>
		[FieldOffset(0)]
		public half M00;

		/// <summary>
		/// Row of the Half3x1.
		/// </summary>
		public Half1 Row1 { get { return new Half1(this.M10); } set {this.M10 = value.X;} }

		/// <summary>
		/// Row 1, Column 0 of the Half3x1.
		/// </summary>
		[FieldOffset(2)]
		public half M10;

		/// <summary>
		/// Row of the Half3x1.
		/// </summary>
		public Half1 Row2 { get { return new Half1(this.M20); } set {this.M20 = value.X;} }

		/// <summary>
		/// Row 2, Column 0 of the Half3x1.
		/// </summary>
		[FieldOffset(4)]
		public half M20;

		public static readonly Half3x1 Identity = new Half3x1(1, 0, 0);
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
				hashCode = (hashCode * 397) ^ this.M20.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Half3x1 left, Half3x1 right) { return left.Equals(right); }
		public static bool operator !=(Half3x1 left, Half3x1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half3x1 other)
		{
			return this.M00.Equals(other.M00) && this.M10.Equals(other.M10) && this.M20.Equals(other.M20);
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

		public override string ToString() { return string.Format("({0}, {1}, {2})", this.M00, this.M10, this.M20); }
	}
	public static partial class MathHelper
	{
	}
}
