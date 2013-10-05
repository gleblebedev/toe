using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspSubmeshStreams
	{
		public ListMeshStream<int> PositionIndices;
		public ListMeshStream<int> NormalIndices;
		public ListMeshStream<int> TexCoord0Indices;
		public ListMeshStream<int> TexCoord1Indices;
		public ListMeshStream<int> ColorsIndices;

		public BspSubmeshStreams(SeparateStreamsSubmesh separateStreamsSubmesh, BspMeshStreams meshStreams)
		{
			if (meshStreams.Positions != null)
				PositionIndices = separateStreamsSubmesh.SetIndexStream(Streams.Position,0, new ListMeshStream<int>());
			if (meshStreams.Normals != null)
				NormalIndices = separateStreamsSubmesh.SetIndexStream(Streams.Normal, 0, new ListMeshStream<int>());
			if (meshStreams.TexCoord0 != null)
				TexCoord0Indices = separateStreamsSubmesh.SetIndexStream(Streams.TexCoord, 0, new ListMeshStream<int>());
			if (meshStreams.TexCoord1 != null)
				TexCoord1Indices = separateStreamsSubmesh.SetIndexStream(Streams.TexCoord, 1, new ListMeshStream<int>());
			if (meshStreams.Colors != null)
				ColorsIndices = separateStreamsSubmesh.SetIndexStream(Streams.Color, 0, new ListMeshStream<int>());
		}

		public void AddToAllStreams(int index)
		{
			foreach (var p in this.GetAllStreams()) p.Add(index);
		}

		private IEnumerable<ListMeshStream<int>> GetAllStreams()
		{
			if (this.PositionIndices != null)
			{
				yield return PositionIndices;
			}
			if (this.NormalIndices != null)
			{
				yield return NormalIndices;
			}
			if (this.TexCoord0Indices != null)
			{
				yield return TexCoord0Indices;
			}
			if (this.TexCoord1Indices != null)
			{
				yield return TexCoord1Indices;
			}
			if (this.ColorsIndices != null)
			{
				yield return ColorsIndices;
			}
		}
	}
}