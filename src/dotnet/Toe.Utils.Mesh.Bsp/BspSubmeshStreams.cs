using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspSubmeshStreams
	{
		private readonly IStreamConverterFactory streamConverterFactory;

		public ListMeshStream<int> PositionIndices;
		public ListMeshStream<int> NormalIndices;
		public ListMeshStream<int> TexCoord0Indices;
		public ListMeshStream<int> TexCoord1Indices;
		public ListMeshStream<int> ColorsIndices;

		public BspSubmeshStreams(SeparateStreamsSubmesh separateStreamsSubmesh, BspMeshStreams meshStreams,IStreamConverterFactory streamConverterFactory)
		{
			this.streamConverterFactory = streamConverterFactory;
			if (meshStreams.Positions != null)
			{
				PositionIndices = EnsureStream(separateStreamsSubmesh, Streams.Position, 0);
			}
			if (meshStreams.Normals != null)
				NormalIndices = EnsureStream(separateStreamsSubmesh, Streams.Normal, 0);
			if (meshStreams.TexCoord0 != null)
				TexCoord0Indices = EnsureStream(separateStreamsSubmesh, Streams.TexCoord, 0);
			if (meshStreams.TexCoord1 != null)
				TexCoord1Indices = EnsureStream(separateStreamsSubmesh, Streams.TexCoord, 1);
			if (meshStreams.Colors != null)
				ColorsIndices = EnsureStream(separateStreamsSubmesh, Streams.Color, 0);
		}

		private ListMeshStream<int> EnsureStream(SeparateStreamsSubmesh separateStreamsSubmesh, string key, int channel)
		{
			return separateStreamsSubmesh.GetIndexStream(key, channel) as ListMeshStream<int> ?? separateStreamsSubmesh.SetIndexStream(key, channel, new ListMeshStream<int>(streamConverterFactory));
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