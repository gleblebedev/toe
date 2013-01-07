using Toe.Resources;

namespace Toe.Utils.Mesh
{
	public abstract class BaseSubmesh : ISubMesh
	{
		private string material;

		public abstract void RenderOpenGL();

		public string Material
		{
			get
			{
				return this.material;
			}
			set
			{
				if (this.material != value)
				{
					this.material = value;
					this.materialHash = Hash.Get(value);
				}
			}
		}

		private string name;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
				}
			}
		}

		private uint materialHash;

		public uint MaterialHash
		{
			get
			{
				return this.materialHash;
			}
			set
			{
				this.materialHash = value;
				this.material = null;
			}
		}
	}
}