// Upgrade NOTE: upgraded instancing buffer 'ComplexCharacterShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ComplexCharacterShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Splatmap("Splatmap", 2D) = "white" {}
		_InvulnerableEmissionColor("InvulnerableEmissionColor", Color) = (0,0,0,0)
		_Albedo("Albedo", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_AlbedoTint("AlbedoTint", Color) = (0.07586217,1,0,0)
		_BorderColor("BorderColor", Color) = (0,0,0,0)
		_Float1("Float 1", Float) = 0.86
		_Float0("Float 0", Float) = 4.3
		_DissolveIntensity("DissolveIntensity", Float) = 0
		_DissolveEmissionColor("DissolveEmissionColor", Color) = (0,0,0,0)
		_InvulnerableSwitch("InvulnerableSwitch", Float) = 0
		_CNDefault_Emission("CN - Default_Emission", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0.1310344,1,0)
		_CNv2Default_MetallicSmoothness("CNv2 - Default_MetallicSmoothness", 2D) = "white" {}
		_CNDefault_Normal("CN - Default_Normal", 2D) = "bump" {}
		_EmissionIntensity("EmissionIntensity", Float) = 21.95
		_Metallic("Metallic", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _CNDefault_Normal;
		uniform float4 _CNDefault_Normal_ST;
		uniform sampler2D _Splatmap;
		uniform float4 _Splatmap_ST;
		uniform float4 _AlbedoTint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _DissolveIntensity;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _CNDefault_Emission;
		uniform float4 _CNDefault_Emission_ST;
		uniform float _Float1;
		uniform float _Float0;
		uniform sampler2D _CNv2Default_MetallicSmoothness;
		uniform float4 _CNv2Default_MetallicSmoothness_ST;
		uniform float _Cutoff = 0.5;

		UNITY_INSTANCING_BUFFER_START(ComplexCharacterShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _DissolveEmissionColor)
#define _DissolveEmissionColor_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissionIntensity)
#define _EmissionIntensity_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color0)
#define _Color0_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _BorderColor)
#define _BorderColor_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _InvulnerableEmissionColor)
#define _InvulnerableEmissionColor_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _InvulnerableSwitch)
#define _InvulnerableSwitch_arr ComplexCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
#define _Metallic_arr ComplexCharacterShader
		UNITY_INSTANCING_BUFFER_END(ComplexCharacterShader)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_CNDefault_Normal = i.uv_texcoord * _CNDefault_Normal_ST.xy + _CNDefault_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _CNDefault_Normal, uv_CNDefault_Normal ) );
			float4 _DissolveEmissionColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_DissolveEmissionColor_arr, _DissolveEmissionColor);
			float2 uv_Splatmap = i.uv_texcoord * _Splatmap_ST.xy + _Splatmap_ST.zw;
			float4 tex2DNode98 = tex2D( _Splatmap, uv_Splatmap );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 lerpResult23 = lerp( ( ( tex2DNode98.g * _AlbedoTint ) + ( tex2DNode98.r * tex2D( _Albedo, uv_Albedo ) ) ) , float4( 0,0,0,0 ) , ( 0.0 * 10.0 ));
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 DissolveIntensityVar5 = saturate( ( _DissolveIntensity + tex2D( _TextureSample0, uv_TextureSample0 ) ) );
			float4 lerpResult9 = lerp( _DissolveEmissionColor_Instance , ( float4( 0,0,0,0 ) + saturate( lerpResult23 ) ) , DissolveIntensityVar5.r);
			o.Albedo = lerpResult9.rgb;
			float _EmissionIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissionIntensity_arr, _EmissionIntensity);
			float4 _Color0_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color0_arr, _Color0);
			float2 uv_CNDefault_Emission = i.uv_texcoord * _CNDefault_Emission_ST.xy + _CNDefault_Emission_ST.zw;
			float4 _BorderColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_BorderColor_arr, _BorderColor);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV17 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode17 = ( 0.0 + _Float1 * pow( 1.0 - fresnelNDotV17, _Float0 ) );
			float4 lerpResult10 = lerp( _DissolveEmissionColor_Instance , ( ( ( _EmissionIntensity_Instance * _Color0_Instance ) * tex2D( _CNDefault_Emission, uv_CNDefault_Emission ) ) + ( _BorderColor_Instance * fresnelNode17 ) ) , DissolveIntensityVar5.r);
			float4 _InvulnerableEmissionColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_InvulnerableEmissionColor_arr, _InvulnerableEmissionColor);
			float mulTime35 = _Time.y * 25.0;
			float mulTime34 = _Time.y * 10.0;
			float fresnelNDotV42 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode42 = ( 0.0 + saturate( sin( mulTime35 ) ) * pow( 1.0 - fresnelNDotV42, ( 5.0 + sin( mulTime34 ) ) ) );
			float _InvulnerableSwitch_Instance = UNITY_ACCESS_INSTANCED_PROP(_InvulnerableSwitch_arr, _InvulnerableSwitch);
			float4 lerpResult48 = lerp( lerpResult10 , saturate( ( 10.0 * ( _InvulnerableEmissionColor_Instance * fresnelNode42 ) ) ) , _InvulnerableSwitch_Instance);
			o.Emission = lerpResult48.rgb;
			float _Metallic_Instance = UNITY_ACCESS_INSTANCED_PROP(_Metallic_arr, _Metallic);
			o.Metallic = _Metallic_Instance;
			float2 uv_CNv2Default_MetallicSmoothness = i.uv_texcoord * _CNv2Default_MetallicSmoothness_ST.xy + _CNv2Default_MetallicSmoothness_ST.zw;
			o.Smoothness = tex2D( _CNv2Default_MetallicSmoothness, uv_CNv2Default_MetallicSmoothness ).r;
			o.Alpha = 1;
			clip( DissolveIntensityVar5.r - _Cutoff );
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
886;92;480;614;4114.933;2372.369;8.51766;False;False
Node;AmplifyShaderEditor.RangedFloatNode;33;-2200.799,517.598;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2256.574,344.3964;Float;False;Constant;_Float4;Float 4;2;0;Create;True;0;0;False;0;25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;98;-2264.927,-990.8022;Float;True;Property;_Splatmap;Splatmap;3;0;Create;True;0;0;False;0;b77317bcae135a345baebe2f9a8ff3c0;c07c51151c9600045bb148f705baabb4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;94;-2262.918,-625.2271;Float;True;Property;_Albedo;Albedo;5;0;Create;True;0;0;False;0;275cc477033bbda428fb9bbf8c6eb7d5;381e364d40cac9d418d4469fc45873f7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;100;-1958.808,-854.1597;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;99;-2176.806,-800.7761;Float;False;Property;_AlbedoTint;AlbedoTint;8;0;Create;True;0;0;False;0;0.07586217,1,0,0;0.07586213,1,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2470.176,-254.966;Float;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2472.257,-85.66336;Float;False;Property;_Float0;Float 0;11;0;Create;True;0;0;False;0;4.3;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;34;-2046.249,522.6927;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;35;-2102.023,349.4913;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2471.981,-169.586;Float;False;Property;_Float1;Float 1;10;0;Create;True;0;0;False;0;0.86;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1923.441,-829.7326;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-1927.801,-722.4036;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1920.571,437.7756;Float;False;Constant;_Float6;Float 6;5;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;36;-1869.62,519.2964;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-2228.153,-418.1687;Float;False;InstancedProperty;_BorderColor;BorderColor;9;0;Create;True;0;0;False;0;0,0,0,0;0,1,0.04827593,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-2177.891,-20.90604;Float;False;Constant;_Float3;Float 3;0;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;17;-2269.746,-245.9145;Float;True;Tangent;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;38;-1931.298,349.9438;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-1764.082,-779.6785;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1834.448,708.9753;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;1bcad549c7971dc49b7da88172414a18;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1754.134,470.0441;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1746.5,266.8518;Float;False;Constant;_Float7;Float 7;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1905.559,-349.7086;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1781.195,632.3427;Float;False;Property;_DissolveIntensity;DissolveIntensity;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;41;-1776.48,356.0139;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1911.057,-104.6148;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;53;-1657.441,-186.3643;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;43;-1594.552,130.7952;Float;False;InstancedProperty;_InvulnerableEmissionColor;InvulnerableEmissionColor;4;0;Create;True;0;0;False;0;0,0,0,0;1,0.9310344,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1479.506,691.6915;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;42;-1578.755,305.327;Float;True;Tangent;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;60;-1616.031,-285.6916;Float;False;InstancedProperty;_Color0;Color 0;16;0;Create;True;0;0;False;0;0,0.1310344,1,0;0,0.1310343,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-1575.647,-661.8246;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1605.783,-369.3995;Float;False;InstancedProperty;_EmissionIntensity;EmissionIntensity;19;0;Create;True;0;0;False;0;21.95;21.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;4;-1260.145,691.7466;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1303.305,226.0255;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;54;-1644.943,39.21679;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1261.459,520.906;Float;False;Constant;_Float8;Float 8;0;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-1379.783,-267.3995;Float;False;2;2;0;FLOAT;1;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;25;-1231.835,-423.6077;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;57;-1603.687,-116.9711;Float;True;Property;_CNDefault_Emission;CN - Default_Emission;15;0;Create;True;0;0;False;0;e88e603603ad2fb4d8eb358246467fc3;7a170cdb7cc88024cb628cfcdbb6705c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-1117.735,684.0246;Float;False;DissolveIntensityVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;-1061.755,191.5604;Float;True;5;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-1088.047,-106.864;Float;False;InstancedProperty;_DissolveEmissionColor;DissolveEmissionColor;13;0;Create;True;0;0;False;0;0,0,0,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;61;-1326.852,88.07658;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1247.749,-120.2328;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1069.746,-342.3915;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1066.722,396.0571;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;55;-865.6791,393.1956;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1089.267,56.50924;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;-718.3569,-98.41257;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;56;-523.402,381.7891;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-695.3655,484.565;Float;False;InstancedProperty;_InvulnerableSwitch;InvulnerableSwitch;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-722.7192,126.6727;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;70;-459.8958,64.15959;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;83;-1799.418,-1627.577;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-434.4951,126.0117;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;71;-168.0073,88.75693;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1736.418,-1415.32;Float;False;Constant;_Float9;Float 9;1;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;68;-406.1796,390.0865;Float;True;Property;_CNv2Default_MetallicSmoothness;CNv2 - Default_MetallicSmoothness;17;0;Create;True;0;0;False;0;4a845a1848957cf4f8fbc7ba322fb3cd;3b383548823fb2e4bb6717b1d63452c6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-116.7015,198.9263;Float;False;InstancedProperty;_Metallic;Metallic;20;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;85;-1524.517,-1547.176;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-727.0004,-1488.972;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;-407.2462,-115.646;Float;True;Property;_CNDefault_Normal;CN - Default_Normal;18;0;Create;True;0;0;False;0;3fb6a90a87fc56444a92cf094b566a70;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;80;-2126.018,-1545.877;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;90;-1323.318,-1765.975;Float;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1024.416,-1384.934;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;86;-1470.917,-1383.977;Float;True;0;3;COLOR;0;FLOAT3;1;FLOAT3;2
Node;AmplifyShaderEditor.SamplerNode;88;-1319.117,-1575.776;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;81;-2098.318,-1684.677;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-1399.076,-1179.133;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;89;-1091.776,-1151.566;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-945.7004,-1564.472;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-714.981,-1255.257;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;62;-789.2533,324.8959;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;118.3961,84.81729;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ComplexCharacterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Overlay;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;100;0;98;1
WireConnection;34;0;33;0
WireConnection;35;0;32;0
WireConnection;95;0;98;2
WireConnection;95;1;99;0
WireConnection;96;0;100;0
WireConnection;96;1;94;0
WireConnection;36;0;34;0
WireConnection;17;1;16;0
WireConnection;17;2;15;0
WireConnection;17;3;14;0
WireConnection;38;0;35;0
WireConnection;97;0;95;0
WireConnection;97;1;96;0
WireConnection;39;0;37;0
WireConnection;39;1;36;0
WireConnection;26;0;24;0
WireConnection;26;1;17;0
WireConnection;41;0;38;0
WireConnection;22;1;19;0
WireConnection;53;0;26;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;42;1;40;0
WireConnection;42;2;41;0
WireConnection;42;3;39;0
WireConnection;23;0;97;0
WireConnection;23;2;22;0
WireConnection;4;0;3;0
WireConnection;45;0;43;0
WireConnection;45;1;42;0
WireConnection;54;0;53;0
WireConnection;72;0;73;0
WireConnection;72;1;60;0
WireConnection;25;0;23;0
WireConnection;5;0;4;0
WireConnection;61;0;54;0
WireConnection;59;0;72;0
WireConnection;59;1;57;0
WireConnection;28;1;25;0
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;55;0;46;0
WireConnection;51;0;59;0
WireConnection;51;1;61;0
WireConnection;9;0;8;0
WireConnection;9;1;28;0
WireConnection;9;2;7;0
WireConnection;56;0;55;0
WireConnection;10;0;8;0
WireConnection;10;1;51;0
WireConnection;10;2;7;0
WireConnection;70;0;9;0
WireConnection;83;0;81;0
WireConnection;83;1;80;0
WireConnection;48;0;10;0
WireConnection;48;1;56;0
WireConnection;48;2;49;0
WireConnection;71;0;70;0
WireConnection;85;0;83;0
WireConnection;85;1;82;0
WireConnection;85;2;82;0
WireConnection;93;0;92;0
WireConnection;93;1;87;0
WireConnection;87;0;86;0
WireConnection;87;1;84;0
WireConnection;88;1;85;0
WireConnection;92;0;90;0
WireConnection;92;1;88;0
WireConnection;91;0;87;0
WireConnection;91;1;89;0
WireConnection;62;0;7;0
WireConnection;0;0;71;0
WireConnection;0;1;69;0
WireConnection;0;2;48;0
WireConnection;0;3;74;0
WireConnection;0;4;68;0
WireConnection;0;10;62;0
ASEEND*/
//CHKSM=11EFEBFDF43EB7279AA7BECC70A564959CB5477B