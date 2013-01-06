namespace Toe.Resources
{
	public struct ResourceItemSource
	{
		#region Constants and Fields

		public static ResourceItemSource Null = new ResourceItemSource(null, null);

		private readonly IResourceFile source;

		private readonly object value;

		#endregion

		#region Constructors and Destructors

		public ResourceItemSource(object value, IResourceFile source)
		{
			this.value = value;
			this.source = source;
		}

		#endregion

		#region Public Properties

		public IResourceFile Source
		{
			get
			{
				return this.source;
			}
		}

		public object Value
		{
			get
			{
				return this.value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(ResourceItemSource left, ResourceItemSource right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ResourceItemSource left, ResourceItemSource right)
		{
			return !left.Equals(right);
		}

		public bool Equals(ResourceItemSource other)
		{
			return Equals(other.Value, this.Value) && Equals(other.Source, this.Source);
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
			if (obj.GetType() != typeof(ResourceItemSource))
			{
				return false;
			}
			return this.Equals((ResourceItemSource)obj);
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
				return ((this.Value != null ? this.Value.GetHashCode() : 0) * 397)
				       ^ (this.Source != null ? this.Source.GetHashCode() : 0);
			}
		}

		#endregion
	}
}