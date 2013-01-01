using System.ComponentModel;

using Toe.Utils.Marmalade;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Resources
{
	public class ResourceFileItem : IResourceFileItem, INotifyPropertyChanged
	{
		#region Constants and Fields

		private readonly Managed resource;

		private readonly uint type;

		#endregion

		#region Constructors and Destructors

		public ResourceFileItem(uint type, Managed resource)
		{
			this.type = type;
			this.resource = resource;
			this.resource.PropertyChanged += this.OnPropertyChanged;
			this.resource.PropertyChanging += this.OnPropertyChanging;
		}

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region Public Properties

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

		public object Resource
		{
			get
			{
				return this.resource;
			}
		}

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void RaisePropertyChanging(string propertyName)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				this.RaisePropertyChanged("Name");
			}
			else if (e.PropertyName == "NameHash")
			{
				this.RaisePropertyChanged("NameHash");
			}
		}

		private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				this.RaisePropertyChanging("Name");
			}
			else if (e.PropertyName == "NameHash")
			{
				this.RaisePropertyChanging("NameHash");
			}
		}

		#endregion
	}
}