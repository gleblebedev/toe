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

		Color GetColor(int index);

		Vector3 GetVector3(int index);

		#endregion
	}

	public class Accessor
	{
		#region Constants and Fields

		private readonly int stride;

		#endregion

		#region Constructors and Destructors

		public Accessor(XElement accessorElement, ColladaSchema schema)
		{
			this.stride = int.Parse(accessorElement.Attribute(schema.strideName).Value, CultureInfo.InvariantCulture);
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

		#endregion
	}

	public class FloatArraySource : ISource
	{
		#region Constants and Fields

		private readonly Accessor accessor;

		private readonly float[] data;

		#endregion

		#region Constructors and Destructors

		public FloatArraySource(XElement source, ColladaSchema schema)
		{
			var techniqueCommon = source.Element(schema.techniqueCommonName);
			var accessorElement = techniqueCommon.Element(schema.accessorName);
			this.accessor = new Accessor(accessorElement, schema);
			var floatArray = source.Element(schema.floatArrayName);
			this.data = new float[int.Parse(floatArray.Attribute(schema.countName).Value, CultureInfo.InvariantCulture)];
			var strValues = floatArray.Value.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			for (int index = 0; index < strValues.Length; index++)
			{
				this.data[index] = float.Parse(strValues[index], CultureInfo.InvariantCulture);
			}
		}

		#endregion

		#region Public Methods and Operators

		public Color GetColor(int index)
		{
			var s = this.accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
			{
				return Color.FromArgb(255, this.ClampColor(index0 + 0), this.ClampColor(index0 + 0), this.ClampColor(index0 + 0));
			}
			if (s <= 2)
			{
				return Color.FromArgb(255, this.ClampColor(index0 + 0), this.ClampColor(index0 + 1), 0);
			}
			if (s <= 3)
			{
				return Color.FromArgb(255, this.ClampColor(index0 + 0), this.ClampColor(index0 + 1), this.ClampColor(index0 + 2));
			}
			return Color.FromArgb(
				this.ClampColor(index0 + 3), this.ClampColor(index0 + 0), this.ClampColor(index0 + 1), this.ClampColor(index0 + 2));
		}

		public Vector3 GetVector3(int index)
		{
			var s = this.accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
			{
				return new Vector3(this.data[index0 + 0], 0, 0);
			}
			if (s <= 2)
			{
				return new Vector3(this.data[index0 + 0], this.data[index0 + 1], 0);
			}
			return new Vector3(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2]);
		}

		public Vector4 GetVector4(int index)
		{
			var s = this.accessor.Stride;
			var index0 = index * s;
			if (s <= 1)
			{
				return new Vector4(this.data[index0 + 0], 0, 0, 0);
			}
			if (s <= 2)
			{
				return new Vector4(this.data[index0 + 0], this.data[index0 + 1], 0, 0);
			}
			if (s <= 3)
			{
				return new Vector4(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2], 0);
			}
			return new Vector4(this.data[index0 + 0], this.data[index0 + 1], this.data[index0 + 2], this.data[index0 + 3]);
		}

		#endregion

		#region Methods

		private int ClampColor(int index)
		{
			var d = this.data[index];
			if (d <= 0)
			{
				return 0;
			}
			if (d >= 1)
			{
				return 255;
			}
			return (int)(d * 255);
		}

		#endregion
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

		public Input(XElement input, ColladaSchema schema, XElement mesh)
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
			if (this.source[0] != '#')
			{
				throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Invalid element reference {0}", this.source));
			}
			var id = this.source.Substring(1);
			this.sourceElement =
				(from node in mesh.Descendants() let a = node.Attribute(schema.idName) where a != null && a.Value == id select node)
					.First();
			if (this.sourceElement.Name == schema.verticesName)
			{
				this.sourceData = this.ParseVerticesSource(this.sourceElement, schema, mesh);
			}
			else if (this.sourceElement.Name == schema.sourceName)
			{
				this.sourceData = this.ParseSource(this.sourceElement, schema, mesh);
			}
			else
			{
				throw new DaeException(
					string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", this.sourceElement.Name));
			}
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

		private ISource ParseFloatArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new FloatArraySource(xElement, schema);
			return s;
		}

		private ISource ParseSource(XElement xElement, ColladaSchema schema, XElement mesh)
		{
			var floatArray = xElement.Element(schema.floatArrayName);
			if (floatArray != null)
			{
				return this.ParseFloatArraySource(xElement, schema);
			}
			throw new DaeException(
				string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", this.sourceElement.Name));
		}

		private ISource ParseVerticesSource(XElement xElement, ColladaSchema schema, XElement mesh)
		{
			var s = new VertexSource(new Input(xElement.Element(schema.inputName), schema, mesh));
			return s;
		}

		#endregion
	}
}