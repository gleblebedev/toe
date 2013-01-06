using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public class EditorConfiguration<T> : IEditorOptions<T>
		where T : new()
	{
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
					this.options = new T();
					this.Load();
				}
				return this.options;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Load()
		{
		}

		public void Save()
		{
		}

		#endregion
	}
}