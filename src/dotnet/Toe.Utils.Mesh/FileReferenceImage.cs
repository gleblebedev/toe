using System;
using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public class FileReferenceImage : SceneItem, IImage
	{
		#region Constants and Fields

		protected static PropertyChangedEventArgs PathChangedEventArgs =
			new PropertyChangedEventArgs(Expr.Path<FileReferenceImage>(a => a.Path));

		/// <summary>
		/// File path.
		/// </summary>
		private string path;

		#endregion

		#region Public Properties

		/// <summary>
		/// File path.
		/// </summary>
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				if (this.path != value)
				{
					this.path = value;
					this.RaisePropertyChanged(PathChangedEventArgs);
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public string GetFilePath()
		{
			return this.Path;
		}

		public byte[] GetRawData()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return this.path ?? "<no_file>";
		}

		#endregion
	}
}