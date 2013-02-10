namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material effect.
	/// </summary>
	public interface IEffect : ISceneItem
	{
		IColorSource Ambient { get; set; }
		IColorSource Emission { get; set; }
		IColorSource Specular { get; set; }
		IColorSource Diffuse { get; set; }
		IColorSource Reflective { get; set; }
		IColorSource Transparent { get; set; }

		float Shininess { get; set; }
		float Reflectivity { get; set; }
		float Transparency { get; set; }
	}
}