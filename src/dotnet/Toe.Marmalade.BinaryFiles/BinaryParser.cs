using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Toe.Marmalade.BinaryFiles
{
	public class BinaryParser
	{
		#region Constants and Fields

		private readonly string basePath;

		private readonly BinaryReader source;

		#endregion

		#region Constructors and Destructors

		public BinaryParser(BinaryReader source, string basePath)
		{
			this.source = source;
			this.basePath = basePath;
		}

		public string BasePath
		{
			get
			{
				return this.basePath;
			}
		}

		public long Position
		{
			get
			{
				return this.source.BaseStream.Position;
			}
			set
			{
				this.source.BaseStream.Position = value;
			}
		}

		#endregion

		public byte ConsumeByte()
		{
			return this.source.ReadByte();
		}

		public ushort ConsumeUInt16()
		{
			return this.source.ReadUInt16();
		}
		public uint PeekUInt32()
		{
			var pos = this.source.BaseStream.Position;
			var res = this.source.ReadUInt32();
			this.source.BaseStream.Position = pos;
			return res;
		}

		public uint ConsumeUInt32()
		{
			return this.source.ReadUInt32();
		}

		public void Skip(uint len)
		{
			this.source.BaseStream.Position += len;
		}

		readonly List<byte> stringBuffer = new List<byte>(256);

		public string ConsumeStringZ()
		{
			this.stringBuffer.Clear();
			for (;;)
			{
				var b = this.ConsumeByte();
				if (b == 0)
					break;
				this.stringBuffer.Add(b);
			}
			return Encoding.UTF8.GetString(this.stringBuffer.ToArray());
		}

		public int ConsumeInt32()
		{
			return this.source.ReadInt32();
		}

		public bool ConsumeBool()
		{
			return this.ConsumeByte() != 0;
		}

		public short ConsumeInt16()
		{
			return this.source.ReadInt16();
		}
		public void Expect(byte val)
		{
			var v = this.ConsumeByte();
			if (v != val)
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
		}

		public void Expect(short val)
		{
			var v = this.ConsumeInt16();
			if (v != val)
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
		}

		public void Expect(ushort val)
		{
			var v = this.ConsumeUInt16();
			if (v != val)
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
		}

		public void Expect(bool val)
		{
			var v = this.ConsumeBool();
			if (v != val)
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
		}

		public byte ConsumeUInt8()
		{
			return this.ConsumeByte();
		}

		public void ConsumeArray(byte[] uints)
		{
			int pos = 0;
			while (pos < uints.Length)
			{
				var len = this.source.BaseStream.Read(uints, pos, uints.Length - pos);
				if (len <= 0)
					throw new FormatException();
				pos += len;
			}
		}

		public Color ConsumeColor()
		{
			var r = this.ConsumeByte();
			var g = this.ConsumeByte();
			var b = this.ConsumeByte();
			var a = this.ConsumeByte();
			return Color.FromArgb(a, r, g, b);
		}

		public void Expect(uint val)
		{
			var v = this.ConsumeUInt32();
			if (v != val)
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
		}

		public float ConsumeFloat()
		{
			return this.source.ReadSingle();
		}
	}
}