using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class EditTextureView : SingleControlView<ButtonView>, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly ICommandHistory history;

		DataContextContainer dataContext = new DataContextContainer();

		public EditTextureView(IEditorEnvironment editorEnvironment, ICommandHistory history)
		{
			this.editorEnvironment = editorEnvironment;
			this.history = history;
			//this.label.AutoSize = true;
			new DataContextBinding(ViewControl, this.DataContext,false);
			this.ViewControl.Click += OnButtonClick;
			this.dataContext.PropertyChanged += OnReferencePropertyChanged;
			this.dataContext.DataContextChanged += OnReferencePropertyChanged;
		}

		private void OnReferencePropertyChanged(object sender, EventArgs e)
		{
			if (this.Reference.Resource == null && !this.Reference.IsEmpty)
			{
				this.BackColor = Color.Red;
			}
			else
			{
				this.BackColor = Color.FromArgb(255, 240, 240, 240);
			}
		}

		private void OnButtonClick(object sender, EventArgs a)
		{
			var filePath = this.Reference.FilePath;
			if (!string.IsNullOrEmpty(filePath))
				editorEnvironment.Open(filePath);
		}

		#region Implementation of IView

		public ResourceReference Reference
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