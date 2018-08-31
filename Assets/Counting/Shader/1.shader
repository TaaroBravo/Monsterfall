// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "1"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,1)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.68
		_cristal_DefaultMaterial_Normal("cristal_DefaultMaterial_Normal", 2D) = "bump" {}
		_cristal_DefaultMaterial_AlbedoTransparency("cristal_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_cristal_DefaultMaterial_Emission("cristal_DefaultMaterial_Emission", 2D) = "white" {}
		_cristal_DefaultMaterial_MetallicSmoothness("cristal_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:outlineVertexDataFunc
		struct Input
		{
			fixed filler;
		};
		uniform fixed4 _ASEOutlineColor;
		uniform fixed _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline fixed4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return fixed4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o ) { o.Emission = _ASEOutlineColor.rgb; o.Alpha = 1; }
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Background+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _cristal_DefaultMaterial_Normal;
		uniform float4 _cristal_DefaultMaterial_Normal_ST;
		uniform sampler2D _cristal_DefaultMaterial_AlbedoTransparency;
		uniform float4 _cristal_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _cristal_DefaultMaterial_Emission;
		uniform float4 _cristal_DefaultMaterial_Emission_ST;
		uniform sampler2D _cristal_DefaultMaterial_MetallicSmoothness;
		uniform float4 _cristal_DefaultMaterial_MetallicSmoothness_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_cristal_DefaultMaterial_Normal = i.uv_texcoord * _cristal_DefaultMaterial_Normal_ST.xy + _cristal_DefaultMaterial_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _cristal_DefaultMaterial_Normal, uv_cristal_DefaultMaterial_Normal ) );
			float2 uv_cristal_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _cristal_DefaultMaterial_AlbedoTransparency_ST.xy + _cristal_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _cristal_DefaultMaterial_AlbedoTransparency, uv_cristal_DefaultMaterial_AlbedoTransparency ).rgb;
			float2 uv_cristal_DefaultMaterial_Emission = i.uv_texcoord * _cristal_DefaultMaterial_Emission_ST.xy + _cristal_DefaultMaterial_Emission_ST.zw;
			o.Emission = tex2D( _cristal_DefaultMaterial_Emission, uv_cristal_DefaultMaterial_Emission ).rgb;
			float2 uv_cristal_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _cristal_DefaultMaterial_MetallicSmoothness_ST.xy + _cristal_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _cristal_DefaultMaterial_MetallicSmoothness, uv_cristal_DefaultMaterial_MetallicSmoothness ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
0;92;783;515;1083.65;277.4233;2.449462;True;False
Node;AmplifyShaderEditor.SamplerNode;2;-715.6273,-180.8168;Float;True;Property;_cristal_DefaultMaterial_AlbedoTransparency;cristal_DefaultMaterial_AlbedoTransparency;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-711.559,24.54436;Float;True;Property;_cristal_DefaultMaterial_Normal;cristal_DefaultMaterial_Normal;0;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-633.3888,261.2145;Float;True;Property;_cristal_DefaultMaterial_Emission;cristal_DefaultMaterial_Emission;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-547.0382,472.9785;Float;True;Property;_cristal_DefaultMaterial_MetallicSmoothness;cristal_DefaultMaterial_MetallicSmoothness;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;32,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;Background;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;True;0.68;0,0,0,1;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;2;0
WireConnection;0;1;1;0
WireConnection;0;2;3;0
WireConnection;0;3;4;0
ASEEND*/
//CHKSM=1F7706A7B0AD82A523306688330A8E675D4279C6