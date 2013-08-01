using System.Runtime.InteropServices;

namespace Toe.CircularArrayQueue
{
	[StructLayout(LayoutKind.Explicit)]
	public struct BufferItem
	{
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		public bool Equals(BufferItem other)
		{
			return this.Int32 == other.Int32;
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
			return obj is BufferItem && this.Equals((BufferItem)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.Int32;
		}

		/// <summary>
		/// Indicates whether instancies are equal.
		/// </summary>
		public static bool operator ==(BufferItem left, BufferItem right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether instancies are not equal.
		/// </summary>
		public static bool operator !=(BufferItem left, BufferItem right)
		{
			return !left.Equals(right);
		}

		[FieldOffset(0)]
		public int Int32;

		[FieldOffset(0)]
		public float Single;

		[FieldOffset(0)]
		public byte Byte0;

		[FieldOffset(1)]
		public byte Byte1;

		[FieldOffset(2)]
		public byte Byte2;

		[FieldOffset(3)]
		public byte Byte3;
	}
}