using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspReader : ISceneReader
	{
		#region Implementation of IMeshReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream)
		{
			var pos = stream.Position;
			var magic = stream.ReadUInt32();

			IBspReader reader = null;
			if (magic == 0x1D)
				reader = new Q1.BspReader();
			else if (magic == 0x1E)
				reader = new HL1.BspReader();
			else if (magic == 0x50534256)
			{
				magic = stream.ReadUInt32();
				if (magic == 17)
					reader = new HL2.BspReader17();
				else if (magic == 19)
					reader = new HL2.BspReader19();
				else if (magic == 20)
					reader = new HL2.BspReader20();
			}
			else if (magic == 0x50534249)
			{
				magic = stream.ReadUInt32();
				if (magic == 0x26)
					reader = new Q2.BspReader();
				else if (magic == 0x2E)
					reader = new Q3.BspReader();
				else if (magic == 0x2F)
					reader = new QLive.BspReader();
			}
			stream.Position = pos;

			return reader.LoadScene(stream);
		}

		#endregion
	}
}
