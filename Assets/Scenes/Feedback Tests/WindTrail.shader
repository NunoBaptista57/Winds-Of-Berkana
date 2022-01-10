// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WindTrail"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin][Header(Noise)]_NoiseScale("Noise Scale", Float) = 10
		_NoiseTimeScale("Noise Time Scale", Float) = 1
		_Seed("Seed", Float) = 0
		_OctaveScaleMultiplier("Octave Scale Multiplier", Float) = 2
		_InitialOctaveStrength("Initial Octave Strength", Float) = 0.5
		_OctaveStrengthMultiplier("Octave Strength Multiplier", Float) = 0.4
		[Header(Mapping)]_UVScale("UV Scale", Vector) = (1,1,0,0)
		_ScrollSpeed("Scroll Speed", Vector) = (0,0,0,0)
		_RemapParams("Remap Params", Vector) = (0,1,0,1)
		_TrubulenceScale("Trubulence Scale", Float) = 2
		_TurbulenceScroll("Turbulence Scroll", Vector) = (-0.7,0,0,0)
		_TurbulenceStrength("Turbulence Strength", Float) = 0.025
		[ASEEnd]_Color("Color", Color) = (1,1,1,0)

		[HideInInspector]_RenderQueueType("Render Queue Type", Float) = 5
		[HideInInspector][ToggleUI]_AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		//[HideInInspector]_ShadowMatteFilter("Shadow Matte Filter", Float) = 2
		[HideInInspector]_StencilRef("Stencil Ref", Int) = 0
		[HideInInspector]_StencilWriteMask("StencilWrite Mask", Int) = 6
		[HideInInspector]_StencilRefDepth("StencilRefDepth", Int) = 0
		[HideInInspector]_StencilWriteMaskDepth("_StencilWriteMaskDepth", Int) = 8
		[HideInInspector]_StencilRefMV("_StencilRefMV", Int) = 32
		[HideInInspector]_StencilWriteMaskMV("_StencilWriteMaskMV", Int) = 40
		[HideInInspector]_StencilRefDistortionVec("_StencilRefDistortionVec", Int) = 4
		[HideInInspector]_StencilWriteMaskDistortionVec("_StencilWriteMaskDistortionVec", Int) = 4
		[HideInInspector]_StencilWriteMaskGBuffer("_StencilWriteMaskGBuffer", Int) = 14
		[HideInInspector]_StencilRefGBuffer("_StencilRefGBuffer", Int) = 2
		[HideInInspector]_ZTestGBuffer("_ZTestGBuffer", Int) = 4
		[HideInInspector][ToggleUI]_RequireSplitLighting("_RequireSplitLighting", Float) = 0
		[HideInInspector][ToggleUI]_ReceivesSSR("_ReceivesSSR", Float) = 0
		[HideInInspector]_SurfaceType("_SurfaceType", Float) = 1
		[HideInInspector]_BlendMode("_BlendMode", Float) = 0
		[HideInInspector]_SrcBlend("_SrcBlend", Float) = 1
		[HideInInspector]_DstBlend("_DstBlend", Float) = 0
		[HideInInspector]_AlphaSrcBlend("Vec_AlphaSrcBlendtor1", Float) = 1
		[HideInInspector]_AlphaDstBlend("_AlphaDstBlend", Float) = 0
		[HideInInspector][ToggleUI]_ZWrite("_ZWrite", Float) = 1
		[HideInInspector][ToggleUI]_TransparentZWrite("_TransparentZWrite", Float) = 1
		[HideInInspector]_CullMode("Cull Mode", Float) = 2
		[HideInInspector]_TransparentSortPriority("_TransparentSortPriority", Int) = 0
		[HideInInspector][ToggleUI]_EnableFogOnTransparent("_EnableFogOnTransparent", Float) = 1
		[HideInInspector]_CullModeForward("_CullModeForward", Float) = 2
		[HideInInspector][Enum(Front, 1, Back, 2)]_TransparentCullMode("_TransparentCullMode", Float) = 2
		[HideInInspector]_ZTestDepthEqualForOpaque("_ZTestDepthEqualForOpaque", Int) = 4
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)]_ZTestTransparent("_ZTestTransparent", Float) = 4
		[HideInInspector][ToggleUI]_TransparentBackfaceEnable("_TransparentBackfaceEnable", Float) = 0
		[HideInInspector][ToggleUI]_AlphaCutoffEnable("_AlphaCutoffEnable", Float) = 0
		[HideInInspector][ToggleUI]_UseShadowThreshold("_UseShadowThreshold", Float) = 0
		[HideInInspector][ToggleUI]_DoubleSidedEnable("_DoubleSidedEnable", Float) = 0
		[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)]_DoubleSidedNormalMode("_DoubleSidedNormalMode", Float) = 2
		[HideInInspector]_DoubleSidedConstants("_DoubleSidedConstants", Vector) = (1, 1, -1, 0)
		[HideInInspector]_DistortionEnable("_DistortionEnable",Float) = 0
		[HideInInspector]_DistortionOnly("_DistortionOnly",Float) = 0
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Transparent" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma only_renderers d3d11 metal vulkan xboxone xboxseries playstation switch 
		#pragma instancing_options renderinglayer

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		
		Pass
		{
			
			Name "Forward Unlit"
			Tags { "LightMode"="ForwardOnly" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Cull [_CullMode]
			ZTest [_ZTestTransparent]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_FORWARD_UNLIT
			#pragma multi_compile _ DEBUG_DISPLAY

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
				#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
				#define HAS_LIGHTLOOP
				#define SHADOW_OPTIMIZE_REGISTER_USAGE 1

				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Shadow/HDShadowContext.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadow.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/PunctualLightCommon.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadowLoop.hlsl"
			#endif



			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float4 ShadowTint;
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				surfaceData.color = surfaceDescription.Color;
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription , FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
					HDShadowContext shadowContext = InitShadowContext();
					float shadow;
					float3 shadow3;
					posInput = GetPositionInput(fragInputs.positionSS.xy, _ScreenSize.zw, fragInputs.positionSS.z, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
					float3 normalWS = normalize(fragInputs.tangentToWorld[1]);
					uint renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
					ShadowLoopMin(shadowContext, posInput, normalWS, asuint(_ShadowMatteFilter), renderingLayers, shadow3);
					shadow = dot(shadow3, float3(1.0f/3.0f, 1.0f/3.0f, 1.0f/3.0f));

					float4 shadowColor = (1 - shadow)*surfaceDescription.ShadowTint.rgba;
					float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);

					#ifdef _SURFACE_TYPE_TRANSPARENT
						surfaceData.color = lerp(shadowColor.rgb*surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow), surfaceDescription.Alpha);
					#else
						surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow);
					#endif
					localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
					surfaceDescription.Alpha = localAlpha;
				#endif

				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;
				builtinData.emissiveColor = surfaceDescription.Emission;
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				o.positionRWS = positionRWS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag( VertexOutput packedInput ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.positionRWS;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Color = _Color.rgb;
				surfaceDescription.Emission = 0;
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				surfaceDescription.ShadowTint = float4( 0, 0 ,0 ,1 );
				float2 Distortion = float2 ( 0, 0 );
				float DistortionBlur = 0;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );

				float4 outColor = ApplyBlendMode( bsdfData.color + builtinData.emissiveColor * GetCurrentExposureMultiplier(), builtinData.opacity );
				outColor = EvaluateAtmosphericScattering( posInput, V, outColor );

				#ifdef DEBUG_DISPLAY
					int bufferSize = int(_DebugViewMaterialArray[0].x);
					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index].x);
						if (indexMaterialProperty != 0)
						{
							float3 result = float3(1.0, 0.0, 1.0);
							bool needLinearToSRGB = false;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);

							if (!needLinearToSRGB)
								result = SRGBToLinear(max(0, result));

							outColor = float4(result, 1.0);
						}
					}

					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
				#endif
				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_SHADOWS

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#if _ALPHATEST_ON
				DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE (BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					#ifdef WRITE_NORMAL_BUFFER
					, out float4 outNormalBuffer : SV_Target0
						#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target1
						#endif
					#elif defined(WRITE_MSAA_DEPTH)
					, out float4 outNormalBuffer : SV_Target0
					, out float1 depthColor : SV_Target1
					#elif defined(SCENESELECTIONPASS)
					, out float4 outColor : SV_Target0
					#endif
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			CBUFFER_START( UnityMetaPass )
			bool4 unity_MetaVertexControl;
			bool4 unity_MetaFragmentControl;
			CBUFFER_END

			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData( FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );
				surfaceData.color = surfaceDescription.Color;
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData )
			{
				#if _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData( fragInputs, surfaceDescription, V, surfaceData );
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
				builtinData.emissiveColor = surfaceDescription.Emission;
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float2 uv = float2( 0.0, 0.0 );
				if( unity_MetaVertexControl.x )
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if( unity_MetaVertexControl.y )
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				o.positionCS = float4( uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0 );
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag( VertexOutput packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS );

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Color = _Color.rgb;
				surfaceDescription.Emission = 0;
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData( surfaceDescription,input, V, posInput, surfaceData, builtinData );

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );
				LightTransportData lightTransportData = GetLightTransportData( surfaceData, builtinData, bsdfData );

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				if( unity_MetaFragmentControl.x )
				{
					res.rgb = clamp( pow( abs( lightTransportData.diffuseColor ), saturate( unity_OneOverOutputBoost ) ), 0, unity_MaxOutputValue );
				}

				if( unity_MetaFragmentControl.y )
				{
					res.rgb = lightTransportData.emissiveColor;
				}

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull [_CullMode]
			ZWrite On

			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS
			#pragma editor_sync_compilation

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			int _ObjectId;
			int _PassValue;

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					, out float4 outColor : SV_Target0
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthForwardOnly"
			Tags { "LightMode"="DepthForwardOnly" }

			Cull [_CullMode]
			ZWrite On
			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			ColorMask 0 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					#ifdef WRITE_NORMAL_BUFFER
					, out float4 outNormalBuffer : SV_Target0
						#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target1
						#endif
					#elif defined(WRITE_MSAA_DEPTH)
					, out float4 outNormalBuffer : SV_Target0
					, out float1 depthColor : SV_Target1
					#elif defined(SCENESELECTIONPASS)
					, out float4 outColor : SV_Target0
					#endif
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Motion Vectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 100700

			#define SHADERPASS SHADERPASS_MOTION_VECTORS
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _RemapParams;
			float2 _ScrollSpeed;
			float2 _UVScale;
			float2 _TurbulenceScroll;
			float _NoiseScale;
			float _OctaveScaleMultiplier;
			float _InitialOctaveStrength;
			float _OctaveStrengthMultiplier;
			float _Seed;
			float _TrubulenceScale;
			float _TurbulenceStrength;
			float _NoiseTimeScale;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;
			}

			VertexInput ApplyMeshModification(VertexInput inputMesh, float3 timeParameters, inout VertexOutput o )
			{
				_TimeParameters.xyz = timeParameters;
				o.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS =  inputMesh.normalOS ;
				return inputMesh;
			}

			VertexOutput VertexFunction(VertexInput inputMesh)
			{
				VertexOutput o = (VertexOutput)0;
				VertexInput defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, o);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				//#if defined(UNITY_REVERSED_Z)
				//	VMESHpositionCS.z -= unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#else
				//	VMESHpositionCS.z += unity_MotionVectorsParams.z * VMESHpositionCS.w;
				//#endif

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						VertexInput previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						VertexOutput test = (VertexOutput)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				o.vmeshPositionCS = VMESHpositionCS;
				o.vmeshInterp00.xyz = VMESHpositionRWS;

				o.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				o.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						, out float4 outMotionVector : SV_Target0
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target1
							#ifdef WRITE_MSAA_DEPTH
								, out float1 depthColor : SV_Target2
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target1
						, out float1 depthColor : SV_Target2
						#endif

						#ifdef _DEPTHOFFSET_ON
							, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 appendResult164 = (float4(_NoiseScale , _OctaveScaleMultiplier , _InitialOctaveStrength , _OctaveStrengthMultiplier));
				float4 NoiseParams163 = appendResult164;
				float4 break166 = NoiseParams163;
				float temp_output_26_0_g10 = break166.z;
				float2 ScrollSpeed232 = _ScrollSpeed;
				float2 texCoord11 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner29 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord11);
				float2 texCoord46 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 1.0 * _Time.y * _TurbulenceScroll + texCoord46);
				float simplePerlin2D44 = snoise( panner47*_TrubulenceScale );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float turbulence50 = simplePerlin2D44;
				float2 appendResult52 = (float2(0.0 , ( (-1.0 + (turbulence50 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _TurbulenceStrength )));
				float2 break21 = ( ( panner29 * _UVScale ) + appendResult52 );
				float NoiseTimeScale186 = _NoiseTimeScale;
				float mulTime14 = _TimeParameters.x * NoiseTimeScale186;
				float3 appendResult24 = (float3(break21.x , break21.y , mulTime14));
				float3 temp_output_24_0_g10 = ( ( _Seed * 10000.0 ) + appendResult24 );
				float temp_output_1_0_g10 = break166.x;
				float simplePerlin3D18_g10 = snoise( temp_output_24_0_g10*temp_output_1_0_g10 );
				simplePerlin3D18_g10 = simplePerlin3D18_g10*0.5 + 0.5;
				float temp_output_25_0_g10 = break166.w;
				float temp_output_8_0_g10 = break166.y;
				float simplePerlin3D19_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * temp_output_8_0_g10 ) );
				simplePerlin3D19_g10 = simplePerlin3D19_g10*0.5 + 0.5;
				float simplePerlin3D20_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 2.0 ) ) );
				simplePerlin3D20_g10 = simplePerlin3D20_g10*0.5 + 0.5;
				float simplePerlin3D21_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 3.0 ) ) );
				simplePerlin3D21_g10 = simplePerlin3D21_g10*0.5 + 0.5;
				float simplePerlin3D35_g10 = snoise( temp_output_24_0_g10*( temp_output_1_0_g10 * pow( temp_output_8_0_g10 , 4.0 ) ) );
				simplePerlin3D35_g10 = simplePerlin3D35_g10*0.5 + 0.5;
				float clampResult34_g10 = clamp( ( ( temp_output_26_0_g10 * simplePerlin3D18_g10 ) + ( temp_output_26_0_g10 * temp_output_25_0_g10 * simplePerlin3D19_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 2.0 ) * simplePerlin3D20_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D21_g10 ) + ( temp_output_26_0_g10 * pow( temp_output_25_0_g10 , 3.0 ) * simplePerlin3D35_g10 ) ) , 0.0 , 1.0 );
				float4 RemapParams83 = _RemapParams;
				float4 break84 = RemapParams83;
				float clampResult34 = clamp( (break84.z + (clampResult34_g10 - break84.x) * (break84.w - break84.z) / (break84.y - break84.x)) , 0.0 , 1.0 );
				float4 break167 = NoiseParams163;
				float temp_output_26_0_g5 = break167.z;
				float2 texCoord131 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime150 = _TimeParameters.x * 0.52;
				float2 appendResult149 = (float2(texCoord131.y , mulTime150));
				float2 temp_output_24_0_g5 = appendResult149;
				float temp_output_1_0_g5 = break167.x;
				float simplePerlin2D18_g5 = snoise( temp_output_24_0_g5*temp_output_1_0_g5 );
				simplePerlin2D18_g5 = simplePerlin2D18_g5*0.5 + 0.5;
				float temp_output_25_0_g5 = break167.w;
				float temp_output_8_0_g5 = break167.y;
				float simplePerlin2D19_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * temp_output_8_0_g5 ) );
				simplePerlin2D19_g5 = simplePerlin2D19_g5*0.5 + 0.5;
				float simplePerlin2D20_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 2.0 ) ) );
				simplePerlin2D20_g5 = simplePerlin2D20_g5*0.5 + 0.5;
				float simplePerlin2D21_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 3.0 ) ) );
				simplePerlin2D21_g5 = simplePerlin2D21_g5*0.5 + 0.5;
				float simplePerlin2D35_g5 = snoise( temp_output_24_0_g5*( temp_output_1_0_g5 * pow( temp_output_8_0_g5 , 4.0 ) ) );
				simplePerlin2D35_g5 = simplePerlin2D35_g5*0.5 + 0.5;
				float clampResult34_g5 = clamp( ( ( temp_output_26_0_g5 * simplePerlin2D18_g5 ) + ( temp_output_26_0_g5 * temp_output_25_0_g5 * simplePerlin2D19_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 2.0 ) * simplePerlin2D20_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D21_g5 ) + ( temp_output_26_0_g5 * pow( temp_output_25_0_g5 , 3.0 ) * simplePerlin2D35_g5 ) ) , 0.0 , 1.0 );
				float temp_output_148_0 = ( clampResult34_g5 * 0.32 );
				float smoothstepResult137 = smoothstep( temp_output_148_0 , ( temp_output_148_0 + 0.01 ) , texCoord131.x);
				float4 break194 = NoiseParams163;
				float temp_output_26_0_g9 = break194.z;
				float2 texCoord190 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime192 = _TimeParameters.x * -0.52;
				float2 appendResult193 = (float2(texCoord190.y , mulTime192));
				float2 temp_output_24_0_g9 = appendResult193;
				float temp_output_1_0_g9 = break194.x;
				float simplePerlin2D18_g9 = snoise( temp_output_24_0_g9*temp_output_1_0_g9 );
				simplePerlin2D18_g9 = simplePerlin2D18_g9*0.5 + 0.5;
				float temp_output_25_0_g9 = break194.w;
				float temp_output_8_0_g9 = break194.y;
				float simplePerlin2D19_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * temp_output_8_0_g9 ) );
				simplePerlin2D19_g9 = simplePerlin2D19_g9*0.5 + 0.5;
				float simplePerlin2D20_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 2.0 ) ) );
				simplePerlin2D20_g9 = simplePerlin2D20_g9*0.5 + 0.5;
				float simplePerlin2D21_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 3.0 ) ) );
				simplePerlin2D21_g9 = simplePerlin2D21_g9*0.5 + 0.5;
				float simplePerlin2D35_g9 = snoise( temp_output_24_0_g9*( temp_output_1_0_g9 * pow( temp_output_8_0_g9 , 4.0 ) ) );
				simplePerlin2D35_g9 = simplePerlin2D35_g9*0.5 + 0.5;
				float clampResult34_g9 = clamp( ( ( temp_output_26_0_g9 * simplePerlin2D18_g9 ) + ( temp_output_26_0_g9 * temp_output_25_0_g9 * simplePerlin2D19_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 2.0 ) * simplePerlin2D20_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D21_g9 ) + ( temp_output_26_0_g9 * pow( temp_output_25_0_g9 , 3.0 ) * simplePerlin2D35_g9 ) ) , 0.0 , 1.0 );
				float temp_output_196_0 = ( clampResult34_g9 * 0.32 );
				float smoothstepResult198 = smoothstep( temp_output_196_0 , ( temp_output_196_0 + 0.01 ) , ( 1.0 - texCoord190.x ));
				float2 texCoord224 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner225 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord224);
				float simplePerlin2D226 = snoise( panner225*3.0 );
				simplePerlin2D226 = simplePerlin2D226*0.5 + 0.5;
				float temp_output_227_0 = ( simplePerlin2D226 * 0.2 );
				float smoothstepResult231 = smoothstep( temp_output_227_0 , ( temp_output_227_0 + 0.01 ) , texCoord224.y);
				float2 texCoord218 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner220 = ( 1.0 * _Time.y * ScrollSpeed232 + texCoord218);
				float simplePerlin2D221 = snoise( panner220*3.0 );
				simplePerlin2D221 = simplePerlin2D221*0.5 + 0.5;
				float temp_output_213_0 = ( simplePerlin2D221 * 0.2 );
				float smoothstepResult216 = smoothstep( temp_output_213_0 , ( temp_output_213_0 + 0.01 ) , ( 1.0 - texCoord218.y ));
				
				surfaceDescription.Alpha = ( _Color.a * clampResult34 * ( smoothstepResult137 * smoothstepResult198 * smoothstepResult231 * smoothstepResult216 ) );
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );

				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.vmeshPositionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.vmeshPositionCS.z;
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

	
	}
	
	CustomEditor "Rendering.HighDefinition.HDUnlitGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18928
361;73;1270;646;4985.22;314.7386;5.386691;True;False
Node;AmplifyShaderEditor.Vector2Node;57;-1852.713,790.7524;Inherit;False;Property;_TurbulenceScroll;Turbulence Scroll;10;0;Create;True;1;;0;0;False;0;False;-0.7,0;-0.7,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-1968.241,623.0529;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;47;-1561.045,633.5035;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1576.256,781.5915;Inherit;False;Property;_TrubulenceScale;Trubulence Scale;9;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;44;-1334.615,638.4576;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;30;-2562.041,550.5414;Inherit;False;Property;_ScrollSpeed;Scroll Speed;7;0;Create;True;0;0;0;False;0;False;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-1096.062,636.8113;Inherit;True;turbulence;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-1792.88,-41.1403;Inherit;False;50;turbulence;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;232;-2384.333,549.0991;Inherit;False;ScrollSpeed;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;211;-1596.274,-33.8075;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-2615.469,266.5679;Inherit;False;Property;_OctaveScaleMultiplier;Octave Scale Multiplier;3;0;Create;True;0;0;0;False;0;False;2;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1621.73,132.3722;Inherit;False;Property;_TurbulenceStrength;Turbulence Strength;11;0;Create;True;0;0;0;False;0;False;0.025;0.025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2554.308,190.2394;Inherit;False;Property;_NoiseScale;Noise Scale;0;1;[Header];Create;True;1;Noise;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2624.301,442.9999;Inherit;False;Property;_OctaveStrengthMultiplier;Octave Strength Multiplier;5;0;Create;True;0;0;0;False;0;False;0.4;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2611.001,357.2997;Inherit;False;Property;_InitialOctaveStrength;Initial Octave Strength;4;0;Create;True;0;0;0;False;0;False;0.5;0.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;-1683.8,372.1539;Inherit;False;232;ScrollSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1722.817,225.921;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-2337.689,93.283;Inherit;False;Property;_NoiseTimeScale;Noise Time Scale;1;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;29;-1419.686,225.1177;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1391.73,116.3722;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-1384.307,347.2227;Inherit;False;Property;_UVScale;UV Scale;6;1;[Header];Create;True;1;Mapping;0;0;False;0;False;1,1;0.6,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;164;-2364.234,268.8631;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;-1230.88,119.8597;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;186;-2120.335,91.53394;Inherit;False;NoiseTimeScale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1227.601,225.504;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;-2216.033,261.063;Inherit;False;NoiseParams;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-1089.88,224.8597;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;234;-1386.435,2823.388;Inherit;False;232;ScrollSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;218;-1416.462,2645.037;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;190;-1478.084,1598.224;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;131;-1527.794,963.0087;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;168;-1585.416,1380.665;Inherit;False;163;NoiseParams;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;192;-1507.752,1800.857;Inherit;False;1;0;FLOAT;-0.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;150;-1518.224,1167.043;Inherit;False;1;0;FLOAT;0.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;224;-1387.035,2230.726;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;187;-1235.711,366.1322;Inherit;False;186;NoiseTimeScale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-1535.706,2015.88;Inherit;False;163;NoiseParams;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;235;-1342.924,2431.788;Inherit;False;232;ScrollSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;167;-1390.418,1387.99;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;149;-1339.151,1142.018;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;193;-1289.441,1777.233;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;194;-1340.708,2023.206;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PannerNode;220;-1158.505,2806.462;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;225;-1142.962,2406.034;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;21;-974.0224,229.1096;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Vector4Node;43;-2307.448,-143.9067;Inherit;False;Property;_RemapParams;Remap Params;8;0;Create;True;0;0;0;False;0;False;0,1,0,1;0.25,0.26,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1036.674,370.402;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-953.5198,37.86523;Inherit;False;Property;_Seed;Seed;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-854.4209,228.4032;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;195;-1150.235,1880.15;Inherit;True;Octave Noise 2D;-1;;9;c2c0af719f7ef45458320a45e70694da;0;5;24;FLOAT2;0,0;False;1;FLOAT;1;False;8;FLOAT;0.5;False;26;FLOAT;0.8;False;25;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;169;-1199.945,1244.935;Inherit;True;Octave Noise 2D;-1;;5;c2c0af719f7ef45458320a45e70694da;0;5;24;FLOAT2;0,0;False;1;FLOAT;1;False;8;FLOAT;0.5;False;26;FLOAT;0.8;False;25;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;226;-920.0025,2409.252;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;221;-935.5464,2809.681;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;-2102.841,-137.5663;Inherit;False;RemapParams;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-808.3001,114;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10000;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;165;-766.4034,398.3887;Inherit;False;163;NoiseParams;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-631.8998,140.8001;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-740.0715,1750.395;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-378.8629,507.9844;Inherit;False;83;RemapParams;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;-713.6263,2805.705;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;227;-698.0821,2405.277;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;166;-574.8794,405.7505;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-826.6534,1237.383;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;200;-955.7214,1626.917;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-576.7396,1867.76;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-612.7535,1198.833;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;230;-552.4653,2491.883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;170;-400.5,257.2001;Inherit;True;Octave Noise 3D;-1;;10;ec732fbb68dec504fb3a7cb83c8b1002;0;5;24;FLOAT3;0,0,0;False;1;FLOAT;1;False;8;FLOAT;0.5;False;26;FLOAT;0.8;False;25;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;215;-568.0092,2892.311;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;217;-630.5027,2677.154;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;84;-175.5198,513.8652;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;33;-0.5000777,465.2001;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.49;False;2;FLOAT;0.51;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;198;-425.3549,1543.612;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.4;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;137;-468.0489,994.3358;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.4;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;216;-450.0094,2679.311;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;231;-434.4653,2278.883;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-11.23856,980.6215;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;147.2998,230.1001;Inherit;False;Property;_Color;Color;12;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;34;223.4999,497.1999;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-1668.054,1516.66;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;367.5,401.2001;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1891.713,1604.983;Inherit;False;Property;_BorderAspectRatio;Border Aspect Ratio;13;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;42;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DistortionVectors;0;6;DistortionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;1;False;-1;1;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-11;255;False;-1;255;True;-12;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;1;LightMode=DistortionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;38;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;META;0;2;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;40;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DepthForwardOnly;0;4;DepthForwardOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;True;True;0;True;-7;255;False;-1;255;True;-8;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;39;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;37;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;36;543.5005,225.2001;Float;False;True;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;WindTrail;7f5cb9c3ea6481f469fdd856555439ef;True;Forward Unlit;0;0;Forward Unlit;9;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;0;True;-20;0;True;-21;1;0;True;-22;0;True;-23;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-5;255;False;-1;255;True;-6;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;0;True;-24;True;0;True;-32;False;True;1;LightMode=ForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;29;Surface Type;1;  Rendering Pass ;0;  Rendering Pass;1;  Blending Mode;0;  Receive Fog;1;  Distortion;0;    Distortion Mode;0;    Distortion Only;1;  Depth Write;1;  Cull Mode;0;  Depth Test;4;Double-Sided;0;Alpha Clipping;0;Motion Vectors;1;  Add Precomputed Velocity;0;Shadow Matte;0;Cast Shadows;1;DOTS Instancing;0;GPU Instancing;1;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;7;True;True;True;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;41;0,0;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;Motion Vectors;0;5;Motion Vectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-9;255;False;-1;255;True;-10;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=MotionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;47;0;46;0
WireConnection;47;2;57;0
WireConnection;44;0;47;0
WireConnection;44;1;45;0
WireConnection;50;0;44;0
WireConnection;232;0;30;0
WireConnection;211;0;53;0
WireConnection;29;0;11;0
WireConnection;29;2;233;0
WireConnection;55;0;211;0
WireConnection;55;1;56;0
WireConnection;164;0;17;0
WireConnection;164;1;75;0
WireConnection;164;2;74;0
WireConnection;164;3;72;0
WireConnection;52;1;55;0
WireConnection;186;0;25;0
WireConnection;27;0;29;0
WireConnection;27;1;26;0
WireConnection;163;0;164;0
WireConnection;54;0;27;0
WireConnection;54;1;52;0
WireConnection;167;0;168;0
WireConnection;149;0;131;2
WireConnection;149;1;150;0
WireConnection;193;0;190;2
WireConnection;193;1;192;0
WireConnection;194;0;191;0
WireConnection;220;0;218;0
WireConnection;220;2;234;0
WireConnection;225;0;224;0
WireConnection;225;2;235;0
WireConnection;21;0;54;0
WireConnection;14;0;187;0
WireConnection;24;0;21;0
WireConnection;24;1;21;1
WireConnection;24;2;14;0
WireConnection;195;24;193;0
WireConnection;195;1;194;0
WireConnection;195;8;194;1
WireConnection;195;26;194;2
WireConnection;195;25;194;3
WireConnection;169;24;149;0
WireConnection;169;1;167;0
WireConnection;169;8;167;1
WireConnection;169;26;167;2
WireConnection;169;25;167;3
WireConnection;226;0;225;0
WireConnection;221;0;220;0
WireConnection;83;0;43;0
WireConnection;70;0;81;0
WireConnection;71;0;70;0
WireConnection;71;1;24;0
WireConnection;196;0;195;0
WireConnection;213;0;221;0
WireConnection;227;0;226;0
WireConnection;166;0;165;0
WireConnection;148;0;169;0
WireConnection;200;0;190;1
WireConnection;197;0;196;0
WireConnection;139;0;148;0
WireConnection;230;0;227;0
WireConnection;170;24;71;0
WireConnection;170;1;166;0
WireConnection;170;8;166;1
WireConnection;170;26;166;2
WireConnection;170;25;166;3
WireConnection;215;0;213;0
WireConnection;217;0;218;2
WireConnection;84;0;85;0
WireConnection;33;0;170;0
WireConnection;33;1;84;0
WireConnection;33;2;84;1
WireConnection;33;3;84;2
WireConnection;33;4;84;3
WireConnection;198;0;200;0
WireConnection;198;1;196;0
WireConnection;198;2;197;0
WireConnection;137;0;131;1
WireConnection;137;1;148;0
WireConnection;137;2;139;0
WireConnection;216;0;217;0
WireConnection;216;1;213;0
WireConnection;216;2;215;0
WireConnection;231;0;224;2
WireConnection;231;1;227;0
WireConnection;231;2;230;0
WireConnection;201;0;137;0
WireConnection;201;1;198;0
WireConnection;201;2;231;0
WireConnection;201;3;216;0
WireConnection;34;0;33;0
WireConnection;127;1;126;0
WireConnection;58;0;67;4
WireConnection;58;1;34;0
WireConnection;58;2;201;0
WireConnection;36;0;67;0
WireConnection;36;2;58;0
ASEEND*/
//CHKSM=B6BFF66DB4BEA789B8135571B4A45ADB942E1CBF