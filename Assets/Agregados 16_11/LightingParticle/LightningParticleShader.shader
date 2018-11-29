// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader 1"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Tinte("Tinte", Color) = (0.9862069,1,0,0)
		_balltexttest("balltexttest", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _balltexttest;
		uniform float4 _balltexttest_ST;
		uniform float4 _Tinte;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_balltexttest = i.uv_texcoord * _balltexttest_ST.xy + _balltexttest_ST.zw;
			float4 tex2DNode14 = tex2D( _balltexttest, uv_balltexttest );
			float4 temp_output_3_0 = ( tex2DNode14 * _Tinte );
			o.Albedo = temp_output_3_0.rgb;
			o.Emission = temp_output_3_0.rgb;
			float temp_output_5_0 = 0.0;
			o.Metallic = temp_output_5_0;
			o.Smoothness = temp_output_5_0;
			o.Alpha = 1;
			clip( tex2DNode14.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
190;200;1389;795;1225.325;431.3929;1.292541;True;False
Node;AmplifyShaderEditor.ColorNode;4;-774.839,78.54895;Float;False;Property;_Tinte;Tinte;1;0;Create;True;0;0;False;0;0.9862069,1,0,0;1,0.9724137,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-793.6161,-200.0281;Float;True;Property;_balltexttest;balltexttest;2;0;Create;True;0;0;False;0;b7672c74ebaeffb41b886ad3f12012b9;2373f0ec7d7db8a4abdc64a05807ac43;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-472.3843,-44.24245;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-106.5959,43.65028;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;107.2809,-31.02099;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Transparent;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;14;0
WireConnection;3;1;4;0
WireConnection;0;0;3;0
WireConnection;0;2;3;0
WireConnection;0;3;5;0
WireConnection;0;4;5;0
WireConnection;0;10;14;0
ASEEND*/
//CHKSM=3DB3F5ECB3DD87DD57956595CD261D296EC0A01C