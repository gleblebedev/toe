using System.Collections.Generic;

using OpenTK;

namespace Toe.Marmalade.IwGraphics.TangentSpace
{
	public class TangentMixer
	{
		#region Constants and Fields

		private readonly Dictionary<TangentKey, TangentMix> mix = new Dictionary<TangentKey, TangentMix>();

		private int count;

		#endregion

		#region Public Properties

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public Dictionary<TangentKey, TangentMix> Mix
		{
			get
			{
				return this.mix;
			}
		}

		#endregion

		#region Public Methods and Operators

		public int Add(TangentKey key0, ref Vector3 normal, Vector3 tangent)
		{
			TangentMix tmix;
			if (!this.mix.TryGetValue(key0, out tmix))
			{
				tmix = new TangentMix();
				this.mix.Add(key0, tmix);
			}

			var i = tmix.IndexOf(ref normal, ref tangent);
			if (i < 0)
			{
				i = this.Count;
				this.count = this.Count + 1;
				tmix.Add(ref normal, ref tangent, i);
			}
			return i;
		}

		#endregion
	}
}