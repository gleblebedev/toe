using System;
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
using Panel = System.Windows.Forms.Panel;
using StackPanel = Toe.Editors.Interfaces.Views.StackPanel;

namespace Toe.Editors.Marmalade
{
	public class MaterialEditor : System.Windows.Forms.UserControl, IView
	{
		#region Implementation of IView

		public Material Material
		{
			get
			{
				if (dataContext.Value == null) return null;
				return (Material)dataContext.Value;
			}
			set
			{
				dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return dataContext;
			}
		}

		#endregion
		
		public override Size GetPreferredSize(Size proposedSize)
		{
			return stackPanel.GetPreferredSize(proposedSize);
			//return base.GetPreferredSize(proposedSize);
		}
		
		public MaterialEditor()
		{
			//this.AutoScroll = true;
			//this.VScroll = true;

			//BackColor = Color.Red;
			this.AutoSize = true;
			this.Padding = new Padding(10);

			//, Font = new Font(FontFamily.GenericSansSerif, 14)
			var materialGroup = new GroupBox { Text="Material", Dock = DockStyle.Fill, AutoSize = true,Padding = new Padding(10)};
			this.Controls.Add(materialGroup);

			var panel = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };
			materialGroup.Controls.Add(panel);

			stackPanel = new TableLayoutPanel() { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };
			panel.Controls.Add(stackPanel);

			panel.Controls.Add(new PictureBox { Image = Resources.material, Dock = DockStyle.Fill, AutoSize = true });

			{
				stackPanel.Controls.Add(new PictureBox { Image = Resources.material, AutoSize = true });
				stackPanel.Controls.Add(new Label());
			}
			{
				stackPanel.Controls.Add(new StringView { Text = "Material name" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Name, (mtl, value) => mtl.Name = value);
			}
			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 0 (diffuse map)" });
				var valueCtrl = new EditTextureView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Texture0, (mtl, value) => mtl.Texture0 = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 1" });
				var valueCtrl = new EditTextureView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.Texture1, (mtl, value) => mtl.Texture1 = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify vertex shader to use" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.vertexShader, (mtl, value) => mtl.vertexShader = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Emissive colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, dataContext, mtl => mtl.ColEmissive, (mtl, value) => mtl.ColEmissive = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Ambient colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.ColAmbient, (mtl, value) => mtl.ColAmbient = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Diffuse colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.ColDiffuse, (mtl, value) => mtl.ColDiffuse = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specular colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(valueCtrl, dataContext, mtl => mtl.ColSpecular, (mtl, value) => mtl.ColSpecular = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "The specular cosine power" });
				var valueCtrl = new EditIntegerView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, int>(valueCtrl, dataContext, mtl => mtl.SpecularPower, (mtl, value) => mtl.SpecularPower = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Shading mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = new EnumWellKnownValues { { ShadeMode.SHADE_FLAT, "Flat" }, { ShadeMode.SHADE_GOURAUD, "Gouraud" } } };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ShadeMode>(valueCtrl, dataContext, mtl => mtl.ShadeMode, (mtl, value) => mtl.ShadeMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Modulation mode (of texel by vertex colour)" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ModulateMode>(valueCtrl, dataContext, mtl => mtl.ModulateMode, (mtl, value) => mtl.ModulateMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Backface culling mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, CullMode>(valueCtrl, dataContext, mtl => mtl.CullMode, (mtl, value) => mtl.CullMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Transparency (alpha) mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaMode>(valueCtrl, dataContext, mtl => mtl.AlphaMode, (mtl, value) => mtl.AlphaMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Blend mode between texture stages 0 and 1" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, BlendMode>(valueCtrl, dataContext, mtl => mtl.BlendMode, (mtl, value) => mtl.BlendMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Preset multi-texturing effect" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, EffectPreset>(valueCtrl, dataContext, mtl => mtl.EffectPreset, (mtl, value) => mtl.EffectPreset = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test function" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaTestMode>(valueCtrl, dataContext, mtl => mtl.AlphaTestMode, (mtl, value) => mtl.AlphaTestMode = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test reference value" });
				var valueCtrl = new EditByteView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, byte>(valueCtrl, dataContext, mtl => mtl.AlphaTestValue, (mtl, value) => mtl.AlphaTestValue = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using SW rasterisation" });
				var valueCtrl = new EditShortView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, short>(valueCtrl, dataContext, mtl => mtl.ZDepthOfs, (mtl, value) => mtl.ZDepthOfs = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using HW rasterisation" });
				var valueCtrl = new EditShortView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, short>(valueCtrl, dataContext, mtl => mtl.ZDepthOfsHW, (mtl, value) => mtl.ZDepthOfsHW = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Checked only if the material should not be rendered" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.Invisible, (mtl, value) => mtl.Invisible = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Checked only if the material's textures should use bilinear filtering" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.Filtering, (mtl, value) => mtl.Filtering = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = " Enable writing to depth buffer. Only affects HW rasterisation" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.DepthWriteEnable, (mtl, value) => mtl.DepthWriteEnable = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Enable geometry merging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.MergeGeom, (mtl, value) => mtl.MergeGeom = value);
			}
			/*
			celW  0 .. 255  UV animation: width of cel (U delta to apply at each update).  celW 4 
 
	celH  0 .. 255  UV animation: height of cel (V delta to apply when a "row" of cels is complete).  celH 16 
 
	celNum  0 .. n  UV animation: total number of cels before animation restarts.  celNum 32 
 
	celNumU  0 .. n  UV animation: total number of cels in a "row", i.e. before applying a V delta.  celNumU 8 
 
	celPeriod  0 .. n  UV animation: number of application updates before each UV animation update.  celPeriod 1 	 
			 */

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify a format to convert the image to, before uploading for SW rasterisation" });
				var valueCtrl = new EditEnumView() { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(valueCtrl, dataContext, mtl => mtl.FormatSW, (mtl, value) => mtl.FormatSW = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Specify a format to convert the image to, before uploading for HW rasterisation" });
				var valueCtrl = new EditEnumView() { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(valueCtrl, dataContext, mtl => mtl.FormatHW, (mtl, value) => mtl.FormatHW = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Keep the contents of the texture after upload" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.KeepAfterUpload, (mtl, value) => mtl.KeepAfterUpload = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "If true the texture UV coordinates are clamped" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.ClampUV, (mtl, value) => mtl.ClampUV = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Disable fogging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, dataContext, mtl => mtl.NoFog, (mtl, value) => mtl.NoFog = value);
			}

			{
				
				
				stackPanel.Controls.Add(new StringView { Text = "Attach the specified shader to this material" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, dataContext, mtl => mtl.ShaderTechnique, (mtl, value) => mtl.ShaderTechnique = value);
			}
		}

		private DataContextContainer dataContext = new DataContextContainer();

		private Panel stackPanel;

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			//if (stackPanel.Width != ClientRectangle.Width)
			//    stackPanel.Width = ClientRectangle.Width;
		}

		
	}
}
