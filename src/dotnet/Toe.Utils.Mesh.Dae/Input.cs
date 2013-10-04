using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using OpenTK;

namespace Toe.Utils.Mesh.Dae
{
	public interface ISource
	{
		#region Public Methods and Operators

		//Color GetColor(int index);

		//Vector3 GetVector3(int index);


		Type GetSourceType();

		#endregion

		int GetStride();

		int GetCount();
	}

	public class Input
	{
		#region Constants and Fields

		private readonly int offset;

		private readonly string semantic;

		private readonly int set;

		private readonly string source;

		private readonly ISource sourceData;

		private readonly XElement sourceElement;

		#endregion

		#region Constructors and Destructors

		public Input(XElement input, ColladaSchema schema)
		{
			var setAttr = input.Attribute(schema.setName);
			if (setAttr != null)
			{
				this.set = int.Parse(setAttr.Value, CultureInfo.InvariantCulture);
			}
			var offsetAttr = input.Attribute(schema.offsetName);
			if (offsetAttr != null)
			{
				this.offset = int.Parse(offsetAttr.Value, CultureInfo.InvariantCulture);
			}
			this.semantic = input.Attribute(schema.semanticName).Value;
			this.source = input.Attribute(schema.sourceAttributeName).Value;
			//if (this.source[0] != '#')
			//{
			//	throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Invalid element reference {0}", this.source));
			//}
			//var id = this.source.Substring(1);
			//this.sourceElement =
			//	(from node in mesh.Descendants() let a = node.Attribute(schema.idName) where a != null && a.Value == id select node)
			//		.First();
			//if (this.sourceElement.Name == schema.verticesName)
			//{
			//	this.sourceData = this.ParseVerticesSource(this.sourceElement, schema, mesh);
			//}
			//else if (this.sourceElement.Name == schema.sourceName)
			//{
			//	this.sourceData = this.ParseSource(this.sourceElement, schema, mesh);
			//}
			//else
			//{
			//	throw new DaeException(
			//		string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", this.sourceElement.Name));
			//}
		}

		#endregion

		#region Public Properties

		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		public string Semantic
		{
			get
			{
				return this.semantic;
			}
		}

		public int Set
		{
			get
			{
				return this.set;
			}
		}

		public ISource SourceData
		{
			get
			{
				return this.sourceData;
			}
		}

		public XElement SourceElement
		{
			get
			{
				return this.sourceElement;
			}
		}

		#endregion

		#region Methods

		

		
		//private ISource ParseVerticesSource(XElement xElement, ColladaSchema schema, XElement mesh)
		//{
		//	var s = new VertexSource(new Input(xElement.Element(schema.inputName), schema, mesh));
		//	return s;
		//}

		#endregion
	}
}