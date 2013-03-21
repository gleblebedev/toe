using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Utils.Mesh;

namespace Toe.Editors
{
	public partial class ImportSceneDialog : Form
	{
		#region Constants and Fields

		private readonly IEnumerable<ISceneFileFormat> formats;

		#endregion

		#region Constructors and Destructors

		public ImportSceneDialog(IEnumerable<ISceneFileFormat> formats)
		{
			this.formats = formats;
			this.BuildFilterString();
			this.InitializeComponent();
		}

		#endregion

		#region Public Methods and Operators

		public IScene ImportScene()
		{
			var f = new OpenFileDialog();

			if (f.ShowDialog() == DialogResult.OK)
			{
			}
			return null;
		}

		#endregion

		#region Methods

		private void BuildFilterString()
		{
			StringBuilder sb = new StringBuilder();
			List<string> ex = (from format in this.formats from f in format.Extensions select f).ToList();
		}

		#endregion
	}
}