using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material.
	/// </summary>
	public class SceneMaterial : SceneItem, IMaterial
	{
		#region Constants and Fields

		protected static PropertyChangedEventArgs EffectChangedEventArgs =
			new PropertyChangedEventArgs(Expr.Path<SceneMaterial>(a => a.Effect));

		private IEffect effect;

		#endregion

		#region Public Properties

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
					this.RaisePropertyChanged(EffectChangedEventArgs);
				}
			}
		}

		#endregion
	}
}