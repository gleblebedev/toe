using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL;

using Toe.Gx;

using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Toe.Marmalade.IwGx
{
	public class Image
	{
		#region Constants and Fields

		/// <summary>
		/// The a 3_ palett e 5_ bg r_555.
		/// </summary>
		public const byte A3_PALETTE5_BGR_555 = 0x2F; // !< 8BPP = 0x;of which 3 are alpha and 5 are palette index

		/// <summary>
		/// The a 5_ palett e 3_ bg r_555.
		/// </summary>
		public const byte A5_PALETTE3_BGR_555 = 0x2E; // !< 8BPP = 0x;of which 5 are alpha and 3 are palette index

		/// <summary>
		/// The abg r_1555.
		/// </summary>
		public const byte ABGR_1555 = 8; // !< Unpalettised 16-bit = ;alpha.

		/// <summary>
		/// The abg r_2 aaa.
		/// </summary>
		public const byte ABGR_2AAA = 0x10; // !< Unpalettised 32-bit = 0x;alpha.

		/// <summary>
		/// The abg r_4444.
		/// </summary>
		public const byte ABGR_4444 = 6; // !< Unpalettised 16-bit = ;alpha.

		/// <summary>
		/// The abg r_6666.
		/// </summary>
		public const byte ABGR_6666 = 0xC; // !< Unpalettised 24-bit = 0x;alpha.

		/// <summary>
		/// The abg r_8888.
		/// </summary>
		public const byte ABGR_8888 = 0xE; // !< Unpalettised 32-bit = 0x;alpha.

		/// <summary>
		/// The alph a_ f.
		/// </summary>
		public const uint ALPHA_F = 1 << 5; // if set, has alpha (i.e. RGBA or ABGR).

		/// <summary>
		/// The alph a_ fli p_ f.
		/// </summary>
		public const uint ALPHA_FLIP_F = 1 << 10;

		/// <summary>
		/// The arg b_8888.
		/// </summary>
		public const byte ARGB_8888 = 0x28; // !< Unpalettised 32-bit = 0x;alpha.

		/// <summary>
		/// The atitc.
		/// </summary>
		public const byte ATITC = 0x22; // !< ATI compressed format.

		/// <summary>
		/// The a_8.
		/// </summary>
		public const byte A_8 = 0x26; // !< Unpalettised 8-bit alpha.

		/// <summary>
		/// The bg r_332.
		/// </summary>
		public const byte BGR_332 = 2; // !< Unpalettised 8-bit.

		/// <summary>
		/// The bg r_565.
		/// </summary>
		public const byte BGR_565 = 4; // !< Unpalettised 16-bit = ;no alpha.

		/// <summary>
		/// The bg r_888.
		/// </summary>
		public const byte BGR_888 = 0xA; // !< Unpalettised 24-bit = 0x;no alpha.

		/// <summary>
		/// The compressed.
		/// </summary>
		public const byte COMPRESSED = 0x23; // !< gfx specific compressed format

		/// <summary>
		/// The dx t 1.
		/// </summary>
		public const byte DXT1 = 0x34; // !< DXT1 compressed format

		/// <summary>
		/// The dx t 3.
		/// </summary>
		public const byte DXT3 = 0x2B; // !< DXT3 compressed format

		/// <summary>
		/// The dx t 5.
		/// </summary>
		public const byte DXT5 = 0x35; // !< DXT5 compressed format

		/// <summary>
		/// The etc.
		/// </summary>
		public const byte ETC = 0x27; // !< Ericsson compressed format

		/// <summary>
		/// The forma t_ max.
		/// </summary>
		public const byte FORMAT_MAX = 0x36; // (Terminator)

		/// <summary>
		/// The forma t_ undefined.
		/// </summary>
		public const byte FORMAT_UNDEFINED = 0; // !< Format is undefined.

		/// <summary>
		/// The no n_ palett e_ alph a_ f.
		/// </summary>
		public const uint NON_PALETTE_ALPHA_F = 1 << 11;

		// if set, has alpha but stored in texels not in palette (for DS A5I3 and A3I5)

		/// <summary>
		/// The palett e 4_ abg r_1555.
		/// </summary>
		public const byte PALETTE4_ABGR_1555 = 0x16;

		/// <summary>
		/// The palett e 4_ abg r_4444.
		/// </summary>
		public const byte PALETTE4_ABGR_4444 = 0x24; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ abg r_8888.
		/// </summary>
		public const byte PALETTE4_ABGR_8888 = 0x31; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ arg b_8888.
		/// </summary>
		public const byte PALETTE4_ARGB_8888 = 0x29; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ bg r 555.
		/// </summary>
		public const byte PALETTE4_BGR555 = 0x2C; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ bg r_565.
		/// </summary>
		public const byte PALETTE4_BGR_565 = 0x30; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ rgb a_4444.
		/// </summary>
		public const byte PALETTE4_RGBA_4444 = 0x14; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ rgb a_5551.
		/// </summary>
		public const byte PALETTE4_RGBA_5551 = 0x15; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ rgb a_8888.
		/// </summary>
		public const byte PALETTE4_RGBA_8888 = 0x12; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ rg b_565.
		/// </summary>
		public const byte PALETTE4_RGB_565 = 0x13; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 4_ rg b_888.
		/// </summary>
		public const byte PALETTE4_RGB_888 = 0x11; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 5_ abg r_1555.
		/// </summary>
		public const byte PALETTE5_ABGR_1555 = 0x1F; // !< 128-colour palettised.

		/// <summary>
		/// The palett e 6_ abg r_1555.
		/// </summary>
		public const byte PALETTE6_ABGR_1555 = 0x1E; // !< 64-colour palettised.

		/// <summary>
		/// The palett e 7_ abg r_1555.
		/// </summary>
		public const byte PALETTE7_ABGR_1555 = 0x1D; // !< 32-colour palettised.

		/// <summary>
		/// The palett e 8_ abg r_1555.
		/// </summary>
		public const byte PALETTE8_ABGR_1555 = 0x1C;

		// PVRTC formats

		/// <summary>
		/// The palett e 8_ abg r_4444.
		/// </summary>
		public const byte PALETTE8_ABGR_4444 = 0x25; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ abg r_8888.
		/// </summary>
		public const byte PALETTE8_ABGR_8888 = 0x33; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ arg b_8888.
		/// </summary>
		public const byte PALETTE8_ARGB_8888 = 0x2A; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ bg r 555.
		/// </summary>
		public const byte PALETTE8_BGR555 = 0x2D; // !< 16-colour palettised.

		/// <summary>
		/// The palett e 8_ bg r_565.
		/// </summary>
		public const byte PALETTE8_BGR_565 = 0x32; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ rgb a_4444.
		/// </summary>
		public const byte PALETTE8_RGBA_4444 = 0x1A; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ rgb a_5551.
		/// </summary>
		public const byte PALETTE8_RGBA_5551 = 0x1B; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ rgb a_8888.
		/// </summary>
		public const byte PALETTE8_RGBA_8888 = 0x18; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ rg b_565.
		/// </summary>
		public const byte PALETTE8_RGB_565 = 0x19; // !< 256-colour palettised.

		/// <summary>
		/// The palett e 8_ rg b_888.
		/// </summary>
		public const byte PALETTE8_RGB_888 = 0x17; // !< 256-colour palettised.

		/// <summary>
		/// The palettise d_4 bi t_ f.
		/// </summary>
		public const uint PALETTISED_4BIT_F = 1 << 0;

		/// <summary>
		/// The palettise d_5 bi t_ f.
		/// </summary>
		public const uint PALETTISED_5BIT_F = 1 << 1;

		/// <summary>
		/// The palettise d_6 bi t_ f.
		/// </summary>
		public const uint PALETTISED_6BIT_F = 1 << 2;

		/// <summary>
		/// The palettise d_7 bi t_ f.
		/// </summary>
		public const uint PALETTISED_7BIT_F = 1 << 3;

		/// <summary>
		/// The palettise d_8 bi t_ f.
		/// </summary>
		public const uint PALETTISED_8BIT_F = 1 << 4;

		/// <summary>
		/// The palettise d_ mask.
		/// </summary>
		public const uint PALETTISED_MASK =
			PALETTISED_4BIT_F | PALETTISED_5BIT_F | PALETTISED_6BIT_F | PALETTISED_7BIT_F | PALETTISED_8BIT_F;

		/// <summary>
		/// The pvrt c_2.
		/// </summary>
		public const byte PVRTC_2 = 0x20; // !< PowerVR compressed format.

		/// <summary>
		/// The pvrt c_4.
		/// </summary>
		public const byte PVRTC_4 = 0x21; // !< PowerVR compressed format.

		/// <summary>
		/// The revers e_ f.
		/// </summary>
		public const uint REVERSE_F = 1 << 6; // if set, is reverse order (i.e. BGR or ABGR).

		/// <summary>
		/// The rgb a_4444.
		/// </summary>
		public const byte RGBA_4444 = 5; // !< Unpalettised 16-bit = ;alpha.

		/// <summary>
		/// The rgb a_5551.
		/// </summary>
		public const byte RGBA_5551 = 7; // !< Unpalettised 16-bit = ;alpha.

		/// <summary>
		/// The rgb a_6666.
		/// </summary>
		public const byte RGBA_6666 = 0xB; // !< Unpalettised 24-bit = 0x;alpha.

		/// <summary>
		/// The rgb a_8888.
		/// </summary>
		public const byte RGBA_8888 = 0xD; // !< Unpalettised 32-bit = 0x;alpha.

		/// <summary>
		/// The rgb a_ aa a 2.
		/// </summary>
		public const byte RGBA_AAA2 = 0xF; // !< Unpalettised 32-bit = 0x;alpha.

		/// <summary>
		/// The rg b_332.
		/// </summary>
		public const byte RGB_332 = 1; // !< Unpalettised 8-bit.

		/// <summary>
		/// The rg b_565.
		/// </summary>
		public const byte RGB_565 = 3; // !< Unpalettised 16-bit = ;no alpha.

		/// <summary>
		/// The rg b_888.
		/// </summary>
		public const byte RGB_888 = 9; // !< Unpalettised 24-bit = ;no alpha.

		/// <summary>
		/// The siz e_16_ f.
		/// </summary>
		public const uint SIZE_16_F = 2 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		/// <summary>
		/// The siz e_24_ f.
		/// </summary>
		public const uint SIZE_24_F = 3 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		/// <summary>
		/// The siz e_32_ f.
		/// </summary>
		public const uint SIZE_32_F = 4 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		/// <summary>
		/// The siz e_8_ f.
		/// </summary>
		public const uint SIZE_8_F = 1 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		/// <summary>
		/// The siz e_ mask.
		/// </summary>
		public const uint SIZE_MASK = 0x7 << 7;

		public ushort Flags;

		public byte[] data;

		public ushort height;

		public uint palette;

		public ushort pitch;

		public ushort width;

		#endregion

		#region Constructors and Destructors

		public Image()
		{
		}

		public Image(Bitmap bitmap)
		{
			this.width = (ushort)bitmap.Width;
			this.height = (ushort)bitmap.Height;
			this.Format = BGR_888;
			BitmapData data = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			this.data = new byte[this.width * this.height * 3];
			Marshal.Copy(data.Scan0, this.data, 0, this.data.Length);
			bitmap.UnlockBits(data);
		}

		#endregion

		#region Public Properties

		public byte Format { get; set; }

		#endregion

		#region Public Methods and Operators

		public void OpenGLUpload()
		{
			OpenTK.Graphics.OpenGL.PixelFormat pixelFormat;
			PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba;
			switch (this.Format)
			{
				case RGB_888:
					pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
					break;
				case BGR_888:
					pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
					break;
				default:
					throw new FormatException("TODO: convert image to device compatible format");
			}
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				pixelInternalFormat,
				this.width,
				this.height,
				0,
				pixelFormat,
				PixelType.UnsignedByte,
				this.data);
			OpenTKHelper.Assert();
		}

		#endregion
	}
}