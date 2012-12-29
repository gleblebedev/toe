using System;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class EditTextureView : SingleControlView<ButtonView>, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		DataContextContainer dataContext = new DataContextContainer();

		public EditTextureView(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			//this.label.AutoSize = true;
			new DataContextBinding(ViewControl, this.DataContext,false);
			this.ViewControl.Click += OnButtonClick;
		}

		private void OnButtonClick(object sender, EventArgs a)
		{
			var filePath = Text.FilePath;
			if (!string.IsNullOrEmpty(filePath))
				editorEnvironment.Open(filePath);
		}

		#region Implementation of IView

		public ResourceReference Text
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return this.dataContext.Value as ResourceReference;
			}
			set
			{
				this.dataContext.Value  = value;
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