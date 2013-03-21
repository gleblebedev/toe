using System.ComponentModel;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class DefaultEditorConfiguration<T> : IEditorOptions<T>
		where T : class, new()
	{
		#region Constants and Fields

		private readonly IEditorConfigStorage editorConfigStorage;

		private T options;

		#endregion

		#region Constructors and Destructors

		public DefaultEditorConfiguration(IEditorConfigStorage editorConfigStorage)
		{
			this.editorConfigStorage = editorConfigStorage;
		}

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
					if (changed != null)
					{
						changed.PropertyChanged += this.OnPropertyChanged;
					}
				}
				return this.options;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Save()
		{
			this.editorConfigStorage.Save(this.options);
		}

		#endregion

		#region Methods

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Save();
		}

		#endregion
	}
}