using System;
using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh.Dae
{
	public class VertexSource : ISource
	{
		private readonly Input input;

		public VertexSource(Input input)
		{
			this.input = input;
		}

		public Vector3 GetVector3(int index)
		{
			return this.input.SourceData.GetVector3(index);
		}

		public Color GetColor(int index)
		{
			throw new NotImplementedException();
		}
	}
}