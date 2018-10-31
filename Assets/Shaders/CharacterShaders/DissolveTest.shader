// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveTest"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_1_vs239SecVBaB4HvLsZ8O5Q("1_vs239SecVBaB4HvLsZ8O5Q", 2D) = "white" {}
		_Intesitiy("Intesitiy", Range( -1 , 1)) = 1
		_DissolveColor("DissolveColor", Color) = (1,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _DissolveColor;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Intesitiy;
		uniform sampler2D _1_vs239SecVBaB4HvLsZ8O5Q;
		uniform float4 _1_vs239SecVBaB4HvLsZ8O5Q_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_1_vs239SecVBaB4HvLsZ8O5Q = i.uv_texcoord * _1_vs239SecVBaB4HvLsZ8O5Q_ST.xy + _1_vs239SecVBaB4HvLsZ8O5Q_ST.zw;
			float4 DissolveValue14 = saturate( ( _Intesitiy + tex2D( _1_vs239SecVBaB4HvLsZ8O5Q, uv_1_vs239SecVBaB4HvLsZ8O5Q ) ) );
			float4 lerpResult3 = lerp( _DissolveColor , tex2D( _TextureSample0, uv_TextureSample0 ) , DissolveValue14);
			o.Albedo = lerpResult3.rgb;
			float4 lerpResult2 = lerp( _DissolveColor , float4( 0,0,0,0 ) , DissolveValue14.r);
			o.Emission = lerpResult2.rgb;
			o.Alpha = 1;
			clip( DissolveValue14.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
7;29;1352;692;1890.324;1142.939;2.122835;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-710.6198,100.8293;Float;True;Property;_1_vs239SecVBaB4HvLsZ8O5Q;1_vs239SecVBaB4HvLsZ8O5Q;1;0;Create;True;0;0;False;0;1bcad549c7971dc49b7da88172414a18;1bcad549c7971dc49b7da88172414a18;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-657.3666,24.19678;Float;False;Property;_Intesitiy;Intesitiy;2;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-355.6774,83.54552;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;13;-136.3166,83.60067;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;6.093511,75.8786;Float;False;DissolveValue;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;20;-493.6437,-840.3882;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;84508b93f15f2b64386ec07486afc7a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;17;-329.2404,-192.8623;Float;True;14;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;11;-406.4793,-481.8173;Float;False;Property;_DissolveColor;DissolveColor;4;0;Create;True;0;0;False;0;1,0,0,0;0.986207,1,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;3;-36.78909,-473.3658;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;2;-41.15136,-248.2805;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-403.3187,-651.9659;Float;False;Property;_BaseColor;BaseColor;3;0;Create;True;0;0;False;0;0,1,0.1724138,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;19;-106.0814,-58.68172;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;318.6396,-303.4757;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;DissolveTest;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Overlay;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;9;0
WireConnection;8;1;1;0
WireConnection;13;0;8;0
WireConnection;14;0;13;0
WireConnection;3;0;11;0
WireConnection;3;1;20;0
WireConnection;3;2;17;0
WireConnection;2;0;11;0
WireConnection;2;2;17;0
WireConnection;19;0;17;0
WireConnection;0;0;3;0
WireConnection;0;2;2;0
WireConnection;0;10;19;0
ASEEND*/
//CHKSM=A20E023E92DECB62A73B8CB4CE1FF6A1C157A828