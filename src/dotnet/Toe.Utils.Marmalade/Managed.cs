using System;
using System.ComponentModel;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade
{
	public abstract class Managed: INotifyPropertyChanged, IDisposable
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

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public override string ToString()
		{
			return Name+" ("+this.GetType().Name+")";
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