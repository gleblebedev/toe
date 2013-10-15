using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint3x2: IEquatable<Uint3x2>
	{

		/// <summary>
		/// Constructor of the Uint3x2.
		/// </summary>
		public Uint3x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21 )
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M10 = m10;
			this.M11 = m11;
			this.M20 = m20;
			this.M21 = m21;
		}

		/// <summary>
		/// Row of the Uint3x2.
		/// </summary>
		public Uint2 Row0 { get { return new Uint2(this.M00, this.M01); } set {this.M00 = value.X;this.M01 = value.Y;} }

		/// <summary>
		/// Row 0, Column 0 of the Uint3x2.
		/// </summary>
		[FieldOffset(0)]
		public uint M00;

		/// <summary>
		/// Row 0, Column 1 of the Uint3x2.
		/// </summary>
		[FieldOffset(4)]
		public uint M01;

		/// <summary>
		/// Row of the Uint3x2.
		/// </summary>
		public Uint2 Row1 { get { return new Uint2(this.M10, this.M11); } set {this.M10 = value.X;this.M11 = value.Y;} }

		/// <summary>
		/// Row 1, Column 0 of the Uint3x2.
		/// </summary>
		[FieldOffset(8)]
		public uint M10;

		/// <summary>
		/// Row 1, Column 1 of the Uint3x2.
		/// </summary>
		[FieldOffset(12)]
		public uint M11;

		/// <summary>
		/// Row of the Uint3x2.
		/// </summary>
		public Uint2 Row2 { get { return new Uint2(this.M20, this.M21); } set {this.M20 = value.X;this.M21 = value.Y;} }

		/// <summary>
		/// Row 2, Column 0 of the Uint3x2.
		/// </summary>
		[FieldOffset(16)]
		public uint M20;

		/// <summary>
		/// Row 2, Column 1 of the Uint3x2.
		/// </summary>
		[FieldOffset(20)]
		public uint M21;

		public static readonly Uint3x2 Identity = new Uint3x2(1, 0, 0, 1, 0, 0);
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
				hashCode = (hashCode * 397) ^ this.M10.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M11.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M20.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M21.GetHashCode();
				return hashCode;
			}
		}
		public static bool operator ==(Uint3x2 left, Uint3x2 right) { return left.Equals(right); }
		public static bool operator !=(Uint3x2 left, Uint3x2 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint3x2 other)
		{
			return this.M00.Equals(other.M00) && this.M01.Equals(other.M01) && this.M10.Equals(other.M10) && this.M11.Equals(other.M11) && this.M20.Equals(other.M20) && this.M21.Equals(other.M21);
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

		public override string ToString() { return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", this.M00, this.M01, this.M10, this.M11, this.M20, this.M21); }
	}
	public static partial class MathHelper
	{
	}
}
