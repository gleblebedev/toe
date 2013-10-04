using System.Globalization;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class Accessor
	{
		#region Constants and Fields

		private readonly int stride;

		private int count;

		#endregion

		#region Constructors and Destructors

		public Accessor(XElement accessorElement, ColladaSchema schema)
		{
			var attribute = accessorElement.Attribute(schema.strideName);
			if (attribute != null)
			{
				this.stride = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
			}
			else
			{
				this.stride = 1;
			}
			var xAttribute = accessorElement.Attribute(schema.countName);
			if (xAttribute != null)
			{
				this.count = int.Parse(xAttribute.Value, CultureInfo.InvariantCulture);
			}
		}

		#endregion

		#region Public Properties

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

		#endregion
	}
}