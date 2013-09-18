﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public struct Int2x3: IEquatable<Int2x3>
	{

		/// <summary>
		/// Row of the Int2x3.
		/// </summary>
		[FieldOffset(0)]
		public Int3 Row0;

		/// <summary>
		/// Row 0, Column 0 of the Int2x3.
		/// </summary>
		[FieldOffset(0)]
		public int M00;

		/// <summary>
		/// Row 0, Column 1 of the Int2x3.
		/// </summary>
		[FieldOffset(4)]
		public int M01;

		/// <summary>
		/// Row 0, Column 2 of the Int2x3.
		/// </summary>
		[FieldOffset(8)]
		public int M02;

		/// <summary>
		/// Row of the Int2x3.
		/// </summary>
		[FieldOffset(12)]
		public Int3 Row1;

		/// <summary>
		/// Row 1, Column 0 of the Int2x3.
		/// </summary>
		[FieldOffset(12)]
		public int M10;

		/// <summary>
		/// Row 1, Column 1 of the Int2x3.
		/// </summary>
		[FieldOffset(16)]
		public int M11;

		/// <summary>
		/// Row 1, Column 2 of the Int2x3.
		/// </summary>
		[FieldOffset(20)]
		public int M12;
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
				return hashCode;
			}
		}
		public static bool operator ==(Int2x3 left, Int2x3 right) { return left.Equals(right); }
		public static bool operator !=(Int2x3 left, Int2x3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int2x3 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M12.Equals(other.M12);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", this.M00, this.M01, this.M02, this.M10, this.M11, this.M12); }
	}
	public static partial class MathHelper
	{
	}
}