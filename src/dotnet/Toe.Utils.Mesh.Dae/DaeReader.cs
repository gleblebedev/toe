using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using OpenTK;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeReader : ISceneReader
	{
		private ColladaSchema schema;

		private XElement collada;

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
			var r = doc.Root;
			schema = new ColladaSchema(r.Name.Namespace.NamespaceName);
			collada = doc.Element(this.schema.colladaName);
			ParseAsset(scene, collada.Element(schema.assetName));
			ParseImages(scene, collada.Element(schema.libraryImagesName));
			ParseEffects(scene, collada.Element(schema.libraryEffectsName));
			ParseMaterials(scene, collada.Element(schema.libraryMaterialsName));
			ParseGeometries(scene, collada.Element(schema.libraryGeometriesName));
			ParseScene(scene, collada, collada.Element(schema.sceneName));
			return scene;
		}

		private void ParseMaterials(Scene scene, XElement element)
		{
			if (element == null)
				return;
			foreach (var material in element.Elements(schema.materialName))
			{
				ParseMaterial(scene, material);
			}
		}

		private void ParseMaterial(Scene scene, XElement material)
		{
			var sceneMaterial = new SceneMaterial();
			this.ParseIdAndName(material, sceneMaterial);
			var instanceEffect = material.Element(schema.instanceEffectName);
			if(instanceEffect != null)
			{
				string id = instanceEffect.Attribute(schema.urlName).Value.Substring(1);
				sceneMaterial.Effect = (from g in scene.Effects where g.Id == id select g).First();
			}
			scene.Materials.Add(sceneMaterial);
		}

		private void ParseEffects(Scene scene, XElement element)
		{
			if (element == null)
				return;
			foreach (var effect in element.Elements(schema.effectName))
			{
				ParseEffect(scene, effect);
			}
		}

		private void ParseEffect(Scene scene, XElement effect)
		{
			var sceneEffect = new SceneEffect();
			this.ParseIdAndName(effect, sceneEffect);
			var profileCommon = effect.Element(schema.profileCommonName);
			if (profileCommon != null)
			{
				var technique = profileCommon.Element(schema.techniqueName);
				if (technique != null)
				{
					var techniqueImpl = technique.Element(schema.phongName);
					if (techniqueImpl == null)
					{
						techniqueImpl = technique.Element(schema.lambertName);
						if(techniqueImpl == null)
						{
							techniqueImpl = technique.Element(schema.blinnName);
							if (techniqueImpl == null)
							{
								techniqueImpl = technique.Element(schema.constantName);
								if (techniqueImpl == null)
								{
									throw new DaeException(string.Format("Unknow technique at {0}", scene.Name));
								}
							}
						}
					}
					sceneEffect.Diffuse = ParseColorSource(scene,techniqueImpl.Element(schema.diffuseName));
				}


			}
			scene.Effects.Add(sceneEffect);
		}

		private IColorSource ParseColorSource(Scene scene, XElement element)
		{
			var texture = element.Element(schema.textureName);
			if (texture != null)
			{
				var imageColorSource = new ImageColorSource();
				var texAttr = texture.Attribute(schema.textureAttributeName);
				if (texAttr != null)
				{
					var sampler = (from p1 in element.Parent.Parent.Parent.Elements() where p1.Name == schema.newparamName && p1.Attribute(schema.sidName).Value == texAttr.Value select p1).First();
					var sampler2D = sampler.Element(schema.sampler2DName);
					if (sampler2D != null)
					{
						var sampler2DSource = sampler2D.Element(schema.sourceName);
						var surfaceParam = (from p1 in element.Parent.Parent.Parent.Elements() where p1.Name == schema.newparamName && p1.Attribute(schema.sidName).Value == sampler2DSource.Value select p1).First();
						var surface = surfaceParam.Element(schema.surfaceName);
						var imageId = surface.Element(schema.initFromName).Value;
						foreach (var i in scene.Images.Where(i => i.Id == imageId))
						{
							imageColorSource.Image = i; 
							break;
						}
					}
				}
				return imageColorSource;
			}
			var color = element.Element(schema.colorName);
			if (color != null)
			{
				var solidColorSource = new SolidColorSource() { Color = ParseColor(color.Value) };
				
				return solidColorSource;
			}
			return null;
		}

		private Color ParseColor(string value)
		{
			var values = (from c in value.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) select (int)(255*float.Parse(c, CultureInfo.InvariantCulture))).ToArray();
			if (values.Length == 0) return Color.White;
			if (values.Length <= 1) return Color.FromArgb(255, values[0], values[0], values[0]);
			if (values.Length <= 2) return Color.FromArgb(255, values[0], values[1], values[0]);
			if (values.Length <= 3) return Color.FromArgb(255, values[0], values[1], values[2]);
			return Color.FromArgb(values[3], values[0], values[1], values[2]);
		}

		private void ParseIdAndName(XElement effect, ISceneItem sceneEffect)
		{
			var idAttr = effect.Attribute(this.schema.idName);
			if (idAttr != null)
			{
				sceneEffect.Id = idAttr.Value;
			}
			var nameAttr = effect.Attribute(this.schema.nameName);
			sceneEffect.Name = (nameAttr != null) ? nameAttr.Value : sceneEffect.Id;
		}

		private void ParseGeometries(Scene scene, XElement element)
		{
			if (element == null)
				return;
			foreach (var geometry in element.Elements(schema.geometryName))
			{
				ParseGeometry(scene, geometry);
			}
		}

		private void ParseGeometry(Scene scene, XElement geometry)
		{
			var mesh = geometry.Element(schema.meshName);

			var dstMesh = new VertexBufferMesh();
			this.ParseIdAndName(geometry, dstMesh);
			scene.Geometries.Add(dstMesh);
			if (mesh != null)
			{
				ParseMesh(scene, mesh, dstMesh);
				return;
			}
			throw new DaeException(string.Format("Geometry type at {0} not supported yet.", geometry.Attribute(schema.idName).Value));
		}

		private void ParseMesh(Scene scene, XElement mesh, VertexBufferMesh dstMesh)
		{
			foreach (var element in mesh.Elements())
			{
				if (element.Name == schema.polygonsName)
				{
					this.ParsePolygons(mesh, dstMesh,   element);
				}
				else if (element.Name == schema.polylistName)
				{
					this.ParsePolylist(mesh, dstMesh,   element);
				}
				else if (element.Name == schema.trianglesName)
				{
					this.ParseTriangles(mesh, dstMesh,   element);
				}
				else if (element.Name == schema.verticesName || element.Name == schema.sourceName)
				{
				}
				else
				{
					throw new DaeException(message: "Unknown mesh element "+element.Name.LocalName);
				}
			}
		}

		private void ParseTriangles(XElement mesh, VertexBufferMesh dstMesh,  XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);
			SetAvailableStreams(dstMesh, meshInputs);
			int[] p = new int[meshInputs.Count*meshInputs.Stride*3];
			this.ParseIntArray(element.Element(schema.pName).Value, p);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();
			subMesh.VertexSourceType = VertexSourceType.TrianleList;

			this.ParseElements(p, subMesh, meshInputs, meshInputs.Count*3);

		}

		private void ParsePolylist(XElement mesh, VertexBufferMesh dstMesh,   XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);
			SetAvailableStreams(dstMesh, meshInputs);
			int[]vcount = new int[meshInputs.Count];
			this.ParseIntArray(element.Element(schema.vcountName).Value, vcount);
			var points = vcount.Sum(a => a);
			var firstSize = vcount[0];
			bool isAllTheSame = vcount.All(i => i == firstSize);

			int[] p = new int[points * meshInputs.Stride];
			this.ParseIntArray(element.Element(schema.pName).Value, p);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();

			if (isAllTheSame)
			{
				if (firstSize == 3)
				{
					subMesh.VertexSourceType = VertexSourceType.TrianleList;
					this.ParseElements(p, subMesh, meshInputs, points);
					return;
				}
				if (firstSize == 4)
				{
					subMesh.VertexSourceType = VertexSourceType.QuadList;
					this.ParseElements(p, subMesh, meshInputs, points);
					return;
				}
			}

			Vertex vertex = new Vertex();
			subMesh.VertexSourceType = VertexSourceType.TrianleList;
			int startIndex=0;
			for (int polygonIndex=0; polygonIndex<vcount.Length;++polygonIndex)
			{
				for (int i = 2; i < vcount[polygonIndex]; i++)
				{
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex+0);
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex + i-1);
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex + i);
				}
				startIndex += vcount[polygonIndex];
			}
		}

		private void ParsePolygons(
			XElement mesh, VertexBufferMesh dstMesh, XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();
			subMesh.VertexSourceType = VertexSourceType.TrianleList;


			SetAvailableStreams(dstMesh, meshInputs);

			int[] poligonArray = new int[3 * meshInputs.Stride];

			foreach (var polygon in element.Elements(this.schema.pName))
			{
				this.ParseIntArray(polygon.Value, poligonArray);

				this.ParseElements(poligonArray, subMesh, meshInputs,  3);
			}
		}

		private void ParseElements(int[] poligonArray, VertexBufferSubmesh subMesh, MeshInputMap meshInputs, int numElements)
		{
			 Vertex vertex = new Vertex();
			for (int index = 0; index < numElements; ++index)
			{
				this.ParseElement(poligonArray, subMesh, meshInputs, ref vertex, index);
			}
		}

		private void ParseElement(
			int[] poligonArray, VertexBufferSubmesh subMesh, MeshInputMap meshInputs, ref Vertex vertex, int index)
		{
			if (meshInputs.Vertex != null)
			{
				vertex.Position = GetVector3(index, meshInputs.Stride, meshInputs.Vertex, poligonArray);
			}
			if (meshInputs.Normal != null)
			{
				vertex.Normal = GetVector3(index, meshInputs.Stride, meshInputs.Normal, poligonArray);
			}
			if (meshInputs.TexCoord0 != null)
			{
				vertex.UV0 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord0, poligonArray);
			}
			if (meshInputs.TexCoord1 != null)
			{
				vertex.UV1 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord1, poligonArray);
			}
			if (meshInputs.Color != null)
			{
				vertex.Color = this.GetColor(index, meshInputs.Stride, meshInputs.Color, poligonArray);
			}
			subMesh.Add(ref vertex);
		}

		private static void SetAvailableStreams(VertexBufferMesh dstMesh, MeshInputMap meshInputs)
		{
			dstMesh.IsNormalStreamAvailable |= meshInputs.Normal != null;
			dstMesh.IsUV0StreamAvailable |= meshInputs.TexCoord0 != null;
			dstMesh.IsUV1StreamAvailable |= meshInputs.TexCoord1 != null;
			dstMesh.IsColorStreamAvailable |= meshInputs.Color != null;
			dstMesh.IsTangentStreamAvailable |= meshInputs.Tangent != null;
			dstMesh.IsBinormalStreamAvailable |= meshInputs.Binormal != null;
		}

		private Color GetColor(int index, int stride, Input input, int[] poligonArray)
		{
			return input.SourceData.GetColor(poligonArray[index * stride + input.Offset]);
		}

		private static Vector3 GetVector3(int index, int stride, Input input, int[] poligonArray)
		{
			return input.SourceData.GetVector3(poligonArray[index * stride + input.Offset]);
		}

		private void ParseIntArray(string polygon, int[] poligonArray)
		{
			var items = polygon.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			if (items.Length != poligonArray.Length) throw new DaeException(string.Format("Array size doesn't match expected value {0}", poligonArray.Length));
			for (int index = 0; index < items.Length; ++index)
			{
				poligonArray[index] = int.Parse( items[index], CultureInfo.InvariantCulture);
			}
		}

		private void ParseInputs(MeshInputMap meshInputs, XElement src, XElement mesh)
		{
			meshInputs.Count = int.Parse(src.Attribute(schema.countName).Value, CultureInfo.InvariantCulture);
			foreach (var input in src.Elements(this.schema.inputName))
			{
				var semantic = input.Attribute(this.schema.semanticName).Value;
				var inputInfo = new Input(input, this.schema, mesh);
				if (meshInputs.Stride <= inputInfo.Offset) meshInputs.Stride = inputInfo.Offset + 1;
				if (0 == string.Compare(semantic, "VERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Vertex = inputInfo;
				}
				else if (0 == string.Compare(semantic, "NORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Normal = inputInfo;
				}
				else if (0 == string.Compare(semantic, "TEXCOORD", StringComparison.InvariantCultureIgnoreCase))
				{
					switch (inputInfo.Set)
					{
						case 0:
							meshInputs.TexCoord0 = inputInfo;
							break;
						case 1:
							meshInputs.TexCoord1 = inputInfo;
							break;
					}
				}
				else if (0 == string.Compare(semantic, "COLOR", StringComparison.InvariantCultureIgnoreCase))
				{
					switch (inputInfo.Set)
					{
						case 0:
							meshInputs.Color = inputInfo;
							break;
					}
				}
				else if (0 == string.Compare(semantic, "POSITION", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Position = inputInfo;
				}
				else
				{
					throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Unknown semantic {0}", semantic));
				}
			}
		}

		private void ParseScene(Scene scene, XElement collada, XElement element)
		{
			if (element == null)
				return;
			var instance_visual_scene = element.Element(schema.instanceVisualSceneName);
			if (instance_visual_scene == null)
				return;
			var url = instance_visual_scene.Attribute(schema.urlName);
			if (url == null)
				return;

			var sceneId = url.Value;

			var libraryVisualScenes = collada.Element(schema.libraryVisualScenesName);
			var visualScene = libraryVisualScenes.ElementById(sceneId.Substring(1));
			if (visualScene == null)
				return;

			this.ParseIdAndName(visualScene, scene);

			ParseNodes(scene, scene, visualScene.Elements(schema.nodeName));
		}

		private void ParseNodes(Scene scene, INodeContainer parent, IEnumerable<XElement> elements)
		{
			foreach (var node in elements)
			{
				ParseNode(scene, parent, node);
			}
		}

		private void ParseNode(Scene scene, INodeContainer parent, XElement node)
		{
			var n = new Node();
			this.ParseIdAndName(node, n);

			var geo = node.Element(schema.instanceGeometryName);
			if(geo != null)
			{
				var url = geo.Attribute(schema.urlName).Value;
				string id = url.Substring(1);
				n.Mesh = (from g in scene.Geometries where g.Id == id select g).First();
			}
			else
			{
				var ctrl = node.Element(schema.instanceControllerName);
				if (ctrl != null)
				{
					var url = ctrl.Attribute(schema.urlName).Value;
					string id = url.Substring(1);
					var controller = collada.Element(schema.libraryControllersName).ElementById(id);
					url = controller.Element(schema.skinName).Attribute(schema.sourceAttributeName).Value;
					id = url.Substring(1);
					n.Mesh = (from g in scene.Geometries where g.Id == id select g).First();
				}
			}
			
			parent.Nodes.Add(n);
		}

		private void ParseImages(Scene scene, XElement element)
		{
			if (element == null)
				return;
			foreach (var image in element.Elements(schema.imageName))
			{
				ParseImage(scene, image);
			}
		}

		private void ParseImage(Scene scene, XElement image)
		{
			
		}

		private void ParseAsset(Scene scene, XElement element)
		{
			if (element == null)
				return;
		}

		#endregion
	}
}
