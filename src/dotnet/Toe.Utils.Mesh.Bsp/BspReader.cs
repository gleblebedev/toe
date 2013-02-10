﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Toe.Utils.Mesh.Bsp.Q3;

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
				else if (magic == 21)
					reader = new HL2.BspReader21();
				else
					throw new BspFormatException("Unknown Source Engine BSP: " + magic);
			}
			else if (magic == 0x50534249)
			{
				magic = stream.ReadUInt32();
				if (magic == 0x26)
					reader = new Q2.BspReader();
				else if (magic == 0x2E)
					reader = new Q3.BspReader();
				else if (magic == 0x2F)
					reader = new QLiveBspReader();
				else
					throw new BspFormatException("Unknown Quake 3 BSP: " + magic);
			}

			stream.Position = pos;
			reader.Stream = stream;
			reader.GameRootPath = EvalGameRootPath(stream);
			return reader.LoadScene();
		}

		private static string EvalGameRootPath(Stream stream)
		{
			var fileStream = stream as FileStream;
			if (fileStream == null) return Directory.GetCurrentDirectory();

			var name = Path.GetDirectoryName(Path.GetFullPath(fileStream.Name));
			var gameRoot = name;
			for (; ; )
			{
				var root = Path.GetDirectoryName(gameRoot);
				if (root == null) return name;
				if (string.Compare(Path.GetFileName(gameRoot), "maps",StringComparison.InvariantCultureIgnoreCase)==0)
				{
					return root;
				}
				gameRoot = root;
			}
		}

		#endregion
	}
}
