using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Toe.Editors
{
	public class GameCameraController : ICameraController
	{
		#region Constants and Fields

		private Point? lastKnownMousePosition;

		#endregion

		#region Public Properties

		public EditorCamera Camera { get; set; }

		#endregion

		#region Public Methods and Operators

		public void Attach(GLControl gl)
		{
		}

		public void Detach(GLControl gl)
		{
		}

		public void GotFocus()
		{
		}

		public void KeyDown(KeyEventArgs keyEventArgs)
		{
			throw new NotImplementedException();
		}

		public void KeyUp(KeyEventArgs keyEventArgs)
		{
			throw new NotImplementedException();
		}

		public void LostFocus()
		{
		}

		public void MouseEnter()
		{
			this.lastKnownMousePosition = null;
		}

		public void MouseLeave()
		{
			this.lastKnownMousePosition = null;
		}

		public void MouseMove(MouseButtons button, Point location)
		{
			if (this.lastKnownMousePosition != null)
			{
				float dx = location.X - this.lastKnownMousePosition.Value.X;
				float dy = location.Y - this.lastKnownMousePosition.Value.Y;
				if (MouseButtons.Left == (button & MouseButtons.Left))
				{
					if (dx != 0)
					{
						var r = Quaternion.FromAxisAngle(this.Camera.WorldUp, -dx / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
					}
					if (dy != 0)
					{
						var right = this.Camera.Right;
						var r = Quaternion.FromAxisAngle(right, -dy / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
					}
				}
			}
			this.lastKnownMousePosition = location;
		}

		public void MouseWheel(float delta, Point location)
		{
		}

		#endregion
	}
}