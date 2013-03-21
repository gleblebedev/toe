namespace Toe.Gx
{
	public class DefaultProgramOptions
	{
		#region Constants and Fields

		private readonly DefaultFragmentShaderOptions fs;

		private readonly DefaultVertexShaderOptions vs;

		#endregion

		#region Constructors and Destructors

		public DefaultProgramOptions(DefaultVertexShaderOptions vs, DefaultFragmentShaderOptions fs)
		{
			this.vs = vs;
			this.fs = fs;
		}

		#endregion

		#region Public Properties

		public DefaultFragmentShaderOptions FragmentShaderOptions
		{
			get
			{
				return this.fs;
			}
		}

		public DefaultVertexShaderOptions VertexShaderOptions
		{
			get
			{
				return this.vs;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(DefaultProgramOptions left, DefaultProgramOptions right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DefaultProgramOptions left, DefaultProgramOptions right)
		{
			return !Equals(left, right);
		}

		public bool Equals(DefaultProgramOptions other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(other.vs, this.vs) && Equals(other.fs, this.fs);
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
			if (obj.GetType() != typeof(DefaultProgramOptions))
			{
				return false;
			}
			return this.Equals((DefaultProgramOptions)obj);
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
				return ((this.vs != null ? this.vs.GetHashCode() : 0) * 397) ^ (this.fs != null ? this.fs.GetHashCode() : 0);
			}
		}

		#endregion
	}
}