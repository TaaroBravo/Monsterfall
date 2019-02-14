// Upgrade NOTE: upgraded instancing buffer 'NewAmplifyShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		_Tint("Tint", Color) = (1,0.4632353,0.4632353,0)
		_bloquehieloyeti_DefaultMaterial_AlbedoTransparency("bloquehieloyeti_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_bloquehieloyeti_DefaultMaterial_Normal("bloquehieloyeti_DefaultMaterial_Normal", 2D) = "bump" {}
		_bloquehieloyeti_DefaultMaterial_MetallicSmoothness("bloquehieloyeti_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _bloquehieloyeti_DefaultMaterial_Normal;
		uniform float4 _bloquehieloyeti_DefaultMaterial_Normal_ST;
		uniform sampler2D _bloquehieloyeti_DefaultMaterial_AlbedoTransparency;
		uniform float4 _bloquehieloyeti_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _bloquehieloyeti_DefaultMaterial_MetallicSmoothness;
		uniform float4 _bloquehieloyeti_DefaultMaterial_MetallicSmoothness_ST;
		uniform float _Float0;

		UNITY_INSTANCING_BUFFER_START(NewAmplifyShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Tint)
#define _Tint_arr NewAmplifyShader
		UNITY_INSTANCING_BUFFER_END(NewAmplifyShader)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_bloquehieloyeti_DefaultMaterial_Normal = i.uv_texcoord * _bloquehieloyeti_DefaultMaterial_Normal_ST.xy + _bloquehieloyeti_DefaultMaterial_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _bloquehieloyeti_DefaultMaterial_Normal, uv_bloquehieloyeti_DefaultMaterial_Normal ) );
			float4 _Tint_Instance = UNITY_ACCESS_INSTANCED_PROP(_Tint_arr, _Tint);
			float2 uv_bloquehieloyeti_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _bloquehieloyeti_DefaultMaterial_AlbedoTransparency_ST.xy + _bloquehieloyeti_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = ( _Tint_Instance * tex2D( _bloquehieloyeti_DefaultMaterial_AlbedoTransparency, uv_bloquehieloyeti_DefaultMaterial_AlbedoTransparency ) ).rgb;
			float2 uv_bloquehieloyeti_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _bloquehieloyeti_DefaultMaterial_MetallicSmoothness_ST.xy + _bloquehieloyeti_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _bloquehieloyeti_DefaultMaterial_MetallicSmoothness, uv_bloquehieloyeti_DefaultMaterial_MetallicSmoothness ).r;
			o.Alpha = _Float0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
431;92;1207;469;946.6244;170.8103;1;True;False
Node;AmplifyShaderEditor.SamplerNode;2;-1061.188,32.95886;Float;True;Property;_bloquehieloyeti_DefaultMaterial_AlbedoTransparency;bloquehieloyeti_DefaultMaterial_AlbedoTransparency;2;0;Create;True;0;0;False;0;2944c319ba2ed8e4cbe7fe8f887d9978;2944c319ba2ed8e4cbe7fe8f887d9978;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-796.957,-134.9094;Float;False;InstancedProperty;_Tint;Tint;0;0;Create;True;0;0;False;0;1,0.4632353,0.4632353,0;0.8676471,0.3891652,0.3891652,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-576.4461,453.3432;Float;True;Property;_bloquehieloyeti_DefaultMaterial_Normal;bloquehieloyeti_DefaultMaterial_Normal;3;0;Create;True;0;0;False;0;28914e2b621372f498f24670edcef55c;28914e2b621372f498f24670edcef55c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-731.1803,246.5743;Float;True;Property;_bloquehieloyeti_DefaultMaterial_MetallicSmoothness;bloquehieloyeti_DefaultMaterial_MetallicSmoothness;4;0;Create;True;0;0;False;0;30965785e46a953409a5621e5b197289;30965785e46a953409a5621e5b197289;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-256.0165,164.9472;Float;False;Property;_Float0;Float 0;5;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-479.1157,-18.99578;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Overlay;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;0;0;3;0
WireConnection;0;1;4;0
WireConnection;0;3;5;0
WireConnection;0;9;6;0
ASEEND*/
//CHKSM=C24035C9BBC00F73655491DE919BD63F4BE42237