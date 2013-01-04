using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Toe.Resources
{
	public class ResourceItem : IResourceItem, INotifyPropertyChanged
	{
		private readonly ResourceManager manager;

		private readonly uint type;

		private readonly uint hash;

		private IList<object> values = new List<object>(1);

		private int referenceCounter;



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
				if (this.values.Count == 0) return null;
				return this.values[0];
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

		public override string ToString()
		{
			object value = Value;
			if (value == null) return string.Empty;
			return value.ToString();
		}

		public void Provide(object value)
		{
			this.values.Add(value);
			RaisePropertyChanged("Value");
		}
		public void Retract(object value)
		{
			if (!this.values.Remove(value))
				throw new ApplicationException("Can't retract resource - it wasn't provided");
			RaisePropertyChanged("Value");
		}
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged!=null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		internal void Consume()
		{
			++referenceCounter;
		}

		public bool IsInUse
		{
			get
			{
				return referenceCounter>0 && values.Count>0;
			}
		}

		internal void Release()
		{
			--referenceCounter;
		}
	}
}