using System;
using System.Globalization;

using Autofac;

using OpenTK;

using Toe.Marmalade.IwGx;

namespace Toe.Marmalade.BinaryFiles.IwGx
{
	public class TextureBinarySerializer : IBinarySerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public TextureBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var texture = this.context.Resolve<Texture>();
			texture.NameHash = parser.ConsumeUInt32();
			texture.Flags = parser.ConsumeUInt32();
			texture.FormatSW = (ImageFormat)parser.ConsumeByte();
			texture.FormatHW = (ImageFormat)parser.ConsumeByte();

			float x = parser.ConsumeFloat();
			float y = parser.ConsumeFloat();
			texture.UVScale = new Vector2(x, y);

			texture.Image = this.ParseImage(parser);

			var e = parser.ConsumeBool();
			//parser.Expect(false);

			return texture;
		}

		#endregion

		#region Methods

		private Image ParseImage(BinaryParser parser)
		{
			var image = new Image();

			image.Format = (ImageFormat)parser.ConsumeUInt8();

			image.Flags = parser.ConsumeUInt16();

			image.width = parser.ConsumeUInt16();
			image.height = parser.ConsumeUInt16();
			image.pitch = parser.ConsumeUInt16();
			image.palette = parser.ConsumeUInt32();

			byte[] d = new byte[image.height * image.pitch];
			parser.ConsumeArray(d);
			image.data = d;

			switch (image.Format)
			{
				case ImageFormat.ABGR_8888:
				case ImageFormat.BGR_888:
				case ImageFormat.RGB_888:
					return image;
					//case Image.PALETTE4_ABGR_1555:
					//    format = (new Palette4Abgr1555(image.width, image.height, image.pitch));
					//    Debug.WriteLine(string.Format("Image PALETTE4_ABGR_1555 {0}x{1}", image.width, image.height));
					//    break;
					//case Image.PALETTE4_RGB_888:
					//    format = (new Palette4Rgb888(image.width, image.height, image.pitch));
					//    Debug.WriteLine(string.Format("Image PALETTE4_RGB_888 {0}x{1}", image.width, image.height));
					//    break;
					//case Image.PALETTE8_ABGR_1555:
					//    format = (new Palette8Abgr1555(image.width, image.height, image.pitch));
					//    Debug.WriteLine(string.Format("Image PALETTE8_ABGR_1555 {0}x{1}", image.width, image.height));
					//    break;
					//case Image.ABGR_1555:
					//    Debug.WriteLine(string.Format("Image ABGR_1555 {0}x{1}", image.width, image.height));
					//    LoadABGR1555(serialise);
					//    return;
					//case Image.RGBA_6666:
					//    Debug.WriteLine(string.Format("Image RGBA_6666 {0}x{1}", image.width, image.height));
					//    LoadRgba6666(serialise);
					//    return;
				case ImageFormat.PALETTE8_RGB_888:
					image.PaletteData = parser.ConsumeByteArray(256 * 3);
					return image;
				case ImageFormat.PALETTE8_ABGR_8888:
				case ImageFormat.PALETTE8_ARGB_8888:
				case ImageFormat.PALETTE8_RGBA_8888:
					image.PaletteData = parser.ConsumeByteArray(256 * 4);
					return image;
				case ImageFormat.PALETTE4_RGB_888:
					image.PaletteData = parser.ConsumeByteArray(16 * 3);
					return image;
				case ImageFormat.PALETTE4_ABGR_8888:
				case ImageFormat.PALETTE4_ARGB_8888:
				case ImageFormat.PALETTE4_RGBA_8888:
					image.PaletteData = parser.ConsumeByteArray(16 * 4);
					return image;
				default:
					throw new FormatException(string.Format(CultureInfo.CurrentCulture, "Unknown image format 0x{0:x}", image.Format));
			}
		}

		#endregion

		//private void Load256ColourPalettised(IwSerialise serialise)
		//{
		//    this.data = new byte[this.height * this.pitch];
		//    serialise.Serialise(ref this.data);

		//    this.palette = new Color[256];
		//    for (int i = 0; i < this.palette.Length; ++i)
		//    {
		//        byte r = 0;
		//        byte g = 0;
		//        byte b = 0;
		//        serialise.UInt8(ref r);
		//        serialise.UInt8(ref g);
		//        serialise.UInt8(ref b);
		//        this.palette[i] = Color.FromArgb(r, g, b);
		//    }

		//    byte[] d = new byte[this.height * this.width * 3];
		//    int j = 0;
		//    for (int y = 0; y < this.height; ++y)
		//    {
		//        for (int x = 0; x < this.width; ++x)
		//        {
		//            d[j] = this.palette[this.data[x + y * this.pitch]].R;
		//            ++j;
		//            d[j] = this.palette[this.data[x + y * this.pitch]].G;
		//            ++j;
		//            d[j] = this.palette[this.data[x + y * this.pitch]].B;
		//            ++j;
		//        }
		//    }

		//    this.data = d;

		//    ////using (var b = new Bitmap(width,height))
		//    ////{
		//    ////        for (int i = 0; i < height;++i)
		//    ////            for (int x = 0; x < width; ++x)
		//    ////            {
		//    ////                b.SetPixel(x,i,palette[data[x+i*pitch]]);
		//    ////            }
		//    ////    b.Save("res.bmp");
		//    ////}
		//}

		//private void LoadABGR1555(IwSerialise serialise)
		//{
		//    var d = new ushort[this.height * this.pitch / 2];
		//    serialise.Serialise(ref d);

		//    this.data = new byte[this.width * this.height * 3];
		//    int dst = 0, src = 0;
		//    for (int y = 0; y < this.height; ++y)
		//    {
		//        for (int x = 0; x < this.width; ++x)
		//        {
		//            var r = d[src];
		//            ++src;
		//            this.data[dst] = (byte)(((r >> 0) & 0x1F) << 3);
		//            ++dst;
		//            this.data[dst] = (byte)(((r >> 5) & 0x1F) << 3);
		//            ++dst;
		//            this.data[dst] = (byte)(((r >> 10) & 0x1F) << 3);
		//            ++dst;
		//        }
		//    }
		//}

		//private void LoadPaletteABGR1555(IwSerialise serialise)
		//{
		//    this.data = new byte[this.height * this.pitch];
		//    serialise.Serialise(ref this.data);

		//    this.palette = new Color[256];
		//    for (int i = 0; i < this.palette.Length; ++i)
		//    {
		//        ushort r = 0;
		//        serialise.UInt16(ref r);
		//        this.palette[i] = Color.FromArgb(
		//            (byte)(((r >> 10) & 0x1F) << 3), (byte)(((r >> 5) & 0x1F) << 3), (byte)(((r >> 0) & 0x1F) << 3));
		//    }

		//    byte[] d = new byte[this.height * this.width * 3];
		//    int j = 0;
		//    for (int y = 0; y < this.height; ++y)
		//    {
		//        for (int x = 0; x < this.width; ++x)
		//        {
		//            d[j] = this.palette[this.data[x + y * this.pitch]].R;
		//            ++j;
		//            d[j] = this.palette[this.data[x + y * this.pitch]].G;
		//            ++j;
		//            d[j] = this.palette[this.data[x + y * this.pitch]].B;
		//            ++j;
		//        }
		//    }

		//    this.data = d;
		//}
	}
}