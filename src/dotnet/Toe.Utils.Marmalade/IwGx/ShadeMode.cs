using Toe.Editors.Interfaces.Views;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Shade modes.
	/// @see GetShadeMode, SetShadeMode
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum ShadeMode
	{
		/// <summary>
		/// Polygons are flat-shaded according to the colour of the first vertex in each polygon,  so each polygon is a single colour.
		/// </summary>
		SHADE_FLAT,

		/// <summary>
		/// Polygons are gouraud-shaded according to the vertex colours.
		/// </summary>
		SHADE_GOURAUD,
	};
}