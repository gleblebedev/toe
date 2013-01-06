using System;
using System.ComponentModel;
using System.Globalization;

using Toe.Resources;

namespace Toe.Marmalade
{
	public abstract class Managed : INotifyPropertyChanging, INotifyPropertyChanged, IDisposable, IBasePathProvider
	{
		#region Constants and Fields

		private string basePath;

		private string name;

		private uint nameHash;

		#endregion

		#region Constructors and Destructors

		~Managed()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region Public Properties

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

		public abstract uint ClassHashCode { get; }

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
					this.RaisePropertyChanging("NameHash");
					this.name = value;
					this.nameHash = Hash.Get(this.Name);
					this.RaisePropertyChanged("Name");
					this.RaisePropertyChanged("NameHash");
				}
			}
		}

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
					this.RaisePropertyChanging("Name");
					this.RaisePropertyChanging("NameHash");
					this.name = null;
					this.nameHash = value;
					this.RaisePropertyChanged("Name");
					this.RaisePropertyChanged("NameHash");
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public override string ToString()
		{
			return string.Format(
				CultureInfo.InvariantCulture,
				"{0} ({1})",
				string.IsNullOrEmpty(this.Name) ? string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", this.nameHash) : this.name,
				this.GetType().Name);
		}

		#endregion

		#region Methods

		protected virtual void Dispose(bool disposing)
		{
		}

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

		#endregion
	}
}