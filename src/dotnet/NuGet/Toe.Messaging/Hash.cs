namespace Toe.Messaging
{
	public static class Hash
	{
		#region Public Methods and Operators

		public static int Eval(string str)
		{
			return str.GetHashCode();
		}

		#endregion
	}
}