using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Toe.Editors.Geometry
{
	public class Autodesk3DMaxCompatibleController : ICameraController
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
						var r = Quaternion.FromAxisAngle(new Vector3(0, 0, 1), -dx / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
						this.Camera.Pos = Vector3.Transform(this.Camera.Pos, r);
					}
					if (dy != 0)
					{
						var right = Vector3.Transform(new Vector3(1, 0, 0), this.Camera.Rot);
						var r = Quaternion.FromAxisAngle(right, -dy / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
						this.Camera.Pos = Vector3.Transform(this.Camera.Pos, r);
					}
				}
				if (MouseButtons.Middle == (button & MouseButtons.Middle))
				{
					var right = Vector3.Transform(new Vector3(1, 0, 0), this.Camera.Rot);
					var up = Vector3.Transform(new Vector3(0, 1, 0), this.Camera.Rot);
					this.Camera.Pos += - dx * right + dy * up;
				}
			}
			this.lastKnownMousePosition = location;
		}

		#endregion
	}
}