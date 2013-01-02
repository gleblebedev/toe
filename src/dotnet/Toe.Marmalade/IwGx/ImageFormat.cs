namespace Toe.Marmalade.IwGx
{
	/// <summary>
	/// Image formats.
	/// </summary>
	public enum ImageFormat
	{
		FORMAT_UNDEFINED, //!< Format is undefined.

		RGB_332, //!< Unpalettised 8-bit.
		BGR_332, //!< Unpalettised 8-bit.

		RGB_565, //!< Unpalettised 16-bit, no alpha.
		BGR_565, //!< Unpalettised 16-bit, no alpha.

		RGBA_4444, //!< Unpalettised 16-bit, alpha.
		ABGR_4444, //!< Unpalettised 16-bit, alpha.
		RGBA_5551, //!< Unpalettised 16-bit, alpha.
		ABGR_1555, //!< Unpalettised 16-bit, alpha.

		RGB_888, //!< Unpalettised 24-bit, no alpha.
		BGR_888, //!< Unpalettised 24-bit, no alpha.

		RGBA_6666, //!< Unpalettised 24-bit, alpha.
		ABGR_6666, //!< Unpalettised 24-bit, alpha.

		RGBA_8888, //!< Unpalettised 32-bit, alpha.
		ABGR_8888, //!< Unpalettised 32-bit, alpha.
		RGBA_AAA2, //!< Unpalettised 32-bit, alpha.
		ABGR_2AAA, //!< Unpalettised 32-bit, alpha.

		PALETTE4_RGB_888, //!< 16-colour palettised.
		PALETTE4_RGBA_8888, //!< 16-colour palettised.
		PALETTE4_RGB_565, //!< 16-colour palettised.
		PALETTE4_RGBA_4444, //!< 16-colour palettised.
		PALETTE4_RGBA_5551, //!< 16-colour palettised.
		PALETTE4_ABGR_1555,

		PALETTE8_RGB_888, //!< 256-colour palettised.
		PALETTE8_RGBA_8888, //!< 256-colour palettised.
		PALETTE8_RGB_565, //!< 256-colour palettised.
		PALETTE8_RGBA_4444, //!< 256-colour palettised.
		PALETTE8_RGBA_5551, //!< 256-colour palettised.
		PALETTE8_ABGR_1555,

		PALETTE7_ABGR_1555, //!< 32-colour palettised.
		PALETTE6_ABGR_1555, //!< 64-colour palettised.
		PALETTE5_ABGR_1555, //!< 128-colour palettised.

		// PVRTC formats
		PVRTC_2, //!< PowerVR compressed format.
		PVRTC_4, //!< PowerVR compressed format.
		ATITC, //!< ATI compressed format.
		COMPRESSED, //!< gfx specific compressed format

		PALETTE4_ABGR_4444, //!< 16-colour palettised.
		PALETTE8_ABGR_4444, //!< 256-colour palettised.

		A_8, //!< Unpalettised 8-bit alpha.

		ETC, //!< Ericsson compressed format
		ARGB_8888, //!< Unpalettised 32-bit, alpha.

		PALETTE4_ARGB_8888, //!< 16-colour palettised.
		PALETTE8_ARGB_8888, //!< 256-colour palettised.

		DXT3, //!< DXT3 compressed format

		PALETTE4_BGR555, //!< 16-colour palettised.
		PALETTE8_BGR555, //!< 16-colour palettised.
		A5_PALETTE3_BGR_555, //!< 8BPP, of which 5 are alpha and 3 are palette index
		A3_PALETTE5_BGR_555, //!< 8BPP, of which 3 are alpha and 5 are palette index

		PALETTE4_BGR_565, //!< 16-colour palettised.
		PALETTE4_ABGR_8888, //!< 16-colour palettised.
		PALETTE8_BGR_565, //!< 256-colour palettised.
		PALETTE8_ABGR_8888, //!< 256-colour palettised.

		DXT1, //!< DXT1 compressed format
		DXT5, //!< DXT5 compressed format

		FORMAT_MAX, // (Terminator)
	};
}