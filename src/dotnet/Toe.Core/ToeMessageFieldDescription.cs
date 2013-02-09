using Toe.Utils;

namespace Toe.Core
{
	public struct ToeMessageFieldDescription
	{
		private readonly string name;

		private readonly string type;

		private readonly int count;

		private readonly uint id;

		private readonly uint typeid;

		public ToeMessageFieldDescription(string name, string type, int count)
		{
			this.name = name;
			this.type = type;
			this.count = count;
			this.id = Hash.Get(name);
			this.typeid = Hash.Get(type);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Type
		{
			get
			{
				return this.type;
			}
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public uint Id
		{
			get
			{
				return this.id;
			}
		}

		public uint Typeid
		{
			get
			{
				return this.typeid;
			}
		}

		public bool Equals(ToeMessageFieldDescription other)
		{
			return other.typeid == this.typeid && other.id == this.id && other.count == this.count;
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
			if (obj.GetType() != typeof(ToeMessageFieldDescription))
			{
				return false;
			}
			return Equals((ToeMessageFieldDescription)obj);
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
				int result = this.typeid.GetHashCode();
				result = (result * 397) ^ this.id.GetHashCode();
				result = (result * 397) ^ this.count;
				return result;
			}
		}

		public static bool operator ==(ToeMessageFieldDescription left, ToeMessageFieldDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ToeMessageFieldDescription left, ToeMessageFieldDescription right)
		{
			return !left.Equals(right);
		}
	}
}