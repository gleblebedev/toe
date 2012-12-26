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
		private readonly IEditorEnvironment editorEnvironment;

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
		
		public MaterialEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			this.InitializeComponent();

			this.InitializeEditor();
		}

		private void InitializeEditor()
		{
			this.SuspendLayout();

			this.AutoSize = true;
			this.Padding = new Padding(10);
			formPreviewSplit.Panel1MinSize = 200;


			this.stackPanel = new TableLayoutPanel() { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true};
			formPreviewSplit.Panel1.Controls.Add(this.stackPanel);

			//var preview = new PictureBox { Image = Resources.material, Dock = DockStyle.Fill, AutoSize = true };
			var preview = new MaterialPreview { Dock = DockStyle.Fill};
			formPreviewSplit.Panel2.Controls.Add(preview);

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Material name" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(valueCtrl, this.dataContext, mtl => mtl.Name, (mtl, value) => mtl.Name = value);
			}
			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 0 (diffuse map)" });
				var valueCtrl = new EditTextureView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(
					valueCtrl, this.dataContext, mtl => mtl.Texture0, (mtl, value) => mtl.Texture0 = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 1" });
				var valueCtrl = new EditTextureView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(
					valueCtrl, this.dataContext, mtl => mtl.Texture1, (mtl, value) => mtl.Texture1 = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify vertex shader to use" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(
					valueCtrl, this.dataContext, mtl => mtl.vertexShader, (mtl, value) => mtl.vertexShader = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Emissive colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColEmissive, (mtl, value) => mtl.ColEmissive = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Ambient colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColAmbient, (mtl, value) => mtl.ColAmbient = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Diffuse colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColDiffuse, (mtl, value) => mtl.ColDiffuse = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specular colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColSpecular, (mtl, value) => mtl.ColSpecular = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "The specular cosine power" });
				var valueCtrl = new EditIntegerView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, int>(
					valueCtrl, this.dataContext, mtl => mtl.SpecularPower, (mtl, value) => mtl.SpecularPower = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Shading mode" });
				var valueCtrl = new EditEnumView
					{
						Margin = new Padding(4),
						WellKnownValues =MaterialEnumsValues.ShadeModeValues
							
					};
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ShadeMode>(
					valueCtrl, this.dataContext, mtl => mtl.ShadeMode, (mtl, value) => mtl.ShadeMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Modulation mode (of texel by vertex colour)" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ModulateModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ModulateMode>(
					valueCtrl, this.dataContext, mtl => mtl.ModulateMode, (mtl, value) => mtl.ModulateMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Backface culling mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.CullModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, CullMode>(
					valueCtrl, this.dataContext, mtl => mtl.CullMode, (mtl, value) => mtl.CullMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Transparency (alpha) mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.AlphaModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaMode>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaMode, (mtl, value) => mtl.AlphaMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Blend mode between texture stages 0 and 1" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.BlendModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, BlendMode>(
					valueCtrl, this.dataContext, mtl => mtl.BlendMode, (mtl, value) => mtl.BlendMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Preset multi-texturing effect" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.EffectPresetValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, EffectPreset>(
					valueCtrl, this.dataContext, mtl => mtl.EffectPreset, (mtl, value) => mtl.EffectPreset = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test function" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.AlphaTestModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaTestMode>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaTestMode, (mtl, value) => mtl.AlphaTestMode = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test reference value" });
				var valueCtrl = new EditByteView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, byte>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaTestValue, (mtl, value) => mtl.AlphaTestValue = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using SW rasterisation" });
				var valueCtrl = new EditShortView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, short>(
					valueCtrl, this.dataContext, mtl => mtl.ZDepthOfs, (mtl, value) => mtl.ZDepthOfs = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using HW rasterisation" });
				var valueCtrl = new EditShortView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, short>(
					valueCtrl, this.dataContext, mtl => mtl.ZDepthOfsHW, (mtl, value) => mtl.ZDepthOfsHW = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Checked only if the material should not be rendered" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.Invisible, (mtl, value) => mtl.Invisible = value);
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Checked only if the material's textures should use bilinear filtering" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.Filtering, (mtl, value) => mtl.Filtering = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = " Enable writing to depth buffer. Only affects HW rasterisation" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.DepthWriteEnable, (mtl, value) => mtl.DepthWriteEnable = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Enable geometry merging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.MergeGeom, (mtl, value) => mtl.MergeGeom = value);
			}
			/*
			celW  0 .. 255  UV animation: width of cel (U delta to apply at each update).  celW 4 
 
	celH  0 .. 255  UV animation: height of cel (V delta to apply when a "row" of cels is complete).  celH 16 
 
	celNum  0 .. n  UV animation: total number of cels before animation restarts.  celNum 32 
 
	celNumU  0 .. n  UV animation: total number of cels in a "row", i.e. before applying a V delta.  celNumU 8 
 
	celPeriod  0 .. n  UV animation: number of application updates before each UV animation update.  celPeriod 1 	 
			 */

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify a format to convert the image to, before uploading for SW rasterisation" });
				var valueCtrl = new EditEnumView() { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl, this.dataContext, mtl => mtl.FormatSW, (mtl, value) => mtl.FormatSW = value);
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify a format to convert the image to, before uploading for HW rasterisation" });
				var valueCtrl = new EditEnumView() { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl, this.dataContext, mtl => mtl.FormatHW, (mtl, value) => mtl.FormatHW = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Keep the contents of the texture after upload" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.KeepAfterUpload, (mtl, value) => mtl.KeepAfterUpload = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "If true the texture UV coordinates are clamped" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, this.dataContext, mtl => mtl.ClampUV, (mtl, value) => mtl.ClampUV = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Disable fogging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(valueCtrl, this.dataContext, mtl => mtl.NoFog, (mtl, value) => mtl.NoFog = value);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Attach the specified shader to this material" });
				var valueCtrl = new EditStringView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, string>(
					valueCtrl, this.dataContext, mtl => mtl.ShaderTechnique, (mtl, value) => mtl.ShaderTechnique = value);
			}
			this.ResumeLayout();
		}

		private DataContextContainer dataContext = new DataContextContainer();
		private SplitContainer formPreviewSplit;

		private Panel stackPanel;

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			//if (stackPanel.Width != ClientRectangle.Width)
			//    stackPanel.Width = ClientRectangle.Width;
		}

		private void InitializeComponent()
		{
			this.formPreviewSplit = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.formPreviewSplit)).BeginInit();
			this.formPreviewSplit.SuspendLayout();
			this.SuspendLayout();
			// 
			// formPreviewSplit
			// 
			this.formPreviewSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.formPreviewSplit.Location = new System.Drawing.Point(0, 0);
			this.formPreviewSplit.Name = "formPreviewSplit";
			this.formPreviewSplit.Size = new System.Drawing.Size(400, 150);
			this.formPreviewSplit.SplitterDistance = 133;
			this.formPreviewSplit.TabIndex = 0;
			// 
			// MaterialEditor
			// 
			this.Controls.Add(this.formPreviewSplit);
			this.Name = "MaterialEditor";
			this.Size = new System.Drawing.Size(400, 150);
			((System.ComponentModel.ISupportInitialize)(this.formPreviewSplit)).EndInit();
			this.formPreviewSplit.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		
	}
}
