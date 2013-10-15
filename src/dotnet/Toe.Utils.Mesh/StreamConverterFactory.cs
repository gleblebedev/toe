using System;
using System.Collections.Generic;
using System.Drawing;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	public class StreamConverterFactory : IStreamConverterFactory
	{
		public static readonly StreamConverterFactory Default;
		static StreamConverterFactory()
		{
			Default = new StreamConverterFactory();
			Default.Register((Color c) => new Float3(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f));
			Default.Register((Color c) => new Float4(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f));
			Default.Register((Float3 c) => Color.FromArgb(255,ClampToByte(c.X * 255), ClampToByte(c.Y * 255), ClampToByte(c.Z * 255)));
			Default.Register((Float4 c) => Color.FromArgb(ClampToByte(c.W * 255), ClampToByte(c.X * 255), ClampToByte(c.Y * 255), ClampToByte(c.Z * 255)));
			Default.Register((Float2 c) => new Float3(c.X, c.Y, 0));
			Default.Register((Float2 c) => new Float4(c.X, c.Y, 0,0));
			Default.Register((Float3 c) => new Float4(c.X, c.Y, c.Z,0));
			Default.Register((Float4 c) => new Float3(c.X, c.Y, c.Z));
			Default.Register((Float4 c) => new Float2(c.X, c.Y));
			Default.Register((Float3 c) => new Float2(c.X, c.Y));
		}

		private static byte ClampToByte(float f)
		{
			if (f < 0) return 0;
			if (f > 255) return 255;
			return (byte)f;
		}

		Dictionary<Tuple<Type, Type>,Delegate> map = new Dictionary<Tuple<Type, Type>, Delegate>();
		public StreamConverterFactory()
		{
			
		}
		
		public void Register<Src,Dst>(Func<Src,Dst> converter)
		{
			this.map[new Tuple<Type, Type>(typeof(Src), typeof(Dst))] = converter;
		}

		#region Implementation of IStreamConverterFactory

		public IList<TRes> ResolveConverter<T, TRes>(IList<T> arrayMeshStream)
		{
			Delegate c;
			if (map.TryGetValue(new Tuple<Type, Type>(typeof(T),typeof(TRes)), out c))
			{
				return new StreamConverter<T, TRes>((Func<T, TRes>)c, arrayMeshStream);
			}
			return null;
		}
		#endregion
	}
}