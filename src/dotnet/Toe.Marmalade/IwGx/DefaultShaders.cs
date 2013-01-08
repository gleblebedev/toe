using System;
using System.Collections.Generic;

namespace Toe.Marmalade.IwGx
{
	public class DefaultShaders : IDisposable
	{
		Dictionary<DefaultFragmentShaderOptions, int> generatedFragmentShaders = new Dictionary<DefaultFragmentShaderOptions, int>();
		Dictionary<DefaultVertexShaderOptions, int> generatedVertexShaders = new Dictionary<DefaultVertexShaderOptions, int>();
		Dictionary<DefaultProgramOptions, int> generatedProgramms = new Dictionary<DefaultProgramOptions, int>();

		string defaultFragmentShader = @"#if defined(TEX0)

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
	}
}