namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Blend modes for 2nd texture, if any.
	/// @see GetBlendMode, SetBlendMode
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum BlendMode
	{
		BLEND_MODULATE, //!< multiply;
		BLEND_DECAL,    //!< decal;
		BLEND_ADD,      //!< additive;
		BLEND_REPLACE,  //!< replace;
		BLEND_BLEND,    //!< blend (strange inverting behaviour);
		BLEND_MODULATE_2X, //!< blend 2x
		BLEND_MODULATE_4X, //!< blend 4x
	};
}