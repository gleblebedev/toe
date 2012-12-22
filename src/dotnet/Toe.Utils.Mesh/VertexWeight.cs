namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertex weight.
	/// </summary>
	public struct VertexWeight
	{
		#region Constants and Fields

		public static VertexWeight Zero = new VertexWeight { BoneIndex = 0, Weight = 0 };

		/// <summary>
		/// Bone.
		/// </summary>
		public int BoneIndex;

		/// <summary>
		/// Weight of the bone.
		/// </summary>
		public float Weight;

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(VertexWeight left, VertexWeight right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(VertexWeight left, VertexWeight right)
		{
			return !left.Equals(right);
		}

		public bool Equals(VertexWeight other)
		{
			if (!other.Weight.Equals(this.Weight))
			{
				return false;
			}
			if (this.Weight == 0)
			{
				return true;
			}
			return other.BoneIndex == this.BoneIndex;
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
			if (obj.GetType() != typeof(VertexWeight))
			{
				return false;
			}
			return this.Equals((VertexWeight)obj);
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
				var i = (this.Weight.GetHashCode() * 397);
				if (this.Weight == 0)
				{
					i = i ^ this.BoneIndex;
				}
				return i;
			}
		}

		#endregion
	}
}