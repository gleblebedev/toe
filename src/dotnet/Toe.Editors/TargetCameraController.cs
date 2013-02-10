using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Toe.Editors
{
	public class TargetCameraController : ICameraController
	{
		#region Constants and Fields

		private readonly Timer animationTimer;

		private bool cameraForward;

		private Vector3 cameraVelocity;

		private Point? lastKnownMousePosition;

		private float targetDistance = 1024;

		private bool cameraBackward;

		private bool cameraStrifeLeft;

		private bool cameraStrifeRight;

		#endregion

		#region Constructors and Destructors

		public TargetCameraController()
		{
			this.animationTimer = new Timer();
			this.animationTimer.Interval = 1000 / 30;
			this.animationTimer.Tick += this.OnKeyAnimationTimer;
		}

		#endregion

		#region Public Properties

		public EditorCamera Camera { get; set; }

		public float TargetDistance
		{
			get
			{
				return this.targetDistance;
			}
			set
			{
				this.targetDistance = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Attach(GLControl gl)
		{
		}

		public void Detach(GLControl gl)
		{
		}

		public void KeyDown(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.W:
					this.cameraForward = true;
					break;
				case Keys.S:
					this.cameraBackward = true;
					break;
				case Keys.A:
					this.cameraStrifeLeft = true;
					break;
				case Keys.D:
					this.cameraStrifeRight = true;
					break;
			}
			this.EnableAnimationIfNecessery();
		}

		public void KeyUp(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.W:
					this.cameraForward = false;
					break;
				case Keys.S:
					this.cameraBackward = false;
					break;
				case Keys.A:
					this.cameraStrifeLeft = false;
					break;
				case Keys.D:
					this.cameraStrifeRight = false;
					break;
			}
			this.EnableAnimationIfNecessery();
		}

		public void LostFocus()
		{
			cameraForward = false;
			this.EnableAnimationIfNecessery();
		}

		public void GotFocus()
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
				var target = this.Camera.Pos + this.Camera.Forward * this.targetDistance;
				if (MouseButtons.Left == (button & MouseButtons.Left))
				{
					if (dx != 0)
					{
						var r = Quaternion.FromAxisAngle(this.Camera.WorldUp, -dx / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
						this.Camera.Pos = Vector3.Transform(this.Camera.Pos - target, r) + target;
					}
					if (dy != 0)
					{
						var right = this.Camera.Right;
						var r = Quaternion.FromAxisAngle(right, -dy / 100.0f);
						this.Camera.Rot = r * this.Camera.Rot;
						this.Camera.Pos = Vector3.Transform(this.Camera.Pos - target, r) + target;
					}
				}
				if (MouseButtons.Middle == (button & MouseButtons.Middle))
				{
					var right = this.Camera.Right;
					var up = this.Camera.Up;
					this.Camera.Pos += - dx * right + dy * up;
				}
			}
			this.lastKnownMousePosition = location;
		}

		public void MouseWheel(float delta, Point location)
		{
			this.Camera.Pos = this.Camera.Pos + this.Camera.Forward * delta;
		}

		#endregion

		#region Methods

		private void EnableAnimationIfNecessery()
		{
			if (this.cameraForward || this.cameraBackward || this.cameraStrifeLeft || this.cameraStrifeRight)
			{
				this.animationTimer.Start();
			}
			else
			{
				this.animationTimer.Stop();
			}
		}

		private void OnKeyAnimationTimer(object sender, EventArgs e)
		{
			this.UpdateCameraVelocity();
			this.Camera.Pos += this.cameraVelocity;
		}

		private void UpdateCameraVelocity()
		{
			Vector3 v = Vector3.Zero;
			float speed = 10.0f;
			if (this.cameraForward)
			{
				v += this.Camera.Forward;
			}
			if (this.cameraBackward)
			{
				v -= this.Camera.Forward;
			}
			if (this.cameraStrifeRight)
			{
				v += this.Camera.Right;
			}
			if (this.cameraStrifeLeft)
			{
				v -= this.Camera.Right;
			}
			v *= speed;
			this.cameraVelocity = v;
		}

		#endregion
	}
}