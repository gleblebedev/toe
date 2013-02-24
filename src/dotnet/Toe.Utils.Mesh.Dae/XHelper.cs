using System.Linq;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public static class XHelper
	{
		#region Constants and Fields

		private static readonly XName idName = XName.Get("id", string.Empty);

		#endregion

		#region Public Methods and Operators

		public static string AttributeValue(this XElement root, XName name)
		{
			if (root == null)
			{
				return null;
			}
			if (name == null)
			{
				return null;
			}
			var a = root.Attribute(name);
			if (a == null)
			{
				return null;
			}
			return a.Value;
		}

		public static XElement DescendantByAttr(this XElement root, XName attrName, string value)
		{
			return
				(from node in root.Descendants() let a = node.Attribute(attrName) where a != null && a.Value == value select node).
					FirstOrDefault();
		}

		public static XElement ElementByAttr(this XElement root, XName attrName, string value)
		{
			return
				(from node in root.Elements() let a = node.Attribute(attrName) where a != null && a.Value == value select node).
					FirstOrDefault();
		}

		public static XElement ElementById(this XElement root, string value)
		{
			return
				(from node in root.Elements() let a = node.Attribute(idName) where a != null && a.Value == value select node).
					FirstOrDefault();
		}

		public static XElement ElementByUrl(this XElement root, string value)
		{
			if (root == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			if (!value.StartsWith("#"))
			{
				throw new DaeException("Url should start with #");
			}
			return root.ElementById(value.Substring(1));
		}

		public static string ElementValue(this XElement root, XName name)
		{
			if (root == null)
			{
				return null;
			}
			if (name == null)
			{
				return null;
			}
			var a = root.Element(name);
			if (a == null)
			{
				return null;
			}
			return a.Value;
		}

		#endregion
	}
}