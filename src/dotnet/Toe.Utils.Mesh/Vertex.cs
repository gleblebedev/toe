using System;
using System.Linq;
using System.Text;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else
using OpenTK;
#endif

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertext, compatible with Marmalade SDK limitations.
	/// Each model vertex can have up to 4 weights.
	/// </summary>
	public class Vertex : IEquatable<Vertex>
	{
		/// <summary>
		/// Position vector.
		/// </summary>
		public Vector3 Position { get; set; }

		/// <summary>
		/// Normal vector.
		/// </summary>
		public Vector3 Normal { get; set; }

		/// <summary>
		/// Tangent for UV0.
		/// </summary>
		public Vector3 Tangent { get; set; }

		/// <summary>
		/// Binormal / bitangent for UV0.
		/// </summary>
		public Vector3 Binormal { get; set; }

		public int Color { get; set; }

		public Vector2 UV0 { get; set; }
		public Vector2 UV1 { get; set; }

		public VertexWeight Bone0 { get; set; }
		public VertexWeight Bone1 { get; set; }
		public VertexWeight Bone2 { get; set; }
		public VertexWeight Bone3 { get; set; }

		public void SortBones()
		{
			VertexWeight tmp;
			if (Bone0.Weight < Bone1.Weight)
			{
				tmp = Bone0;
				Bone0 = Bone1;
				Bone1 = tmp;
			}
			if (Bone2.Weight < Bone3.Weight)
			{
				tmp = Bone2;
				Bone2 = Bone3;
				Bone3 = tmp;
			}
			if (Bone0.Weight < Bone2.Weight)
			{
				tmp = Bone0;
				Bone0 = Bone2;
				Bone2 = tmp;
			}
			if (Bone1.Weight < Bone2.Weight)
			{
				tmp = Bone1;
				Bone1 = Bone2;
				Bone2 = tmp;
			}
			if (Bone2.Weight < Bone3.Weight)
			{
				tmp = Bone2;
				Bone2 = Bone3;
				Bone3 = tmp;
			}
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
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.Position.Equals(Position) && other.Normal.Equals(Normal) && other.Tangent.Equals(Tangent) && other.Binormal.Equals(Binormal) && other.Color == Color && other.UV0.Equals(UV0) && other.UV1.Equals(UV1) && other.Bone0.Equals(Bone0) && other.Bone1.Equals(Bone1) && other.Bone2.Equals(Bone2) && other.Bone3.Equals(Bone3);
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
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Vertex)) return false;
			return Equals((Vertex) obj);
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
				int result = Position.GetHashCode();
				result = (result*397) ^ Normal.GetHashCode();
				result = (result*397) ^ Tangent.GetHashCode();
				result = (result*397) ^ Binormal.GetHashCode();
				result = (result*397) ^ Color;
				result = (result*397) ^ UV0.GetHashCode();
				result = (result*397) ^ UV1.GetHashCode();
				result = (result*397) ^ Bone0.GetHashCode();
				result = (result*397) ^ Bone1.GetHashCode();
				result = (result*397) ^ Bone2.GetHashCode();
				result = (result*397) ^ Bone3.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(Vertex left, Vertex right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Vertex left, Vertex right)
		{
			return !Equals(left, right);
		}
	}
}
