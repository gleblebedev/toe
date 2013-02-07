using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene conainer.
	/// </summary>
	public class Scene : IScene
	{
		#region Constants and Fields

		private readonly IList<IMesh> geometries = new List<IMesh>();

		private readonly IList<INode> nodes = new List<INode>();

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		#endregion

		#region Public Properties

		public IList<IMesh> Geometries
		{
			get
			{
				return this.geometries;
			}
		}

		public string Name { get; set; }

		public IList<INode> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.parameters = new DynamicCollection());
			}
			set
			{
				this.parameters = value;
			}
		}

		#endregion
	}
}