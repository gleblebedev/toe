using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public static class StreamExt
	{
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
		public static void ReadVector3(this Stream stream, out Vector3 v)
		{
			v.X = stream.ReadSingle();
			v.Y = stream.ReadSingle();
			v.Z = stream.ReadSingle();
			if (float.IsInfinity(v.X) || float.IsInfinity(v.Y) || float.IsInfinity(v.Z))
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}, {2}}}", v.X, v.Y, v.Z));
		}
		public static void ReadVector2(this Stream stream, out Vector2 v)
		{
			v.X = stream.ReadSingle();
			v.Y = stream.ReadSingle();
			if (float.IsInfinity(v.X) || float.IsInfinity(v.Y))
				throw new BspFormatException(string.Format("Wrong vertex data {{{0}, {1}}}", v.X, v.Y));
		}
		public static uint ReadUInt32(this Stream stream)
		{
			var a0 = (uint)stream.ReadByte();
			var a1 = (uint)stream.ReadByte();
			var a2 = (uint)stream.ReadByte();
			var a3 = (uint)stream.ReadByte();
			return ((a3 << 24) | (a2 << 16) | (a1 << 8) | (a0));
		}
		public static Color ReadARGB(this Stream stream)
		{
			var a = (byte)stream.ReadByte();
			var r = (byte)stream.ReadByte();
			var g = (byte)stream.ReadByte();
			var b = (byte)stream.ReadByte();
			return Color.FromArgb(a,r,g,b);
		}
		public static int ReadInt32(this Stream stream)
		{
			return (int)stream.ReadUInt32();
		}
	}
}