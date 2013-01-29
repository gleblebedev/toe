namespace Toe.Core
{
	public struct ToeEntityId
	{
		private readonly uint id;
		
		public ToeEntityId(int index, byte version = 0)
			: this()
		{
			this.id = id | ((uint)version << 24);
		}

		private ToeEntityId(uint id)
			: this()
		{
			this.id = id;
		}

		/// <summary>
		/// Entity scene index.
		/// </summary>
		public int Index
		{ 
			get
			{
				return (int)(id & 0x00FFFFFF);
			}
		}

		/// <summary>
		/// Unique ID as combination of index and version.
		/// </summary>
		public uint UniqueId
		{
			get
			{
				return id;
			}
		}

		/// <summary>
		/// Entity version.
		/// </summary>
		public byte Version
		{
			get
			{
				return (byte)(id >> 24);
			}
		}

		public readonly static ToeEntityId Empty = new ToeEntityId(0);

		public ToeEntityId IncreaseVersion()
		{
			return new ToeEntityId(id + 0x01000000);
		}

		public bool Equals(ToeEntityId other)
		{
			return other.id == this.id;
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
			if (obj.GetType() != typeof(ToeEntityId))
			{
				return false;
			}
			return Equals((ToeEntityId)obj);
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
			return this.id.GetHashCode();
		}

		public static bool operator ==(ToeEntityId left, ToeEntityId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ToeEntityId left, ToeEntityId right)
		{
			return !left.Equals(right);
		}
	}
}