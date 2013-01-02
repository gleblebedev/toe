using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toe.Gx
{
	public class ToeGraphicsContext
	{
		private bool flipCulling = true;

		private bool isShadersEnabled = false;

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


	}
}
