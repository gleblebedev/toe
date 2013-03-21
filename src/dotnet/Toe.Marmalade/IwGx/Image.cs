using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Toe.Marmalade.IwGx
{
	public class Image
	{
		//#region Constants and Fields

		///// <summary>
		///// The a 3_ palett e 5_ bg r_555.
		///// </summary>
		//public const byte A3_PALETTE5_BGR_555 = 0x2F; // !< 8BPP = 0x;of which 3 are alpha and 5 are palette index

		///// <summary>
		///// The a 5_ palett e 3_ bg r_555.
		///// </summary>
		//public const byte A5_PALETTE3_BGR_555 = 0x2E; // !< 8BPP = 0x;of which 5 are alpha and 3 are palette index

		///// <summary>
		///// The abg r_1555.
		///// </summary>
		//public const byte ABGR_1555 = 8; // !< Unpalettised 16-bit = ;alpha.

		///// <summary>
		///// The abg r_2 aaa.
		///// </summary>
		//public const byte ABGR_2AAA = 0x10; // !< Unpalettised 32-bit = 0x;alpha.

		///// <summary>
		///// The abg r_4444.
		///// </summary>
		//public const byte ABGR_4444 = 6; // !< Unpalettised 16-bit = ;alpha.

		///// <summary>
		///// The abg r_6666.
		///// </summary>
		//public const byte ABGR_6666 = 0xC; // !< Unpalettised 24-bit = 0x;alpha.

		///// <summary>
		///// The abg r_8888.
		///// </summary>
		//public const byte ABGR_8888 = 0xE; // !< Unpalettised 32-bit = 0x;alpha.

		///// <summary>
		///// The alph a_ f.
		///// </summary>
		//public const uint ALPHA_F = 1 << 5; // if set, has alpha (i.e. RGBA or ABGR).

		///// <summary>
		///// The alph a_ fli p_ f.
		///// </summary>
		//public const uint ALPHA_FLIP_F = 1 << 10;

		///// <summary>
		///// The arg b_8888.
		///// </summary>
		//public const byte ARGB_8888 = 0x28; // !< Unpalettised 32-bit = 0x;alpha.

		///// <summary>
		///// The atitc.
		///// </summary>
		//public const byte ATITC = 0x22; // !< ATI compressed format.

		///// <summary>
		///// The a_8.
		///// </summary>
		//public const byte A_8 = 0x26; // !< Unpalettised 8-bit alpha.

		///// <summary>
		///// The bg r_332.
		///// </summary>
		//public const byte BGR_332 = 2; // !< Unpalettised 8-bit.

		///// <summary>
		///// The bg r_565.
		///// </summary>
		//public const byte BGR_565 = 4; // !< Unpalettised 16-bit = ;no alpha.

		///// <summary>
		///// The bg r_888.
		///// </summary>
		//public const byte BGR_888 = 0xA; // !< Unpalettised 24-bit = 0x;no alpha.

		///// <summary>
		///// The compressed.
		///// </summary>
		//public const byte COMPRESSED = 0x23; // !< gfx specific compressed format

		///// <summary>
		///// The dx t 1.
		///// </summary>
		//public const byte DXT1 = 0x34; // !< DXT1 compressed format

		///// <summary>
		///// The dx t 3.
		///// </summary>
		//public const byte DXT3 = 0x2B; // !< DXT3 compressed format

		///// <summary>
		///// The dx t 5.
		///// </summary>
		//public const byte DXT5 = 0x35; // !< DXT5 compressed format

		///// <summary>
		///// The etc.
		///// </summary>
		//public const byte ETC = 0x27; // !< Ericsson compressed format

		///// <summary>
		///// The forma t_ max.
		///// </summary>
		//public const byte FORMAT_MAX = 0x36; // (Terminator)

		///// <summary>
		///// The forma t_ undefined.
		///// </summary>
		//public const byte FORMAT_UNDEFINED = 0; // !< Format is undefined.

		///// <summary>
		///// The no n_ palett e_ alph a_ f.
		///// </summary>
		//public const uint NON_PALETTE_ALPHA_F = 1 << 11;

		//// if set, has alpha but stored in texels not in palette (for DS A5I3 and A3I5)

		///// <summary>
		///// The palett e 4_ abg r_1555.
		///// </summary>
		//public const byte PALETTE4_ABGR_1555 = 0x16;

		///// <summary>
		///// The palett e 4_ abg r_4444.
		///// </summary>
		//public const byte PALETTE4_ABGR_4444 = 0x24; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ abg r_8888.
		///// </summary>
		//public const byte PALETTE4_ABGR_8888 = 0x31; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ arg b_8888.
		///// </summary>
		//public const byte PALETTE4_ARGB_8888 = 0x29; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ bg r 555.
		///// </summary>
		//public const byte PALETTE4_BGR555 = 0x2C; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ bg r_565.
		///// </summary>
		//public const byte PALETTE4_BGR_565 = 0x30; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ rgb a_4444.
		///// </summary>
		//public const byte PALETTE4_RGBA_4444 = 0x14; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ rgb a_5551.
		///// </summary>
		//public const byte PALETTE4_RGBA_5551 = 0x15; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ rgb a_8888.
		///// </summary>
		//public const byte PALETTE4_RGBA_8888 = 0x12; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ rg b_565.
		///// </summary>
		//public const byte PALETTE4_RGB_565 = 0x13; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 4_ rg b_888.
		///// </summary>
		//public const byte PALETTE4_RGB_888 = 0x11; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 5_ abg r_1555.
		///// </summary>
		//public const byte PALETTE5_ABGR_1555 = 0x1F; // !< 128-colour palettised.

		///// <summary>
		///// The palett e 6_ abg r_1555.
		///// </summary>
		//public const byte PALETTE6_ABGR_1555 = 0x1E; // !< 64-colour palettised.

		///// <summary>
		///// The palett e 7_ abg r_1555.
		///// </summary>
		//public const byte PALETTE7_ABGR_1555 = 0x1D; // !< 32-colour palettised.

		///// <summary>
		///// The palett e 8_ abg r_1555.
		///// </summary>
		//public const byte PALETTE8_ABGR_1555 = 0x1C;

		//// PVRTC formats

		///// <summary>
		///// The palett e 8_ abg r_4444.
		///// </summary>
		//public const byte PALETTE8_ABGR_4444 = 0x25; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ abg r_8888.
		///// </summary>
		//public const byte PALETTE8_ABGR_8888 = 0x33; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ arg b_8888.
		///// </summary>
		//public const byte PALETTE8_ARGB_8888 = 0x2A; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ bg r 555.
		///// </summary>
		//public const byte PALETTE8_BGR555 = 0x2D; // !< 16-colour palettised.

		///// <summary>
		///// The palett e 8_ bg r_565.
		///// </summary>
		//public const byte PALETTE8_BGR_565 = 0x32; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ rgb a_4444.
		///// </summary>
		//public const byte PALETTE8_RGBA_4444 = 0x1A; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ rgb a_5551.
		///// </summary>
		//public const byte PALETTE8_RGBA_5551 = 0x1B; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ rgb a_8888.
		///// </summary>
		//public const byte PALETTE8_RGBA_8888 = 0x18; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ rg b_565.
		///// </summary>
		//public const byte PALETTE8_RGB_565 = 0x19; // !< 256-colour palettised.

		///// <summary>
		///// The palett e 8_ rg b_888.
		///// </summary>
		//public const byte PALETTE8_RGB_888 = 0x17; // !< 256-colour palettised.

		///// <summary>
		///// The palettise d_4 bi t_ f.
		///// </summary>
		//public const uint PALETTISED_4BIT_F = 1 << 0;

		///// <summary>
		///// The palettise d_5 bi t_ f.
		///// </summary>
		//public const uint PALETTISED_5BIT_F = 1 << 1;

		///// <summary>
		///// The palettise d_6 bi t_ f.
		///// </summary>
		//public const uint PALETTISED_6BIT_F = 1 << 2;

		///// <summary>
		///// The palettise d_7 bi t_ f.
		///// </summary>
		//public const uint PALETTISED_7BIT_F = 1 << 3;

		///// <summary>
		///// The palettise d_8 bi t_ f.
		///// </summary>
		//public const uint PALETTISED_8BIT_F = 1 << 4;

		///// <summary>
		///// The palettise d_ mask.
		///// </summary>
		//public const uint PALETTISED_MASK =
		//    PALETTISED_4BIT_F | PALETTISED_5BIT_F | PALETTISED_6BIT_F | PALETTISED_7BIT_F | PALETTISED_8BIT_F;

		///// <summary>
		///// The pvrt c_2.
		///// </summary>
		//public const byte PVRTC_2 = 0x20; // !< PowerVR compressed format.

		///// <summary>
		///// The pvrt c_4.
		///// </summary>
		//public const byte PVRTC_4 = 0x21; // !< PowerVR compressed format.

		///// <summary>
		///// The revers e_ f.
		///// </summary>
		//public const uint REVERSE_F = 1 << 6; // if set, is reverse order (i.e. BGR or ABGR).

		///// <summary>
		///// The rgb a_4444.
		///// </summary>
		//public const byte RGBA_4444 = 5; // !< Unpalettised 16-bit = ;alpha.

		///// <summary>
		///// The rgb a_5551.
		///// </summary>
		//public const byte RGBA_5551 = 7; // !< Unpalettised 16-bit = ;alpha.

		///// <summary>
		///// The rgb a_6666.
		///// </summary>
		//public const byte RGBA_6666 = 0xB; // !< Unpalettised 24-bit = 0x;alpha.

		///// <summary>
		///// The rgb a_8888.
		///// </summary>
		//public const byte RGBA_8888 = 0xD; // !< Unpalettised 32-bit = 0x;alpha.

		///// <summary>
		///// The rgb a_ aa a 2.
		///// </summary>
		//public const byte RGBA_AAA2 = 0xF; // !< Unpalettised 32-bit = 0x;alpha.

		///// <summary>
		///// The rg b_332.
		///// </summary>
		//public const byte RGB_332 = 1; // !< Unpalettised 8-bit.

		///// <summary>
		///// The rg b_565.
		///// </summary>
		//public const byte RGB_565 = 3; // !< Unpalettised 16-bit = ;no alpha.

		///// <summary>
		///// The rg b_888.
		///// </summary>
		//public const byte RGB_888 = 9; // !< Unpalettised 24-bit = ;no alpha.

		///// <summary>
		///// The siz e_16_ f.
		///// </summary>
		//public const uint SIZE_16_F = 2 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		///// <summary>
		///// The siz e_24_ f.
		///// </summary>
		//public const uint SIZE_24_F = 3 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		///// <summary>
		///// The siz e_32_ f.
		///// </summary>
		//public const uint SIZE_32_F = 4 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		///// <summary>
		///// The siz e_8_ f.
		///// </summary>
		//public const uint SIZE_8_F = 1 << 7; // Size of texel (for unpalettised) or palette entry (for palettised)

		///// <summary>
		///// The siz e_ mask.
		///// </summary>
		//public const uint SIZE_MASK = 0x7 << 7;

		#region Constants and Fields

		public ushort Flags;

		public byte[] data;

		public ushort height;

		public uint palette;

		public int pitch;

		public ushort width;

		private ImageFormat format;

		#endregion

		//#endregion

		#region Constructors and Destructors

		public Image()
		{
		}

		public Image(ushort w, ushort h, int p, ImageFormat format, byte[] data)
		{
			this.width = w;
			this.height = h;
			this.pitch = p;
			this.format = format;
			this.data = data;
		}

		public Image(Bitmap bitmap)
		{
			this.width = (ushort)bitmap.Width;
			this.height = (ushort)bitmap.Height;
			this.pitch = this.width * 4;
			this.format = ImageFormat.ABGR_8888;
			BitmapData data = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			this.data = new byte[this.pitch * this.height];
			Marshal.Copy(data.Scan0, this.data, 0, this.data.Length);
			for (int i = 0; i < this.data.Length; i += 4)
			{
				byte b = this.data[i];
				this.data[i] = this.data[i + 2];
				this.data[i + 2] = b;
			}
			bitmap.UnlockBits(data);
		}

		#endregion

		#region Public Properties

		public ImageFormat Format
		{
			get
			{
				return this.format;
			}
			set
			{
				if (this.format != value)
				{
					this.format = value;
				}
			}
		}

		public byte[] PaletteData { get; set; }

		#endregion

		#region Public Methods and Operators

		public Image ConvertToAbgr8888()
		{
			var image = new Image();
			image.height = this.height;
			image.width = this.width;
			image.format = ImageFormat.ABGR_8888;
			image.pitch = this.width * 4;
			image.data = new byte[image.pitch * image.height];
			switch (this.format)
			{
				case ImageFormat.FORMAT_UNDEFINED:
					throw new FormatException("Can't convert undefined format");
					//case ImageFormat.RGB_332:
					//    break;
					//case ImageFormat.BGR_332:
					//    break;
					//case ImageFormat.RGB_565:
					//    break;
					//case ImageFormat.BGR_565:
					//    break;
					//case ImageFormat.RGBA_4444:
					//    break;
					//case ImageFormat.ABGR_4444:
					//    break;
					//case ImageFormat.RGBA_5551:
					//    break;
					//case ImageFormat.ABGR_1555:
					//    break;
					//case ImageFormat.RGB_888:
					//    break;
					//case ImageFormat.BGR_888:
					//    break;
					//case ImageFormat.RGBA_6666:
					//    break;
					//case ImageFormat.ABGR_6666:
					//    break;
					//case ImageFormat.RGBA_8888:
					//    break;
					//case ImageFormat.ABGR_8888:
					//    break;
					//case ImageFormat.RGBA_AAA2:
					//    break;
					//case ImageFormat.ABGR_2AAA:
					//    break;
				case ImageFormat.PALETTE4_RGB_888:
					this.Palette4Rgb888ToAbgr8888(image);
					break;
					//case ImageFormat.PALETTE4_RGBA_8888:
					//    break;
					//case ImageFormat.PALETTE4_RGB_565:
					//    break;
					//case ImageFormat.PALETTE4_RGBA_4444:
					//    break;
					//case ImageFormat.PALETTE4_RGBA_5551:
					//    break;
					//case ImageFormat.PALETTE4_ABGR_1555:
					//    break;
				case ImageFormat.PALETTE8_RGB_888:
					this.Palette8Rgb888ToAbgr8888(image);
					break;
				case ImageFormat.PALETTE8_RGBA_8888:
					this.Palette8Rgba8888ToAbgr8888(image);
					break;
					//case ImageFormat.PALETTE8_RGB_565:
					//    break;
					//case ImageFormat.PALETTE8_RGBA_4444:
					//    break;
					//case ImageFormat.PALETTE8_RGBA_5551:
					//    break;
					//case ImageFormat.PALETTE8_ABGR_1555:
					//    break;
					//case ImageFormat.PALETTE7_ABGR_1555:
					//    break;
					//case ImageFormat.PALETTE6_ABGR_1555:
					//    break;
					//case ImageFormat.PALETTE5_ABGR_1555:
					//    break;
					//case ImageFormat.PVRTC_2:
					//    break;
					//case ImageFormat.PVRTC_4:
					//    break;
					//case ImageFormat.ATITC:
					//    break;
					//case ImageFormat.COMPRESSED:
					//    break;
					//case ImageFormat.PALETTE4_ABGR_4444:
					//    break;
					//case ImageFormat.PALETTE8_ABGR_4444:
					//    break;
					//case ImageFormat.A_8:
					//    break;
					//case ImageFormat.ETC:
					//    break;
					//case ImageFormat.ARGB_8888:
					//    break;
					//case ImageFormat.PALETTE4_ARGB_8888:
					//    break;
					//case ImageFormat.PALETTE8_ARGB_8888:
					//    break;
					//case ImageFormat.DXT3:
					//    break;
					//case ImageFormat.PALETTE4_BGR555:
					//    break;
					//case ImageFormat.PALETTE8_BGR555:
					//    break;
					//case ImageFormat.A5_PALETTE3_BGR_555:
					//    break;
					//case ImageFormat.A3_PALETTE5_BGR_555:
					//    break;
					//case ImageFormat.PALETTE4_BGR_565:
					//    break;
					//case ImageFormat.PALETTE4_ABGR_8888:
					//    break;
					//case ImageFormat.PALETTE8_BGR_565:
					//    break;
					//case ImageFormat.PALETTE8_ABGR_8888:
					//    break;
					//case ImageFormat.DXT1:
					//    break;
					//case ImageFormat.DXT5:
					//    break;
					//case ImageFormat.FORMAT_MAX:
					//    break;
				default:
					throw new FormatException("Can't convert to ABGR 8888");
			}
			return image;
		}

		public void FlipVerticaly()
		{
			if (this.data == null)
			{
				return;
			}
			byte[] row = new byte[this.pitch];
			for (int i = 0; i < this.height / 2; ++i)
			{
				Array.Copy(this.data, i * this.pitch, row, 0, this.pitch);
				Array.Copy(this.data, (this.height - 1 - i) * this.pitch, this.data, i * this.pitch, this.pitch);
				Array.Copy(row, 0, this.data, (this.height - 1 - i) * this.pitch, this.pitch);
			}
		}

		#endregion

		#region Methods

		private void Palette4Rgb888ToAbgr8888(Image image)
		{
			for (var y = 0; y < this.height; ++y)
			{
				int dstIndex = image.pitch * y;
				int srcIndex = this.pitch * y;
				for (var x = 0; x < this.width; x += 2)
				{
					byte byteIndex = this.data[srcIndex];
					++srcIndex;
					var i = (byteIndex >> 4) * 3;
					image.data[dstIndex++] = this.PaletteData[i + 2];
					image.data[dstIndex++] = this.PaletteData[i + 1];
					image.data[dstIndex++] = this.PaletteData[i + 0];
					image.data[dstIndex++] = 255;
					i = (byteIndex & 0x0F) * 3;
					image.data[dstIndex++] = this.PaletteData[i + 2];
					image.data[dstIndex++] = this.PaletteData[i + 1];
					image.data[dstIndex++] = this.PaletteData[i + 0];
					image.data[dstIndex++] = 255;
				}
			}
		}

		private void Palette8Rgb888ToAbgr8888(Image image)
		{
			for (var y = 0; y < this.height; ++y)
			{
				int dstIndex = image.pitch * y;
				int srcIndex = this.pitch * y;
				for (var x = 0; x < this.width; ++x)
				{
					var i = this.data[srcIndex] * 3;
					++srcIndex;
					image.data[dstIndex++] = this.PaletteData[i + 2];
					image.data[dstIndex++] = this.PaletteData[i + 1];
					image.data[dstIndex++] = this.PaletteData[i + 0];
					image.data[dstIndex++] = 255;
				}
			}
		}

		private void Palette8Rgba8888ToAbgr8888(Image image)
		{
			for (var y = 0; y < this.height; ++y)
			{
				int dstIndex = image.pitch * y;
				int srcIndex = this.pitch * y;
				for (var x = 0; x < this.width; ++x)
				{
					var i = this.data[srcIndex] * 4;
					++srcIndex;
					image.data[dstIndex++] = this.PaletteData[i + 3];
					image.data[dstIndex++] = this.PaletteData[i + 2];
					image.data[dstIndex++] = this.PaletteData[i + 1];
					image.data[dstIndex++] = this.PaletteData[i + 0];
				}
			}
		}

		#endregion
	}
}