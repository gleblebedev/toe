using System;
using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public abstract class BaseBspReader
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
			ReadVertices();
			ReadTextures();
			ReadEffects();
			ReadLightmaps();
			ReadFaces();
			ReadVisibilityGraph();
			this.BuildScene();

			return scene;
		}

		protected virtual void ReadVisibilityGraph()
		{
			
		}
		protected virtual void ReadTextures()
		{

		}
		protected virtual void ReadEffects()
		{

		}
		protected virtual void ReadLightmaps()
		{
			
		}

		protected virtual void ReadFaces()
		{
			
		}
		protected virtual void BuildScene()
		{

		}

		protected void SeekEntryAt(long offset)
		{
			this.Stream.Position = startOfTheFile + offset;
		}
		protected void AssertStreamPossition(long position)
		{
			if (Stream.Position != position)
				throw new BspFormatException(string.Format("Unknow data format (file position {0}, expected {1})", Stream.Position, position));
		}
		protected int EvalNumItems(long total, long structSize)
		{
			if (total % structSize != 0)
				throw new BspFormatException(string.Format("BSP entry size {0} should be power of {1}", total, structSize));
			return (int)(total / structSize);
		}
		protected abstract void ReadHeader();

		protected abstract void ReadVertices();
	}
}