namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextBinding : IBinding
	{
		#region Constants and Fields

		private readonly DataContextContainer source;

		private readonly IView target;

		#endregion

		#region Constructors and Destructors

		public DataContextBinding(IView target, DataContextContainer source, bool twoWay)
		{
			this.target = target;
			this.source = source;
			this.source.DataContextChanged += this.OnSourceChanged;
			if (twoWay)
			{
				target.DataContext.DataContextChanged += this.OnTargetDataContextChanged;
			}
		}

		#endregion

		//public DataContextBinding(IView target, DataContextContainer source, Func<object, object> setValue, Func<object, object> updateValue)
		//{
		//    this.target = target;
		//    this.source = source;
		//    this.source.DataContextChanged += this.OnSourceChanged;
		//    target.DataContext.DataContextChanged += this.OnTargetDataContextChanged;
		//}

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

		private void OnSourceChanged(object sender, DataContextChangedEventArgs e)
		{
			this.Target.DataContext.Value = e.NewValue;
		}

		private void OnTargetDataContextChanged(object sender, DataContextChangedEventArgs e)
		{
			this.source.Value = e.NewValue;
		}

		#endregion
	}
}