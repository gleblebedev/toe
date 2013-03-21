using System;
using System.Globalization;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade
{
	public abstract class Managed : ClassWithNotification, IDisposable, IBasePathProvider
	{
		#region Constants and Fields

		protected static PropertyEventArgs BasePathEventArgs = Expr.PropertyEventArgs<Managed>(x => x.BasePath);

		protected static PropertyEventArgs ContextDataEventArgs = Expr.PropertyEventArgs<Managed>(x => x.ContextData);

		protected static PropertyEventArgs NameEventArgs = Expr.PropertyEventArgs<Managed>(x => x.Name);

		protected static PropertyEventArgs NameHashEventArgs = Expr.PropertyEventArgs<Managed>(x => x.NameHash);

		private string basePath;

		private IContextData contextData;

		private string name;

		private uint nameHash;

		#endregion

		#region Constructors and Destructors

		~Managed()
		{
			this.Dispose(false);
		}

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
					this.RaisePropertyChanging(BasePathEventArgs.Changing);
					this.basePath = value;
					this.RaisePropertyChanged(BasePathEventArgs.Changed);
				}
			}
		}

		public abstract uint ClassHashCode { get; }

		public IContextData ContextData
		{
			get
			{
				return this.contextData;
			}
			set
			{
				if (this.contextData != value)
				{
					this.RaisePropertyChanging(ContextDataEventArgs.Changing);
					this.contextData = value;
					this.RaisePropertyChanged(ContextDataEventArgs.Changed);
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
					this.RaisePropertyChanging(NameEventArgs.Changing);
					this.RaisePropertyChanging(NameHashEventArgs.Changing);
					this.name = value;
					this.nameHash = Hash.Get(this.Name);
					this.RaisePropertyChanged(NameEventArgs.Changed);
					this.RaisePropertyChanged(NameHashEventArgs.Changed);
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
					this.RaisePropertyChanging(NameEventArgs.Changing);
					this.RaisePropertyChanging(NameHashEventArgs.Changing);
					this.name = null;
					this.nameHash = value;
					this.RaisePropertyChanged(NameEventArgs.Changed);
					this.RaisePropertyChanged(NameHashEventArgs.Changed);
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
			if (disposing)
			{
				if (this.contextData != null)
				{
					this.contextData.Dispose();
					this.contextData = null;
				}
			}
		}

		#endregion
	}
}