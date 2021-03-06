﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Toe.Utils.ToeMath;

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
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex : IEquatable<Vertex>
	{
		#region Public Properties

		/// <summary>
		/// Vertex position.
		/// </summary>
		[MeshStream(Streams.Position)]
		public Float3 Position;

		/// <summary>
		/// Vertex normal vector.
		/// </summary>
		[MeshStream(Streams.Normal)]
		public Float3 Normal;

		/// <summary>
		/// Vertex color.
		/// </summary>
		[MeshStream(Streams.Color)]
		public Color Color;

		[MeshStream(Streams.TexCoord,0)]
		public Float3 UV0;

		[MeshStream(Streams.TexCoord, 1)]
		public Float3 UV1;

		/// <summary>
		/// Tangent for UV0.
		/// </summary>
		[MeshStream(Streams.Tangent, 0)]
		public Float3 Tangent;

		/// <summary>
		/// Binormal / bitangent for UV0.
		/// </summary>
		[MeshStream(Streams.Binormal, 0)]
		public Float3 Binormal;

		[MeshStream(Streams.Weight, 0)]
		public VertexWeights Weights;

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