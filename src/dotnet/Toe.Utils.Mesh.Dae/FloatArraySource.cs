using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

using OpenTK;

namespace Toe.Utils.Mesh.Dae
{
	public class FloatArraySource : BaseArraySource<float>, ISource
	{

		#region Constructors and Destructors

		public FloatArraySource(XElement source, ColladaSchema schema):base(source,schema)
		{
			var elements = SplitElements(schema.floatArrayName);
			this.data = new float[elements.Count];
			for (int index = 0; index < elements.Count; index++)
			{
				this.data[index] = float.Parse(elements[index], CultureInfo.InvariantCulture);
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
}