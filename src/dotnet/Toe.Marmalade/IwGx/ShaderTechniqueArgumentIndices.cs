namespace Toe.Marmalade.IwGx
{
	public class ShaderTechniqueArgumentIndices
	{
		#region Constants and Fields

		public int ProgramId;

		/// <summary>
		/// Alpha test value. 
		/// </summary>
		public int inAlphaTestValue;

		/// <summary>
		/// Ambient light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inAmbient; 
		/// </summary>
		public int inAmbient;

		public int inBiTangent;

		/// <summary>
		/// Translation of the view matrix. 
		/// </summary>
		public int inCamPos;

		public int inCol;

		/// <summary>
		/// Size of IwGx device - equivalent to (IwGxGetDeviceWidth(), IwGxGetDeviceHeight())  Floating point vector  Not used  uniform mediump vec2 inDeviceSize; 
		/// </summary>
		public int inDeviceSize;

		/// <summary>
		/// Diffuse light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inDiffuse; 
		/// </summary>
		public int inDiffuse;

		/// <summary>
		/// Direction of diffuse light transformed into local coordinate space.  Floating point vector  Used for lighting vertices  uniform lowp vec3 inDiffuseDir; 
		/// </summary>
		public int inDiffuseDir;

		/// <summary>
		/// Transformation from window space to IwGx screen space. This can be useful for effects on rotated displays. If IwGx is rendering rotated then window coordinates are not orientated the same way as the 'logical' screen. Multiplying gl_FragCoord by this matrix produces a vector with extents 0.0-1.0 which is orientated the same way as the logical screen.  3x3 floating point matrix  Not used  uniform mediump mat3 inDisplayRotScaleMat; 
		/// </summary>
		public int inDisplayRotScaleMat;

		/// <summary>
		/// Size of IwGx display - equivalent to (IwGxGetDisplayWidth(), IwGxGetDisplayHeight())  Floating point vector  Not used  uniform mediump vec2 inDisplaySize; 
		/// </summary>
		public int inDisplaySize;

		/// <summary>
		/// Material emissive light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inEmissive; 
		/// </summary>
		public int inEmissive;

		public int inEyePos;

		/// <summary>
		/// Fog colour  Normalised floating point vector  Used for fogging  uniform mediump vec4 inFogColour; 
		/// </summary>
		public int inFogColour;

		/// <summary>
		/// Fog near value in clip space.  Floating point value.  Used for fogging  uniform mediump float inFogNear; 
		/// </summary>
		public int inFogNear;

		/// <summary>
		/// Fog range value in clip space.  Floating point value.  Used for fogging  uniform mediump float inFogRange; 
		/// </summary>
		public int inFogRange;

		/// <summary>
		/// Model-view matrix. The transform to view space. 
		/// </summary>
		public int inMVMat;

		/// <summary>
		/// Rotation/scale part of model-view matrix. Can be used to orient coordinate space directions (like normals) to view space. 
		/// 3x3 floating point matrix 
		/// </summary>
		public int inMVRotMat;

		/// <summary>
		/// Material ambient colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inMaterialAmbient; 
		/// </summary>
		public int inMaterialAmbient;

		/// <summary>
		/// Material diffuse colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inMaterialDiffuse; 
		/// </summary>
		public int inMaterialDiffuse;

		public int inMaterialSpecular;

		/// <summary>
		/// Translation of the model matrix. Can be used with 'inModelRotMat' to move position between model space and world space. 
		/// </summary>
		public int inModelPos;

		/// <summary>
		/// Rotation/scale part of model matrix. Can be used to orient coordinate space directions (like normals) to world space. 
		/// 3x3 floating point matrix 
		/// </summary>
		public int inModelRotMat;

		public int inNorm;

		/// <summary>
		/// One over texel dimensions of texture in stage 0  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public int inOOTextureSize;

		/// <summary>
		/// One over texel dimensions of texture in stage 1  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public int inOOTextureSize1;

		/// <summary>
		/// One over texel dimensions of texture in stage 2  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public int inOOTextureSize2;

		/// <summary>
		/// One over texel dimensions of texture in stage 3  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public int inOOTextureSize3;

		/// <summary>
		/// Projected model-view matrix. The full transform from vertex coordinate space to clip space 
		/// </summary>
		public int inPMVMat;

		/// <summary>
		/// 2 component vector describing the current pipeline config. If the first element is 1, transform is set to HW, if the second element is 1 lighting is set to HW.  Integer vector  Not used  uniform mediump ivec2 inPipelineConfig;  
		/// </summary>
		public int inPipelineConfig;

		/// <summary>
		/// Texture sampler for texture stage 0 
		/// </summary>
		public int inSampler0;

		/// <summary>
		/// Texture sampler for texture stage 1
		/// </summary>
		public int inSampler1;

		/// <summary>
		/// Texture sampler for texture stage 2
		/// </summary>
		public int inSampler2;

		/// <summary>
		/// Texture sampler for texture stage 3
		/// </summary>
		public int inSampler3;

		public int inSkinIndices;

		/// <summary>
		/// Array of matrices which contain transforms from bind-pose space to model space.
		/// </summary>
		public int inSkinMats;

		public int inSkinWeights;

		public int inSpecular;

		public int inSpecularHalfVec;

		public int inTVScale;

		public int inTangent;

		/// <summary>
		/// Texel dimensions of texture in stage 0  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public int inTextureSize;

		/// <summary>
		/// Texel dimensions of texture in stage 1  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public int inTextureSize1;

		/// <summary>
		/// Texel dimensions of texture in stage 2  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public int inTextureSize2;

		/// <summary>
		/// Texel dimensions of texture in stage 3  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public int inTextureSize3;

		public int inUV0;

		public int inUV1;

		/// <summary>
		/// Scale for UV stream 1.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV1Scale; 
		/// </summary>
		public int inUV1Scale;

		/// <summary>
		/// Scale for UV stream 2.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV2Scale; 
		/// </summary>
		public int inUV2Scale;

		/// <summary>
		/// Scale for UV stream 3.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV3Scale; 
		/// </summary>
		public int inUV3Scale;

		/// <summary>
		/// Offset for UV stream 0. inUV0 should be multiplied by inUVScale before applying the offset.  Floating point value  Used for UV animation  uniform mediump vec2 inUVOffset; 
		/// </summary>
		public int inUVOffset;

		/// <summary>
		/// Scale for UV stream 0.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUVScale; 
		/// </summary>
		public int inUVScale;

		public int inVert;

		#endregion
	}
}