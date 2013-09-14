using System.Collections.Generic;
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

		private IList<IMaterial> submaterials = new List<IMaterial>();

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

		public IList<IMaterial> Submaterials
		{
			get
			{
				return this.submaterials;
			}
			set
			{
				this.submaterials = value;
			}
		}

		#endregion
	}
}