using System;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class ResourceFileReferenceEditor : SingleControlView<ButtonView>, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		public ResourceFileReferenceEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			//this.label.AutoSize = true;
			new DataContextBinding(ViewControl, this.DataContext,false);
			this.ViewControl.Click += OnButtonClick;
		}

		private void OnButtonClick(object sender, EventArgs a)
		{
			editorEnvironment.Open(Text.FilePath);
		}

		DataContextContainer dataContext = new DataContextContainer();

		#region Implementation of IView

		public IResourceFile Text
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return this.dataContext.Value as IResourceFile;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion
	}
}