namespace Toe.Marmalade.TextFiles.IwGraphics
{
	public abstract class ResourceTextSerializer: ITextSerializer
	{
		#region Implementation of ITextSerializer

		/// <summary>
		/// Default file extension for text resource file for this particular resource.
		/// </summary>
		public abstract string DefaultFileExtension { get; }

		/// <summary>
		/// Parse text block.
		/// </summary>
		public virtual void Serialize(TextSerializer parser, Managed managed)
		{
			parser.WriteAttribute(ClassName);
			parser.OpenBlock();
			this.SerializeContent(parser,managed);
			parser.CloseBlock();

		}

		/// <summary>
		/// Parse text block.
		/// </summary>
		public virtual void SerializeContent(TextSerializer parser, Managed managed)
		{
			parser.WriteAttribute("name");
			parser.WriteStringValue(managed.Name);

		}

		protected abstract string ClassName { get; }

		#endregion
	}
}