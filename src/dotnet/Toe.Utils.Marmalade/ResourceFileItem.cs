using System.ComponentModel;

using Toe.Utils.Mesh.Marmalade;

namespace Toe.Resources
{
	public class ResourceFileItem : IResourceFileItem, INotifyPropertyChanged
	{
		private readonly uint type;

		private readonly Managed resource;

		public ResourceFileItem(uint type, Managed resource)
		{
			this.type = type;
			this.resource = resource;
			this.resource.PropertyChanged += OnPropertyChanged;
			this.resource.PropertyChanging += OnPropertyChanging;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name") RaisePropertyChanged("Name");
			else if (e.PropertyName == "NameHash") RaisePropertyChanged("NameHash");
		}
		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "Name") RaisePropertyChanging("Name");
			else if (e.PropertyName == "NameHash") RaisePropertyChanging("NameHash");
		}
		protected virtual void RaisePropertyChanging(string propertyName)
		{
			if (PropertyChanging != null)
				PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Implementation of INotifyPropertyChanging

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		public object Resource
		{
			get
			{
				return this.resource;
			}
		}

		public string Name
		{
			get
			{
				return this.resource.Name;
			}
		}

		public uint NameHash
		{
			get
			{
				return this.resource.NameHash;
			}
		}

		public uint Type
		{
			get
			{
				return this.type;
			}
		}
	}
}