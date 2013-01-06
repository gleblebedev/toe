using System;

using OpenTK.Graphics;

namespace Toe.Gx
{
	public class ToeGraphicsContext : IDisposable
	{
		#region Constants and Fields

		private GraphicsContext context;

		private bool flipCulling = true;

		#endregion

		#region Constructors and Destructors

		public ToeGraphicsContext()
		{
			GraphicsContext.ShareContexts = true;
			//context = OpenTK.Graphics.GraphicsContext.CreateDummyContext();
		}

		~ToeGraphicsContext()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Properties

		public bool FlipCulling
		{
			get
			{
				return this.flipCulling;
			}
			set
			{
				this.flipCulling = value;
			}
		}

		public bool IsShadersEnabled { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Methods

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.context != null)
				{
					this.context.Dispose();
					this.context = null;
				}
			}
		}

		#endregion
	}
}