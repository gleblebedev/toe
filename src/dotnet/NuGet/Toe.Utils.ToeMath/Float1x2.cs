﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Float1x2: IEquatable<Float1x2>
	{

		/// <summary>
		/// Constructor of the Float1x2.
		/// </summary>
		public Float1x2(float m00, float m01 )
		{
			this.M00 = m00;
			this.M01 = m01;
		}

		/// <summary>
		/// Row of the Float1x2.
		/// </summary>
		public Float2 Row0 { get { return new Float2(this.M00, this.M01); } set {this.M00 = value.X;this.M01 = value.Y;} }

		/// <summary>
		/// Row 0, Column 0 of the Float1x2.
		/// </summary>
		[FieldOffset(0)]
		public float M00;

		/// <summary>
		/// Row 0, Column 1 of the Float1x2.
		/// </summary>
		[FieldOffset(4)]
		public float M01;

		public static readonly Float1x2 Identity = new Float1x2(1, 0);
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
				return hashCode;
			}
		}
		public static bool operator ==(Float1x2 left, Float1x2 right) { return left.Equals(right); }
		public static bool operator !=(Float1x2 left, Float1x2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Float1x2 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01);
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

		public override string ToString() { return string.Format("({0}, {1})", this.M00, this.M01); }
	}
	public static partial class MathHelper
	{
	}
}
