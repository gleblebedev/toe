namespace Toe.Utils.Mesh
{
	public class MaterialBinding : ClassWithNotification, IMaterialBinding
	{
		#region Constants and Fields

		private readonly string target;

		private static PropertyEventArgs MaterialEventArgs = Expr.PropertyEventArgs<MaterialBinding>(x => x.Material);

		private IMaterial material;

		#endregion

		#region Constructors and Destructors

		public MaterialBinding(string target)
		{
			this.target = target;
		}

		#endregion

		#region Public Properties

		public IMaterial Material
		{
			get
			{
				return this.material;
			}
			set
			{
				if (this.material != null)
				{
					this.RaisePropertyChanging(MaterialEventArgs.Changing);
					this.material = value;
					this.RaisePropertyChanged(MaterialEventArgs.Changed);
				}
			}
		}

		public string Target
		{
			get
			{
				return this.target;
			}
		}

		#endregion
	}
}