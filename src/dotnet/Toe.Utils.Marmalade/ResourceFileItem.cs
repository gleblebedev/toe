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

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

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
	}
}