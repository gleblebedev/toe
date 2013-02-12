namespace Toe.Utils.Mesh.Bsp.Utils
{
	public interface IMaterialProvider
	{
		IMaterial CreateMaterial(BspMaterialKey material);
	}
	public struct BspMaterialKey
	{
		private readonly int material;

		private readonly int lightmap;

		internal BspMaterialKey(int material, int lightmap)
		{
			this.material = material;
			this.lightmap = lightmap;
		}

		public int Material
		{
			get
			{
				return this.material;
			}
		}

		public int Lightmap
		{
			get
			{
				return this.lightmap;
			}
		}

		public bool Equals(BspMaterialKey other)
		{
			return other.material == this.material && other.lightmap == this.lightmap;
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
			if (obj.GetType() != typeof(BspMaterialKey))
			{
				return false;
			}
			return this.Equals((BspMaterialKey)obj);
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
				return (this.material * 397) ^ this.lightmap;
			}
		}

		public static bool operator ==(BspMaterialKey left, BspMaterialKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BspMaterialKey left, BspMaterialKey right)
		{
			return !left.Equals(right);
		}
	}
}