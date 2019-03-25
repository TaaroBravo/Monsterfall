// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Cloth"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_BanderaCurvaMask("BanderaCurvaMask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _BanderaCurvaMask;
		uniform float4 _BanderaCurvaMask_ST;
		uniform float _Speed;
		uniform float _Amplitude;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_BanderaCurvaMask = v.texcoord * _BanderaCurvaMask_ST.xy + _BanderaCurvaMask_ST.zw;
			float mulTime7 = _Time.y * _Speed;
			float3 temp_cast_0 = (( tex2Dlod( _BanderaCurvaMask, float4( uv_BanderaCurvaMask, 0, 0.0) ).r * ( sin( mulTime7 ) * _Amplitude ) )).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode1 = tex2D( _TextureSample0, uv_TextureSample0 );
			o.Albedo = ( tex2DNode1.r * float4(0.6544118,0.01924742,0.01924742,0) ).rgb;
			o.Occlusion = tex2DNode1.r;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
7;29;1352;692;1734.832;306.0039;1.817968;True;False
Node;AmplifyShaderEditor.RangedFloatNode;8;-1125.546,328.318;Float;False;Property;_Speed;Speed;1;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;7;-932.7568,338.013;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-688.0843,423.4475;Float;False;Property;_Amplitude;Amplitude;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;5;-685.3211,307.2965;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;4;-1070.641,74.17329;Float;False;Constant;_Tint;Tint;1;0;0.6544118,0.01924742,0.01924742,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-514.2032,360.9169;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-590.8628,129.0284;Float;True;Property;_BanderaCurvaMask;BanderaCurvaMask;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-1075.361,-189.3675;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-583.7953,-128.0404;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-206.2287,340.0056;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;235.8227,-135.5408;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Cloth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;8;0
WireConnection;5;0;7;0
WireConnection;9;0;5;0
WireConnection;9;1;10;0
WireConnection;2;0;1;1
WireConnection;2;1;4;0
WireConnection;12;0;11;1
WireConnection;12;1;9;0
WireConnection;0;0;2;0
WireConnection;0;5;1;1
WireConnection;0;10;1;4
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=F0D80A0B87FACD5B739F5EB0AD9036FA72FD652C