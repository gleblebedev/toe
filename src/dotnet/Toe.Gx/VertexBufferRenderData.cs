using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;
using Toe.Utils.Mesh;

namespace Toe.Gx
{
	public class VertexBufferRenderData:IContextData
	{
		private Vector3[] v;
		private Vector3[] n;
		private Vector3[] t;
		private Vector3[] b;
		private byte[] c;
		private Vector3[] uv0;
		private Vector3[] uv1;
		public VertexBufferRenderData(IVertexStreamSource mesh)
		{
			this.v = null;
			if (mesh.IsVertexStreamAvailable)
			{
				this.v = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitVertices((ref Vector3 vv) =>
					{
						this.v[i] = vv;
						++i;
					});
			}
			this.n = null;
			if (mesh.IsNormalStreamAvailable)
			{
				this.n = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitNormals((ref Vector3 vv) =>
					{
						this.n[i] = vv;
						++i;
					});
			}
			this.t = null;
			if (mesh.IsTangentStreamAvailable)
			{
				this.t = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitTangents((ref Vector3 vv) =>
				{
					this.t[i] = vv;
					++i;
				});
			}
			this.b = null;
			if (mesh.IsTangentStreamAvailable)
			{
				this.b = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitBinormals((ref Vector3 vv) =>
				{
					this.b[i] = vv;
					++i;
				});
			}
			this.uv0 = null;
			if (mesh.IsUV0StreamAvailable)
			{
				this.uv0 = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitUV(0, (ref Vector3 vv) =>
					{
						this.uv0[i] = vv;
						++i;
					});
			}
			this.uv1 = null;
			if (mesh.IsUV1StreamAvailable)
			{
				this.uv1 = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitUV(0, (ref Vector3 vv) =>
					{
						this.uv1[i] = vv;
						++i;
					});
			}
			this.c = null;
			if (mesh.IsColorStreamAvailable)
			{
				this.c = new byte[mesh.Count * 4];
				int i = 0;
				mesh.VisitColors((ref Color vv) =>
					{
						this.c[i] = vv.R;
						++i;
						this.c[i] = vv.G;
						++i;
						this.c[i] = vv.B;
						++i;
						this.c[i] = vv.A;
						++i;
					});
			}
		}
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
		public void Enable(ShaderTechniqueArgumentIndices p)
		{
			if (p.inVert >= 0 && this.v != null)
			{
				GL.EnableVertexAttribArray(p.inVert);
				GL.VertexAttribPointer(p.inVert, 3, VertexAttribPointerType.Float, true, 4 * 3, this.v);
			}
			if (p.inNorm >= 0 && this.n != null)
			{
				GL.EnableVertexAttribArray(p.inNorm);
				GL.VertexAttribPointer(p.inNorm, 3, VertexAttribPointerType.Float, true, 4 * 3, this.n);
			}
			if (p.inTangent >= 0 && this.t != null)
			{
				GL.EnableVertexAttribArray(p.inTangent);
				GL.VertexAttribPointer(p.inTangent, 3, VertexAttribPointerType.Float, true, 4 * 3, this.t);
			}
			if (p.inBiTangent >= 0 && this.b != null)
			{
				GL.EnableVertexAttribArray(p.inBiTangent);
				GL.VertexAttribPointer(p.inBiTangent, 3, VertexAttribPointerType.Float, true, 4 * 3, this.b);
			}
			if (p.inCol >= 0 && this.c != null)
			{
				GL.EnableVertexAttribArray(p.inCol);
				GL.VertexAttribPointer(p.inCol, 4, VertexAttribPointerType.UnsignedByte, true, 4 * 1, this.c);
			}
			if (p.inUV0 >= 0 && this.uv0 != null)
			{
				GL.EnableVertexAttribArray(p.inUV0);
				GL.VertexAttribPointer(p.inUV0, 3, VertexAttribPointerType.Float, true, 4 * 3, this.uv0);
			}
			if (p.inUV1 >= 0 && this.uv1 != null)
			{
				GL.EnableVertexAttribArray(p.inUV1);
				GL.VertexAttribPointer(p.inUV1, 3, VertexAttribPointerType.Float, true, 4 * 3, this.uv1);
			}
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			
		}

		#endregion
	}
}