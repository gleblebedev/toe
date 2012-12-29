namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Preset effect modes.
	/// @see GetEffectPreset, SetEffectPreset
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum EffectPreset
	{
		/// <summary>
		/// Combine 2nd texture using BlendMode, use 2nd UV set if available, otherwise 1st UV set.
		/// </summary>
		DEFAULT,

		/// <summary>
		/// Use 2nd texture as a normal map, then use BlendMode to combine 1st texture.
		/// </summary>
		NORMAL_MAPPING, 
		/// <summary>
		/// Combine 2nd texture as a spherical reflection map, Automatically computes reflection UVs
		/// </summary>
		REFLECTION_MAPPING,  

		/// <summary>
		/// PRIVATE
		/// </summary>
		ENVIRONMENT_MAPPING,

		/// <summary>
		/// Use primary colour for RGB
		/// </summary>
		CONSTANT_COLOUR_CHANNEL,

		/// <summary>
		/// Modulate in framebuffer for gl lightmaps
		/// </summary>
		LIGHTMAP_POST_PROCESS, 

		/// <summary>
		/// Use 2nd texture as a specular normal map, then use BlendMode to combine 1st texture
		/// </summary>
		NORMAL_MAPPING_SPECULAR,     

		/// <summary>
		/// Ignore the second texture stage on non-shader HW
		/// </summary>
		TEXTURE0_ONLY,      
	};
}