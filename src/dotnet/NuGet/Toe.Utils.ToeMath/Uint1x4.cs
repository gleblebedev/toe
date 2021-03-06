﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint1x4: IEquatable<Uint1x4>
	{

		/// <summary>
		/// Constructor of the Uint1x4.
		/// </summary>
		public Uint1x4(uint m00, uint m01, uint m02, uint m03 )
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M03 = m03;
		}

		/// <summary>
		/// Row of the Uint1x4.
		/// </summary>
		public Uint4 Row0 { get { return new Uint4(this.M00, this.M01, this.M02, this.M03); } set {this.M00 = value.X;this.M01 = value.Y;this.M02 = value.Z;this.M03 = value.W;} }

		/// <summary>
		/// Row 0, Column 0 of the Uint1x4.
		/// </summary>
		[FieldOffset(0)]
		public uint M00;

		/// <summary>
		/// Row 0, Column 1 of the Uint1x4.
		/// </summary>
		[FieldOffset(4)]
		public uint M01;

		/// <summary>
		/// Row 0, Column 2 of the Uint1x4.
		/// </summary>
		[FieldOffset(8)]
		public uint M02;

		/// <summary>
		/// Row 0, Column 3 of the Uint1x4.
		/// </summary>
		[FieldOffset(12)]
		public uint M03;

		public static readonly Uint1x4 Identity = new Uint1x4(1, 0, 0, 0);
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
				return hashCode;
			}
		}
		public static bool operator ==(Uint1x4 left, Uint1x4 right) { return left.Equals(right); }
		public static bool operator !=(Uint1x4 left, Uint1x4 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint1x4 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M02.Equals(other.M02) && this.M03.Equals(other.M03);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3})", this.M00, this.M01, this.M02, this.M03); }
	}
	public static partial class MathHelper
	{
	}
}
