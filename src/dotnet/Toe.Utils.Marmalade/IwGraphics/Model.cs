using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

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

		#endregion

		public void RenderOpenGL()
		{
			foreach (var mesh in this.Meshes)
			{
				foreach (ISubMesh submesh in mesh.Submeshes)
				{
					GL.PushAttrib(AttribMask.AllAttribBits);
					var m = resourceManager.FindResource(Material.TypeHash, Hash.Get(mesh.Name + "/" + submesh.Material)) as Material;
					if (m == null)
						m = resourceManager.FindResource(Material.TypeHash, Hash.Get(submesh.Material)) as Material;
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