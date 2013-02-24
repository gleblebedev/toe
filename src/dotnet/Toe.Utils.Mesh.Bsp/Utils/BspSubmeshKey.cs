namespace Toe.Utils.Mesh.Bsp.Utils
{
	internal struct BspSubmeshKey
	{
		#region Constants and Fields

		private readonly int cluster;

		private readonly BspMaterialKey material;

		#endregion

		#region Constructors and Destructors

		internal BspSubmeshKey(int cluster, BspMaterialKey material)
		{
			this.cluster = cluster;
			this.material = material;
		}

		#endregion

		#region Public Properties

		public int Cluster
		{
			get
			{
				return this.cluster;
			}
		}

		public BspMaterialKey Material
		{
			get
			{
				return this.material;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(BspSubmeshKey left, BspSubmeshKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BspSubmeshKey left, BspSubmeshKey right)
		{
			return !left.Equals(right);
		}

		public bool Equals(BspSubmeshKey other)
		{
			return other.cluster == this.cluster && other.material.Equals(this.material);
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
			if (obj.GetType() != typeof(BspSubmeshKey))
			{
				return false;
			}
			return this.Equals((BspSubmeshKey)obj);
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
				return (this.cluster * 397) ^ this.material.GetHashCode();
			}
		}

		#endregion
	}
}