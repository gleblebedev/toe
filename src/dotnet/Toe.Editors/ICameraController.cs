using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Toe.Editors
{
	public interface ICameraController
	{
		#region Public Properties

		EditorCamera Camera { get; set; }

		float TargetDistance { get; set; }

		#endregion

		#region Public Methods and Operators

		void Attach(GLControl gl);

		void Detach(GLControl gl);

		void GotFocus();

		void KeyDown(KeyEventArgs keyEventArgs);

		void KeyUp(KeyEventArgs keyEventArgs);

		void LostFocus();

		void MouseEnter();

		void MouseLeave();

		void MouseMove(MouseButtons button, Point location);

		void MouseWheel(float delta, Point location);

		#endregion
	}
}