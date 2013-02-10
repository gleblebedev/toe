using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public class MaterialBinding : ClassWithNotification, IMaterialBinding
	{
		private static PropertyEventArgs MaterialEventArgs = Expr.PropertyEventArgs<MaterialBinding>(x => x.Material);

		public MaterialBinding(string target)
		{
			this.target = target;
		}

		private string target;

		private IMaterial material;

		#region Implementation of IMaterialBinding

		public string Target
		{
			get
			{
				return target;
			}
		}

		public IMaterial Material
		{
			get
			{
				return material;
			}
			set
			{
				if (material != null)
				{
					this.RaisePropertyChanging(MaterialEventArgs.Changing);
					material = value;
					this.RaisePropertyChanged(MaterialEventArgs.Changed);
				}
			}
		}

		#endregion
	}
}