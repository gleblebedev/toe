using System;

using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwGraphics;

namespace Toe.Editors
{
	public class EditorFactory
	{
		#region Public Methods and Operators

		public IResourceEditor CreateEditor(string fileName)
		{
			var e = fileName.ToLower();

			if (e.EndsWith(".mtl", StringComparison.InvariantCultureIgnoreCase) ||
				e.EndsWith(".geo", StringComparison.InvariantCultureIgnoreCase) ||
				e.EndsWith(".skin", StringComparison.InvariantCultureIgnoreCase) ||
				e.EndsWith(".skel", StringComparison.InvariantCultureIgnoreCase) ||
				e.EndsWith(".anim", StringComparison.InvariantCultureIgnoreCase) ||
				e.EndsWith(".group", StringComparison.InvariantCultureIgnoreCase))
			{
				return new ResourceFileEditor();
			}
			
			if (e.EndsWith(".group.bin"))
			{
				return new DefaultEditor();
			}
			return null;
		}

		#endregion
	}
}