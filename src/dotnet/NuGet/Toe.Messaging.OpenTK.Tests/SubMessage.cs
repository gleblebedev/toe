using OpenTK;

using Toe.Messaging.Attributes;

namespace Toe.Messaging.OpenTK.Tests
{
	public class SubMessage
	{
		#region Public Properties

		[PropertyType(PropertyTypes.VectorXYZ)]
		public Vector3 Vector3 { get; set; }

		#endregion
	}
}