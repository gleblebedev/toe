using System.Collections.Generic;
using System.Linq;

using Toe.Utils;

namespace Toe.Core
{
	/// <summary>
	/// Message description.
	/// </summary>
	public struct ToeMessageDescription
	{
		#region Constants and Fields

		private readonly FieldDescription[] fields;

		private readonly uint id;

		private readonly string name;

		#endregion

		#region Constructors and Destructors

		public ToeMessageDescription(string name, IEnumerable<FieldDescription> fields)
		{
			this.name = name;
			this.id = Hash.Get(name);
			this.fields = fields.ToArray();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// List of field descriptions.
		/// </summary>
		public IList<FieldDescription> Fields
		{
			get
			{
				return this.fields;
			}
		}

		/// <summary>
		/// Message ID (Name hash).
		/// </summary>
		public uint Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// Name of the message.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(ToeMessageDescription left, ToeMessageDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ToeMessageDescription left, ToeMessageDescription right)
		{
			return !left.Equals(right);
		}

		public bool Equals(ToeMessageDescription other)
		{
			if (other.id != this.id)
			{
				return false;
			}
			if (other.fields.Length != this.fields.Length)
			{
				return false;
			}
			for (int index = 0; index < this.fields.Length; index++)
			{
				if (this.fields[index] != other.fields[index])
				{
					return false;
				}
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
			return this.Equals((ToeMessageDescription)obj);
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

		#endregion
	}
}