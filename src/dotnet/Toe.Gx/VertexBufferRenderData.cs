using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;
using Toe.Utils.Mesh;

namespace Toe.Gx
{
	public class VertexBufferRenderData : IContextData
	{
		#region Constants and Fields

		private readonly IList<Vector3> b;

		private readonly IList<Vector4> c;

		private readonly IList<Vector3> n;

		private readonly IList<Vector3> t;

		private readonly IList<Vector3> uv0;

		private readonly IList<Vector3> uv1;

		private readonly IList<Vector3> v;

		#endregion

		#region Constructors and Destructors

		public VertexBufferRenderData(IVertexStreamSource mesh, IEnumerable<IVertexIndexSource> submeshes)
		{
			var position = mesh.GetStreamReader<Vector3>(Streams.Position, 0);
			var normal = mesh.GetStreamReader<Vector3>(Streams.Normal, 0);
			var tangent = mesh.GetStreamReader<Vector3>(Streams.Tangent, 0);
			var binormal = mesh.GetStreamReader<Vector3>(Streams.Binormal, 0);
			var texcoord0 = mesh.GetStreamReader<Vector3>(Streams.TexCoord, 0);
			var texcoord1 = mesh.GetStreamReader<Vector3>(Streams.TexCoord, 1);
			var color = mesh.GetStreamReader<Vector4>(Streams.Color, 0);

			if (position != null) this.v = new List<Vector3>(mesh.Count);
			if (normal != null) this.n = new List<Vector3>(mesh.Count);
			if (binormal != null) this.b = new List<Vector3>(mesh.Count);
			if (tangent != null) this.t = new List<Vector3>(mesh.Count);
			if (texcoord0 != null) this.uv0 = new List<Vector3>(mesh.Count);
			if (texcoord1 != null) this.uv1 = new List<Vector3>(mesh.Count);
			if (color != null) this.c = new List<Vector4>(mesh.Count);

			foreach (var subMesh in submeshes)
			{
				var positionIndices = subMesh.GetIndexReader(Streams.Position, 0);
				var normalIndices = subMesh.GetIndexReader(Streams.Normal, 0);
				var binormalIndices = subMesh.GetIndexReader(Streams.Binormal, 0);
				var tangentIndices = subMesh.GetIndexReader(Streams.Tangent, 0);
				var colorIndices = subMesh.GetIndexReader(Streams.Color, 0);
				var uv0Indices = subMesh.GetIndexReader(Streams.TexCoord, 0);
				var uv1Indices = subMesh.GetIndexReader(Streams.TexCoord, 1);

				for (int i = 0; i < positionIndices.Count; ++i)
				{
					if (position != null && positionIndices != null)
					{
						this.v.Add(position[positionIndices[i]]);
					}
					if (normal != null && normalIndices != null)
					{
						this.n.Add(normal[normalIndices[i]]);
					}
					if (binormal != null && binormalIndices != null)
					{
						this.n.Add(binormal[binormalIndices[i]]);
					}
					if (tangent != null && tangentIndices != null)
					{
						this.t.Add(tangent[tangentIndices[i]]);
					}
					if (color != null && colorIndices != null)
					{
						this.c.Add(color[colorIndices[i]]);
					}
					if (texcoord0 != null && uv0Indices != null)
					{
						this.uv0.Add(texcoord0[uv0Indices[i]]);
					}
					if (texcoord1 != null && uv1Indices != null)
					{
						this.uv1.Add(texcoord1[uv1Indices[i]]);
					}
				}
			}
			if (v != null)
			{
				v = v.ToArray();
			}
			if (n!= null)
			{
				n = n.ToArray();
			}
			if (b != null)
			{
				b = b.ToArray();
			}
			if (t != null)
			{
				t = t.ToArray();
			}
			if (c != null)
			{
				c = c.ToArray();
			}
			if (uv0 != null)
			{
				uv0 = uv0.ToArray();
			}
			if (uv1 != null)
			{
				uv1 = uv1.ToArray();
			}
			//this.t = null;
			//if (tangent != null)
			//{
			//	this.t = tangent.ToArray();
			//}
			//this.b = null;
			//if (binormal != null)
			//{
			//	this.b = binormal.ToArray();
			//}
			//this.uv0 = texcoord0 != null ? texcoord0.ToArray() : null;
			//this.uv1 = texcoord1 != null ? texcoord1.ToArray() : null;
			//if (color != null)
			//{
			//	this.c = new byte[mesh.Count*4];
			//	int j = 0;
			//	foreach (var col in color)
			//	{
			//		this.c[j] = col.R;
			//		++j;
			//		this.c[j] = col.G;
			//		++j;
			//		this.c[j] = col.B;
			//		++j;
			//		this.c[j] = col.A;
			//		++j;
			//	}
			//}
			//else
			//{
			//	this.c = null;
			//}

		}

		#endregion

		#region Public Methods and Operators

		public void Disable(ShaderTechniqueArgumentIndices p)
		{
			if (p.inVert >= 0)
			{
				GL.DisableVertexAttribArray(p.inVert);
			}
			if (p.inNorm >= 0)
			{
				GL.DisableVertexAttribArray(p.inNorm);
			}
			if (p.inTangent >= 0)
			{
				GL.DisableVertexAttribArray(p.inTangent);
			}
			if (p.inBiTangent >= 0)
			{
				GL.DisableVertexAttribArray(p.inBiTangent);
			}
			if (p.inCol >= 0)
			{
				GL.DisableVertexAttribArray(p.inCol);
			}
			if (p.inUV0 >= 0)
			{
				GL.DisableVertexAttribArray(p.inUV0);
			}
			if (p.inUV1 >= 0)
			{
				GL.DisableVertexAttribArray(p.inUV1);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		public void Enable(ShaderTechniqueArgumentIndices p)
		{
			if (p.inVert >= 0 && this.v != null)
			{
				GL.EnableVertexAttribArray(p.inVert);
				GL.VertexAttribPointer(p.inVert, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.v);
			}
			if (p.inNorm >= 0 && this.n != null)
			{
				GL.EnableVertexAttribArray(p.inNorm);
				GL.VertexAttribPointer(p.inNorm, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.n);
			}
			if (p.inTangent >= 0 && this.t != null)
			{
				GL.EnableVertexAttribArray(p.inTangent);
				GL.VertexAttribPointer(p.inTangent, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.t);
			}
			if (p.inBiTangent >= 0 && this.b != null)
			{
				GL.EnableVertexAttribArray(p.inBiTangent);
				GL.VertexAttribPointer(p.inBiTangent, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.b);
			}
			if (p.inCol >= 0 && this.c != null)
			{
				GL.EnableVertexAttribArray(p.inCol);
				GL.VertexAttribPointer(p.inCol, 4, VertexAttribPointerType.Float, true, 4 * 4, (Vector4[])this.c);
			}
			if (p.inUV0 >= 0 && this.uv0 != null)
			{
				GL.EnableVertexAttribArray(p.inUV0);
				GL.VertexAttribPointer(p.inUV0, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.uv0);
			}
			if (p.inUV1 >= 0 && this.uv1 != null)
			{
				GL.EnableVertexAttribArray(p.inUV1);
				GL.VertexAttribPointer(p.inUV1, 3, VertexAttribPointerType.Float, true, 4 * 3, (Vector3[])this.uv1);
			}
		}

		#endregion
	}
}