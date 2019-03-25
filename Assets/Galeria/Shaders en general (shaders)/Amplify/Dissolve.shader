// Upgrade NOTE: upgraded instancing buffer 'Dissolve' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = -0.8
		_Noise("Noise", 2D) = "white" {}
		_Tint("Tint", Color) = (0.4338235,0.953144,1,0)
		_Multiplier("Multiplier", Float) = -1
		_Intensity("Intensity", Float) = 0.5
		_PannerSpeed("PannerSpeed", Vector) = (0,0,0,0)
		_PatternScale("PatternScale", Float) = 0.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float2 texcoord_0;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Intensity;
		uniform float _Multiplier;
		uniform sampler2D _Noise;
		uniform float2 _PannerSpeed;
		uniform float _PatternScale;
		uniform float _Cutoff = -0.8;

		UNITY_INSTANCING_BUFFER_START(Dissolve)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Tint)
#define _Tint_arr Dissolve
		UNITY_INSTANCING_BUFFER_END(Dissolve)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Tint_Instance = UNITY_ACCESS_INSTANCED_PROP(_Tint_arr, _Tint);
			float2 panner36 = ( i.texcoord_0 + 1.0 * _Time.y * _PannerSpeed);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV10 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode10 = ( -0.69 + 1.73 * pow( 1.0 - fresnelNDotV10, 8.53 ) );
			float4 lerpResult7 = lerp( _Tint_Instance , float4( 1,1,1,0 ) , ( ( _Intensity * ( _Multiplier + tex2D( _Noise, (panner36*_PatternScale + 0.0) ) ) ) * ( 1.0 - fresnelNode10 ) ));
			o.Emission = lerpResult7.rgb;
			o.Alpha = 1;
			clip( lerpResult7.r - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			# include "HLSLSupport.cginc"
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
				float3 worldPos : TEXCOORD6;
				float3 worldNormal : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
Version=13701
32;184;525;395;3192.288;830.2419;7.360356;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-1970.182,317.8407;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;40;-1975.06,616.8925;Float;False;Property;_PannerSpeed;PannerSpeed;5;0;0,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;36;-1715.697,431.8102;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;50;-1768.115,809.2303;Float;False;Property;_PatternScale;PatternScale;6;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ScaleAndOffsetNode;49;-1664.17,613.0767;Float;False;3;0;FLOAT2;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;5;-1455.901,278.2577;Float;False;Property;_Multiplier;Multiplier;1;0;-1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-1487.677,397.5544;Float;True;Property;_Noise;Noise;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;10;-1161.734,607.0716;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;-0.69;False;2;FLOAT;1.73;False;3;FLOAT;8.53;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1153.754,339.9293;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;9;-1031.455,226.6489;Float;False;Property;_Intensity;Intensity;3;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-829.9412,327.9816;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;22;-770.6674,619.8943;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-552.4839,403.22;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;1;-596.9453,-90.21812;Float;False;InstancedProperty;_Tint;Tint;0;0;0.4338235,0.953144,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;7;-336.437,150.9442;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;-0.8;True;True;0;True;TransparentCutout;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;37;0
WireConnection;36;2;40;0
WireConnection;49;0;36;0
WireConnection;49;1;50;0
WireConnection;2;1;49;0
WireConnection;3;0;5;0
WireConnection;3;1;2;0
WireConnection;8;0;9;0
WireConnection;8;1;3;0
WireConnection;22;0;10;0
WireConnection;23;0;8;0
WireConnection;23;1;22;0
WireConnection;7;0;1;0
WireConnection;7;2;23;0
WireConnection;0;2;7;0
WireConnection;0;10;7;0
ASEEND*/
//CHKSM=435A4420E2BCA68F0711530A12F333BC71EE2507