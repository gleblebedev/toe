using System.Collections.ObjectModel;

namespace Toe.Core
{
	public class ToeSceneConfiguration
	{
		#region Constants and Fields

		private readonly ObservableCollection<ToeSystemConfiguration> systems =
			new ObservableCollection<ToeSystemConfiguration>();

		private int numClientEntities = 16384;

		private int numServerEntities = 16384;

		#endregion

		#region Public Properties

		public int NumClientEntities
		{
			get
			{
				return this.numClientEntities;
			}
			set
			{
				this.numClientEntities = value;
			}
		}

		public int NumServerEntities
		{
			get
			{
				return this.numServerEntities;
			}
			set
			{
				this.numServerEntities = value;
			}
		}

		public ObservableCollection<ToeSystemConfiguration> Systems
		{
			get
			{
				return this.systems;
			}
		}

		#endregion
	}
}