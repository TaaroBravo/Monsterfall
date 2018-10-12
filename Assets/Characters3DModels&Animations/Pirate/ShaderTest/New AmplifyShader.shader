// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PirateTextureShader"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_splatmap("splatmap", 2D) = "white" {}
		_Color0("Color 0", Color) = (1,0,0,0)
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

		uniform float4 _Color0;
		uniform sampler2D _splatmap;
		uniform float4 _splatmap_ST;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_splatmap = i.uv_texcoord * _splatmap_ST.xy + _splatmap_ST.zw;
			float4 tex2DNode2 = tex2D( _splatmap, uv_splatmap );
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			o.Albedo = ( ( _Color0 * tex2DNode2.r ) + ( tex2D( _Texture, uv_Texture ) * tex2DNode2.g ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
489;69;1404;674;1245.168;407.715;1.488366;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-808.2066,223.743;Float;True;Property;_Texture;Texture;0;0;Assets/Characters3DModels&Animations/Pirate/ShaderTest/Texture.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-778.6025,-286.7887;Float;True;Property;_splatmap;splatmap;1;0;Assets/Characters3DModels&Animations/Pirate/ShaderTest/splatmap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;3;-741.7012,-48.87735;Float;False;Property;_Color0;Color 0;2;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-383.4042,16.4691;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-404.2415,229.3054;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-69.35909,120.6546;Float;True;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;226.2315,110.139;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PirateTextureShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;4;1;2;1
WireConnection;5;0;1;0
WireConnection;5;1;2;2
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=508CE5E2A2F05DC8A0F9D7310AFFDA867EDEF26F