using System;
using System.Collections.Generic;
using System.Linq;

using Toe.Utils.ToeMath;

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
		public static INode FindNode(this INodeContainer scene,Func<INode,bool> predicate)
		{
			return GetAllNodes(scene).FirstOrDefault(predicate);
		}
		
		public static BoundingBox GetBoundingBox(this INode node)
		{
			if (node.Mesh == null) return BoundingBox.Empty;
			return new BoundingBox(
				Float3.Multiply(node.Mesh.BoundingBoxMin, node.Scale) + node.Position,
				Float3.Multiply(node.Mesh.BoundingBoxMax, node.Scale) + node.Position);

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