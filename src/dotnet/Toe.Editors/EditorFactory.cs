using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Editors
{
	public class EditorFactory
	{
		#region Public Methods and Operators

		public IResourceEditor CreateEditor(string getExtension)
		{
			var e = getExtension.ToLower();
			switch (e)
			{
				case ".geo":
					return new GeometryEditor(new GeoReader(), null);
				default:
					return new DefaultEditor();
			}
		}

		#endregion
	}
}