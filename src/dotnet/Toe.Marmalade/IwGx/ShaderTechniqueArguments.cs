using OpenTK;

namespace Toe.Marmalade.IwGx
{
	public class ShaderTechniqueArguments
	{
		// Texture sampler for texture stage 0 
		//inSampler0..3

		#region Constants and Fields

		/// <summary>
		/// Alpha test value. 
		/// </summary>
		public float inAlphaTestValue;

		/// <summary>
		/// Ambient light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inAmbient; 
		/// </summary>
		public Vector4 inAmbient = new Vector4(1, 1, 1, 1);

		/// <summary>
		/// Translation of the view matrix. 
		/// </summary>
		public Vector3 inCamPos = Vector3.Zero;

		/// <summary>
		/// Size of IwGx device - equivalent to (IwGxGetDeviceWidth(), IwGxGetDeviceHeight())  Floating point vector  Not used  uniform mediump vec2 inDeviceSize; 
		/// </summary>
		public Vector2 inDeviceSize = new Vector2(1, 1);

		/// <summary>
		/// Diffuse light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inDiffuse; 
		/// </summary>
		public Vector4 inDiffuse = new Vector4(1, 1, 1, 1);

		/// <summary>
		/// Direction of diffuse light transformed into local coordinate space.  Floating point vector  Used for lighting vertices  uniform lowp vec3 inDiffuseDir; 
		/// </summary>
		public Vector3 inDiffuseDir = new Vector3(0, 0, 1);

		/// <summary>
		/// Transformation from window space to IwGx screen space. This can be useful for effects on rotated displays. If IwGx is rendering rotated then window coordinates are not orientated the same way as the 'logical' screen. Multiplying gl_FragCoord by this matrix produces a vector with extents 0.0-1.0 which is orientated the same way as the logical screen.  3x3 floating point matrix  Not used  uniform mediump mat3 inDisplayRotScaleMat; 
		/// </summary>
		public Matrix4 inDisplayRotScaleMat = Matrix4.Identity;

		/// <summary>
		/// Size of IwGx display - equivalent to (IwGxGetDisplayWidth(), IwGxGetDisplayHeight())  Floating point vector  Not used  uniform mediump vec2 inDisplaySize; 
		/// </summary>
		public Vector2 inDisplaySize = new Vector2(1, 1);

		/// <summary>
		/// Material emissive light colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inEmissive; 
		/// </summary>
		public Vector4 inEmissive = new Vector4(0, 0, 0, 0);

		public Vector3 inEyePos;

		/// <summary>
		/// Fog colour  Normalised floating point vector  Used for fogging  uniform mediump vec4 inFogColour; 
		/// </summary>
		public Vector4 inFogColour = new Vector4(1, 1, 1, 1);

		/// <summary>
		/// Fog near value in clip space.  Floating point value.  Used for fogging  uniform mediump float inFogNear; 
		/// </summary>
		public float inFogNear;

		/// <summary>
		/// Fog range value in clip space.  Floating point value.  Used for fogging  uniform mediump float inFogRange; 
		/// </summary>
		public float inFogRange = 4096;

		/// <summary>
		/// Model-view matrix. The transform to view space. 
		/// </summary>
		public Matrix4 inMVMat = Matrix4.Identity;

		/// <summary>
		/// Rotation/scale part of model-view matrix. Can be used to orient coordinate space directions (like normals) to view space. 
		/// 3x3 floating point matrix 
		/// </summary>
		public Matrix4 inMVRotMat = Matrix4.Identity;

		/// <summary>
		/// Material ambient colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inMaterialAmbient; 
		/// </summary>
		public Vector4 inMaterialAmbient = new Vector4(1, 1, 1, 1);

		/// <summary>
		/// Material diffuse colour value  Normalised floating point vector  Used for lighting vertices  uniform lowp vec4 inMaterialDiffuse; 
		/// </summary>
		public Vector4 inMaterialDiffuse = new Vector4(1, 1, 1, 1);

		public Vector4 inMaterialSpecular;

		/// <summary>
		/// Translation of the model matrix. Can be used with 'inModelRotMat' to move position between model space and world space. 
		/// </summary>
		public Vector3 inModelPos = Vector3.Zero;

		/// <summary>
		/// Rotation/scale part of model matrix. Can be used to orient coordinate space directions (like normals) to world space. 
		/// 3x3 floating point matrix 
		/// </summary>
		public Matrix4 inModelRotMat = Matrix4.Identity;

		/// <summary>
		/// One over texel dimensions of texture in stage 0  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public Vector2 inOOTextureSize = new Vector2(1, 1);

		/// <summary>
		/// One over texel dimensions of texture in stage 1  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public Vector2 inOOTextureSize1 = new Vector2(1, 1);

		/// <summary>
		/// One over texel dimensions of texture in stage 2  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public Vector2 inOOTextureSize2 = new Vector2(1, 1);

		/// <summary>
		/// One over texel dimensions of texture in stage 3  Floating point vector  Not used  uniform mediump vec2 inOOTextureSize; 
		/// </summary>
		public Vector2 inOOTextureSize3 = new Vector2(1, 1);

		/// <summary>
		/// Projected model-view matrix. The full transform from vertex coordinate space to clip space 
		/// </summary>
		public Matrix4 inPMVMat = Matrix4.Identity;

		public Vector4 inSpecular;

		public Vector3 inSpecularHalfVec;

		public Vector2 inTVScale = new Vector2(1, 1);

		/// <summary>
		/// Texel dimensions of texture in stage 0  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public Vector2 inTextureSize = new Vector2(1, 1);

		/// <summary>
		/// Texel dimensions of texture in stage 1  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public Vector2 inTextureSize1 = new Vector2(1, 1);

		/// <summary>
		/// Texel dimensions of texture in stage 2  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public Vector2 inTextureSize2 = new Vector2(1, 1);

		/// <summary>
		/// Texel dimensions of texture in stage 3  Floating point vector  Not used  uniform mediump vec2 inTextureSize; 
		/// </summary>
		public Vector2 inTextureSize3 = new Vector2(1, 1);

		/// <summary>
		/// Scale for UV stream 1.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV1Scale; 
		/// </summary>
		public Vector2 inUV1Scale = new Vector2(1, 1);

		/// <summary>
		/// Scale for UV stream 2.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV2Scale; 
		/// </summary>
		public Vector2 inUV2Scale = new Vector2(1, 1);

		/// <summary>
		/// Scale for UV stream 3.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV3Scale; 
		/// </summary>
		public Vector2 inUV3Scal = new Vector2(1, 1);

		/// <summary>
		/// Scale for UV stream 3.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUV2Scale; 
		/// </summary>
		public Vector2 inUV3Scale = new Vector2(1, 1);

		/// <summary>
		/// Offset for UV stream 0. inUV0 should be multiplied by inUVScale before applying the offset.  Floating point value  Used for UV animation  uniform mediump vec2 inUVOffset; 
		/// </summary>
		public Vector2 inUVOffset = Vector2.Zero;

		/// <summary>
		/// Scale for UV stream 0.  Floating point value  Primarily used to transform fixed coords into floating coords. UV scale is also used for non-power-of-2 textures and in some cases of texture atlassing.  uniform mediump vec2 inUVScale; 
		/// </summary>
		public Vector2 inUVScale = new Vector2(1, 1);

		#endregion

		//int[] inPipelineConfig  2 component vector describing the current pipeline config. If the first element is 1, transform is set to HW, if the second element is 1 lighting is set to HW.  Integer vector  Not used  uniform mediump ivec2 inPipelineConfig;  
	}
}