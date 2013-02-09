using System.Collections.Generic;
using System.Linq;

using Toe.Utils;

namespace Toe.Core
{
	public struct ToeMessageDescription
	{
		public ToeMessageDescription(string name, IEnumerable<ToeMessageFieldDescription> fields)
		{
			this.name = name;
			this.id = Hash.Get(name);
			this.fields = fields.ToArray();
		}
		private readonly string name;

		private readonly uint id;

		private ToeMessageFieldDescription[] fields;

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public uint Id
		{
			get
			{
				return this.id;
			}
		}

		public ToeMessageFieldDescription[] Fields
		{
			get
			{
				return this.fields;
			}
		}

		public bool Equals(ToeMessageDescription other)
		{
			if (other.id != this.id)
				return false;
			if (other.fields.Length != this.fields.Length) return false;
			for (int index = 0; index < this.fields.Length; index++)
			{
				if (this.fields[index] != other.fields[index]) return false;
			}
			return true;
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
			if (obj.GetType() != typeof(ToeMessageDescription))
			{
				return false;
			}
			return Equals((ToeMessageDescription)obj);
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
				return (this.id.GetHashCode() * 397) ^ (this.fields != null ? this.fields.GetHashCode() : 0);
			}
		}

		public static bool operator ==(ToeMessageDescription left, ToeMessageDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ToeMessageDescription left, ToeMessageDescription right)
		{
			return !left.Equals(right);
		}
	}
}