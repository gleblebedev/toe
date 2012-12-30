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
		MODULATE, //!< multiply;
		DECAL, //!< decal;
		ADD, //!< additive;
		REPLACE, //!< replace;
		BLEND, //!< blend (strange inverting behaviour);
		MODULATE_2X, //!< blend 2x
		MODULATE_4X, //!< blend 4x
	};
}