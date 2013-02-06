using System;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh
{
	public delegate void Vector3VisitorCallback(ref Vector3 vec);
	public delegate void ColorVisitorCallback(ref Color vec);
	public interface IVertexStreamSource
	{

		int Count { get; }
		bool IsVertexStreamAvailable
		{
			get;
		}

		void VisitVertices(Vector3VisitorCallback callback);
		void VisitNormals(Vector3VisitorCallback callback);
		void VisitColors(ColorVisitorCallback callback);
		void VisitUV(int stage, Vector3VisitorCallback callback);

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

		void VisitTangents(Vector3VisitorCallback action);

		void VisitBinormals(Vector3VisitorCallback action);
	}

	public interface IVertexIndexSource: IEnumerable<int>
	{
		int Count { get; }
		VertexSourceType VertexSourceType { get; }
	}
	public enum VertexSourceType
	{
		TrianleList,
		TrianleStrip,

		QuadList,
		QuadStrip,

		LineLine,
		LineStrip,
	}
}