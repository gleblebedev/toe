using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Toe.Editors.Interfaces.Bindings
{
	public class PropertyBinding<Model, Type> : PropertyBinding
	{
		#region Constructors and Destructors

		public PropertyBinding(
			IView target,
			DataContextContainer source,
			string propertyName,
			Func<Model, Type> setValue,
			Action<Model, Type> updateValue)
			: base(target, source, propertyName, setValue, updateValue)
		{
		}

		public PropertyBinding(
			IView target, DataContextContainer source, Expression<Func<Model, Type>> setValue, Action<Model, Type> updateValue)
			: base(target, source, ((MemberExpression)setValue.Body).Member.Name, setValue.Compile(), updateValue)
		{
		}

		#endregion
	}

	public class PropertyBinding : IBinding
	{
		#region Constants and Fields

		private readonly string propertyName;

		private readonly Delegate setValue;

		private readonly DataContextContainer source;

		private readonly IView target;

		private readonly Delegate updateValue;

		#endregion

		#region Constructors and Destructors

		public PropertyBinding(
			IView target, DataContextContainer source, string propertyName, Delegate setValue, Delegate updateValue)
		{
			this.target = target;
			this.source = source;
			this.propertyName = propertyName;
			this.setValue = setValue;
			this.updateValue = updateValue;
			this.source.DataContextChanged += this.OnSourceChanged;
			this.source.PropertyChanged += this.OnPropertyChanged;
			target.DataContext.DataContextChanged += this.OnTargetDataContextChanged;

			if (source.Value != null)
			{
				this.UpdateValue(source.Value);
			}
		}

		#endregion

		#region Public Properties

		public IView Target
		{
			get
			{
				return this.target;
			}
		}

		#endregion

		#region Methods

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.propertyName)
			{
				this.UpdateValue(sender);
			}
		}

		private void OnSourceChanged(object sender, DataContextChangedEventArgs e)
		{
			this.UpdateValue(e.NewValue);
		}

		private void OnTargetDataContextChanged(object sender, DataContextChangedEventArgs e)
		{
			this.UpdateSource(e.NewValue);
		}

		private void UpdateSource(object o)
		{
			if (this.updateValue != null)
			{
				this.updateValue.DynamicInvoke(this.source.Value, o);
			}
		}

		private void UpdateValue(object sourceValue)
		{
			if (this.setValue != null)
			{
				this.target.DataContext.Value = this.setValue.DynamicInvoke(sourceValue);
			}
		}

		#endregion
	}
}