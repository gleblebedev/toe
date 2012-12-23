namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextBinding : IBinding
	{
		private readonly bool twoWay;

		private readonly IView target;

		private readonly DataContextContainer source;

		public DataContextBinding(IView target, DataContextContainer source, bool twoWay)
		{
			this.twoWay = twoWay;
			this.target = target;
			this.source = source;
			this.source.DataContextChanged += this.OnSourceChanged;
			target.DataContext.DataContextChanged += this.OnTargetDataContextChanged;
		}

		public IView Target
		{
			get
			{
				return this.target;
			}
		}

		private void OnSourceChanged(object sender, DataContextChangedEventArgs e)
		{
			this.Target.DataContext.Value = e.NewValue;
		}
		private void OnTargetDataContextChanged(object sender, DataContextChangedEventArgs e)
		{
			if (twoWay)
			{
				source.Value = e.NewValue;
			}
		}
	}
}