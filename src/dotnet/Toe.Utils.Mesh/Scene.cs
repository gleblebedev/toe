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
		/// Geometries (meshes, shapes, etc.)
		/// </summary>
		private readonly IList<IMesh> geometries = new ObservableCollection<IMesh>();

		/// <summary>
		/// Scene nodes.
		/// </summary>
		private readonly IList<INode> nodes = new ObservableCollection<INode>();

		/// <summary>
		/// Collection of effects.
		/// </summary>
		private readonly IList<IEffect> effects = new ObservableCollection<IEffect>();

		/// <summary>
		/// Collection of images.
		/// </summary>
		private readonly IList<IImage> images = new ObservableCollection<IImage>();

		/// <summary>
		/// Collection of materials.
		/// </summary>
		private readonly IList<IMaterial> materials = new ObservableCollection<IMaterial>();

		#endregion

		#region Public Properties

		/// <summary>
		/// Collection of images.
		/// </summary>
		public IList<IImage> Images
		{
			get
			{
				return images;
			}
		}

		/// <summary>
		/// Collection of effects.
		/// </summary>
		public IList<IEffect> Effects
		{
			get
			{
				return effects;
			}
		}

		/// <summary>
		/// Collection of materials.
		/// </summary>
		public IList<IMaterial> Materials
		{
			get
			{
				return materials;
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
		/// Scene nodes.
		/// </summary>
		public IList<INode> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		#endregion
	}
}