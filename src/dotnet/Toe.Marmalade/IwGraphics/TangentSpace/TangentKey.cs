using OpenTK;

namespace Toe.Marmalade.IwGraphics.TangentSpace
{
	public struct TangentKey
	{
		#region Constants and Fields

		private readonly Vector3 position;

		private readonly Vector2 uv0;

		#endregion

		#region Constructors and Destructors

		public TangentKey(Vector3 p0, Vector2 pu0)
		{
			this.position = p0;
			this.uv0 = pu0;
		}

		#endregion

		#region Public Properties

		public Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		public Vector2 UV0
		{
			get
			{
				return this.uv0;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(TangentKey left, TangentKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TangentKey left, TangentKey right)
		{
			return !left.Equals(right);
		}

		public bool Equals(TangentKey other)
		{
			return other.Position.Equals(this.Position) && other.UV0.Equals(this.UV0);
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
			if (obj.GetType() != typeof(TangentKey))
			{
				return false;
			}
			return this.Equals((TangentKey)obj);
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
				return (this.Position.GetHashCode() * 397) ^ this.UV0.GetHashCode();
			}
		}

		#endregion
	}
}