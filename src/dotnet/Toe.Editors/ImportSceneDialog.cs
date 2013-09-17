using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Ase;

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
			this.InitializeComponent();
		}

		#endregion

		#region Public Methods and Operators

		public IScene ImportScene()
		{
			var f = new OpenFileDialog();
			f.Filter = BuildFilter();
			if (f.ShowDialog() != DialogResult.OK)
				return null;
			foreach (var format in formats)
			{
				if (format.CanLoad(f.FileName))
				{
					var r = format.CreateReader();
					using (var stream = File.Open(f.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						var s = r.Load(stream,Path.GetDirectoryName(Path.GetFullPath( f.FileName)));
						return s;
					}
				}
			}
			return null;
		}
		private string BuildFilter()
		{
			StringBuilder filterBuilder = new StringBuilder();
			filterBuilder.Append("All supported types (");
			foreach (var format in formats)
			{
				foreach (var ex in format.Extensions)
				{
					filterBuilder.Append(ex);
				}
			}
			filterBuilder.Append(")|");
			string separator = "";
			foreach (var format in formats)
			{
				foreach (var ex in format.Extensions)
				{
					filterBuilder.Append(separator);
					filterBuilder.Append("*");
					filterBuilder.Append(ex);
					separator = ";";
				}
			}
			filterBuilder.Append("|");
			foreach (var format in formats)
			{
				BuildFilterOption(format, filterBuilder);
				filterBuilder.Append("|");
			}
			
			filterBuilder.Append("All (*.*)|*.*");

			var filter = filterBuilder.ToString();
			return filter;
		}
		private static void BuildFilterOption(ISceneFileFormat format, StringBuilder allBuilder)
		{
			allBuilder.Append(format.Name);
			allBuilder.Append(" (");
			string separator = String.Empty;
			foreach (var ex in format.Extensions)
			{
				allBuilder.Append(separator);
				allBuilder.Append("*");
				allBuilder.Append(ex);
				separator = ", ";
			}
			allBuilder.Append(")|");
			separator = String.Empty;
			foreach (var ex in format.Extensions)
			{
				allBuilder.Append(separator);
				allBuilder.Append("*");
				allBuilder.Append(ex);
				separator = ";";
			}
		}
		#endregion

		#region Methods


		#endregion
	}
}