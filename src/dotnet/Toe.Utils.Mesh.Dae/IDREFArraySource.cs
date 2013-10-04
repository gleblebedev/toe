using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class IDREFArraySource : BaseArraySource<string>, ISource
	{

		#region Constructors and Destructors

		public IDREFArraySource(XElement source, ColladaSchema schema)
			: base(source, schema)
		{
			var elements = this.SplitElements(schema.floatArrayName);
			this.data = new string[elements.Count];
			for (int index = 0; index < elements.Count; index++)
			{
				this.data[index] = elements[index];
			}
		}


		#endregion

		#region Implementation of ISource



		#endregion
	}
}