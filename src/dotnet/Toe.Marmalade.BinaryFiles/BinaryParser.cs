using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using OpenTK;

using Toe.Resources;

namespace Toe.Marmalade.BinaryFiles
{
	public class BinaryParser
	{
		#region Constants and Fields

		private readonly string basePath;

		private readonly IResourceFile resourceFile;

		private readonly BinaryReader source;

		private readonly List<byte> stringBuffer = new List<byte>(256);

		#endregion

		#region Constructors and Destructors

		public BinaryParser(BinaryReader source, string basePath, IResourceFile resourceFile)
		{
			this.source = source;
			this.basePath = basePath;
			this.resourceFile = resourceFile;
		}

		#endregion

		#region Public Properties

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

		public IResourceFile ResourceFile
		{
			get
			{
				return this.resourceFile;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void ConsumeArray(byte[] uints)
		{
			int pos = 0;
			while (pos < uints.Length)
			{
				var len = this.source.BaseStream.Read(uints, pos, uints.Length - pos);
				if (len <= 0)
				{
					throw new FormatException();
				}
				pos += len;
			}
		}

		public bool ConsumeBool()
		{
			return this.ConsumeByte() != 0;
		}

		public byte ConsumeByte()
		{
			return this.source.ReadByte();
		}

		public byte[] ConsumeByteArray(int size)
		{
			byte[] uints = new byte[size];
			this.ConsumeArray(uints);
			return uints;
		}

		public Color ConsumeColor()
		{
			var r = this.ConsumeByte();
			var g = this.ConsumeByte();
			var b = this.ConsumeByte();
			var a = this.ConsumeByte();
			return Color.FromArgb(a, r, g, b);
		}

		public float ConsumeFloat()
		{
			return this.source.ReadSingle();
		}

		public float[] ConsumeFloatArray(int size)
		{
			float[] res = new float[size];
			int pos = 0;
			while (pos < size)
			{
				res[pos] = this.source.ReadSingle();
				++pos;
			}
			return res;
		}

		public short ConsumeInt16()
		{
			return this.source.ReadInt16();
		}

		public int ConsumeInt32()
		{
			return this.source.ReadInt32();
		}

		public Quaternion ConsumeQuaternion()
		{
			float w = this.ConsumeFloat();
			float x = this.ConsumeFloat();
			float y = this.ConsumeFloat();
			float z = this.ConsumeFloat();
			return new Quaternion(x, y, z, w);
		}

		public sbyte ConsumeSByte()
		{
			return this.source.ReadSByte();
		}

		public string ConsumeStringZ()
		{
			this.stringBuffer.Clear();
			for (;;)
			{
				var b = this.ConsumeByte();
				if (b == 0)
				{
					break;
				}
				this.stringBuffer.Add(b);
			}
			return Encoding.UTF8.GetString(this.stringBuffer.ToArray());
		}

		public ushort ConsumeUInt16()
		{
			return this.source.ReadUInt16();
		}

		public ushort[] ConsumeUInt16Array(int size)
		{
			ushort[] res = new ushort[size];
			int pos = 0;
			while (pos < size)
			{
				res[pos] = this.source.ReadUInt16();
				++pos;
			}
			return res;
		}

		public uint ConsumeUInt32()
		{
			return this.source.ReadUInt32();
		}

		public byte ConsumeUInt8()
		{
			return this.ConsumeByte();
		}

		public Vector2 ConsumeVector2()
		{
			float x = this.ConsumeFloat();
			float y = this.ConsumeFloat();
			return new Vector2(x, y);
		}

		public Vector3 ConsumeVector3()
		{
			float x = this.ConsumeFloat();
			float y = this.ConsumeFloat();
			float z = this.ConsumeFloat();
			return new Vector3(x, y, z);
		}

		public Vector3[] ConsumeVector3Array(int size)
		{
			Vector3[] res = new Vector3[size];
			int pos = 0;
			while (pos < size)
			{
				res[pos] = this.ConsumeVector3();
				++pos;
			}
			return res;
		}

		public void Expect(byte val)
		{
			var v = this.ConsumeByte();
			if (v != val)
			{
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
			}
		}

		public void Expect(short val)
		{
			var v = this.ConsumeInt16();
			if (v != val)
			{
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
			}
		}

		public void Expect(ushort val)
		{
			var v = this.ConsumeUInt16();
			if (v != val)
			{
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
			}
		}

		public void Expect(bool val)
		{
			var v = this.ConsumeBool();
			if (v != val)
			{
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
			}
		}

		public void Expect(uint val)
		{
			var v = this.ConsumeUInt32();
			if (v != val)
			{
				throw new FormatException(string.Format("Expected {0} but found {1}", val, v));
			}
		}

		public uint PeekUInt32()
		{
			var pos = this.source.BaseStream.Position;
			var res = this.source.ReadUInt32();
			this.source.BaseStream.Position = pos;
			return res;
		}

		public void Skip(uint len)
		{
			this.source.BaseStream.Position += len;
		}

		#endregion
	}
}