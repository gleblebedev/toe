namespace Toe.Utils.Marmalade.IwGx
{
	/// <summary>
	/// Backface culling modes.
	/// @see GetCullMode, SetCullMode
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum CullMode
	{
		/// <summary>
		/// Polygons with a clockwise winding order are culled.
		/// </summary>
		FRONT,

		/// <summary>
		/// Polygons with an anti-clockwise winding order are culled.
		/// </summary>
		BACK,

		/// <summary>
		/// Polygons are not culled (polygons are "double-sided").
		/// </summary>
		NONE,
	};
}