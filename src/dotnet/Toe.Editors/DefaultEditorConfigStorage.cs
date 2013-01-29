using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class DefaultEditorConfigStorage : IEditorConfigStorage
	{
		private string folder;
		public string Folder
		{
			get
			{
				if (this.folder == null)
				{
					this.folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
					if (!Directory.Exists(this.folder)) Directory.CreateDirectory(this.folder);
					this.folder = Path.Combine(this.folder,"TinyOpenEngineEditor");
					if (!Directory.Exists(this.folder)) Directory.CreateDirectory(this.folder);
				}
				return this.folder;
			}
		}

		#region Implementation of IEditorConfigStorage

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
			catch(Exception ex)
			{
				return null;
			}
		}

		private string ConfigPath(Type options)
		{
			return Path.Combine(this.Folder,options.Name+".json");
		}

		public void Save(object options)
		{
			var path = this.ConfigPath(options.GetType());
			var s = JsonSerializer.Create(new JsonSerializerSettings());
			var stringBuilder = new StringBuilder();
			using (var w = new StringWriter(stringBuilder))
			{
				s.Serialize(w,options);
			}
			File.WriteAllText(path,stringBuilder.ToString(),Encoding.UTF8);
		}

		#endregion
	}
}