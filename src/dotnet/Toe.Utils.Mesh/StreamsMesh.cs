using System.Collections.Generic;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else
using System.Drawing;
using OpenTK;
#endif

namespace Toe.Utils.Mesh
{
	public class StreamMesh : IMesh
	{
		#region Constants and Fields

		public MeshStream<VertexWeight> weights = new MeshStream<VertexWeight>();

		private readonly List<MeshBone> bones = new List<MeshBone>();

		private readonly MeshStream<Color> colors = new MeshStream<Color>();

		private readonly MeshStream<Vector3> normals = new MeshStream<Vector3>();

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		private readonly List<MeshStream<Vector2>> uv = new List<MeshStream<Vector2>>();

		private readonly MeshStream<Vector3> vertices = new MeshStream<Vector3>();

		public string useGroup { get; set; }

		#endregion

		#region Public Properties

		public List<MeshBone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		public MeshStream<Color> Colors
		{
			get
			{
				return this.colors;
			}
		}

		public string Name { get; set; }

		public string BaseName { get; set; }

		public MeshStream<Vector3> Normals
		{
			get
			{
				return this.normals;
			}
		}

		public float Scale { get; set; }

		public string Skeleton { get; set; }

		public string SkeletonModel { get; set; }

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return this.submeshes;
			}
		}

		public IList<MeshStream<Vector2>> UV
		{
			get
			{
				return this.uv;
			}
		}

		public MeshStream<Vector3> Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		public MeshStream<VertexWeight> Weights
		{
			get
			{
				return this.weights;
			}
		}

		public string useGeo { get; set; }

		#endregion

		#region Public Methods and Operators

		public ISubMesh CreateSubmesh()
		{
			var streamSubmesh = new StreamSubmesh(this);
			this.Submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		public int EnsureBone(string boneName)
		{
			int index;
			for (index = 0; index < this.bones.Count; index++)
			{
				var bone = this.bones[index];
				if (bone.Name == boneName)
				{
					return index;
				}
			}
			this.bones.Add(new MeshBone { Name = boneName });
			return index;
		}

		public MeshStream<Vector2> EnsureUVStream(int setId)
		{
			while (this.UV.Count <= setId)
			{
				this.UV.Add(new MeshStream<Vector2>());
			}
			return this.UV[setId];
		}
#if WINDOWS_PHONE
#else
		public void RenderOpenGL()
		{
			var subMeshes = this.submeshes;
			foreach (ISubMesh subMesh in subMeshes)
			{
				subMesh.RenderOpenGL();
			}
		}
#endif
		#endregion
	}
}