namespace Toe.Gx
{
	public class DefaultVertexShaderOptions
	{
		#region Public Properties

		public bool BITANGENT_STREAM { get; set; }

		public bool COL_STREAM { get; set; }

		public bool FAST_FOG { get; set; }

		public bool FOG { get; set; }

		public bool LIGHT_AMBIENT { get; set; }

		public bool LIGHT_DIFFUSE { get; set; }

		public bool LIGHT_EMISSIVE { get; set; }

		public bool LIGHT_SPECULAR { get; set; }

		public bool NORM_STREAM { get; set; }

		public bool SKINWEIGHT_STREAM { get; set; }

		public bool SKIN_MAJOR_BONE { get; set; }

		public bool SKIN_NORMALS { get; set; }

		public bool TANGENT_STREAM { get; set; }

		public bool UV0_STREAM { get; set; }

		public bool UV1_STREAM { get; set; }

		#endregion

		#region Public Methods and Operators

		public static bool operator ==(DefaultVertexShaderOptions left, DefaultVertexShaderOptions right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DefaultVertexShaderOptions left, DefaultVertexShaderOptions right)
		{
			return !Equals(left, right);
		}

		public bool Equals(DefaultVertexShaderOptions other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return other.UV0_STREAM.Equals(this.UV0_STREAM) && other.UV1_STREAM.Equals(this.UV1_STREAM)
			       && other.COL_STREAM.Equals(this.COL_STREAM) && other.LIGHT_AMBIENT.Equals(this.LIGHT_AMBIENT)
			       && other.LIGHT_EMISSIVE.Equals(this.LIGHT_EMISSIVE) && other.LIGHT_DIFFUSE.Equals(this.LIGHT_DIFFUSE)
			       && other.LIGHT_SPECULAR.Equals(this.LIGHT_SPECULAR) && other.FAST_FOG.Equals(this.FAST_FOG)
			       && other.FOG.Equals(this.FOG) && other.NORM_STREAM.Equals(this.NORM_STREAM)
			       && other.TANGENT_STREAM.Equals(this.TANGENT_STREAM) && other.BITANGENT_STREAM.Equals(this.BITANGENT_STREAM)
			       && other.SKINWEIGHT_STREAM.Equals(this.SKINWEIGHT_STREAM) && other.SKIN_NORMALS.Equals(this.SKIN_NORMALS)
			       && other.SKIN_MAJOR_BONE.Equals(this.SKIN_MAJOR_BONE);
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
			if (obj.GetType() != typeof(DefaultVertexShaderOptions))
			{
				return false;
			}
			return this.Equals((DefaultVertexShaderOptions)obj);
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
				int result = this.UV0_STREAM.GetHashCode();
				result = (result * 397) ^ this.UV1_STREAM.GetHashCode();
				result = (result * 397) ^ this.COL_STREAM.GetHashCode();
				result = (result * 397) ^ this.LIGHT_AMBIENT.GetHashCode();
				result = (result * 397) ^ this.LIGHT_EMISSIVE.GetHashCode();
				result = (result * 397) ^ this.LIGHT_DIFFUSE.GetHashCode();
				result = (result * 397) ^ this.LIGHT_SPECULAR.GetHashCode();
				result = (result * 397) ^ this.FAST_FOG.GetHashCode();
				result = (result * 397) ^ this.FOG.GetHashCode();
				result = (result * 397) ^ this.NORM_STREAM.GetHashCode();
				result = (result * 397) ^ this.TANGENT_STREAM.GetHashCode();
				result = (result * 397) ^ this.BITANGENT_STREAM.GetHashCode();
				result = (result * 397) ^ this.SKINWEIGHT_STREAM.GetHashCode();
				result = (result * 397) ^ this.SKIN_NORMALS.GetHashCode();
				result = (result * 397) ^ this.SKIN_MAJOR_BONE.GetHashCode();
				return result;
			}
		}

		#endregion
	}
}