﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;

using Autofac;
using Autofac.Core;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade.Views;
using Toe.Gx;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class MaterialEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly ICommandHistory history;

		private readonly IResourceManager resourceManager;

		private SplitContainer formPreviewSplit;

		private TableLayoutPanel stackPanel;

		#endregion

		#region Constructors and Destructors

		public MaterialEditor(
			IEditorEnvironment editorEnvironment,
			IResourceManager resourceManager,
			ICommandHistory history,
			ToeGraphicsContext graphicsContext,
			IComponentContext context)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.history = history;
			this.graphicsContext = graphicsContext;
			this.context = context;

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
			var preview = this.context.Resolve<MaterialPreview>(new Parameter[] { TypedParameter.From(this) });
			preview.Dock = DockStyle.Fill;
			this.formPreviewSplit.Panel2.Controls.Add(preview);
			this.dataContext.DataContextChanged += (s, a) => preview.RefreshScene();
			this.dataContext.PropertyChanged += (s, a) => preview.RefreshScene();

			int row = 0;
			{
				this.stackPanel.Controls.Add(new StringView { Text = "Material name", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditNameView(this.history) { Margin = new Padding(4), Dock = DockStyle.Fill };
				new DataContextBinding(valueCtrl, this.dataContext, false);
				//var valueCtrl = new EditStringView() { Margin = new Padding(4) };
				//Expression<Func<Material, string>> expression = mtl => mtl.Name;
				//Action<Material, string> updateValue = (mtl, value) => history.SetValue(mtl, mtl.Name, value, (a, b) => { a.Name = b; });
				//new PropertyBinding<Material, string>(valueCtrl, this.dataContext, expression, updateValue);

				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
			}
			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify texture for stage 0 (diffuse map)", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditResourceReferenceView(
					this.editorEnvironment, this.resourceManager, this.history, this.context, true)
					{ Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture0, null);
				++row;
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 1", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditResourceReferenceView(
					this.editorEnvironment, this.resourceManager, this.history, this.context, true)
					{ Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture1, null);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 2", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditResourceReferenceView(
					this.editorEnvironment, this.resourceManager, this.history, this.context, true)
					{ Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture2, null);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specify texture for stage 3", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditResourceReferenceView(
					this.editorEnvironment, this.resourceManager, this.history, this.context, true)
					{ Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.Texture3, null);
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify vertex shader to use", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditStringView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				Expression<Func<Material, string>> expression = mtl => mtl.VertexShader;
				Action<Material, string> updateValue = (mtl, value) => mtl.VertexShader = value;
				new PropertyBinding<Material, string>(valueCtrl, this.dataContext, expression, updateValue);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Emissive colour", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditColorView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				Expression<Func<Material, Color>> expression = mtl => mtl.ColEmissive;
				Action<Material, Color> updateValue =
					(mtl, value) => this.history.SetValue(mtl, mtl.ColEmissive, value, (a, b) => { a.ColEmissive = b; });
				new PropertyBinding<Material, Color>(valueCtrl, this.dataContext, expression, updateValue);
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Ambient colour", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditColorView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, Color>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ColAmbient,
					(mtl, value) => this.history.SetValue(mtl, mtl.ColAmbient, value, (a, b) => { a.ColAmbient = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Diffuse colour", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditColorView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, Color>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ColDiffuse,
					(mtl, value) => this.history.SetValue(mtl, mtl.ColDiffuse, value, (a, b) => { a.ColDiffuse = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Specular colour", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditColorView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, Color>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ColSpecular,
					(mtl, value) => this.history.SetValue(mtl, mtl.ColSpecular, value, (a, b) => { a.ColSpecular = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "The specular cosine power", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditByteView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, byte>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.SpecularPower,
					(mtl, value) => this.history.SetValue(mtl, mtl.SpecularPower, value, (a, b) => { a.SpecularPower = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Shading mode", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView { Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ShadeModeValues };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ShadeMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ShadeMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.ShadeMode, value, (a, b) => { a.ShadeMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Modulation mode (of texel by vertex colour)", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), Dock = DockStyle.Fill, WellKnownValues = MaterialEnumsValues.ModulateModeValues };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ModulateMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ModulateMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.ModulateMode, value, (a, b) => { a.ModulateMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Backface culling mode", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), Dock = DockStyle.Fill, WellKnownValues = MaterialEnumsValues.CullModeValues };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, CullMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.CullMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.CullMode, value, (a, b) => { a.CullMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(new StringView { Text = "Transparency (alpha) mode", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), Dock = DockStyle.Fill, WellKnownValues = MaterialEnumsValues.AlphaModeValues };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, AlphaMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.AlphaMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.AlphaMode, value, (a, b) => { a.AlphaMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Blend mode between texture stages 0 and 1", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), Dock = DockStyle.Fill, WellKnownValues = MaterialEnumsValues.BlendModeValues };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, BlendMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.BlendMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.BlendMode, value, (a, b) => { a.BlendMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Preset multi-texturing effect", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.EffectPresetValues, Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, EffectPreset>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.EffectPreset,
					(mtl, value) => this.history.SetValue(mtl, mtl.EffectPreset, value, (a, b) => { a.EffectPreset = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify the alpha test function", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.AlphaTestModeValues, Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, AlphaTestMode>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.AlphaTestMode,
					(mtl, value) => this.history.SetValue(mtl, mtl.AlphaTestMode, value, (a, b) => { a.AlphaTestMode = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Specify the alpha test reference value", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditByteView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, byte>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.AlphaTestValue,
					(mtl, value) => this.history.SetValue(mtl, mtl.AlphaTestValue, value, (a, b) => { a.AlphaTestValue = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Depth offset to apply when using SW rasterisation", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditIntView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, int>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ZDepthOfs,
					(mtl, value) => this.history.SetValue(mtl, mtl.ZDepthOfs, value, (a, b) => { a.ZDepthOfs = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Depth offset to apply when using HW rasterisation", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditIntView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, int>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ZDepthOfsHW,
					(mtl, value) => this.history.SetValue(mtl, mtl.ZDepthOfsHW, value, (a, b) => { a.ZDepthOfsHW = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Checked only if the material should not be rendered", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.Invisible,
					(mtl, value) => this.history.SetValue(mtl, mtl.Invisible, value, (a, b) => { a.Invisible = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView
						{ Text = "Checked only if the material's textures should use bilinear filtering", Dock = DockStyle.Fill },
					0,
					row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.Filtering,
					(mtl, value) => this.history.SetValue(mtl, mtl.Filtering, value, (a, b) => { a.Filtering = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = " Enable writing to depth buffer. Only affects HW rasterisation", Dock = DockStyle.Fill },
					0,
					row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.DepthWriteEnable,
					(mtl, value) => this.history.SetValue(mtl, mtl.DepthWriteEnable, value, (a, b) => { a.DepthWriteEnable = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Enable geometry merging for this material", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.MergeGeom,
					(mtl, value) => this.history.SetValue(mtl, mtl.MergeGeom, value, (a, b) => { a.MergeGeom = b; }));
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
					new StringView
						{
							Text = "Specify a format to convert the image to, before uploading for SW rasterisation",
							Dock = DockStyle.Fill
						},
					0,
					row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues, Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.FormatSW,
					(mtl, value) => this.history.SetValue(mtl, mtl.FormatSW, value, (a, b) => { a.FormatSW = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView
						{
							Text = "Specify a format to convert the image to, before uploading for HW rasterisation",
							Dock = DockStyle.Fill
						},
					0,
					row);
				var valueCtrl = new EditEnumView
					{ Margin = new Padding(4), WellKnownValues = MaterialEnumsValues.ImageFormatValues, Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ImageFormat>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.FormatHW,
					(mtl, value) => this.history.SetValue(mtl, mtl.FormatHW, value, (a, b) => { a.FormatHW = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Keep the contents of the texture after upload", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.KeepAfterUpload,
					(mtl, value) => this.history.SetValue(mtl, mtl.KeepAfterUpload, value, (a, b) => { a.KeepAfterUpload = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "If true the texture UV coordinates are clamped", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.ClampUV,
					(mtl, value) => this.history.SetValue(mtl, mtl.ClampUV, value, (a, b) => { a.ClampUV = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Disable fogging for this material", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditBoolView { Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, bool>(
					valueCtrl,
					this.dataContext,
					mtl => mtl.NoFog,
					(mtl, value) => this.history.SetValue(mtl, mtl.NoFog, value, (a, b) => { a.NoFog = b; }));
			}

			{
				this.stackPanel.Controls.Add(
					new StringView { Text = "Attach the specified shader to this material", Dock = DockStyle.Fill }, 0, row);
				var valueCtrl = new EditResourceReferenceView(
					this.editorEnvironment, this.resourceManager, this.history, this.context, true)
					{ Margin = new Padding(4), Dock = DockStyle.Fill };
				this.stackPanel.Controls.Add(valueCtrl, 1, row);
				++row;
				new PropertyBinding<Material, ResourceReference>(valueCtrl, this.dataContext, mtl => mtl.ShaderTechnique, null);
			}
			this.ResumeLayout();
		}

		#endregion
	}
}