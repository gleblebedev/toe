using System.Drawing;

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

		private readonly Vector3[] b;

		private readonly byte[] c;

		private readonly Vector3[] n;

		private readonly Vector3[] t;

		private readonly Vector3[] uv0;

		private readonly Vector3[] uv1;

		private readonly Vector3[] v;

		#endregion

		#region Constructors and Destructors

		public VertexBufferRenderData(IVertexStreamSource mesh)
		{
			this.v = null;
			if (mesh.IsVertexStreamAvailable)
			{
				this.v = new Vector3[mesh.Count];
				for (int i = 0; i < mesh.Count; ++i)
				{
					mesh.GetVertexAt(i, out this.v[i]);
				}
			}
			this.n = null;
			if (mesh.IsNormalStreamAvailable)
			{
				this.n = new Vector3[mesh.Count];
				for (int i = 0; i < mesh.Count; ++i)
				{
					mesh.GetNormalAt(i, out this.n[i]);
				}
			}
			this.t = null;
			if (mesh.IsTangentStreamAvailable)
			{
				this.t = new Vector3[mesh.Count];
				int i = 0;
				mesh.VisitTangents(
					(ref Vector3 vv) =>
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
				mesh.VisitBinormals(
					(ref Vector3 vv) =>
						{
							this.b[i] = vv;
							++i;
						});
			}
			this.uv0 = null;
			if (mesh.IsUV0StreamAvailable)
			{
				this.uv0 = new Vector3[mesh.Count];
				for (int i = 0; i < mesh.Count; ++i)
				{
					mesh.GetUV3At(i, 0, out this.uv0[i]);
				}
			}
			this.uv1 = null;
			if (mesh.IsUV1StreamAvailable)
			{
				this.uv1 = new Vector3[mesh.Count];
				for (int i = 0; i < mesh.Count; ++i)
				{
					mesh.GetUV3At(i, 1, out this.uv1[i]);
				}
			}
			this.c = null;
			if (mesh.IsColorStreamAvailable)
			{
				this.c = new byte[mesh.Count * 4];
				int i = 0;
				for (int j = 0; j < mesh.Count; ++j)
				{
					Color col;
					mesh.GetColorAt(j, out col);
					this.c[i] = col.R;
					++i;
					this.c[i] = col.G;
					++i;
					this.c[i] = col.B;
					++i;
					this.c[i] = col.A;
					++i;
				}
			}
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

		#endregion
	}
}