using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class DefaultEditorConfigStorage : IEditorConfigStorage
	{
		#region Constants and Fields

		private string folder;

		#endregion

		#region Public Properties

		public string Folder
		{
			get
			{
				if (this.folder == null)
				{
					this.folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
					if (!Directory.Exists(this.folder))
					{
						Directory.CreateDirectory(this.folder);
					}
					this.folder = Path.Combine(this.folder, "TinyOpenEngineEditor");
					if (!Directory.Exists(this.folder))
					{
						Directory.CreateDirectory(this.folder);
					}
				}
				return this.folder;
			}
		}

		#endregion

		#region Public Methods and Operators

		public object Load(Type type)
		{
			var path = this.ConfigPath(type);
			if (!File.Exists(path))
			{
				return null;
			}
			try
			{
				var s = JsonSerializer.Create(new JsonSerializerSettings());
				using (var w = File.OpenText(path))
				{
					return s.Deserialize(w, type);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void Save(object options)
		{
			var path = this.ConfigPath(options.GetType());
			var s = JsonSerializer.Create(new JsonSerializerSettings());
			var stringBuilder = new StringBuilder();
			using (var w = new StringWriter(stringBuilder))
			{
				s.Serialize(w, options);
			}
			File.WriteAllText(path, stringBuilder.ToString(), Encoding.UTF8);
		}

		#endregion

		#region Methods

		private string ConfigPath(Type options)
		{
			return Path.Combine(this.Folder, options.Name + ".json");
		}

		#endregion
	}
}