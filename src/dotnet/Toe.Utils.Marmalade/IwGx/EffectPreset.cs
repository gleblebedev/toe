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
		DEFAULT,             //!< Combine 2nd texture using BlendMode, use 2nd UV set if available, otherwise 1st UV set
		NORMAL_MAPPING,      //!< Use 2nd texture as a normal map, then use BlendMode to combine 1st texture
		REFLECTION_MAPPING,  //!< Combine 2nd texture as a spherical reflection map, Automatically computes reflection UVs
		ENVIRONMENT_MAPPING, // PRIVATE
		CONSTANT_COLOUR_CHANNEL, //!< Use primary colour for RGB
		LIGHTMAP_POST_PROCESS, //!< Modulate in framebuffer for gl lightmaps
		NORMAL_MAPPING_SPECULAR,     //!< Use 2nd texture as a specular normal map, then use BlendMode to combine 1st texture
		TEXTURE0_ONLY,       //!< Ignore the second texture stage on non-shader HW
	};
}