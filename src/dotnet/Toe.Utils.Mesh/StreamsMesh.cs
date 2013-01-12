using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	public class StreamMesh : IMesh
	{
		#region Constants and Fields

		public MeshStream<VertexWeight> weights = new MeshStream<VertexWeight>();
		public object RenderData
		{
			get;
			set;
		}
		private readonly BoneCollection bones = new BoneCollection();

		private readonly MeshStream<Color> colors = new MeshStream<Color>();

		private readonly MeshStream<Vector3> normals = new MeshStream<Vector3>();

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		private readonly List<MeshStream<Vector3>> uv = new List<MeshStream<Vector3>>();

		private readonly MeshStream<Vector3> vertices = new MeshStream<Vector3>();

		public string useGroup { get; set; }

		#endregion
		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		private MeshStream<Vector3> binormals = new MeshStream<Vector3>();

		private MeshStream<Vector3> tangents = new MeshStream<Vector3>();

		#region Implementation of ISceneItem

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.parameters = new DynamicCollection());
			}
			set
			{
				this.parameters = value;
			}
		}

		#endregion
		#region Public Properties

		public BoneCollection Bones
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

		public MeshStream<Vector3> Binormals
		{
			get
			{
				return this.binormals;
			}
		}

		public MeshStream<Vector3> Tangents
		{
			get
			{
				return this.tangents;
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

		public IList<MeshStream<Vector3>> UV
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

		public uint NameHash { get; set; }



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
			return this.bones.EnsureBone(boneName);
		}

		public MeshStream<Vector3> EnsureUVStream(int setId)
		{
			while (this.UV.Count <= setId)
			{
				this.UV.Add(new MeshStream<Vector3>());
			}
			return this.UV[setId];
		}

		#endregion

		

		#region Implementation of IVertexSource

		public int Count
		{
			get
			{
				return this.submeshes.Sum(submesh => submesh.Count);
			}
		}

		public bool IsVertexStreamAvailable
		{
			get
			{
				return vertices != null && vertices.Count > 0;
			}
		}

		public void VisitVertices(Vector3VisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = vertices[index.Vertex];
					callback(ref v);
				}
			}
		}

		public void VisitNormals(Vector3VisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = normals[index.Vertex];
					callback(ref v);
				}
			}
		}

		public void VisitColors(ColorVisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = colors[index.Vertex];
					callback(ref v);
				}
			}
		}

		public void VisitUV(int stage, Vector3VisitorCallback callback)
		{
			var uvstream = uv[stage];
			if (stage==0)
			{
				foreach (StreamSubmesh submesh in submeshes)
				{
					foreach (var index in submesh.Indices)
					{
						var v = uvstream[index.UV0];
						callback(ref v);
					}
				}
			}
			else
			{
				foreach (StreamSubmesh submesh in submeshes)
				{
					foreach (var index in submesh.Indices)
					{
						var v = uvstream[index.UV1];
						callback(ref v);
					}
				}
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return normals != null && normals.Count > 0;
			}
		}

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return binormals != null && binormals.Count > 0;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return tangents != null && tangents.Count > 0;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return colors != null && colors.Count > 0;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return uv.Count > 0 && uv[0].Count > 0;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return uv.Count > 1 && uv[1].Count > 0;
			}
		}


		#endregion
	}
}