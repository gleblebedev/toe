namespace Toe.Core
{
	public struct ToeEntityId
	{
		#region Constants and Fields

		public static readonly ToeEntityId Empty = new ToeEntityId(0);

		private readonly uint id;

		#endregion

		#region Constructors and Destructors

		public ToeEntityId(int index, byte version = 0)
			: this()
		{
			this.id = this.id | ((uint)version << 24);
		}

		private ToeEntityId(uint id)
			: this()
		{
			this.id = id;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Entity scene index.
		/// </summary>
		public int Index
		{
			get
			{
				return (int)(this.id & 0x00FFFFFF);
			}
		}

		/// <summary>
		/// Unique ID as combination of index and version.
		/// </summary>
		public uint UniqueId
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// Entity version.
		/// </summary>
		public byte Version
		{
			get
			{
				return (byte)(this.id >> 24);
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(ToeEntityId left, ToeEntityId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ToeEntityId left, ToeEntityId right)
		{
			return !left.Equals(right);
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
			return this.Equals((ToeEntityId)obj);
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

		public ToeEntityId IncreaseVersion()
		{
			return new ToeEntityId(this.id + 0x01000000);
		}

		#endregion
	}
}