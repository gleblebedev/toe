﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int1x3: IEquatable<Int1x3>
	{

		/// <summary>
		/// Constructor of the Int1x3.
		/// </summary>
		public Int1x3(int m00, int m01, int m02 )
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
		}

		/// <summary>
		/// Row of the Int1x3.
		/// </summary>
		public Int3 Row0 { get { return new Int3(this.M00, this.M01, this.M02); } set {this.M00 = value.X;this.M01 = value.Y;this.M02 = value.Z;} }

		/// <summary>
		/// Row 0, Column 0 of the Int1x3.
		/// </summary>
		[FieldOffset(0)]
		public int M00;

		/// <summary>
		/// Row 0, Column 1 of the Int1x3.
		/// </summary>
		[FieldOffset(4)]
		public int M01;

		/// <summary>
		/// Row 0, Column 2 of the Int1x3.
		/// </summary>
		[FieldOffset(8)]
		public int M02;

		public static readonly Int1x3 Identity = new Int1x3(1, 0, 0);
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
				return hashCode;
			}
		}
		public static bool operator ==(Int1x3 left, Int1x3 right) { return left.Equals(right); }
		public static bool operator !=(Int1x3 left, Int1x3 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int1x3 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02);
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

		public override string ToString() { return string.Format("({0}, {1}, {2})", this.M00, this.M01, this.M02); }
	}
	public static partial class MathHelper
	{
	}
}
