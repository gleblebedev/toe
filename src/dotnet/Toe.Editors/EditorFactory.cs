using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class EditorFactory
	{
		public IResourceEditor CreateEditor(string getExtension)
		{
			return new GeometryEditor();
		}
	}
}
