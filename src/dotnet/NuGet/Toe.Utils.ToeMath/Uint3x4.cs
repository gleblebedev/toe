﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint3x4: IEquatable<Uint3x4>
	{

		/// <summary>
		/// Constructor of the Uint3x4.
		/// </summary>
		public Uint3x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23 )
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
		}

		/// <summary>
		/// Row of the Uint3x4.
		/// </summary>
		public Uint4 Row0 { get { return new Uint4(this.M00, this.M01, this.M02, this.M03); } set {this.M00 = value.X;this.M01 = value.Y;this.M02 = value.Z;this.M03 = value.W;} }

		/// <summary>
		/// Row 0, Column 0 of the Uint3x4.
		/// </summary>
		[FieldOffset(0)]
		public uint M00;

		/// <summary>
		/// Row 0, Column 1 of the Uint3x4.
		/// </summary>
		[FieldOffset(4)]
		public uint M01;

		/// <summary>
		/// Row 0, Column 2 of the Uint3x4.
		/// </summary>
		[FieldOffset(8)]
		public uint M02;

		/// <summary>
		/// Row 0, Column 3 of the Uint3x4.
		/// </summary>
		[FieldOffset(12)]
		public uint M03;

		/// <summary>
		/// Row of the Uint3x4.
		/// </summary>
		public Uint4 Row1 { get { return new Uint4(this.M10, this.M11, this.M12, this.M13); } set {this.M10 = value.X;this.M11 = value.Y;this.M12 = value.Z;this.M13 = value.W;} }

		/// <summary>
		/// Row 1, Column 0 of the Uint3x4.
		/// </summary>
		[FieldOffset(16)]
		public uint M10;

		/// <summary>
		/// Row 1, Column 1 of the Uint3x4.
		/// </summary>
		[FieldOffset(20)]
		public uint M11;

		/// <summary>
		/// Row 1, Column 2 of the Uint3x4.
		/// </summary>
		[FieldOffset(24)]
		public uint M12;

		/// <summary>
		/// Row 1, Column 3 of the Uint3x4.
		/// </summary>
		[FieldOffset(28)]
		public uint M13;

		/// <summary>
		/// Row of the Uint3x4.
		/// </summary>
		public Uint4 Row2 { get { return new Uint4(this.M20, this.M21, this.M22, this.M23); } set {this.M20 = value.X;this.M21 = value.Y;this.M22 = value.Z;this.M23 = value.W;} }

		/// <summary>
		/// Row 2, Column 0 of the Uint3x4.
		/// </summary>
		[FieldOffset(32)]
		public uint M20;

		/// <summary>
		/// Row 2, Column 1 of the Uint3x4.
		/// </summary>
		[FieldOffset(36)]
		public uint M21;

		/// <summary>
		/// Row 2, Column 2 of the Uint3x4.
		/// </summary>
		[FieldOffset(40)]
		public uint M22;

		/// <summary>
		/// Row 2, Column 3 of the Uint3x4.
		/// </summary>
		[FieldOffset(44)]
		public uint M23;

		public static readonly Uint3x4 Identity = new Uint3x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0);
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
				return hashCode;
			}
		}
		public static bool operator ==(Uint3x4 left, Uint3x4 right) { return left.Equals(right); }
		public static bool operator !=(Uint3x4 left, Uint3x4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint3x4 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M03.Equals(other.M03) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M12.Equals(other.M12) && this.M13.Equals(other.M13) && this.M20.Equals(other.M20) && this.M21.Equals(other.M21) && this.M22.Equals(other.M22) && this.M23.Equals(other.M23);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})", this.M00, this.M01, this.M02, this.M03, this.M10, this.M11, this.M12, this.M13, this.M20, this.M21, this.M22, this.M23); }
	}
	public static partial class MathHelper
	{
	}
}
