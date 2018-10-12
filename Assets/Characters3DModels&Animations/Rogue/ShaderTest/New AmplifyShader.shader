// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RogueShader"
{
	Properties
	{
		_splatmap("splatmap", 2D) = "white" {}
		_AlbedoRogueShader("AlbedoRogueShader", 2D) = "white" {}
		_Color("Color", Color) = (0,0.07900602,0.6029412,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _splatmap;
		uniform float4 _splatmap_ST;
		uniform float4 _Color;
		uniform sampler2D _AlbedoRogueShader;
		uniform float4 _AlbedoRogueShader_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_splatmap = i.uv_texcoord * _splatmap_ST.xy + _splatmap_ST.zw;
			float4 tex2DNode1 = tex2D( _splatmap, uv_splatmap );
			float2 uv_AlbedoRogueShader = i.uv_texcoord * _AlbedoRogueShader_ST.xy + _AlbedoRogueShader_ST.zw;
			o.Albedo = ( ( tex2DNode1.r * _Color ) + ( tex2DNode1.g * tex2D( _AlbedoRogueShader, uv_AlbedoRogueShader ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
663;69;1230;674;1274.54;467.416;1.308324;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-945.9557,-336.3073;Float;True;Property;_splatmap;splatmap;0;0;Assets/Characters3DModels&Animations/Rogue/ShaderTest/splatmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-926.8816,173.9896;Float;True;Property;_AlbedoRogueShader;AlbedoRogueShader;1;0;Assets/Characters3DModels&Animations/Rogue/ShaderTest/AlbedoRogueShader.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;3;-856.993,-129.941;Float;False;Property;_Color;Color;2;0;0,0.07900602,0.6029412,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-520.9448,21.89717;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-526.1781,-230.6094;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-253.118,-74.66225;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-6.968429,-77.56991;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;RogueShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;1;2
WireConnection;8;1;2;0
WireConnection;9;0;1;1
WireConnection;9;1;3;0
WireConnection;7;0;9;0
WireConnection;7;1;8;0
WireConnection;0;0;7;0
ASEEND*/
//CHKSM=205C1983E3EABD534872B6B1003203AD90A6FC1E