using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class MeshInputs
	{
		private List<Input> inputs = new List<Input>();

		private int stride;

		private int count;

		public MeshInputs(XElement element,ColladaSchema schema)
		{
			var countAttribute = element.Attribute(schema.countName);
			if (countAttribute != null)
			{
				this.count = int.Parse(countAttribute.Value, CultureInfo.InvariantCulture);
			}
			foreach (var xElement in element.Elements(schema.inputName))
			{
				inputs.Add(new Input(xElement, schema));
			}
			this.stride = inputs.Max(x => x.Offset)+1;
		}

		public List<Input> Inputs
		{
			get
			{
				return this.inputs;
			}
		}

		public int Stride
		{
			get
			{
				return this.stride;
			}
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}
	}
}