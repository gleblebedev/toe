using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class IntArraySource : BaseArraySource<int>, ISource
	{
		#region Constructors and Destructors

		public IntArraySource(XElement source, ColladaSchema schema)
			: base(source, schema)
		{
			var elements = SplitElements(schema.floatArrayName);
			this.data = new int[elements.Count];
			for (int index = 0; index < elements.Count; index++)
			{
				this.data[index] = int.Parse(elements[index],CultureInfo.InvariantCulture);
			}
		}


		#endregion

		#region Implementation of ISource


		#endregion
	}
}