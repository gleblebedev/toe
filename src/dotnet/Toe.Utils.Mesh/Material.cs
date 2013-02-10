using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material.
	/// </summary>
	public class SceneMaterial:SceneItem, IMaterial
	{
		protected static PropertyChangedEventArgs EffectChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<SceneMaterial>(a=>a.Effect));

		private IEffect effect;

		public IEffect Effect
		{
			get
			{
				return this.effect;
			}
			set
			{
				if (this.effect != value)
				{
					this.effect = value;
					RaisePropertyChanged(EffectChangedEventArgs);
				}
			}
		}
	}
}