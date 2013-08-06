namespace Toe.Messaging
{
	public class PropertyDescription
	{
		#region Constants and Fields

		private readonly string name;

		private readonly int nameHash;

		private readonly int offset;

		private readonly int propertyType;

		private readonly int size;

		#endregion

		#region Constructors and Destructors

		public PropertyDescription(string name, int offset, int size, int propertyType)
		{
			this.name = name;
			this.nameHash = Hash.Eval(name);
			this.offset = offset;
			this.size = size;
			this.propertyType = propertyType;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int NameHash
		{
			get
			{
				return this.nameHash;
			}
		}

		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		public int PropertyType
		{
			get
			{
				return this.propertyType;
			}
		}

		public int Size
		{
			get
			{
				return this.size;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(PropertyDescription left, PropertyDescription right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(PropertyDescription left, PropertyDescription right)
		{
			return !Equals(left, right);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return this.Equals((PropertyDescription)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (this.name != null ? this.name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ this.nameHash;
				hashCode = (hashCode * 397) ^ this.offset;
				hashCode = (hashCode * 397) ^ this.size;
				hashCode = (hashCode * 397) ^ this.propertyType.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} (type: {1}, offset: {2}, size: {3})", this.name, this.propertyType, this.offset, this.size);
		}

		#endregion

		#region Methods

		protected bool Equals(PropertyDescription other)
		{
			return string.Equals(this.name, other.name) && this.nameHash == other.nameHash && this.offset == other.offset
			       && this.size == other.size && this.propertyType.Equals(other.propertyType);
		}

		#endregion
	}
}