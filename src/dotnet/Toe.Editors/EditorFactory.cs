using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;
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
			if (e.EndsWith(".geo"))
			{
				return new GeometryEditor(new GeoReader(), null);
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