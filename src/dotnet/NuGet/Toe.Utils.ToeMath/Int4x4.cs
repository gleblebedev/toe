﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Int4x4: IEquatable<Int4x4>
	{

		/// <summary>
		/// Constructor of the Int4x4.
		/// </summary>
		public Int4x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23, int m30, int m31, int m32, int m33 )
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M03 = m03;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M20 = m20;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M30 = m30;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
		}

		/// <summary>
		/// Row of the Int4x4.
		/// </summary>
		public Int4 Row0 { get { return new Int4(this.M00, this.M01, this.M02, this.M03); } set {this.M00 = value.X;this.M01 = value.Y;this.M02 = value.Z;this.M03 = value.W;} }

		/// <summary>
		/// Row 0, Column 0 of the Int4x4.
		/// </summary>
		[FieldOffset(0)]
		public int M00;

		/// <summary>
		/// Row 0, Column 1 of the Int4x4.
		/// </summary>
		[FieldOffset(4)]
		public int M01;

		/// <summary>
		/// Row 0, Column 2 of the Int4x4.
		/// </summary>
		[FieldOffset(8)]
		public int M02;

		/// <summary>
		/// Row 0, Column 3 of the Int4x4.
		/// </summary>
		[FieldOffset(12)]
		public int M03;

		/// <summary>
		/// Row of the Int4x4.
		/// </summary>
		public Int4 Row1 { get { return new Int4(this.M10, this.M11, this.M12, this.M13); } set {this.M10 = value.X;this.M11 = value.Y;this.M12 = value.Z;this.M13 = value.W;} }

		/// <summary>
		/// Row 1, Column 0 of the Int4x4.
		/// </summary>
		[FieldOffset(16)]
		public int M10;

		/// <summary>
		/// Row 1, Column 1 of the Int4x4.
		/// </summary>
		[FieldOffset(20)]
		public int M11;

		/// <summary>
		/// Row 1, Column 2 of the Int4x4.
		/// </summary>
		[FieldOffset(24)]
		public int M12;

		/// <summary>
		/// Row 1, Column 3 of the Int4x4.
		/// </summary>
		[FieldOffset(28)]
		public int M13;

		/// <summary>
		/// Row of the Int4x4.
		/// </summary>
		public Int4 Row2 { get { return new Int4(this.M20, this.M21, this.M22, this.M23); } set {this.M20 = value.X;this.M21 = value.Y;this.M22 = value.Z;this.M23 = value.W;} }

		/// <summary>
		/// Row 2, Column 0 of the Int4x4.
		/// </summary>
		[FieldOffset(32)]
		public int M20;

		/// <summary>
		/// Row 2, Column 1 of the Int4x4.
		/// </summary>
		[FieldOffset(36)]
		public int M21;

		/// <summary>
		/// Row 2, Column 2 of the Int4x4.
		/// </summary>
		[FieldOffset(40)]
		public int M22;

		/// <summary>
		/// Row 2, Column 3 of the Int4x4.
		/// </summary>
		[FieldOffset(44)]
		public int M23;

		/// <summary>
		/// Row of the Int4x4.
		/// </summary>
		public Int4 Row3 { get { return new Int4(this.M30, this.M31, this.M32, this.M33); } set {this.M30 = value.X;this.M31 = value.Y;this.M32 = value.Z;this.M33 = value.W;} }

		/// <summary>
		/// Row 3, Column 0 of the Int4x4.
		/// </summary>
		[FieldOffset(48)]
		public int M30;

		/// <summary>
		/// Row 3, Column 1 of the Int4x4.
		/// </summary>
		[FieldOffset(52)]
		public int M31;

		/// <summary>
		/// Row 3, Column 2 of the Int4x4.
		/// </summary>
		[FieldOffset(56)]
		public int M32;

		/// <summary>
		/// Row 3, Column 3 of the Int4x4.
		/// </summary>
		[FieldOffset(60)]
		public int M33;

		public static readonly Int4x4 Identity = new Int4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
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
				hashCode = (hashCode * 397) ^ this.M20.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M21.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M22.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M23.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M30.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M31.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M32.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M33.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Int4x4 left, Int4x4 right) { return left.Equals(right); }
		public static bool operator !=(Int4x4 left, Int4x4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Int4x4 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M03.Equals(other.M03) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M12.Equals(other.M12) && this.M13.Equals(other.M13) && this.M20.Equals(other.M20) && this.M21.Equals(other.M21) && this.M22.Equals(other.M22) && this.M23.Equals(other.M23) && this.M30.Equals(other.M30) && this.M31.Equals(other.M31) && this.M32.Equals(other.M32) && this.M33.Equals(other.M33);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15})", this.M00, this.M01, this.M02, this.M03, this.M10, this.M11, this.M12, this.M13, this.M20, this.M21, this.M22, this.M23, this.M30, this.M31, this.M32, this.M33); }
	}
	public static partial class MathHelper
	{
	}
}
