using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class EditTextureView : EditResourceReferenceView
	{

		public EditTextureView(IEditorEnvironment editorEnvironment, IResourceManager resourceManager, ICommandHistory history)
			: base(editorEnvironment, resourceManager,history)
		{
		
		}

		protected override void SetNewValue(uint selectedHash, object selectedVal)
		{
			base.SetNewValue(selectedHash, selectedVal);
		}

	}
}