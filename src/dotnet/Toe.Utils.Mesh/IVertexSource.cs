using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface IVertexSource: IEnumerable<Vertex>
	{
		bool IsVertexStreamAvailable
		{
			get;
		}
		bool IsNormalStreamAvailable
		{
			get;
		}
		bool IsBinormalStreamAvailable
		{
			get;
		}
		bool IsTangentStreamAvailable
		{
			get;
		}
		bool IsColorStreamAvailable
		{
			get;
		}
		bool IsUV0StreamAvailable
		{
			get;
		}
		bool IsUV1StreamAvailable
		{
			get;
		}

		VertexSourceType VertexSourceType { get; }
	}
	public enum VertexSourceType
	{
		TrianleList,
		TrianleStrip,

		QuadList
	}
}