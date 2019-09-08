﻿//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _PointLightColor("Point Light Color", Color) = (0, 0, 0)
        _PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;	
            uniform float3 _PointLightColor;
            uniform float3 _PointLightPosition;

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
                float4 color : COLOR;
				float2 uv : TEXCOORD0;
                float4 worldVertex : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				//float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
				//float4 displacement = float4(0.0f, -5.0f, 0.0f, 0.0f); // Q2a
				//float4 displacement = float4(0.0f, _Time.y, 0.0f, 0.0f); // Q2b
				//float4 displacement = float4(0.0f, sin(_Time.y), 0.0f, 0.0f); // Q2c
				//float4 displacement = float4(0.0f, sin(v.vertex.x), 0.0f, 0.0f); // Q3
				//float4 displacement = float4(0.0f, sin(v.vertex.x + _Time.y), 0.0f, 0.0f); // Q4
				//float4 displacement = float4(0.0f, sin(v.vertex.x + _Time.y) * 0.5f, 0.0f, 0.0f); // Q5a
				//float4 displacement = float4(0.0f, sin(v.vertex.x + _Time.y * 2.0f), 0.0f, 0.0f); // Q5b
				float4 displacement = float4(0.0f, 4 * sin(v.vertex.x + _Time.y), 0.0f, 0.0f); // Q5c
				v.vertex += displacement;

				vertOut o;
                
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                // Transform vertex in world coordinates to camera coordinates, and pass colour
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;

                // Pass out the world vertex position and world normal to be interpolated
                // in the fragment shader (and utilised)
                o.worldVertex = worldVertex;
                o.worldNormal = worldNormal;
                
                o.uv = v.uv;
				return o;

				// Challenge answer - replace entire vertex shader with the code below: 
				/*
				// Apply the model and view matrix to the vertex (but not the projection matrix yet)
				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);

				// v.vertex is now in view space. This is the point where we want to apply the displacement.
				v.vertex += float4(0.0f, sin(v.vertex.x + _Time.y), 0.0f, 0.0f);
				
				// Finally apply the projection matrix to complete the transformation into screen space
				v.vertex = mul(UNITY_MATRIX_P, v.vertex);

				// Build output structure
				vertOut o;
				o.vertex = v.vertex;
				o.uv = v.uv;
				return o;
				*/
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
                
                // Our interpolated normal might not be of length 1
                float3 interpNormal = normalize(v.worldNormal);

                // Calculate ambient RGB intensities
                float Ka = 1;
                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
                // (when calculating the reflected ray in our specular component)
                float fAtt = 1;
                float Kd = 1;
                float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
                float LdotN = dot(L, interpNormal);
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

                // Calculate specular reflections
                float Ks = 1;
                float specN = 5; // Values>>1 give tighter highlights
                float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                // Using classic reflection calculation:
                //float3 R = normalize((2.0 * LdotN * interpNormal) - L);
                //float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);
                // Using Blinn-Phong approximation:
                specN = 25; // We usually need a higher specular power when using Blinn-Phong
                float3 H = normalize(V + L);
                float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

                // Combine Phong illumination model components
                float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = v.color.a;
                
                
                
				return col*0.5f + returnColor*0.5f;
			}
			ENDCG
		}
	}
}