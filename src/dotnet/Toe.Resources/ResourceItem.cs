using System.ComponentModel;

namespace Toe.Resources
{
	public class ResourceItem : IResourceItem, INotifyPropertyChanged
	{
		private readonly ResourceManager manager;

		private readonly uint type;

		private readonly uint hash;

		private object value;

		public ResourceItem(ResourceManager manager, uint type, uint hash)
		{
			this.manager = manager;
			this.type = type;
			this.hash = hash;
		}

		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					this.value = value;
					this.RaisePropertyChanged("Value");
				}
			}
		}

		public uint Hash
		{
			get
			{
				return this.hash;
			}
		}

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		public ResourceManager Manager
		{
			get
			{
				return this.manager;
			}
		}

		public void Provide(object value)
		{
			this.value = value;
		}
		protected virtual void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged!=null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}