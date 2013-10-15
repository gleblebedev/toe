using System;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	public struct BoundingBox
	{
		public bool Equals(BoundingBox other)
		{
			return this.min.Equals(other.min) && this.max.Equals(other.max);
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
			return obj is BoundingBox && this.Equals((BoundingBox)obj);
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
				return (this.min.GetHashCode() * 397) ^ this.max.GetHashCode();
			}
		}

		public static bool operator ==(BoundingBox left, BoundingBox right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BoundingBox left, BoundingBox right)
		{
			return !left.Equals(right);
		}

		public BoundingBox(Float3 min, Float3 max)
		{
			this.min = min;
			this.max = max;
		}
		private readonly Float3 min;
		private readonly Float3 max;

		public Float3 Min
		{
			get
			{
				return this.min;
			}
		}

		public Float3 Max
		{
			get
			{
				return this.max;
			}
		}

		public readonly static BoundingBox Empty = new BoundingBox(new Float3(float.MaxValue, float.MaxValue, float.MaxValue), new Float3(float.MinValue, float.MinValue, float.MinValue));
		public readonly static BoundingBox Zero = new BoundingBox(Float3.Zero, Float3.Zero);
		public readonly static BoundingBox MaxValue = new BoundingBox(new Float3(float.MinValue, float.MinValue, float.MinValue), new Float3(float.MaxValue, float.MaxValue, float.MaxValue));

		public BoundingBox Union(BoundingBox box)
		{
			return new BoundingBox(
				new Float3(Math.Min(min.X, box.Min.X), Math.Min(min.Y, box.Min.Y), Math.Min(min.Z, box.Min.Z)), 
				new Float3(Math.Max(max.X, box.Max.X), Math.Max(max.Y, box.Max.Y), Math.Max(max.Z, box.Max.Z))
				);

		}

		public float Size()
		{
			if (IsEmpty) return 0;
			if (this == MaxValue) return float.MaxValue;
			return (max - min).Length;
		}

		public bool IsEmpty
		{
			get
			{
				if (min.X >= max.X) return true;
				if (min.Y >= max.Y) return true;
				if (min.Z >= max.Z) return true;
				return false;
			}
			
		}

		public Float3 Center { get
		{
			return (min + max) * 0.5f;
		} }

		public BoundingBox Union(Float3 vertex)
		{
			return new BoundingBox(
				new Float3(Math.Min(min.X, vertex.X), Math.Min(min.Y, vertex.Y), Math.Min(min.Z, vertex.Z)),
				new Float3(Math.Max(max.X, vertex.X), Math.Max(max.Y, vertex.Y), Math.Max(max.Z, vertex.Z))
				);
		}
	}
}