using System;
using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public class BaseBspReader
	{
		private Scene scene;

		private Stream stream;

		private long startOfTheFile;

		public Scene Scene
		{
			get
			{
				return this.scene;
			}
		}

		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		public long StartOfTheFile
		{
			get
			{
				return this.startOfTheFile;
			}
		}

		/// <summary>
		/// Load generic scene from BSP file.
		/// </summary>
		/// <param name="stream"> </param>
		/// <returns>Loaded scene.</returns>
		public virtual IScene LoadScene(Stream stream)
		{
			this.stream = stream;
			this.scene = new Scene();
			this.startOfTheFile = stream.Position;

			ReadHeader();

			return scene;
		}

		protected virtual void ReadHeader()
		{
			throw new NotImplementedException();
		}
	}
}