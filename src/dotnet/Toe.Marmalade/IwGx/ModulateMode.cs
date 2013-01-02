namespace Toe.Utils.Marmalade.IwGx
{
	/// <summary>
	/// Modulation modes. For textured materials, this affects how the texel colour is modified by the vertex colour to calculate the colour to be written to the backbuffer.
	/// </summary>
	public enum ModulateMode
	{
		/// <summary>
		/// Texel colour is modified by vertex colour in the following way:
		/// Cr = Tr/// Vr, Cg = Tg/// Vg, Cb = Tb/// Vb, Ca = Ta/// Va
		/// Because this carries a significant performance penalty in SW rendering, an application must opt-in
		/// to full SW RGB modulation by including "[GX] UseRGBLighting=1" in its icf file. HW accelerated
		/// rendering always respects MODULATE_RGB.
		/// </summary>
		RGB,

		/// <summary>
		/// Texel colour is modified by vertex colour in the following way:
		/// Cr = Cg = Cb = Tr/// Vr,  Ca = 0xFF
		/// </summary>
		R,

		/// <summary>
		/// Texel colour is NOT modified by vertex colour, so the final colour is always equal to the texel colour:
		/// Cr = Tr, Cg = Tg, Cb = Tb, Ca = Ta
		/// </summary>
		NONE,

		MASK
	};
}