namespace Toe.Marmalade.IwGx
{
	public class DefaultFragmentShaderOptions 
	{
		public bool TEX0 { get; set; }
		public bool TEX1 { get; set; }
		public bool UV0_STREAM { get; set; }
		public bool UV1_STREAM { get; set; }
		public bool FAST_FOG { get; set; }
		public bool COL_STREAM { get; set; }
		public bool LIGHT_AMBIENT { get; set; }
		public bool LIGHT_EMISSIVE { get; set; }
		public bool LIGHT_DIFFUSE { get; set; }
		public bool LIGHT_SPECULAR { get; set; }
		public bool ALPHA_TEST { get; set; }
		public int ALPHA_BLEND { get; set; }
		public bool FOG { get; set; }
		public bool IW_GX_PLATFORM_TEGRA2 { get; set; }
		public int EFFECT_PRESET { get; set; }
		public int BLEND { get; set; }

		public bool Equals(DefaultFragmentShaderOptions other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return other.TEX0.Equals(this.TEX0) && other.TEX1.Equals(this.TEX1) && other.UV0_STREAM.Equals(this.UV0_STREAM) && other.FAST_FOG.Equals(this.FAST_FOG) && other.UV1_STREAM.Equals(this.UV1_STREAM) && other.COL_STREAM.Equals(this.COL_STREAM) && other.LIGHT_AMBIENT.Equals(this.LIGHT_AMBIENT) && other.LIGHT_EMISSIVE.Equals(this.LIGHT_EMISSIVE) && other.LIGHT_DIFFUSE.Equals(this.LIGHT_DIFFUSE) && other.LIGHT_SPECULAR.Equals(this.LIGHT_SPECULAR) && other.ALPHA_TEST.Equals(this.ALPHA_TEST) && other.FOG.Equals(this.FOG) && other.ALPHA_BLEND == this.ALPHA_BLEND && other.IW_GX_PLATFORM_TEGRA2.Equals(this.IW_GX_PLATFORM_TEGRA2) && other.EFFECT_PRESET == this.EFFECT_PRESET && other.BLEND == this.BLEND;
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
			if (obj.GetType() != typeof(DefaultFragmentShaderOptions))
			{
				return false;
			}
			return Equals((DefaultFragmentShaderOptions)obj);
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
				int result = this.TEX0.GetHashCode();
				result = (result * 397) ^ this.TEX1.GetHashCode();
				result = (result * 397) ^ this.UV0_STREAM.GetHashCode();
				result = (result * 397) ^ this.FAST_FOG.GetHashCode();
				result = (result * 397) ^ this.UV1_STREAM.GetHashCode();
				result = (result * 397) ^ this.COL_STREAM.GetHashCode();
				result = (result * 397) ^ this.LIGHT_AMBIENT.GetHashCode();
				result = (result * 397) ^ this.LIGHT_EMISSIVE.GetHashCode();
				result = (result * 397) ^ this.LIGHT_DIFFUSE.GetHashCode();
				result = (result * 397) ^ this.LIGHT_SPECULAR.GetHashCode();
				result = (result * 397) ^ this.ALPHA_TEST.GetHashCode();
				result = (result * 397) ^ this.FOG.GetHashCode();
				result = (result * 397) ^ this.ALPHA_BLEND;
				result = (result * 397) ^ this.IW_GX_PLATFORM_TEGRA2.GetHashCode();
				result = (result * 397) ^ this.EFFECT_PRESET;
				result = (result * 397) ^ this.BLEND;
				return result;
			}
		}

		public static bool operator ==(DefaultFragmentShaderOptions left, DefaultFragmentShaderOptions right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DefaultFragmentShaderOptions left, DefaultFragmentShaderOptions right)
		{
			return !Equals(left, right);
		}
	}
}