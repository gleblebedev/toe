﻿using System;
using System.Runtime.InteropServices;

namespace Toe.Utils.ToeMath
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Uint1x1: IEquatable<Uint1x1>
	{

		/// <summary>
		/// Constructor of the Uint1x1.
		/// </summary>
		public Uint1x1(uint m00 )
		{
			this.M00 = m00;
		}

		/// <summary>
		/// Row of the Uint1x1.
		/// </summary>
		public Uint1 Row0 { get { return new Uint1(this.M00); } set {this.M00 = value.X;} }

		/// <summary>
		/// Row 0, Column 0 of the Uint1x1.
		/// </summary>
		[FieldOffset(0)]
		public uint M00;

		public static readonly Uint1x1 Identity = new Uint1x1(1);
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
				return hashCode;
			}
		}
		public static bool operator ==(Uint1x1 left, Uint1x1 right) { return left.Equals(right); }
		public static bool operator !=(Uint1x1 left, Uint1x1 right) { return !left.Equals(right); }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Uint1x1 other)
		{
			return this.M00.Equals(other.M00);
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

		public override string ToString() { return string.Format("({0})", this.M00); }
	}
	public static partial class MathHelper
	{
	}
}
