namespace Toe.Utils.Mesh.Marmalade.Tests
{
	internal class Program
	{
		#region Methods

		private static void Main(string[] args)
		{
			//(new TestGeo()).TestLegs();
			//(new TestSkin()).TestLegs();
			//(new TestSkel()).TestLegs();
			var testGroup = (new TestGroup());
			try
			{
				testGroup.TestFixtureSetUp();
				testGroup.TestMarmaladeFolderForBinary();
			}
			finally
			{
				testGroup.TestFixtureTearDown();
			}
		}

		#endregion
	}
}