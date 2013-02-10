using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public class FileReferenceImage : SceneItem, IImage
	{
		protected static PropertyChangedEventArgs PathChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<FileReferenceImage>(a => a.Path));

		/// <summary>
		/// File path.
		/// </summary>
		private string path;

		/// <summary>
		/// File path.
		/// </summary>
		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (path != value)
				{
					path = value;
					this.RaisePropertyChanged(PathChangedEventArgs);
				}
			}
		}

		public override string ToString()
		{
			return path??"<no_file>";
		}
	}
}