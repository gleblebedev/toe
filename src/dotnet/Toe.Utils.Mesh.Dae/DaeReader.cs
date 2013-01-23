using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeReader : ISceneReader
	{
		private const string ColladaSchema = "http://www.collada.org/2005/11/COLLADASchema";
		private static XName colladaName = XName.Get("COLLADA", ColladaSchema);
		private static XName assetName = XName.Get("asset", ColladaSchema);
		private static XName libraryImagesName = XName.Get("library_images", ColladaSchema);
		private static XName libraryMaterialsName = XName.Get("library_materials", ColladaSchema);
		private static XName libraryEffectsName = XName.Get("library_effects", ColladaSchema);
		private static XName libraryGeometriesName = XName.Get("library_geometries", ColladaSchema);
		private static XName libraryAnimationsName = XName.Get("library_animations", ColladaSchema);
		private static XName libraryLightsName = XName.Get("library_lights", ColladaSchema);
		private static XName libraryVisualScenesName = XName.Get("library_visual_scenes", ColladaSchema);
		private static XName instanceVisualSceneName = XName.Get("instance_visual_scene", ColladaSchema);
		private static XName urlName = XName.Get("url", ColladaSchema);
		
		private static XName sceneName = XName.Get("scene", ColladaSchema);
					

		#region Implementation of IMeshReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream)
		{
			var scene = new Scene();
			
			var doc = XDocument.Load(stream);
			var collada = doc.Element(colladaName);
			ParseAsset(scene, collada.Element(assetName));
			ParseImages(scene, collada.Element(libraryImagesName));
			ParseScene(scene, collada.Element(sceneName));
			return scene;
		}

		private void ParseScene(Scene scene, XElement element)
		{
			if (element == null)
				return;
			var instance_visual_scene = element.Element(instanceVisualSceneName);
			if (instance_visual_scene == null)
				return;
			var url = instance_visual_scene.Attribute(urlName);
			if (url == null)
				return;

			var sceneId = url.Value;

		}

		private void ParseImages(Scene scene, XElement element)
		{
			if (element == null)
				return;
		}

		private void ParseAsset(Scene scene, XElement element)
		{
			if (element == null)
				return;
		}

		#endregion
	}
}
