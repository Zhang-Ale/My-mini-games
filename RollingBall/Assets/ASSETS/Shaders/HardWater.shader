Shader "NABA/HardWater" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Scale", Range(0,4)) = 1
		_Speed ("Speed", Range(0,4)) = 1
		_Frequency ("Frequency", Range(0,10)) = 1
		_Hardness ("Hardness", Range(-2,2)) = 1

	}
	SubShader 
	{

		Tags { "Queue"="Transparent" "RenderType"="Transparent"}

		LOD 200
		Cull Off

		CGPROGRAM
		#pragma surface surf Standard alpha
		#pragma vertex vert
		#pragma target 3.0


		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		float _Scale, _Speed, _Frequency;
		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		float4 _Color;
		float _Hardness;

		void vert (inout appdata_full v) 
		{
			half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));
			half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);
			v.vertex.y += value;
		}



		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			o.Albedo = _Color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
			o.Occlusion = _Color.a;

			float3 x = ddx(IN.worldPos);  
			float3 y = ddy(IN.worldPos);
			float3 normal = normalize(cross(x, y));
			o.Normal = normal * _Hardness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
