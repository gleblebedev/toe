using Toe.CircularArrayQueue;

namespace Toe.Messaging.Types
{
	public static class MathHelper
	{
		#region Public Methods and Operators

		public static float InverseSqrtFast(float x)
		{
			BufferItem item = new BufferItem();
			item.Single = x;

			float xhalf = 0.5f * item.Single;
			int i = item.Int32; // Read bits as integer.
			item.Int32 = 0x5f375a86 - (i >> 1); // Make an initial guess for Newton-Raphson approximation
			return item.Single * (1.5f - xhalf * item.Single * item.Single); // Perform left single Newton-Raphson step.
		}

		#endregion
	}
}