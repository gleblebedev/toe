using Toe.Utils;

namespace Toe.Core
{
	public struct FieldDescription
	{
		#region Constants and Fields

		//private readonly int count;

		private readonly uint id;

		private readonly string name;

		private readonly string type;

		private readonly uint typeid;

		#endregion

		#region Constructors and Destructors

		public FieldDescription(string name, string type)
		{
			this.name = name;
			this.type = type;
			this.id = Hash.Get(name);
			this.typeid = Hash.Get(type);
		}
		#endregion

		#region Public Properties

		//public int Count
		//{
		//    get
		//    {
		//        return this.count;
		//    }
		//}

		/// <summary>
		/// Message field id (Name hash).
		/// </summary>
		public uint Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// Message field name.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// File type.
		/// </summary>
		public string Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>
		/// File type ID (Type hash).
		/// </summary>
		public uint Typeid
		{
			get
			{
				return this.typeid;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(FieldDescription left, FieldDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FieldDescription left, FieldDescription right)
		{
			return !left.Equals(right);
		}

		public bool Equals(FieldDescription other)
		{
			return other.typeid == this.typeid && other.id == this.id;// && other.count == this.count;
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
			if (obj.GetType() != typeof(FieldDescription))
			{
				return false;
			}
			return this.Equals((FieldDescription)obj);
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
				//result = (result * 397) ^ this.count;
				return result;
			}
		}

		#endregion
	}
}