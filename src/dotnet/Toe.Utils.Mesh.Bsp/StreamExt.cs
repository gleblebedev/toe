using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public static class StreamExt
	{
		#region Public Methods and Operators

		public static Color ReadARGB(this Stream stream)
		{
			var a = (byte)stream.ReadByte();
			var r = (byte)stream.ReadByte();
			var g = (byte)stream.ReadByte();
			var b = (byte)stream.ReadByte();
			return Color.FromArgb(a, r, g, b);
		}

		public static Color ReadBGRA(this Stream stream)
		{
			var b = (byte)stream.ReadByte();
			var g = (byte)stream.ReadByte();
			var r = (byte)stream.ReadByte();
			var a = (byte)stream.ReadByte();
			return Color.FromArgb(a, r, g, b);
		}

		public static byte[] ReadBytes(this Stream stream, int count)
		{
			var res = new byte[count];
			for (int i = 0; i < count; i++)
			{
				res[i] = (byte)stream.ReadByte();
			}
			return res;
		}

		public static short ReadInt16(this Stream stream)
		{
			var a0 = (short)stream.ReadByte();
			var a1 = (short)stream.ReadByte();
			return (short)((a1 << 8) | (a0));
		}

		public static int ReadInt32(this Stream stream)
		{
			return (int)stream.ReadUInt32();
		}

		public static float ReadSingle(this Stream stream)
		{
			Byte2Float f;
			f.f = 0;
			f.a0 = (byte)stream.ReadByte();
			f.a1 = (byte)stream.ReadByte();
			f.a2 = (byte)stream.ReadByte();
			f.a3 = (byte)stream.ReadByte();
			return f.f;
		}

		public static string ReadStringZ(this Stream stream)
		{
			List<byte> buf = new List<byte>(256);
			for (;;)
			{
				var b = stream.ReadByte();
				if (b < 0 || b == 0)
				{
					break;
				}
				buf.Add((byte)b);
			}
			return Encoding.UTF8.GetString(buf.ToArray());
		}

		public static ushort ReadUInt16(this Stream stream)
		{
			var a0 = (ushort)stream.ReadByte();
			var a1 = (ushort)stream.ReadByte();
			return (ushort)((a1 << 8) | (a0));
		}

		public static uint ReadUInt32(this Stream stream)
		{
			var a0 = (uint)stream.ReadByte();
			var a1 = (uint)stream.ReadByte();
			var a2 = (uint)stream.ReadByte();
			var a3 = (uint)stream.ReadByte();
			return ((a3 << 24) | (a2 << 16) | (a1 << 8) | (a0));
		}

		public static void ReadVector2(this Stream stream, out Vector2 v)
		{
			v.X = stream.ReadSingle();
			v.Y = stream.ReadSingle();
			if (float.IsInfinity(v.X) || float.IsInfinity(v.Y))
			{
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}}}", v.X, v.Y));
			}
		}

		public static void ReadVector3(this Stream stream, out Vector3 v)
		{
			v.X = stream.ReadSingle();
			v.Y = stream.ReadSingle();
			v.Z = stream.ReadSingle();
			if (float.IsInfinity(v.X) || float.IsInfinity(v.Y) || float.IsInfinity(v.Z))
			{
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}, {2}}}", v.X, v.Y, v.Z));
			}
		}
		public static Vector3 ReadVector3(this Stream stream)
		{
			var X = stream.ReadSingle();
			var Y = stream.ReadSingle();
			var Z = stream.ReadSingle();
			if (float.IsInfinity(X) || float.IsInfinity(Y) || float.IsInfinity(Z))
			{
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}, {2}}}", X, Y, Z));
			}
			return new Vector3(X, Y, Z);
		}
		public static Vector2 ReadVector2(this Stream stream)
		{
			var X = stream.ReadSingle();
			var Y = stream.ReadSingle();
			if (float.IsInfinity(X) || float.IsInfinity(Y))
			{
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}}}", X, Y));
			}
			return new Vector2(X, Y);
		}
		#endregion

		[StructLayout(LayoutKind.Explicit)]
		internal struct Byte2Float
		{
			[FieldOffset(0)]
			public byte a0;

			[FieldOffset(1)]
			public byte a1;

			[FieldOffset(2)]
			public byte a2;

			[FieldOffset(3)]
			public byte a3;

			[FieldOffset(0)]
			public float f;
		}
	}
}