using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Interfaces.Views;
using Toe.Gx;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class MaterialEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ICommandHistory history;

		private readonly ToeGraphicsContext graphicsContext;

		private SplitContainer formPreviewSplit;

		private TableLayoutPanel stackPanel;

		#endregion

		#region Constructors and Destructors

		public MaterialEditor(IEditorEnvironment editorEnvironment, ICommandHistory history, ToeGraphicsContext graphicsContext)
		{
			this.editorEnvironment = editorEnvironment;
			this.history = history;
			this.graphicsContext = graphicsContext;

			this.InitializeComponent();

			this.InitializeEditor();
		}

		
		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public Material Material
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (Material)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public override Size GetPreferredSize(Size proposedSize)
		{
			return this.stackPanel.GetPreferredSize(proposedSize);
			//return base.GetPreferredSize(proposedSize);
		}

		#endregion

		#region Methods

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			//if (stackPanel.Width != ClientRectangle.Width)
			//    stackPanel.Width = ClientRectangle.Width;
		}

		private void InitializeComponent()
		{
			this.formPreviewSplit = new SplitContainer();
			var i = this.formPreviewSplit as ISupportInitialize;
			if (i != null)
			{
				i.BeginInit();
			}
			this.formPreviewSplit.SuspendLayout();
			this.SuspendLayout();
			// 
			// formPreviewSplit
			// 
			this.formPreviewSplit.Dock = DockStyle.Fill;
			this.formPreviewSplit.Location = new Point(0, 0);
			this.formPreviewSplit.Name = "formPreviewSplit";
			this.formPreviewSplit.Size = new Size(400, 150);
			this.formPreviewSplit.SplitterDistance = 133;
			this.formPreviewSplit.TabIndex = 0;
			// 
			// MaterialEditor
			// 
			this.Controls.Add(this.formPreviewSplit);
			this.Name = "MaterialEditor";
			this.Size = new Size(400, 150);
			if (i != null)
			{
				i.EndInit();
			}
			this.formPreviewSplit.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		private void InitializeEditor()
		{
			this.SuspendLayout();

			this.AutoSize = true;
			this.Padding = new Padding(10);
			this.formPreviewSplit.Panel1MinSize = 200;

			this.stackPanel = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };
			this.stackPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
			this.stackPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
			var sp = new StackPanel { Dock = DockStyle.Fill, AutoSize = true, AutoScroll = true };

			sp.Controls.Add(this.stackPanel);
			this.formPreviewSplit.Panel1.Controls.Add(sp);

			//var preview = new PictureBox { Image = Resources.material, Dock = DockStyle.Fill, AutoSize = true };
			var preview = new MaterialPreview(this,this.graphicsContext) { Dock = DockStyle.Fill };
			this.formPreviewSplit.Panel2.Controls.Add(preview);
			this.dataContext.DataContextChanged += (s, a) => preview.RefreshScene();
			this.dataContext.PropertyChanged += (s, a) => preview.RefreshScene();

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Material name" });
				var valueCtrl = new EditStringView() { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				Expression<Func<Material, string>> expression = mtl => mtl.Name;
				Action<Material, string> updateValue = (mtl, value) => history.SetValue(mtl, mtl.Name, value, (a, b) => { a.Name = b; });
				new PropertyBinding<Material, string>(valueCtrl, this.dataContext, expression, updateValue);
			}
			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 0 (diffuse map)" });
				var valueCtrl = new EditTextureView(this.editorEnvironment, history) { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture0, null);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 1" });
				var valueCtrl = new EditTextureView(this.editorEnvironment, history) { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture1, null);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify vertex shader to use" });
				var valueCtrl = new EditStringView() { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				Expression<Func<Material, string>> expression = mtl => mtl.VertexShader;
				Action<Material, string> updateValue = (mtl, value) => mtl.VertexShader = value;
				new PropertyBinding<Material, string>(
					valueCtrl, this.dataContext, expression, updateValue);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Emissive colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				Expression<Func<Material, Color>> expression = mtl => mtl.ColEmissive;
				Action<Material, Color> updateValue = (mtl, value) => history.SetValue(mtl, mtl.ColEmissive, value, (a, b) => { a.ColEmissive = b; });
				new PropertyBinding<Material, Color>(valueCtrl, this.dataContext, expression, updateValue);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Ambient colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColAmbient, (mtl, value) => history.SetValue(mtl, mtl.ColAmbient, value, (a, b) => { a.ColAmbient = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Diffuse colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColDiffuse, (mtl, value) => history.SetValue(mtl, mtl.ColDiffuse, value, (a, b) => { a.ColDiffuse = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specular colour" });
				var valueCtrl = new EditColorView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, Color>(
					valueCtrl, this.dataContext, mtl => mtl.ColSpecular, (mtl, value) => history.SetValue(mtl, mtl.ColSpecular, value, (a, b) => { a.ColSpecular = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "The specular cosine power" });
				var valueCtrl = new EditIntegerView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, int>(
					valueCtrl, this.dataContext, mtl => mtl.SpecularPower, (mtl, value) => history.SetValue(mtl, mtl.SpecularPower, value, (a, b) => { a.SpecularPower = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Shading mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ShadeModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ShadeMode>(
					valueCtrl, this.dataContext, mtl => mtl.ShadeMode, (mtl, value) => history.SetValue(mtl, mtl.ShadeMode, value, (a, b) => { a.ShadeMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Modulation mode (of texel by vertex colour)" });
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ModulateModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ModulateMode>(
					valueCtrl, this.dataContext, mtl => mtl.ModulateMode, (mtl, value) => history.SetValue(mtl, mtl.ModulateMode, value, (a, b) => { a.ModulateMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Backface culling mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.CullModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, CullMode>(
					valueCtrl, this.dataContext, mtl => mtl.CullMode, (mtl, value) => history.SetValue(mtl, mtl.CullMode, value, (a, b) => { a.CullMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Transparency (alpha) mode" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.AlphaModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaMode>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaMode, (mtl, value) => history.SetValue(mtl, mtl.AlphaMode, value, (a, b) => { a.AlphaMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Blend mode between texture stages 0 and 1" });
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.BlendModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, BlendMode>(
					valueCtrl, this.dataContext, mtl => mtl.BlendMode, (mtl, value) => history.SetValue(mtl, mtl.BlendMode, value, (a, b) => { a.BlendMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Preset multi-texturing effect" });
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.EffectPresetValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, EffectPreset>(
					valueCtrl, this.dataContext, mtl => mtl.EffectPreset, (mtl, value) => history.SetValue(mtl, mtl.EffectPreset, value, (a, b) => { a.EffectPreset = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test function" });
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.AlphaTestModeValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, AlphaTestMode>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaTestMode, (mtl, value) => history.SetValue(mtl, mtl.AlphaTestMode, value, (a, b) => { a.AlphaTestMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify the alpha test reference value" });
				var valueCtrl = new EditByteView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, byte>(
					valueCtrl, this.dataContext, mtl => mtl.AlphaTestValue, (mtl, value) => history.SetValue(mtl, mtl.AlphaTestValue, value, (a, b) => { a.AlphaTestValue = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using SW rasterisation" });
				var valueCtrl = new EditFloatView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, float>(
					valueCtrl, this.dataContext, mtl => mtl.ZDepthOfs, (mtl, value) => history.SetValue(mtl, mtl.ZDepthOfs, value, (a, b) => { a.ZDepthOfs = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Depth offset to apply when using HW rasterisation" });
				var valueCtrl = new EditFloatView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, float>(
					valueCtrl, this.dataContext, mtl => mtl.ZDepthOfsHW, (mtl, value) => history.SetValue(mtl, mtl.ZDepthOfsHW, value, (a, b) => { a.ZDepthOfsHW = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Checked only if the material should not be rendered" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.Invisible, (mtl, value) => history.SetValue(mtl, mtl.Invisible, value, (a, b) => { a.Invisible = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Checked only if the material's textures should use bilinear filtering" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.Filtering, (mtl, value) => history.SetValue(mtl, mtl.Filtering, value, (a, b) => { a.Filtering = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = " Enable writing to depth buffer. Only affects HW rasterisation" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.DepthWriteEnable, (mtl, value) => history.SetValue(mtl, mtl.DepthWriteEnable, value, (a, b) => { a.DepthWriteEnable = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Enable geometry merging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.MergeGeom, (mtl, value) => history.SetValue(mtl, mtl.MergeGeom, value, (a, b) => { a.MergeGeom = b; }));
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
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl, this.dataContext, mtl => mtl.FormatSW, (mtl, value) => history.SetValue(mtl, mtl.FormatSW, value, (a, b) => { a.FormatSW = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify a format to convert the image to, before uploading for HW rasterisation" });
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl, this.dataContext, mtl => mtl.FormatHW, (mtl, value) => history.SetValue(mtl, mtl.FormatHW, value, (a, b) => { a.FormatHW = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Keep the contents of the texture after upload" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.KeepAfterUpload, (mtl, value) => history.SetValue(mtl, mtl.KeepAfterUpload, value, (a, b) => { a.KeepAfterUpload = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "If true the texture UV coordinates are clamped" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.ClampUV, (mtl, value) => history.SetValue(mtl, mtl.ClampUV, value, (a, b) => { a.ClampUV = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Disable fogging for this material" });
				var valueCtrl = new EditBoolView { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, bool>(
					valueCtrl, this.dataContext, mtl => mtl.NoFog, (mtl, value) => history.SetValue(mtl, mtl.NoFog, value, (a, b) => { a.NoFog = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Attach the specified shader to this material" });
				var valueCtrl = new EditShaderView(history) { Margin = new Padding(4) };
				this.stackPanel.Controls.Add(valueCtrl);
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.ShaderTechnique, null);
			}
			this.ResumeLayout();
		}

		#endregion

	}

	
}