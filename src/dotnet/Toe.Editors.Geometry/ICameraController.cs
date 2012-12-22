using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Toe.Editors.Geometry
{
	public interface ICameraController
	{
		#region Public Properties

		EditorCamera Camera { get; set; }

		#endregion

		#region Public Methods and Operators

		void Attach(GLControl gl);

		void Detach(GLControl gl);

		void MouseEnter();

		void MouseLeave();

		void MouseMove(MouseButtons button, Point location);

		#endregion
	}
}