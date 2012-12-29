using System;
using System.ComponentModel;
using System.Globalization;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade
{
	public abstract class Managed : INotifyPropertyChanging, INotifyPropertyChanged, IDisposable, IBasePathProvider
	{
		~Managed()
		{
			this.Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			
		}

		private string name;

		private uint nameHash;

		public uint NameHash
		{
			get
			{
				return this.nameHash;
			}
			set
			{
				if (this.nameHash != value)
				{
					this.RaisePropertyChanging("NameHash");
					this.nameHash = value;
					this.RaisePropertyChanged("NameHash");
				}
			}
		}

		/// <summary>
		/// Object name.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.RaisePropertyChanging("Name");
					this.name = value;
					this.RaisePropertyChanged("Name");
					this.NameHash = Hash.Get(Name);
				}
			}
		}

		private string basePath;

		public string BasePath
		{
			get
			{
				return this.basePath;
			}
			set
			{
				if (this.basePath != value)
				{
					this.RaisePropertyChanging("BasePath");
					this.basePath = value;
					this.RaisePropertyChanged("BasePath");
				}
			}
		}

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", this.Name, this.GetType().Name);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		public abstract uint GetClassHashCode();
	}
}