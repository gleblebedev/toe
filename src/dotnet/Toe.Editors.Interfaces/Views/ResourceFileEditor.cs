using System;

using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class ResourceFileReferenceEditor : SingleControlView<ButtonView>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		#endregion

		#region Constructors and Destructors

		public ResourceFileReferenceEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			//this.label.AutoSize = true;
			new DataContextBinding(this.ViewControl, this.DataContext, false);
			this.ViewControl.Click += this.OnButtonClick;
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public new IResourceFile Text
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return this.dataContext.Value as IResourceFile;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		private void OnButtonClick(object sender, EventArgs a)
		{
			this.editorEnvironment.Open(this.Text.FilePath);
		}

		#endregion
	}
}