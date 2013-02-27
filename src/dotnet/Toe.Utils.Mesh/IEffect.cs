namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material effect.
	/// </summary>
	public interface IEffect : ISceneItem
	{
		#region Public Properties

		IColorSource Ambient { get; set; }

		CullMode CullMode { get; set; }

		IColorSource Diffuse { get; set; }

		IColorSource Emission { get; set; }

		IColorSource Reflective { get; set; }

		float Reflectivity { get; set; }

		float Shininess { get; set; }

		IColorSource Specular { get; set; }

		float Transparency { get; set; }

		IColorSource Transparent { get; set; }

		#endregion
	}
}