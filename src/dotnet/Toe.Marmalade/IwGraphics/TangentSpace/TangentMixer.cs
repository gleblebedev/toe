using System.Collections.Generic;

using OpenTK;

namespace Toe.Marmalade.IwGraphics.TangentSpace
{
	public class TangentMixer
	{
		private Dictionary<TangentKey, TangentMix> mix = new Dictionary<TangentKey, TangentMix>();

		private int count = 0;

		public Dictionary<TangentKey, TangentMix> Mix
		{
			get
			{
				return this.mix;
			}
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public int Add(TangentKey key0, ref Vector3 normal, Vector3 tangent)
		{
			TangentMix tmix;
			if (!mix.TryGetValue(key0, out tmix))
			{
				tmix = new TangentMix();
				mix.Add(key0, tmix);
			}

			var i = tmix.IndexOf( ref normal, ref tangent);
			if (i<0)
			{
				i = this.Count;
				this.count = this.Count + 1;
				tmix.Add(ref normal, ref tangent, i);
			}
			return i;
		}
	}
}