using System;
using System.Drawing;

using OpenTK;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertext, compatible with Marmalade SDK limitations.
	/// Each model vertex can have up to 4 weights.
	/// </summary>
	public struct Vertex : IEquatable<Vertex>
	{
		#region Public Properties

		private Vector3 binormal;

		/// <summary>
		/// Binormal / bitangent for UV0.
		/// </summary>
		public Vector3 Binormal
		{
			get
			{
				return this.binormal;
			}
			set
			{
				this.binormal = value;
			}
		}

		private Color color;

		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}

		private Vector3 normal;

		/// <summary>
		/// Normal vector.
		/// </summary>
		public Vector3 Normal
		{
			get
			{
				return this.normal;
			}
			set
			{
				this.normal = value;
			}
		}

		private Vector3 position;

		/// <summary>
		/// Position vector.
		/// </summary>
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		private Vector3 tangent;

		/// <summary>
		/// Tangent for UV0.
		/// </summary>
		public Vector3 Tangent
		{
			get
			{
				return this.tangent;
			}
			set
			{
				this.tangent = value;
			}
		}

		private Vector3 uv0;

		public Vector3 UV0
		{
			get
			{
				return this.uv0;
			}
			set
			{
				this.uv0 = value;
			}
		}

		private Vector3 uv1;

		public Vector3 UV1
		{
			get
			{
				return this.uv1;
			}
			set
			{
				this.uv1 = value;
			}
		}

		private VertexWeights weights;

		public VertexWeights Weights
		{
			get
			{
				return this.weights;
			}
			set
			{
				this.weights = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(Vertex left, Vertex right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Vertex left, Vertex right)
		{
			return !Equals(left, right);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Vertex other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return other.Position.Equals(this.Position) && other.Normal.Equals(this.Normal) && other.Tangent.Equals(this.Tangent)
			       && other.Binormal.Equals(this.Binormal) && other.Color == this.Color && other.UV0.Equals(this.UV0)
			       && other.UV1.Equals(this.UV1) && other.Weights.Equals(this.Weights);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
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
			if (obj.GetType() != typeof(Vertex))
			{
				return false;
			}
			return this.Equals((Vertex)obj);
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
				int result = this.Position.GetHashCode();
				result = (result * 397) ^ this.Normal.GetHashCode();
				result = (result * 397) ^ this.Tangent.GetHashCode();
				result = (result * 397) ^ this.Binormal.GetHashCode();
				result = (result * 397) ^ this.Color.GetHashCode();
				result = (result * 397) ^ this.UV0.GetHashCode();
				result = (result * 397) ^ this.UV1.GetHashCode();
				result = (result * 397) ^ this.Weights.GetHashCode();
				return result;
			}
		}

		#endregion
	}
}