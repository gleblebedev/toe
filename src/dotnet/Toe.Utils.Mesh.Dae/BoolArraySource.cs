using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class BoolArraySource : BaseArraySource<bool>, ISource
	{

		#region Constructors and Destructors

		public BoolArraySource(XElement source, ColladaSchema schema):base(source,schema)
		{
			var elements = this.SplitElements(schema.floatArrayName);
			this.data = new bool[elements.Count];
			for (int index = 0; index < elements.Count; index++)
			{
				this.data[index] = bool.Parse(elements[index]);
			}
		}


		#endregion

		#region Implementation of ISource

	

		#endregion
	}
}