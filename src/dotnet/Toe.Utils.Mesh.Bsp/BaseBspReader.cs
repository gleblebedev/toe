using System;
using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public abstract class BaseBspReader
	{
		private IScene scene;

		private Stream stream;

		private long startOfTheFile;

		public IScene Scene
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
			set
			{
				this.stream = value;
				this.startOfTheFile = stream.Position;
			}
		}

		public long StartOfTheFile
		{
			get
			{
				return this.startOfTheFile;
			}
		}

		public virtual string GameRootPath
		{
			get
			{
				return this.gameRootPath;
			}
			set
			{
				this.gameRootPath = value;
			}
		}

		private string gameRootPath;


		/// <summary>
		/// Load generic scene from BSP file.
		/// </summary>
		/// <param name="stream"> </param>
		/// <returns>Loaded scene.</returns>
		public virtual IScene LoadScene()
		{
			this.scene = new Scene();

			ReadHeader();
			ReadVertices();
			ReadEdges();
			ReadPlanes();
			ReadTextures();
			ReadEffects();
			ReadLightmaps();
			ReadFaces();
			ReadVisibilityGraph();
			this.BuildScene();

			return scene;
		}

		protected virtual void ReadPlanes()
		{
			
		}

		protected virtual void ReadEdges()
		{
			
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