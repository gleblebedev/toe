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
		Vector3 GetVector3(int index);

		Color GetColor(int index);
	}

	public class Accessor
	{
		private int stride;

		public Accessor(XElement accessorElement, ColladaSchema schema)
		{
			stride = int.Parse(accessorElement.Attribute(schema.strideName).Value, CultureInfo.InvariantCulture);
		}

		public int Stride
		{
			get
			{
				return this.stride;
			}
		}
	}
	public class FloatArraySource : ISource
	{
		private float[] data;

		private Accessor accessor;
		public FloatArraySource(XElement source, ColladaSchema schema)
		{
			var techniqueCommon = source.Element(schema.techniqueCommonName);
			var accessorElement = techniqueCommon.Element(schema.accessorName);
			this.accessor = new Accessor(accessorElement, schema);
			var floatArray = source.Element(schema.floatArrayName);
			data = new float[int.Parse(floatArray.Attribute(schema.countName).Value, CultureInfo.InvariantCulture)];
			var strValues = floatArray.Value.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			for (int index = 0; index < strValues.Length; index++)
			{
				data[index] = float.Parse(strValues[index], CultureInfo.InvariantCulture);
			}
		}

		public Vector3 GetVector3(int index)
		{
			var s = accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
				return new Vector3(this.data[index0+0],0,0);
			if (s <= 2)
				return new Vector3(this.data[index0 + 0], this.data[index0 + 1], 0);
			return new Vector3(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2]);
		}
		public Vector4 GetVector4(int index)
		{
			var s = accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
				return new Vector4(this.data[index0 + 0], 0, 0, 0);
			if (s <= 2)
				return new Vector4(this.data[index0 + 0], this.data[index0 + 1], 0, 0);
			if (s <= 3)
				return new Vector4(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2], 0);
			return new Vector4(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2], this.data[index0 + 3]);
		}

		public Color GetColor(int index)
		{
			var s = accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
				return Color.FromArgb(255, ClampColor(index0 + 0), ClampColor(index0 + 0), ClampColor(index0 + 0));
			if (s <= 2)
				return Color.FromArgb(255, ClampColor(index0 + 0), ClampColor(index0 + 1), 0);
			if (s <= 3)
				return Color.FromArgb(255, ClampColor(index0 + 0), ClampColor(index0 + 1), ClampColor(index0 + 2));
			return Color.FromArgb(ClampColor(index0 + 3), ClampColor(index0 + 0), ClampColor(index0 + 1), ClampColor(index0 + 2));
		}

		private int ClampColor(int index)
		{
			var d = data[index];
			if (d <= 0) return 0;
			if (d >= 1) return 255;
			return (int)(d * 255);
		}
	}
	public class Input
	{
		private int set;

		private int offset;

		private string semantic;

		private XElement sourceElement;

		private string source;

		private ISource sourceData;

		public Input(XElement input, ColladaSchema schema, XElement mesh)
		{
			var setAttr = input.Attribute(schema.setName);
			if (setAttr != null) set = int.Parse(setAttr.Value, CultureInfo.InvariantCulture);
			var offsetAttr = input.Attribute(schema.offsetName);
			if (offsetAttr != null) this.offset = int.Parse(offsetAttr.Value, CultureInfo.InvariantCulture);
			this.semantic = input.Attribute(schema.semanticName).Value;
			this.source = input.Attribute(schema.sourceAttributeName).Value;
			if (this.source[0] != '#')
				throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Invalid element reference {0}", this.source));
			var id = this.source.Substring(1);
			this.sourceElement = (from node in mesh.Descendants() let a = node.Attribute(schema.idName) where a != null && a.Value == id select node).First();
			if (this.sourceElement.Name == schema.verticesName)
			{
				this.sourceData = ParseVerticesSource(this.sourceElement, schema, mesh);
			}
			else if (this.sourceElement.Name == schema.sourceName)
			{
				this.sourceData = ParseSource(this.sourceElement, schema, mesh);
			}
			else
			{
				throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", this.sourceElement.Name));
			}
		}

		private ISource ParseSource(XElement xElement, ColladaSchema schema, XElement mesh)
		{
			var floatArray = xElement.Element(schema.floatArrayName);
			if (floatArray != null)
			{
				return ParseFloatArraySource(xElement, schema);
			}
			throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", this.sourceElement.Name));
		}

		private ISource ParseFloatArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new FloatArraySource(xElement,schema);
			return s;
		}

		private ISource ParseVerticesSource(XElement xElement, ColladaSchema schema, XElement mesh)
		{
			var s = new VertexSource(new Input(xElement.Element(schema.inputName), schema, mesh));
			return s;
		}

		public int Set
		{
			get
			{
				return this.set;
			}
		}

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

		public XElement SourceElement
		{
			get
			{
				return this.sourceElement;
			}
		}

		public ISource SourceData
		{
			get
			{
				return this.sourceData;
			}
		}
	}
}