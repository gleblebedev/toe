using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene conainer.
	/// </summary>
	public class Scene : SceneItem, IScene
	{
		#region Constants and Fields

		/// <summary>
		/// Visible surface determination provider changed event arguments.
		/// </summary>
		protected static PropertyEventArgs VsdProviderEventArgs = Expr.PropertyEventArgs<Scene>(x => x.VsdProvider);

		/// <summary>
		/// Collection of effects.
		/// </summary>
		private readonly IList<IEffect> effects = new ObservableCollection<IEffect>();

		/// <summary>
		/// Geometries (meshes, shapes, etc.)
		/// </summary>
		private readonly IList<IMesh> geometries = new ObservableCollection<IMesh>();

		/// <summary>
		/// Collection of images.
		/// </summary>
		private readonly IList<IImage> images = new ObservableCollection<IImage>();

		/// <summary>
		/// Collection of materials.
		/// </summary>
		private readonly IList<IMaterial> materials = new ObservableCollection<IMaterial>();

		/// <summary>
		/// Scene nodes.
		/// </summary>
		private readonly IList<INode> nodes = new ObservableCollection<INode>();

		/// <summary>
		/// Visible surface determination provider.
		/// </summary>
		private IVsdProvider vsdProvider;

		#endregion

		#region Public Properties

		/// <summary>
		/// Collection of effects.
		/// </summary>
		public IList<IEffect> Effects
		{
			get
			{
				return this.effects;
			}
		}

		/// <summary>
		/// Geometries (meshes, shapes, etc.)
		/// </summary>
		public IList<IMesh> Geometries
		{
			get
			{
				return this.geometries;
			}
		}

		/// <summary>
		/// Collection of images.
		/// </summary>
		public IList<IImage> Images
		{
			get
			{
				return this.images;
			}
		}

		/// <summary>
		/// Collection of materials.
		/// </summary>
		public IList<IMaterial> Materials
		{
			get
			{
				return this.materials;
			}
		}

		/// <summary>
		/// Scene nodes.
		/// </summary>
		public IList<INode> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		/// <summary>
		/// Visible surface determination provider.
		/// </summary>
		public IVsdProvider VsdProvider
		{
			get
			{
				return this.vsdProvider;
			}
			set
			{
				if (this.vsdProvider != value)
				{
					this.RaisePropertyChanging(VsdProviderEventArgs.Changing);
					this.vsdProvider = value;
					this.RaisePropertyChanged(VsdProviderEventArgs.Changed);
				}
			}
		}

		#endregion

		
	}
}