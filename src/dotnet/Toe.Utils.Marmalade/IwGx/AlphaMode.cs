namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Alpha (transparency) modes.
	/// @see GetAlphaMode, SetAlphaMode
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum AlphaMode
	{
		/// <summary>
		/// Material is opaque (not transparent).
		/// </summary>
		NONE,

		/// <summary>
		/// Material is transparent. Source colour (as calculated by the lighting pipeline) and destination colour (the current
		/// colour in the backbuffer) are combined as follows:
		/// Colour = (Source + Destination) / 2
		/// </summary>
		HALF,

		/// <summary>
		/// Material is transparent. Source colour (as calculated by the lighting pipeline) and destination colour (the current
		/// colour in the backbuffer) are combined as follows:
		/// Colour = (Source + Destination)
		/// </summary>
		ADD,

		/// <summary>
		/// Material is transparent. Source colour (as calculated by the lighting pipeline) and destination colour (the current
		/// colour in the backbuffer) are combined as follows:
		/// Colour = (Destination - Source)
		/// </summary>
		SUB,

		/// <summary>
		/// Material is transparent. Source colour (as calculated by the lighting pipeline) and destination colour (the current
		/// colour in the backbuffer) are combined as follows:
		/// Colour = (Source/// SourceAlpha) + (Destination/// (1 - SourceAlpha))
		/// @note This mode is not available when using software rasterisation; in that case, it performs identically to ALPHA_NONE.
		/// </summary>
		BLEND,

		/// <summary>
		/// Material is transparent. Source colour (as calculated by the lighting pipeline) and destination colour (the current
		/// colour in the backbuffer) are combined as follows:
		/// Colour = (Source + Destination) / 2
		/// </summary>
		DEFAULT,
	};
}