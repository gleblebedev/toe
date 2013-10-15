using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Toe.Utils.Mesh
{
	public class ListMeshStream<T> : List<T>, IMeshStream
	{
		#region Public Methods and Operators

		public void EnsureAt(int index)
		{
			while (this.Count <= index)
			{
				this.Add(default(T));
			}
		}
		public override string ToString()
		{
			if (base.Count > 0)
				return string.Format("{0}[{1}] {{ {2} ... }}", typeof(T).Name, base.Count, this.First());
			return string.Format("{0}[{1}]", typeof(T).Name, base.Count);
		}
		public void ModifyAt(int index, ModifyAtFunc<T> func)
		{
			var v = this[index];
			func(ref v);
			this[index] = v;
		}

		#endregion

		#region Implementation of IMeshStream

		public IList<TValue> GetReader<TValue>()
		{
			if (typeof(TValue) == typeof(T))
				return new ReadOnlyCollection<TValue>((IList<TValue>)(object)this);
			if (ConverterFactory != null)
			{
				var resolveConverter = ConverterFactory.ResolveConverter<T, TValue>(this);
				if (resolveConverter != null)
					return resolveConverter;
			}
			throw new NotImplementedException(string.Format("{0} to {1} converter is not defined", typeof(T).FullName, typeof(TValue).FullName));
		}

		private IStreamConverterFactory converterFactory = StreamConverterFactory.Default;

		#endregion

		public ListMeshStream(IStreamConverterFactory converterFactory)
		{
			this.ConverterFactory = converterFactory;
		}
		public ListMeshStream(int capacity, IStreamConverterFactory converterFactory):base(capacity)
		{
			this.ConverterFactory = converterFactory;
		}

		public IStreamConverterFactory ConverterFactory
		{
			get
			{
				return this.converterFactory;
			}
			set
			{
				this.converterFactory = value;
			}
		}
	}
}