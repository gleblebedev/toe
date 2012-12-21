using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Editors
{
	public class EditorFactory
	{
		public IResourceEditor CreateEditor(string getExtension)
		{
			var e = getExtension.ToLower();
			switch (e)
			{
				case ".geo":
					return new GeometryEditor(new GeoReader());
				default:
					return new DefaultEditor();
			}
		}
	}

}
