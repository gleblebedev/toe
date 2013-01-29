using System.ComponentModel;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class DefaultEditorConfiguration<T> : IEditorOptions<T>
		where T : class, new()
	{
		private readonly IEditorConfigStorage editorConfigStorage;

		public DefaultEditorConfiguration(IEditorConfigStorage editorConfigStorage)
		{
			this.editorConfigStorage = editorConfigStorage;
		}

		#region Constants and Fields

		private T options;

		#endregion

		#region Public Properties

		public T Options
		{
			get
			{
				if (this.options == null)
				{
					this.options = (T)this.editorConfigStorage.Load(typeof(T)) ?? new T();
					var changed = this.options as INotifyPropertyChanged;
					if (changed != null) changed.PropertyChanged += this.OnPropertyChanged;
				}
				return this.options;
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Save();
		}

		#endregion

		#region Public Methods and Operators

		public void Save()
		{
			this.editorConfigStorage.Save(this.options);
		}

		#endregion
	}
}