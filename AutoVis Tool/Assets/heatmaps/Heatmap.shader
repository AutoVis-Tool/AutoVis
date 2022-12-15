// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Alan Zucconi
// www.alanzucconi.com
Shader "Heatmap" {
		Properties{
			_HeatTex("Texture", 2D) = "white" {}
		}
			SubShader{
			Tags{ "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blend

			Pass{
			CGPROGRAM
			//new
			#include "UnityCG.cginc"
#pragma vertex vert             
#pragma fragment frag




		struct vertInput {
			//new
			float4 pos : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct vertOutput {
			float4 pos : POSITION;
			fixed3 worldPos : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		vertOutput vert(vertInput input) {
			vertOutput o;
			//new
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_OUTPUT(vertOutput, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(input.pos);
			o.worldPos = mul(unity_ObjectToWorld, input.pos);
			return o;
		}

		//UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);

		uniform int _Points_Length = 0;
		uniform float3 _Points[100];		// (x, y, z) = position
		uniform float2 _Properties[100];	// x = radius, y = intensity

		sampler2D _HeatTex;

		half4 frag(vertOutput output) : COLOR{
			//new
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(output);
			// Loops over all the points
			half h = 0;
		for (int i = 0; i < _Points_Length; i++)
		{
			// Calculates the contribution of each point
			half di = distance(output.worldPos, _Points[i].xyz);

			half ri = _Properties[i].x;
			half hi = 1 - saturate(di / ri);

			h += hi * _Properties[i].y;
		}

		// Converts (0-1) according to the heat texture
		h = saturate(h);
		half4 color = tex2D(_HeatTex, fixed2(h, 0.5));
		return color;
		}
			ENDCG
		}
		}
			Fallback "Diffuse"
	}