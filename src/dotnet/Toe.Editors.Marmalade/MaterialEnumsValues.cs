using Toe.Editors.Interfaces.Views;
using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public static class MaterialEnumsValues
	{
		#region Constants and Fields

		public static EnumWellKnownValues AlphaModeValues = new EnumWellKnownValues
			{
				{ AlphaMode.NONE, new EnumValueOptions("NONE") },
				{ AlphaMode.HALF, new EnumValueOptions("HALF") },
				{ AlphaMode.ADD, new EnumValueOptions("ADD") },
				{ AlphaMode.SUB, new EnumValueOptions("SUB") },
				{ AlphaMode.BLEND, new EnumValueOptions("BLEND") },
				{ AlphaMode.DEFAULT, new EnumValueOptions("DEFAULT") },
			};

		public static EnumWellKnownValues AlphaTestModeValues = new EnumWellKnownValues
			{
				{ AlphaTestMode.DISABLED, new EnumValueOptions("ALPHATEST_DISABLED") },
				{ AlphaTestMode.NEVER, new EnumValueOptions("ALPHATEST_NEVER") },
				{ AlphaTestMode.LESS, new EnumValueOptions("ALPHATEST_LESS") },
				{ AlphaTestMode.EQUAL, new EnumValueOptions("ALPHATEST_EQUAL") },
				{ AlphaTestMode.LEQUAL, new EnumValueOptions("ALPHATEST_LEQUAL") },
				{ AlphaTestMode.GREATER, new EnumValueOptions("ALPHATEST_GREATER") },
				{ AlphaTestMode.NOTEQUAL, new EnumValueOptions("ALPHATEST_NOTEQUAL") },
				{ AlphaTestMode.GEQUAL, new EnumValueOptions("ALPHATEST_GEQUAL") },
				{ AlphaTestMode.ALWAYS, new EnumValueOptions("ALPHATEST_ALWAYS") },
			};

		public static EnumWellKnownValues BlendModeValues = new EnumWellKnownValues
			{
				{ BlendMode.MODULATE, new EnumValueOptions("BLEND_MODULATE") },
				{ BlendMode.DECAL, new EnumValueOptions("BLEND_DECAL") },
				{ BlendMode.ADD, new EnumValueOptions("BLEND_ADD") },
				{ BlendMode.REPLACE, new EnumValueOptions("BLEND_REPLACE") },
				{ BlendMode.BLEND, new EnumValueOptions("BLEND_BLEND") },
				{ BlendMode.MODULATE_2X, new EnumValueOptions("BLEND_MODULATE_2X") },
				{ BlendMode.MODULATE_4X, new EnumValueOptions("BLEND_MODULATE_4X") },
			};

		public static EnumWellKnownValues CullModeValues = new EnumWellKnownValues
			{
				{ CullMode.BACK, new EnumValueOptions("Back") },
				{ CullMode.FRONT, new EnumValueOptions("Front") },
				{ CullMode.NONE, new EnumValueOptions("Double sided") }
			};

		public static EnumWellKnownValues EffectPresetValues = new EnumWellKnownValues
			{
				{ EffectPreset.DEFAULT, new EnumValueOptions("DEFAULT") },
				{ EffectPreset.NORMAL_MAPPING, new EnumValueOptions("NORMAL_MAPPING") },
				{ EffectPreset.REFLECTION_MAPPING, new EnumValueOptions("REFLECTION_MAPPING") },
				{ EffectPreset.ENVIRONMENT_MAPPING, new EnumValueOptions("ENVIRONMENT_MAPPING") },
				{ EffectPreset.CONSTANT_COLOUR_CHANNEL, new EnumValueOptions("CONSTANT_COLOUR_CHANNEL") },
				{ EffectPreset.LIGHTMAP_POST_PROCESS, new EnumValueOptions("LIGHTMAP_POST_PROCESS") },
				{ EffectPreset.NORMAL_MAPPING_SPECULAR, new EnumValueOptions("NORMAL_MAPPING_SPECULAR") },
				{ EffectPreset.TEXTURE0_ONLY, new EnumValueOptions("TEXTURE0_ONLY") },
			};

		public static EnumWellKnownValues ImageFormatValues = new EnumWellKnownValues
			{
				{ ImageFormat.FORMAT_UNDEFINED, new EnumValueOptions("Undefined") },
				{ ImageFormat.RGB_332, new EnumValueOptions("RGB_332") },
				{ ImageFormat.BGR_332, new EnumValueOptions("BGR_332") },
				{ ImageFormat.RGB_565, new EnumValueOptions("RGB_565") },
				{ ImageFormat.BGR_565, new EnumValueOptions("BGR_565") },
				{ ImageFormat.RGBA_4444, new EnumValueOptions("RGBA_4444") },
				{ ImageFormat.ABGR_4444, new EnumValueOptions("ABGR_4444") },
				{ ImageFormat.RGBA_5551, new EnumValueOptions("RGBA_5551") },
				{ ImageFormat.ABGR_1555, new EnumValueOptions("ABGR_1555") },
				{ ImageFormat.RGB_888, new EnumValueOptions("RGB_888") },
				{ ImageFormat.BGR_888, new EnumValueOptions("BGR_888") },
				{ ImageFormat.RGBA_6666, new EnumValueOptions("RGBA_6666") },
				{ ImageFormat.ABGR_6666, new EnumValueOptions("ABGR_6666") },
				{ ImageFormat.RGBA_8888, new EnumValueOptions("RGBA_8888") },
				{ ImageFormat.ABGR_8888, new EnumValueOptions("ABGR_8888") },
				{ ImageFormat.RGBA_AAA2, new EnumValueOptions("RGBA_AAA2") },
				{ ImageFormat.ABGR_2AAA, new EnumValueOptions("ABGR_2AAA") },
				{ ImageFormat.PALETTE4_RGB_888, new EnumValueOptions("PALETTE4_RGB_888") },
				{ ImageFormat.PALETTE4_RGBA_8888, new EnumValueOptions("PALETTE4_RGBA_8888") },
				{ ImageFormat.PALETTE4_RGB_565, new EnumValueOptions("PALETTE4_RGB_565") },
				{ ImageFormat.PALETTE4_RGBA_4444, new EnumValueOptions("PALETTE4_RGBA_4444") },
				{ ImageFormat.PALETTE4_RGBA_5551, new EnumValueOptions("PALETTE4_RGBA_5551") },
				{ ImageFormat.PALETTE4_ABGR_1555, new EnumValueOptions("PALETTE4_ABGR_1555") },
				{ ImageFormat.PALETTE8_RGB_888, new EnumValueOptions("PALETTE8_RGB_888") },
				{ ImageFormat.PALETTE8_RGBA_8888, new EnumValueOptions("PALETTE8_RGBA_8888") },
				{ ImageFormat.PALETTE8_RGB_565, new EnumValueOptions("PALETTE8_RGB_565") },
				{ ImageFormat.PALETTE8_RGBA_4444, new EnumValueOptions("PALETTE8_RGBA_4444") },
				{ ImageFormat.PALETTE8_RGBA_5551, new EnumValueOptions("PALETTE8_RGBA_5551") },
				{ ImageFormat.PALETTE8_ABGR_1555, new EnumValueOptions("PALETTE8_ABGR_1555") },
				{ ImageFormat.PALETTE7_ABGR_1555, new EnumValueOptions("PALETTE7_ABGR_1555") },
				{ ImageFormat.PALETTE6_ABGR_1555, new EnumValueOptions("PALETTE6_ABGR_1555") },
				{ ImageFormat.PALETTE5_ABGR_1555, new EnumValueOptions("PALETTE5_ABGR_1555") },
				{ ImageFormat.PVRTC_2, new EnumValueOptions("PVRTC_2") },
				{ ImageFormat.PVRTC_4, new EnumValueOptions("PVRTC_4") },
				{ ImageFormat.ATITC, new EnumValueOptions("ATITC") },
				{ ImageFormat.COMPRESSED, new EnumValueOptions("COMPRESSED") },
				{ ImageFormat.PALETTE4_ABGR_4444, new EnumValueOptions("PALETTE4_ABGR_4444") },
				{ ImageFormat.PALETTE8_ABGR_4444, new EnumValueOptions("PALETTE8_ABGR_4444") },
				{ ImageFormat.A_8, new EnumValueOptions("A_8") },
				{ ImageFormat.ETC, new EnumValueOptions("ETC") },
				{ ImageFormat.ARGB_8888, new EnumValueOptions("ARGB_8888") },
				{ ImageFormat.PALETTE4_ARGB_8888, new EnumValueOptions("PALETTE4_ARGB_8888") },
				{ ImageFormat.PALETTE8_ARGB_8888, new EnumValueOptions("PALETTE8_ARGB_8888") },
				{ ImageFormat.DXT3, new EnumValueOptions("DXT3") },
				{ ImageFormat.PALETTE4_BGR555, new EnumValueOptions("PALETTE4_BGR555") },
				{ ImageFormat.PALETTE8_BGR555, new EnumValueOptions("PALETTE8_BGR555") },
				{ ImageFormat.A5_PALETTE3_BGR_555, new EnumValueOptions("A5_PALETTE3_BGR_555") },
				{ ImageFormat.A3_PALETTE5_BGR_555, new EnumValueOptions("A3_PALETTE5_BGR_555") },
				{ ImageFormat.PALETTE4_BGR_565, new EnumValueOptions("PALETTE4_BGR_565") },
				{ ImageFormat.PALETTE4_ABGR_8888, new EnumValueOptions("PALETTE4_ABGR_8888") },
				{ ImageFormat.PALETTE8_BGR_565, new EnumValueOptions("PALETTE8_BGR_565") },
				{ ImageFormat.PALETTE8_ABGR_8888, new EnumValueOptions("PALETTE8_ABGR_8888") },
				{ ImageFormat.DXT1, new EnumValueOptions("DXT1") },
				{ ImageFormat.DXT5, new EnumValueOptions("DXT5") },
			};

		public static EnumWellKnownValues ModulateModeValues = new EnumWellKnownValues
			{
				{ ModulateMode.NONE, new EnumValueOptions("None") },
				{ ModulateMode.R, new EnumValueOptions("R") },
				{ ModulateMode.RGB, new EnumValueOptions("RGB") }
			};

		public static EnumWellKnownValues ShadeModeValues = new EnumWellKnownValues
			{ { ShadeMode.FLAT, new EnumValueOptions("Flat") }, { ShadeMode.GOURAUD, new EnumValueOptions("Gouraud") } };

		#endregion
	}
}