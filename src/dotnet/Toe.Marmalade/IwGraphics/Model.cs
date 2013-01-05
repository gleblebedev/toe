using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Mesh;

namespace Toe.Utils.Marmalade.IwGraphics
{
	public class Model : Managed
	{
		private readonly IResourceManager resourceManager;

		public Model(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwModel");

		private readonly IList<IMesh> meshes = new List<IMesh>();

		private uint flags;

		#endregion

		#region Public Properties

		public IList<IMesh> Meshes
		{
			get
			{
				return this.meshes;
			}
		}

		#endregion

		#region Public Methods and Operators

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public uint Flags
		{
			get
			{
				return flags;
			}
			set
			{
				if (flags != value)
				{
					this.RaisePropertyChanging("Flags");
					flags = value;
					this.RaisePropertyChanged("Flags");
				}
			}
		}

		public Vector3 Center { get; set; }

		public float Radius { get; set; }

		#endregion

		public void RenderOpenGL()
		{
			foreach (var mesh in this.Meshes)
			{
				foreach (ISubMesh submesh in mesh.Submeshes)
				{
					GL.PushAttrib(AttribMask.AllAttribBits);
					Material m = null;
					if (!string.IsNullOrEmpty(submesh.Material))
					{
						m = this.resourceManager.FindResource(Material.TypeHash, Hash.Get(mesh.Name + "/" + submesh.Material)) as Material;
						if (m == null) m = resourceManager.FindResource(Material.TypeHash, Hash.Get(submesh.Material)) as Material;
					}
					else
					{
						m = this.resourceManager.FindResource(Material.TypeHash, submesh.MaterialHash) as Material;
					}
					if (m != null)
					{
						m.ApplyOpenGL();
					}
					submesh.RenderOpenGL();
					GL.PopAttrib();
				}
			}
		}
	}
}