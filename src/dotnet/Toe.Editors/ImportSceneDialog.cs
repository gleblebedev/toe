using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Utils.Mesh;

namespace Toe.Editors
{
	public partial class ImportSceneDialog : Form
	{
		private readonly IEnumerable<ISceneFileFormat> formats;

		public ImportSceneDialog(IEnumerable<ISceneFileFormat> formats)
		{
			this.formats = formats;
			BuildFilterString();
			InitializeComponent();
		}

		private void BuildFilterString()
		{
			StringBuilder sb = new StringBuilder();
			List<string> ex = (from format in formats from f in format.Extensions select f).ToList();

		}

		public IScene ImportScene()
		{
			var f = new OpenFileDialog();

			
			if (f.ShowDialog() == DialogResult.OK)
			{

			}
			return null;
		}
	}
}
