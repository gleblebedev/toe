using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Utils.Mesh.Marmalade.IwGx;

using Control = System.Windows.Forms.Control;
using Label = System.Windows.Forms.Label;
using StackPanel = Toe.Editors.Interfaces.Views.StackPanel;

namespace Toe.Editors.Marmalade
{
	public class MaterialEditor : System.Windows.Forms.UserControl, IResourceEditor
	{
		public MaterialEditor()
		{
			var stackPanel = new FormPanel { Dock = DockStyle.Fill };
			this.Dock = DockStyle.Fill;
			this.Controls.Add(stackPanel);

			dataContext.Value = new Material() { };

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Material name" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Name, (mtl, value) => mtl.Name = value);
			}
			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Specify texture for stage 0 (diffuse map)" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Texture0, (mtl, value) => mtl.Texture0 = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Specify texture for stage 1" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Texture1, (mtl, value) => mtl.Texture1 = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Specify vertex shader to use" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.vertexShader, (mtl, value) => mtl.vertexShader = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Emissive colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, dataContext, mtl => mtl.colEmissive, (mtl, value) => mtl.colEmissive = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Ambient colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.colAmbient, (mtl, value) => mtl.colAmbient = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Diffuse colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.colDiffuse, (mtl, value) => mtl.colDiffuse = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Specular colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.colSpecular, (mtl, value) => mtl.colSpecular = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "The specular cosine power" });
				var valueCtrl = new EditIntegerView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, int>(valueCtrl, dataContext, mtl => mtl.specularPower, (mtl, value) => mtl.specularPower = value);
			}

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Shading mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ShadeMode>(valueCtrl, dataContext, mtl => mtl.shadeMode, (mtl, value) => mtl.shadeMode = value);
			}
			/*
 
modulateMode  (R or RGB or NONE)  Modulation mode (of texel by vertex colour).  modulateMode RGB 
 
cullMode  (BACK or FRONT or NONE)  Backface culling mode.  cullMode NONE 
 
alphaMode  (NONE or ADD or SUB or HALF or BLEND)  Transparency (alpha) mode.  alphaMode ADD 
 
blendMode  (MODULATE or DECAL or ADD or REPLACE or BLEND or MODULATE_2X or MODULATE_4X)  Blend mode between texture stages 0 and 1.  blendMode MODULATE 
 
effectPreset  (DEFAULT or NORMAL_MAPPING or NORMAL_MAPPING_SPECULAR or REFLECTION_MAPPING)  Preset multi-texturing effect.  effectPreset NORMAL_MAPPING 
 
alphaTest  (DISABLED or NEVER or LESS or EQUAL or LEQUAL or GREATER or NOTEQUAL or GEQUAL or ALWAYS, followed by a uint8 value (0..255))  Specify the alpha test function as well as the alpha test reference value.  alphaTest LESS 100 
 
zDepthOfs  (-32768 .. 32767)  Depth offset to apply when using SW rasterisation.  zDepthOfs 100 
 
zDepthOfsHW  (-32768 .. 32767)  Depth offset to apply when using HW rasterisation.  zDepthOfsHW 1 
 
invisible  true or false  True only if the material should not be rendered.  invisible true 
 
filtering  true or false  True only if the material's textures should use bilinear filtering.  filtering false 
 
depthWriteEnable  true or false  Enable writing to depth buffer. Only affects HW rasterisation. Defaults to on.  depthWriteEnable false 
 
mergeGeom  true or false  Enable geometry merging for this material.  mergeGeom true 
 
celW  0 .. 255  UV animation: width of cel (U delta to apply at each update).  celW 4 
 
celH  0 .. 255  UV animation: height of cel (V delta to apply when a "row" of cels is complete).  celH 16 
 
celNum  0 .. n  UV animation: total number of cels before animation restarts.  celNum 32 
 
celNumU  0 .. n  UV animation: total number of cels in a "row", i.e. before applying a V delta.  celNumU 8 
 
celPeriod  0 .. n  UV animation: number of application updates before each UV animation update.  celPeriod 1 
 
formatSW  (image format)  Specify a format to convert the image to, before uploading for SW rasterisation.  formatSW PALETTE4_ABGR_1555 
 
formatHW  (image format)  Specify a format to convert the image to, before uploading for HW rasterisation.  formatHW PALETTE4_RGBA_4444 
 
keepAfterUpload  true or false  Keep the contents of the texture after upload. Normally uploaded texels are deleted.  keepAfterUpload true 
 
clampUV  true or false  If true the texture UV coordinates are clamped.  clampUV true 
 
noFog  true or false  Disable fogging for this material.  noFog true 
			 
			 */

			{
				var rowPanel = new FormRowPanel();
				stackPanel.Controls.Add(rowPanel);
				rowPanel.Controls.Add(new Label { Text = "Attach the specified shader to this material" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				rowPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.shaderTechnique, (mtl, value) => mtl.shaderTechnique = value);
			}
		}

		private DataContextContainer dataContext = new DataContextContainer();


		#region Implementation of IResourceEditor

		public Control Control
		{
			get
			{
				return this;
			}
		}

		public void LoadFile(string filename)
		{
			using (var fileStream = File.OpenRead(filename))
			{
				var r = new MtlReader();
				dataContext.Value = r.Load(fileStream)[0];
			}
		}

		public void RecordCommand(string command)
		{
		}

		public void SaveFile(string filename)
		{
			throw new NotImplementedException();
		}

		public void StopRecorder()
		{
		}

		#endregion
	}
}
