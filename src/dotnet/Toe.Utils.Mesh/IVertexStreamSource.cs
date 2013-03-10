using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertex stream source.
	/// This is suitable to building vertex buffers.
	/// </summary>
	public interface IVertexStreamSource
	{
		#region Public Properties

		/// <summary>
		/// Number of vertices.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Is binormal (bitangent) stream available.
		/// </summary>
		bool IsBinormalStreamAvailable { get; }

		/// <summary>
		/// Is color stream available.
		/// </summary>
		bool IsColorStreamAvailable { get; }

		/// <summary>
		/// Is normal stream available.
		/// </summary>
		bool IsNormalStreamAvailable { get; }

		/// <summary>
		/// Is tangent stream available.
		/// </summary>
		bool IsTangentStreamAvailable { get; }

		/// <summary>
		/// Is texture coordinates 0 stream available.
		/// </summary>
		bool IsUV0StreamAvailable { get; }

		/// <summary>
		/// Is texture coordinates 1 stream available.
		/// </summary>
		bool IsUV1StreamAvailable { get; }

		/// <summary>
		/// Is position stream available. Acutally it should be always true.
		/// </summary>
		bool IsVertexStreamAvailable { get; }

		#endregion

		#region Public Methods and Operators

		void VisitBinormals(Vector3VisitorCallback action);

		void VisitTangents(Vector3VisitorCallback action);

		/// <summary>
		/// Get vertex color by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="color">Vertex color.</param>
		void GetColorAt(int index, out Color color);

		/// <summary>
		/// Get vertex texture coords by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="channel">Texture channel.</param>
		/// <param name="uv">Vertex UV.</param>
		void GetUV3At(int index, int channel, out Vector3 uv);

		/// <summary>
		/// Get vertex position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex position.</param>
		void GetVertexAt(int index, out Vector3 vector);

		/// <summary>
		/// Get normal position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex normal.</param>
		void GetNormalAt(int index, out Vector3 vector);

		#endregion
	}
}