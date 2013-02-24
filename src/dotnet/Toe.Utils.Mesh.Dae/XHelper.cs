using System.Linq;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public static class XHelper
	{
		public static XElement DescendantByAttr(this XElement root, XName attrName, string value)
		{
			return
				(from node in root.Descendants() let a = node.Attribute(attrName) where a != null && a.Value == value select node)
					.FirstOrDefault();
		}
		public static XElement ElementByAttr(this XElement root, XName attrName, string value)
		{
			return
				(from node in root.Elements() let a = node.Attribute(attrName) where a != null && a.Value == value select node)
					.FirstOrDefault();
		}

		private static XName idName = XName.Get("id", string.Empty);

		public static XElement ElementById(this XElement root, string value)
		{

			return
				(from node in root.Elements() let a = node.Attribute(idName) where a != null && a.Value == value select node)
					.FirstOrDefault();
		}
	}
}