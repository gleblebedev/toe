using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public class EditorConfiguration<T>:IEditorOptions<T>
		where T : new()
	{
		#region Implementation of IEditorOptions<T>

		private T options;

		public T Options
		{
			get
			{
				if (this.options == null)
				{
					this.options = new T();
					this.Load();
				}
				return this.options;
			}
		}

		public void Load()
		{
			
		}

		public void Save()
		{
			
		}

		#endregion
	}
}