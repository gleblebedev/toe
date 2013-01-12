using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using OpenTK.Graphics.OpenGL;

using Toe.Marmalade.IwGx;

namespace Toe.Gx
{
	public class DefaultShaders : IDisposable
	{
		Dictionary<DefaultFragmentShaderOptions, int> generatedFragmentShaders = new Dictionary<DefaultFragmentShaderOptions, int>();
		Dictionary<DefaultVertexShaderOptions, int> generatedVertexShaders = new Dictionary<DefaultVertexShaderOptions, int>();
		Dictionary<DefaultProgramOptions, ShaderTechniqueArgumentIndices> generatedProgramms = new Dictionary<DefaultProgramOptions, ShaderTechniqueArgumentIndices>();

//        private string defaultFragmentShader = @"
//void main (void)
//{
//	gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
//}
//";

//        string defaultVertexShader =
//            @"attribute highp vec4    inVert;
//		uniform highp mat4  inPMVMat;
//		
//		void main(void)
//		{
//	
//		  gl_Position = inPMVMat * inVert;
//		}";


		private string defaultFragmentShader = @"#if defined(TEX0)
	uniform sampler2D		inSampler0;
#endif
#if defined(TEX1)
	uniform sampler2D		inSampler1;
#endif
#if defined(UV0_STREAM) && defined(UV1_STREAM)
	varying mediump vec2	outUV;
	varying mediump vec2	outUV1;
	#define OUT_UV0 outUV
	#define OUT_UV1 outUV1
	#elif defined(UV0_STREAM)
	varying mediump vec2	outUV;
	#define OUT_UV0 outUV
#elif defined(UV1_STREAM)
	varying mediump vec2	outUV;
	#define OUT_UV1 outUV
#endif
#if defined(FAST_FOG)
	varying lowp vec4	outFogCol;
#endif
#if defined(COL_STREAM) || defined(LIGHT_AMBIENT) || defined(LIGHT_EMISSIVE) || defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR)
	varying lowp vec4	outCol;
#endif
#if defined(ALPHA_TEST)
	uniform lowp float	inAlphaTestValue;
#endif
uniform lowp vec4	inMaterialAmbient;
#ifdef ALPHA_BLEND
	#if ALPHA_BLEND == 1
		//ALPHA_HALF:
		#pragma profilepragma blendoperation(gl_FragColor, GL_FUNC_ADD, GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA)
	#elif ALPHA_BLEND == 2
		//ALPHA_ADD:
		#pragma profilepragma blendoperation(gl_FragColor, GL_FUNC_ADD, GL_SRC_ALPHA, GL_ONE)
	#elif ALPHA_BLEND == 3
		//ALPHA_SUB
		#pragma profilepragma blendoperation(gl_FragColor, GL_FUNC_ADD, GL_ZERO, GL_ONE_MINUS_SRC_COLOR)
	#elif ALPHA_BLEND == 4
		//ALPHA_BLEND
		#pragma profilepragma blendoperation(gl_FragColor, GL_FUNC_ADD, GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA)
	#endif
#endif

#if defined(FOG)
	varying lowp float outFog;
	uniform lowp vec4  inFogColour;
#endif

//ALPHA_TEST corresponds to gx material alpha test enum:
//		ALPHATEST_DISABLED = 0,		//!< no alpha testing;
//		ALPHATEST_NEVER =	1,		//!< never pass alpha test;
//		ALPHATEST_LESS =	2,			//!< pass alpha test when alpha is less that reference value;
//		ALPHATEST_EQUAL =	3,		//!< pass alpha test when alpha is equal that reference value;
//		ALPHATEST_LEQUAL =	4,		//!< pass alpha test when alpha is less or equal that reference value;
//		ALPHATEST_GREATER = 5,		//!< pass alpha test when alpha is greater that reference value;
//		ALPHATEST_NOTEQUAL = 6,		//!< pass alpha test when alpha is not equal that reference value;
//		ALPHATEST_GEQUAL =	7,		//!< pass alpha test when alpha is greater or equal that reference value;

#if defined(ALPHA_TEST)
#if defined(IW_GX_PLATFORM_TEGRA2)
void performAlphaTest(mediump float val)
#else
void performAlphaTest(lowp float val)
#endif
{
	if( (val
#if ALPHA_TEST==1
		!=  // this condition is never executed - the fragment is discarded immediately - but must be here to compile
#elif ALPHA_TEST==2
		>=
#elif ALPHA_TEST==3
		!=
#elif ALPHA_TEST==4
		>
#elif ALPHA_TEST==5
		<=
#elif ALPHA_TEST==6
		==
#elif ALPHA_TEST==7
		<
#endif
	inAlphaTestValue) )
		discard;
}
#endif
#if defined(COL_STREAM) || defined(LIGHT_AMBIENT) || defined(LIGHT_EMISSIVE) || defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR)
	#define VERT_COL outCol
#else
	#define VERT_COL inMaterialAmbient
#endif
#if defined(TEX0)
	#if defined(UV0_STREAM)
		#if defined(EFFECT_PRESET)
			#if EFFECT_PRESET==1 || EFFECT_PRESET==6
				#define PRECALC mediump float dotresult = 4.0 * dot(VERT_COL.rgb - vec3(0.5), texture2D(inSampler0, OUT_UV0).rgb - vec3(0.5));
				#define TERM0 vec4(dotresult, dotresult, dotresult, VERT_COL.a)
			#elif EFFECT_PRESET==4
				#define TERM0 vec4(VERT_COL.rgb, texture2D(inSampler0, OUT_UV0).a)
			#else
				#define TERM0 VERT_COL * texture2D(inSampler0, OUT_UV0)
			#endif
		#else
			#define TERM0	VERT_COL * texture2D(inSampler0, OUT_UV0)
		#endif
	#else
		#define TERM0	VERT_COL * texture2D(inSampler0, vec2(0,0))
	#endif
#else
	#define TERM0 VERT_COL
#endif
#if defined(TEX1) && defined(UV1_STREAM)
//			BLEND corresponds to CIwMaterial blend mode:
//				BLEND_MODULATE,	//!< multiply;
//				BLEND_DECAL,	//!< decal;
//				BLEND_ADD,		//!< additive;
//				BLEND_REPLACE,	//!< replace;
//				BLEND_BLEND,	//!< blend (strange inverting behaviour);
//				BLEND_MODULATE_2X,	//!< multiply;
//				BLEND_MODULATE_4X,	//!< multiply;
	#if defined(BLEND)
		#if BLEND==1
			#define PRECALC lowp vec4 col1 = texture2D(inSampler1, OUT_UV1)
			#define TERM1  * (1.0 - col1.a) + vec4(col1.rgb * (col1.a), 0.0)
		#elif BLEND==2
			#define TERM1 + texture2D(inSampler1, OUT_UV1)
		#elif BLEND==3
			#undef TERM0
			#define TERM1 texture2D(inSampler1, OUT_UV1)
		#elif BLEND==4
			#define PRECALC lowp vec4 col1 = texture2D(inSampler1, OUT_UV1)
			#define TERM1 * vec4(vec3(1.0) - col1.rgb, col1.a);
		#elif BLEND==5
			#define TERM1 * texture2D(inSampler1, OUT_UV1)*2.0
		#elif BLEND==6
			#define TERM1 * texture2D(inSampler1, OUT_UV1)*4.0
		#endif
	#else
		#define TERM1 * texture2D(inSampler1, OUT_UV1)
	#endif
#endif
void main (void)
{
#if defined(ALPHA_TEST)
#if ALPHA_TEST==1
	discard;
#endif
#endif
#ifdef PRECALC
	PRECALC;
#endif
#if defined(FOG) || (defined(ALPHA_TEST) && !defined(IW_GX_PLATFORM_TEGRA2))
	lowp vec4 fragColor =
#else
	gl_FragColor =
#endif
#ifdef TERM0
	(TERM0)
#endif
#ifdef TERM1
	TERM1
#endif
#if defined(FAST_FOG)
	+ outFogCol
#endif
	;
#if defined(ALPHA_TEST)
#if defined(IW_GX_PLATFORM_TEGRA2)
	performAlphaTest(gl_FragColor.a);
#else
	performAlphaTest(fragColor.a);
#endif
#endif
#if defined(FOG)
	gl_FragColor = mix(fragColor, vec4(inFogColour.xyz, fragColor.a), clamp(outFog, 0.0, 1.0));
#elif (defined(ALPHA_TEST) && !defined(IW_GX_PLATFORM_TEGRA2))
	gl_FragColor = fragColor;
#endif
}
";

		string defaultVertexShader =
			@"attribute highp vec4    inVert;
uniform highp mat4  inPMVMat;

#if defined(UV0_STREAM)
attribute mediump vec2  inUV0;
uniform mediump vec2    inUVOffset;
uniform mediump vec2    inUVScale;
#endif

#if defined(UV1_STREAM)
attribute mediump vec2  inUV1;
uniform mediump vec2    inUV1Scale;
#endif

#if defined(UV0_STREAM) && defined(UV1_STREAM)
varying mediump vec2    outUV;
varying mediump vec2    outUV1;
#define OUT_UV0 outUV

#define OUT_UV1 outUV1
#elif defined(UV0_STREAM)
varying mediump vec2    outUV;
#define OUT_UV0 outUV

#elif defined(UV1_STREAM)
varying mediump vec2    outUV;
#define OUT_UV1 outUV

#endif

#if defined(COL_STREAM)
attribute lowp vec4 inCol;
#else
uniform lowp vec4   inMaterialAmbient;
#endif

#if defined(COL_STREAM) || defined(LIGHT_AMBIENT) || defined(LIGHT_EMISSIVE) || defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR)
varying lowp vec4   outCol;
#endif

#if defined(FAST_FOG)
uniform mediump float inFogNear;
uniform mediump float inFogRange;
uniform lowp vec4  inFogColour;
varying lowp vec4   outFogCol;
#endif

#if defined(FOG)
uniform mediump float inFogNear;
uniform mediump float inFogRange;
varying lowp float  outFog;
#endif

#if defined(NORM_STREAM)
attribute mediump vec4  inNorm;
#endif

#if defined(TANGENT_STREAM)
attribute mediump vec4  inTangent;
#endif

#if defined(BITANGENT_STREAM)
attribute mediump vec4  inBiTangent;
#endif

#if defined(SKINWEIGHT_STREAM)
attribute mediump vec4      inSkinWeights;
attribute mediump vec4      inSkinIndices;
uniform highp mat4  inSkinMats[32];
#endif

#if defined(LIGHT_AMBIENT)
uniform lowp vec4 inAmbient;
#endif

#if defined(LIGHT_EMISSIVE)
uniform lowp vec4 inEmissive;
#endif

#if defined(LIGHT_DIFFUSE)
uniform mediump vec4 inDiffuse;
uniform mediump vec3 inDiffuseDir;
uniform mediump vec4 inMaterialDiffuse;
#endif

#if defined(LIGHT_SPECULAR)
uniform mediump vec4 inSpecular;
uniform mediump vec3 inSpecularHalfVec;
uniform mediump vec4 inMaterialSpecular;
#endif

#if !defined(NORM_STREAM) && (defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR))
attribute mediump vec4  inNorm;
#endif

void main(void)
{
#if defined(SKINWEIGHT_STREAM)
//Qualcomm Adreno 205 doesn't work if indexing of matrix array is inlined
    highp mat4 outSkinMatX = inSkinMats[int(inSkinIndices.x)];
    gl_Position = (inVert * outSkinMatX) * inSkinWeights.x;
    highp mat4 outSkinMatY = inSkinMats[int(inSkinIndices.y)];
    gl_Position += (inVert * outSkinMatY) * inSkinWeights.y;
    highp mat4 outSkinMatZ = inSkinMats[int(inSkinIndices.z)];
    gl_Position += (inVert * outSkinMatZ) * inSkinWeights.z;

//NB. Mali r0p1 can't handle this much vertex shader work
    highp mat4 outSkinMatW = inSkinMats[int(inSkinIndices.w)];
    gl_Position += (inVert * outSkinMatW) * inSkinWeights.w;
    gl_Position = inPMVMat * gl_Position;
#else
    gl_Position = inPMVMat * inVert;

#endif

#if defined(FAST_FOG)
    outFogCol = inFogColour * clamp((gl_Position.z - inFogNear) / inFogRange, 0.0, 1.0);
#endif

#if defined(FOG)
    outFog = (gl_Position.z - inFogNear) / inFogRange;
#endif

#if defined(UV0_STREAM)
    OUT_UV0 = inUV0 * inUVScale + inUVOffset;
#endif

#if defined(UV1_STREAM)
    OUT_UV1 = inUV1 * inUV1Scale;
#endif

#if defined(LIGHT_AMBIENT) || defined(LIGHT_EMISSIVE) || defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR)
    mediump vec4 baseCol =
    #if defined(LIGHT_EMISSIVE)
        inEmissive;
    #else
        vec4(0.0);
    #endif

    lowp vec4 matCol =
    #if defined(COL_STREAM)
        inCol;
    #else
        inMaterialAmbient;
    #endif

    #if defined(LIGHT_AMBIENT)
        baseCol += inAmbient * matCol;
    #else
        baseCol += matCol;
    #endif

    #if defined(LIGHT_DIFFUSE) || defined(LIGHT_SPECULAR)
        #if defined(SKINWEIGHT_STREAM)
            #if defined(SKIN_NORMALS)
                #if defined(SKIN_MAJOR_BONE)
                    mediump vec3 norm = normalize(vec4(inNorm.xyz, 0.0) * inSkinMats[int(inSkinIndices.x)]).xyz;
                #else
                    mediump vec4 normWZ = vec4(inNorm.xyz, 0.0);
                    mediump vec3 norm = (normWZ * outSkinMatX).xyz * inSkinWeights.x;
                    norm += (normWZ * outSkinMatY).xyz * inSkinWeights.y;
                    norm += (normWZ * outSkinMatZ).xyz * inSkinWeights.z;
                    norm += (normWZ * outSkinMatW).xyz * inSkinWeights.w;
                    norm = normalize(norm);
                #endif
            #else
                mediump vec3 norm = normalize(inNorm.xyz);
            #endif
        #else
            mediump vec3 norm = normalize(inNorm.xyz);
        #endif

    #endif

    #if defined(LIGHT_DIFFUSE)
        mediump vec4 matDif =
        #if defined(COL_STREAM)
            inCol;
        #else
            inMaterialDiffuse;
        #endif
        baseCol.rgb += matDif.rgb * inDiffuse.rgb * max(dot(inDiffuseDir, norm), 0.0);
    #endif

    #if defined(LIGHT_SPECULAR)
        mediump float spec = max(dot(inSpecularHalfVec, norm), 0.0);
        spec = pow(spec, inMaterialSpecular.a);
        baseCol.rgb += inSpecular.rgb * inMaterialSpecular.rgb * spec;
        baseCol.a += inSpecular.a * spec;
    #endif

    outCol = clamp(baseCol, 0.0, 1.0);

#elif defined(COL_STREAM)
    outCol = inCol;
#endif
}"
			;

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			
		}

		#endregion

		public ShaderTechniqueArgumentIndices GetProgram(DefaultProgramOptions po)
		{
			ShaderTechniqueArgumentIndices p;
			if (!this.generatedProgramms.TryGetValue(po, out p))
			{
				p = new ShaderTechniqueArgumentIndices();
				int f = this.GenFragmentShader(po.FragmentShaderOptions);
				int v = this.GenVertexShader(po.VertexShaderOptions);
				p.ProgramId = GL.CreateProgram();
				OpenTKHelper.Assert();
				this.generatedProgramms[po] = p;
				GL.AttachShader(p.ProgramId, v);
				OpenTKHelper.Assert();
				GL.AttachShader(p.ProgramId, f);
				OpenTKHelper.Assert();
				GL.LinkProgram(p.ProgramId);

				string programInfoLog;
				GL.GetProgramInfoLog(p.ProgramId, out programInfoLog);
				Debug.WriteLine(programInfoLog);
				OpenTKHelper.Assert();

				p.inVert = GL.GetAttribLocation(p.ProgramId, "inVert");
				OpenTKHelper.Assert();
				p.inUV0 = GL.GetAttribLocation(p.ProgramId, "inUV0");
				OpenTKHelper.Assert();
				p.inUV1 = GL.GetAttribLocation(p.ProgramId, "inUV1");
				OpenTKHelper.Assert();
				p.inCol = GL.GetAttribLocation(p.ProgramId, "inCol");
				OpenTKHelper.Assert();
				p.inNorm = GL.GetAttribLocation(p.ProgramId, "inNorm");
				OpenTKHelper.Assert();
				p.inTangent = GL.GetAttribLocation(p.ProgramId, "inTangent");
				OpenTKHelper.Assert();
				p.inBiTangent = GL.GetAttribLocation(p.ProgramId, "inBiTangent");
				OpenTKHelper.Assert();
				p.inSkinWeights = GL.GetAttribLocation(p.ProgramId, "inSkinWeights");
				OpenTKHelper.Assert();
				p.inSkinIndices = GL.GetAttribLocation(p.ProgramId, "inSkinIndices");
				OpenTKHelper.Assert();


				p.inPMVMat = GL.GetUniformLocation(p.ProgramId, "inPMVMat");
				OpenTKHelper.Assert();
				p.inMVMat = GL.GetUniformLocation(p.ProgramId, "inMVMat");
				OpenTKHelper.Assert();
				p.inMVRotMat = GL.GetUniformLocation(p.ProgramId, "inMVRotMat");
				OpenTKHelper.Assert();
				p.inModelRotMat = GL.GetUniformLocation(p.ProgramId, "inModelRotMat");
				OpenTKHelper.Assert();
				p.inModelPos = GL.GetUniformLocation(p.ProgramId, "inModelPos");
				OpenTKHelper.Assert();
				p.inCamPos = GL.GetUniformLocation(p.ProgramId, "inCamPos");
				OpenTKHelper.Assert();
				p.inSampler0 = GL.GetUniformLocation(p.ProgramId, "inSampler0");
				OpenTKHelper.Assert();
				p.inSampler1 = GL.GetUniformLocation(p.ProgramId, "inSampler1");
				OpenTKHelper.Assert();
				p.inSampler2 = GL.GetUniformLocation(p.ProgramId, "inSampler2");
				OpenTKHelper.Assert();
				p.inSampler3 = GL.GetUniformLocation(p.ProgramId, "inSampler3");
				OpenTKHelper.Assert();
				p.inAlphaTestValue = GL.GetUniformLocation(p.ProgramId, "inAlphaTestValue");
				OpenTKHelper.Assert();
				p.inMaterialAmbient = GL.GetUniformLocation(p.ProgramId, "inMaterialAmbient");
				OpenTKHelper.Assert();
				p.inMaterialDiffuse = GL.GetUniformLocation(p.ProgramId, "inMaterialDiffuse");
				OpenTKHelper.Assert();
				p.inEmissive = GL.GetUniformLocation(p.ProgramId, "inEmissive");
				OpenTKHelper.Assert();
				p.inAmbient = GL.GetUniformLocation(p.ProgramId, "inAmbient");
				OpenTKHelper.Assert();
				p.inDiffuse = GL.GetUniformLocation(p.ProgramId, "inDiffuse");
				OpenTKHelper.Assert();
				p.inDiffuseDir = GL.GetUniformLocation(p.ProgramId, "inDiffuseDir");
				OpenTKHelper.Assert();
				p.inFogNear = GL.GetUniformLocation(p.ProgramId, "inFogNear");
				OpenTKHelper.Assert();
				p.inFogRange = GL.GetUniformLocation(p.ProgramId, "inFogRange");
				OpenTKHelper.Assert();
				p.inFogColour = GL.GetUniformLocation(p.ProgramId, "inFogColour");
				OpenTKHelper.Assert();
				p.inSkinMats = GL.GetUniformLocation(p.ProgramId, "inSkinMats");
				OpenTKHelper.Assert();
				p.inUVOffset = GL.GetUniformLocation(p.ProgramId, "inUVOffset");
				OpenTKHelper.Assert();
				p.inUVScale = GL.GetUniformLocation(p.ProgramId, "inUVScale");
				OpenTKHelper.Assert();
				p.inUV1Scale = GL.GetUniformLocation(p.ProgramId, "inUV1Scale");
				OpenTKHelper.Assert();
				p.inUV2Scale = GL.GetUniformLocation(p.ProgramId, "inUV2Scale");
				OpenTKHelper.Assert();
				p.inUV3Scale = GL.GetUniformLocation(p.ProgramId, "inUV3Scale");
				OpenTKHelper.Assert();
				p.inTextureSize = GL.GetUniformLocation(p.ProgramId, "inTextureSize");
				OpenTKHelper.Assert();
				p.inTextureSize1 = GL.GetUniformLocation(p.ProgramId, "inTextureSize1");
				OpenTKHelper.Assert();
				p.inTextureSize2 = GL.GetUniformLocation(p.ProgramId, "inTextureSize2");
				OpenTKHelper.Assert();
				p.inTextureSize3 = GL.GetUniformLocation(p.ProgramId, "inTextureSize3");
				OpenTKHelper.Assert();
				p.inOOTextureSize = GL.GetUniformLocation(p.ProgramId, "inOOTextureSize");
				OpenTKHelper.Assert();
				p.inOOTextureSize1 = GL.GetUniformLocation(p.ProgramId, "inOOTextureSize1");
				OpenTKHelper.Assert();
				p.inOOTextureSize2 = GL.GetUniformLocation(p.ProgramId, "inOOTextureSize2");
				OpenTKHelper.Assert();
				p.inOOTextureSize3 = GL.GetUniformLocation(p.ProgramId, "inOOTextureSize3");
				OpenTKHelper.Assert();
				p.inDisplaySize = GL.GetUniformLocation(p.ProgramId, "inDisplaySize");
				OpenTKHelper.Assert();
				p.inDeviceSize = GL.GetUniformLocation(p.ProgramId, "inDeviceSize");
				OpenTKHelper.Assert();
				p.inDisplayRotScaleMat = GL.GetUniformLocation(p.ProgramId, "inDisplayRotScaleMat");
				OpenTKHelper.Assert();
				p.inPipelineConfig = GL.GetUniformLocation(p.ProgramId, "inPipelineConfig");
				OpenTKHelper.Assert();

				p.inSpecular = GL.GetUniformLocation(p.ProgramId, "inSpecular");
				OpenTKHelper.Assert();
				p.inMaterialSpecular = GL.GetUniformLocation(p.ProgramId, "inMaterialSpecular");
				OpenTKHelper.Assert();
				p.inSpecularHalfVec = GL.GetUniformLocation(p.ProgramId, "inSpecularHalfVec");
				OpenTKHelper.Assert();
			}
			return p;
		}

		private int GenFragmentShader(DefaultFragmentShaderOptions fso)
		{
			OpenTKHelper.Assert();
			int p;
			if (!this.generatedFragmentShaders.TryGetValue(fso, out p))
			{
				ShaderType fragmentShader = ShaderType.FragmentShader;
				string fragmentShaderSource = this.GetFragmentShaderSource(ref fso);
				p = GenShader(fragmentShader, fragmentShaderSource);
				this.generatedFragmentShaders[fso] = p;
			}
			return p;
		}

		private static int GenShader(ShaderType fragmentShader, string fragmentShaderSource)
		{
			int p;
			p = GL.CreateShader(fragmentShader);
			OpenTKHelper.Assert();
			string adaptSource = ShaderTechnique.AdaptSource(fragmentShaderSource);
			GL.ShaderSource(p, adaptSource);
			OpenTKHelper.Assert();
			GL.CompileShader(p);
			OpenTKHelper.Assert();
			int compileStatus;
			GL.GetShader(p, ShaderParameter.CompileStatus, out compileStatus);
			if (compileStatus == 0)
			{
				string shaderInfoLog;
				GL.GetShaderInfoLog(p, out shaderInfoLog);

				var r = new Regex(@"^(\d+)\:(\d+)\((\d+)\)");
				var m = r.Match(shaderInfoLog);
				if (m.Success)
				{
					var a= int.Parse(m.Groups[1].Value);
					var b = int.Parse(m.Groups[2].Value);
					var c = int.Parse(m.Groups[3].Value);

					var lines = adaptSource.Split(new char[] { '\n' });
					Debug.WriteLine(lines[b]);
					var from = c;
					//Debug.WriteLine(adaptSource.Substring(from, Math.Min(100, adaptSource.Length - from)));
				}


				throw new ApplicationException(shaderInfoLog);
			}
			return p;
		}

		private int GenVertexShader(DefaultVertexShaderOptions fso)
		{
			OpenTKHelper.Assert();
			int p;
			if (!this.generatedVertexShaders.TryGetValue(fso, out p))
			{
				ShaderType fragmentShader = ShaderType.VertexShader;
				string fragmentShaderSource = this.GetVertexShaderSource(ref fso);
				p = GenShader(fragmentShader, fragmentShaderSource);
				this.generatedVertexShaders[fso] = p;
			}
			return p;
		}

		private string GetVertexShaderSource(ref DefaultVertexShaderOptions fso)
		{
			var sb = new StringBuilder();

			if (fso.BITANGENT_STREAM)
			sb.Append("#define BITANGENT_STREAM\n");
			if (fso.FAST_FOG)
				sb.Append("#define FAST_FOG\n");
			if (fso.FOG)
				sb.Append("#define FOG\n");
			if (fso.LIGHT_AMBIENT)
				sb.Append("#define LIGHT_AMBIENT\n");
			if (fso.LIGHT_DIFFUSE)
				sb.Append("#define LIGHT_DIFFUSE\n");
			if (fso.LIGHT_SPECULAR)
				sb.Append("#define LIGHT_SPECULAR\n");
			if (fso.LIGHT_EMISSIVE)
				sb.Append("#define LIGHT_EMISSIVE\n");
			if (fso.NORM_STREAM)
				sb.Append("#define NORM_STREAM\n");
			if (fso.SKIN_MAJOR_BONE)
				sb.Append("#define SKIN_MAJOR_BONE\n");
			if (fso.SKINWEIGHT_STREAM)
				sb.Append("#define SKINWEIGHT_STREAM\n");
			if (fso.SKIN_NORMALS)
				sb.Append("#define SKIN_NORMALS\n");
			if (fso.TANGENT_STREAM)
				sb.Append("#define TANGENT_STREAM\n");
			if (fso.UV0_STREAM)
				sb.Append("#define UV0_STREAM\n");
			if (fso.UV1_STREAM)
				sb.Append("#define UV1_STREAM\n");
			if (fso.COL_STREAM)
				sb.Append("#define COL_STREAM\n");
			sb.Append(defaultVertexShader);
			return sb.ToString();
		}

		private string GetFragmentShaderSource(ref DefaultFragmentShaderOptions fso)
		{
			var sb = new StringBuilder();

			if (fso.COL_STREAM)
				sb.Append("#define COL_STREAM\n");
			if (fso.FAST_FOG)
				sb.Append("#define FAST_FOG\n");
			if (fso.FOG)
				sb.Append("#define FOG\n");
			if (fso.LIGHT_AMBIENT)
				sb.Append("#define LIGHT_AMBIENT\n");
			if (fso.LIGHT_DIFFUSE)
				sb.Append("#define LIGHT_DIFFUSE\n");
			if (fso.LIGHT_SPECULAR)
				sb.Append("#define LIGHT_SPECULAR\n");
			if (fso.LIGHT_EMISSIVE)
				sb.Append("#define LIGHT_EMISSIVE\n");
			if (fso.UV0_STREAM)
				sb.Append("#define UV0_STREAM\n");
			if (fso.UV1_STREAM)
				sb.Append("#define UV1_STREAM\n");
			if (fso.IW_GX_PLATFORM_TEGRA2)
				sb.Append("#define IW_GX_PLATFORM_TEGRA2\n");
			if (fso.TEX0)
				sb.Append("#define TEX0\n");
			if (fso.TEX1)
				sb.Append("#define TEX1\n");
			if (fso.ALPHA_TEST != 0)
				sb.Append(string.Format("#define ALPHA_TEST {0}\n", (int)fso.ALPHA_TEST));
			sb.Append(string.Format("#define ALPHA_BLEND {0}\n", (int)fso.ALPHA_BLEND));
			sb.Append(string.Format("#define BLEND {0}\n", (int)fso.BLEND));
			sb.Append(string.Format("#define EFFECT_PRESET {0}\n", (int)fso.EFFECT_PRESET));
			sb.Append(defaultFragmentShader);
			return sb.ToString();
		}
	}
}