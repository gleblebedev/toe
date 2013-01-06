using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Toe.Resources
{
	public class ResourceItem : IResourceItem, INotifyPropertyChanged
	{
		#region Constants and Fields

		private readonly uint hash;

		private readonly ResourceManager manager;

		private readonly uint type;

		private readonly IList<ResourceItemSource> values = new List<ResourceItemSource>(1);

		private int referenceCounter;

		#endregion

		#region Constructors and Destructors

		public ResourceItem(ResourceManager manager, uint type, uint hash)
		{
			this.manager = manager;
			this.type = type;
			this.hash = hash;
		}

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		public uint Hash
		{
			get
			{
				return this.hash;
			}
		}

		public bool IsInUse
		{
			get
			{
				return this.referenceCounter > 0 || this.values.Count > 0;
			}
		}

		public ResourceManager Manager
		{
			get
			{
				return this.manager;
			}
		}

		public IResourceFile Source
		{
			get
			{
				if (this.values.Count == 0)
				{
					return null;
				}
				return this.values[0].Source;
			}
		}

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		public object Value
		{
			get
			{
				if (this.values.Count == 0)
				{
					return null;
				}
				return this.values[0].Value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Provide(object value, IResourceFile sourceFile)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value is null");
			}
			if (sourceFile == null)
			{
				throw new ArgumentNullException("sourceFile is null");
			}
			this.values.Add(new ResourceItemSource(value, sourceFile));
			this.RaisePropertyChanged("Value");
		}

		public void Retract(object value, IResourceFile sourceFile)
		{
			if (!this.values.Remove(new ResourceItemSource(value, sourceFile)))
			{
				throw new ApplicationException("Can't retract resource - it wasn't provided");
			}
			this.RaisePropertyChanged("Value");
		}

		public override string ToString()
		{
			object value = this.Value;
			if (value == null)
			{
				return string.Empty;
			}
			return value.ToString();
		}

		#endregion

		#region Methods

		internal void Consume()
		{
			++this.referenceCounter;
		}

		internal void Release()
		{
			--this.referenceCounter;
		}

		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		#endregion
	}
}