using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public class SceneEffect:SceneItem, IEffect
	{

		protected static PropertyChangedEventArgs DiffuseChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<SceneEffect>(a => a.Diffuse));

		private IColorSource diffuse;

		public IColorSource Diffuse
		{
			get
			{
				return this.diffuse;
			}
			set
			{
				if (this.diffuse != value)
				{
					this.diffuse = value;
					this.RaisePropertyChanged(DiffuseChangedEventArgs);
				}
			}
		}
	}
}