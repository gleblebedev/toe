namespace Toe.Utils.Mesh
{
	public struct VertexWeights
	{
		#region Constants and Fields

		public static VertexWeights Empty = new VertexWeights
			{ Bone0 = VertexWeight.Zero, Bone1 = VertexWeight.Zero, Bone2 = VertexWeight.Zero, Bone3 = VertexWeight.Zero };

		#endregion

		#region Public Properties

		public VertexWeight Bone0 { get; set; }

		public VertexWeight Bone1 { get; set; }

		public VertexWeight Bone2 { get; set; }

		public VertexWeight Bone3 { get; set; }

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(VertexWeights left, VertexWeights right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(VertexWeights left, VertexWeights right)
		{
			return !left.Equals(right);
		}

		public bool Equals(VertexWeights other)
		{
			return other.Bone0.Equals(this.Bone0) && other.Bone1.Equals(this.Bone1) && other.Bone2.Equals(this.Bone2)
			       && other.Bone3.Equals(this.Bone3);
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
			if (obj.GetType() != typeof(VertexWeights))
			{
				return false;
			}
			return this.Equals((VertexWeights)obj);
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
				int result = this.Bone0.GetHashCode();
				result = (result * 397) ^ this.Bone1.GetHashCode();
				result = (result * 397) ^ this.Bone2.GetHashCode();
				result = (result * 397) ^ this.Bone3.GetHashCode();
				return result;
			}
		}

		public void SortBones()
		{
			VertexWeight tmp;
			if (this.Bone0.Weight < this.Bone1.Weight)
			{
				tmp = this.Bone0;
				this.Bone0 = this.Bone1;
				this.Bone1 = tmp;
			}
			if (this.Bone2.Weight < this.Bone3.Weight)
			{
				tmp = this.Bone2;
				this.Bone2 = this.Bone3;
				this.Bone3 = tmp;
			}
			if (this.Bone0.Weight < this.Bone2.Weight)
			{
				tmp = this.Bone0;
				this.Bone0 = this.Bone2;
				this.Bone2 = tmp;
			}
			if (this.Bone1.Weight < this.Bone2.Weight)
			{
				tmp = this.Bone1;
				this.Bone1 = this.Bone2;
				this.Bone2 = tmp;
			}
			if (this.Bone2.Weight < this.Bone3.Weight)
			{
				tmp = this.Bone2;
				this.Bone2 = this.Bone3;
				this.Bone3 = tmp;
			}
		}

		#endregion
	}
}