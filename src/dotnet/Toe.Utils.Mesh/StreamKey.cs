using System;

namespace Toe.Utils.Mesh
{
	public struct StreamKey : IEquatable<StreamKey>
	{
		#region Constants and Fields

		private readonly int channel;

		private readonly string key;

		#endregion

		#region Constructors and Destructors

		public StreamKey(string key, int channel)
			: this()
		{
			this.key = key;
			this.channel = channel;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}",key,channel);
		}

		public StreamKey(string key)
			: this()
		{
			this.key = key;
			this.channel = 0;
		}

		#endregion

		#region Public Properties

		public int Channel
		{
			get
			{
				return this.channel;
			}
		}

		public string Key
		{
			get
			{
				return this.key;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(StreamKey left, StreamKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StreamKey left, StreamKey right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(StreamKey other)
		{
			return string.Equals(this.key, other.key) && this.channel == other.channel;
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
			return obj is StreamKey && this.Equals((StreamKey)obj);
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
			unchecked
			{
				return ((this.key != null ? this.key.GetHashCode() : 0) * 397) ^ this.channel;
			}
		}

		#endregion
	}
}