using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene conainer interface.
	/// </summary>
	public interface IScene : ISceneItem, INodeContainer
	{
		#region Public Properties

		IList<IEffect> Effects { get; }

		IList<IMesh> Geometries { get; }

		IList<IImage> Images { get; }

		IList<IMaterial> Materials { get; }

		/// <summary>
		/// Visible surface determination provider.
		/// </summary>
		IVsdProvider VsdProvider { get; set; }

		#endregion

	}
	public static class ExtensionMethods
	{
		public static BoundingBox GetBoundingBox(this INode node)
		{
			if (node.Mesh == null) return BoundingBox.Empty;
			return new BoundingBox(
				Vector3.Multiply(node.Mesh.BoundingBoxMin, node.Scale) + node.Position,
				Vector3.Multiply(node.Mesh.BoundingBoxMax, node.Scale) + node.Position);

		}
		public static BoundingBox GetBoundingBox(this IScene scene)
		{
			if (scene == null) return BoundingBox.Zero;
			BoundingBox box = BoundingBox.Zero;
			foreach (var node in GetAllNodes(scene))
			{
				box = box.Union(node.GetBoundingBox());
			}
			return box;
		}
		public static IEnumerable<INode> GetAllNodes(this INodeContainer scene)
		{
			if (scene == null)
				yield break;
			foreach (var node in scene.Nodes)
			{
				yield return node;
				foreach (var subnodes in GetAllNodes(node))
				{
					yield return subnodes;
				}
			}
		}
	}
}