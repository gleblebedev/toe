using System.Collections.ObjectModel;

namespace Toe.Core
{
	public class ToeSceneConfiguration
	{
		private int numServerEntities = 16384;
		private int numClientEntities = 16384;

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

		public ObservableCollection<ToeSystemConfiguration> Systems
		{
			get
			{
				return this.systems;
			}
		}

		private ObservableCollection<ToeSystemConfiguration> systems = new ObservableCollection<ToeSystemConfiguration>();
	}
}