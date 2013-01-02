using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace Toe.Gx
{
	public class ToeGraphicsContext:IDisposable
	{
		public ToeGraphicsContext()
		{
			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
			//context = OpenTK.Graphics.GraphicsContext.CreateDummyContext();
		}
		~ToeGraphicsContext()
		{
			this.Dispose(false);
		}
		private bool flipCulling = true;

		private bool isShadersEnabled = false;

		private GraphicsContext context;

		public bool IsShadersEnabled
		{
			get
			{
				return isShadersEnabled;
			}
			set
			{
				isShadersEnabled = value;
			}
		}

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

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected  virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (context != null)
				{
					context.Dispose();
					context = null;
				}
			}
		}

		#endregion
	}
}
