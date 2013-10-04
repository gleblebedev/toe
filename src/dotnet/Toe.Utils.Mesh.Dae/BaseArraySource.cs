using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class BaseArraySource<T>
	{
		protected readonly XElement source;

		protected readonly ColladaSchema schema;

		protected T[] data;

		protected Accessor accessor;

		public BaseArraySource(XElement source, ColladaSchema schema)
		{
			this.source = source;
			this.schema = schema;
			var techniqueCommon = source.Element(schema.techniqueCommonName);
			var accessorElement = techniqueCommon.Element(schema.accessorName);
			this.accessor = new Accessor(accessorElement, schema);
		}

		protected IList<string> SplitElements(XName floatArrayName)
		{
			var floatArray = source.Element(schema.floatArrayName);
			var strValues = floatArray.Value.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			var count = int.Parse(floatArray.Attribute(schema.countName).Value, CultureInfo.InvariantCulture);
			if (count != strValues.Length)
				throw new InvalidOperationException();
			return strValues;
		}
		public Type GetSourceType()
		{
			return typeof(T);
		}
		public int GetStride()
		{
			return accessor.Stride;
		}
		public int GetCount()
		{
			return accessor.Count;
		}
		public T this[int index]
		{
			get
			{
				return data[index];
			}
		}
	}
}