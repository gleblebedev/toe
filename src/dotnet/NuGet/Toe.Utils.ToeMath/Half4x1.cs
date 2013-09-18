﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Half4x1: IEquatable<Half4x1>
	{

		/// <summary>
		/// Row of the Half4x1.
		/// </summary>
		[FieldOffset(0)]
		public Half1 Row0;

		/// <summary>
		/// Row 0, Column 0 of the Half4x1.
		/// </summary>
		[FieldOffset(0)]
		public half M00;

		/// <summary>
		/// Row of the Half4x1.
		/// </summary>
		[FieldOffset(2)]
		public Half1 Row1;

		/// <summary>
		/// Row 1, Column 0 of the Half4x1.
		/// </summary>
		[FieldOffset(2)]
		public half M10;

		/// <summary>
		/// Row of the Half4x1.
		/// </summary>
		[FieldOffset(4)]
		public Half1 Row2;

		/// <summary>
		/// Row 2, Column 0 of the Half4x1.
		/// </summary>
		[FieldOffset(4)]
		public half M20;

		/// <summary>
		/// Row of the Half4x1.
		/// </summary>
		[FieldOffset(6)]
		public Half1 Row3;

		/// <summary>
		/// Row 3, Column 0 of the Half4x1.
		/// </summary>
		[FieldOffset(6)]
		public half M30;
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
				hashCode = (hashCode * 397) ^ this.M30.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Half4x1 left, Half4x1 right) { return left.Equals(right); }
		public static bool operator !=(Half4x1 left, Half4x1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Half4x1 other)
		{
			return this.M00.Equals(other.M00) && this.M10.Equals(other.M10) && this.M20.Equals(other.M20) && this.M30.Equals(other.M30);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3})", this.M00, this.M10, this.M20, this.M30); }
	}
	public static partial class MathHelper
	{
	}
}