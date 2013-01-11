using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class ShaderEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		#endregion

		#region Constructors and Destructors

		public ShaderEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			this.InitializeComponent();

			this.InitializeEditor();
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

		public ShaderTechnique Material
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (ShaderTechnique)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		private void InitializeComponent()
		{
		}

		private void InitializeEditor()
		{
		}

		#endregion
	}
}