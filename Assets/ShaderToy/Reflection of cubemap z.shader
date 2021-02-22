Shader "ShaderMan/Reflection of cubemap z"
{
    Properties{
        _Mirrorness("Reflection Intensity", Range(0, 1)) = 1
        _Roughness("Roughness", Range(0, 2)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 uv:TEXCOORD0;
                fixed4 tangent : TANGENT;
                fixed3 normal : NORMAL;
            };


            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            //Variables
            sampler2D _MainTex;
            float _Roughness;
            float _Mirrorness;

            #define MAX_STEPS 100
            #define MAX_DIST 100.
            #define EPSILON 0.0001
            #define PI 3.14159265
            #define IVORY 1.
            #define BLUE 2.
            #define BLACK 3.
            #define TIME (_Time * 0.0625)

            #define PHI (sqrt(5.)*0.5 + 0.5)

            fixed2x2 Rot(fixed a) {
                fixed s = sin(a), c = cos(a);
                return fixed2x2(c, -s, s, c);
            }

            fixed sdBox( fixed3 p, fixed3 b )
            {
                fixed3 q = abs(p) - b;
                return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);
            }

            fixed opSmoothUnion( fixed d1, fixed d2, fixed k ) {
                fixed h = clamp( 0.5 + 0.5*(d2-d1)/k, 0.0, 1.0 );
            return lerp( d2, d1, h ) - k*h*(1.0-h); }

            fixed opSmoothSubtraction( fixed d1, fixed d2, fixed k ) {
                fixed h = clamp( 0.5 - 0.5*(d1+d2)/k, 0.0, 1.0 );
            return lerp( d1, -d2, h ) + k*h*(1.0-h); }

            fixed2 getDist(fixed3 p) {
                fixed spheres = length(p) - 1.5;
                [unroll(100)]
                for(int i = 0; i < 4; i++) {
                    fixed3 ps = p;
                    // ps *= 2.;
                    ps += fixed3(1. * sin(TIME.y * 1.5 + 10. * fixed(i)),
                        1. * sin(TIME.y * 2.5 + 10. * fixed(i) + 10.),
                        1. * sin(TIME.y * 3.5 + 10. * fixed(i) + 2.) );
                    spheres = opSmoothUnion(spheres, length(ps) - .5, 1.5);
                }

                [unroll(100)]
                for(i = 0; i < 4; i++) {
                    fixed3 ps = p;
                    // ps *= 2.;
                    ps += fixed3( 1. * sin(TIME.y * 1.7 + 20. * fixed(i)),
                        1. * sin(TIME.y * 2.3 + 20. * fixed(i) + 10.),
                        1. * sin(TIME.y * 3.1 + 20. * fixed(i) + 2.) );
                    spheres = opSmoothSubtraction(spheres, length(ps) - .5, 1.5);
                }
                return fixed2(spheres, BLUE);
            }

            fixed3 rayMarch(fixed3 ro, fixed3 rd) {
                fixed d = 0.;
                fixed info = 0.;
                //fixed glow = 0.;
                fixed minAngleToObstacle = 1e10;
                for (int i = 0; i < MAX_STEPS; i++) {
                    fixed2 distToClosest = getDist(ro + rd * d);
                    minAngleToObstacle = min(minAngleToObstacle, atan2( d,distToClosest.x));
                    d += distToClosest.x;
                    info = distToClosest.y;
                    if(abs(distToClosest.x) < EPSILON || d > MAX_DIST) {
                        break;
                    }
                }
                return fixed3(d, info, minAngleToObstacle);
            }

            fixed3 getNormal(fixed3 p) {
                fixed2 e = fixed2(EPSILON, 0.);
                fixed3 n = getDist(p).x - fixed3(getDist(p - e.xyy).x,
                getDist(p - e.yxy).x,
                getDist(p - e.yyx).x);
                return normalize(n);
            }

            fixed3 getRayDir(fixed2 uv, fixed3 p, fixed3 l, fixed z) {
                fixed3 f = normalize(l-p),
                r = normalize(cross(fixed3(0,1,0), f)),
                u = cross(f,r),
                c = f*z,
                i = c + uv.x*r + uv.y*u,
                d = normalize(i);
                return d;
            }

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.uv;

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 render(fixed3 ro, fixed3 rd, fixed3 worldPos)
            {
                fixed zoom = 1.100;

                fixed3 rm = rayMarch(ro, rd);
                fixed d = rm[0];
                fixed info = rm[1];

                fixed color_bw = 0.;
                fixed3 colorBg = fixed3(.0,.0,.0);

                fixed3 color = fixed3(0., 0., 0.);
                fixed alpha = 0.;

                fixed3 light = fixed3(50, 20, 50);
                fixed3 p = ro + rd * d;
                if (d < MAX_DIST) {
                    fixed3 n = getNormal(p);
                    color = fixed3( n * 0.5 + 0.5 );
                    // self shadeing
                    color_bw = .5 + .5 * dot(n, normalize(light - p));
                    // drop shadows
                    // trying to raymarch to the light for MAX_DIST
                    // and if we hit something, it's shadow
                    fixed3 dirToLight = normalize(light - p);
                    fixed3 rayMarchLight = rayMarch(p + dirToLight * .06, dirToLight);
                    fixed distToObstable = rayMarchLight.x;
                    fixed distToLight = length(light - p);

                    // smooth shadows
                    fixed shadow = smoothstep(0.0, .15, rayMarchLight.z / PI);
                    color_bw *= .5 + .5 * shadow;

                    // reflection
                    fixed3 ref = reflect(rd, n);
                    // hiding sides of reflection on round
                    color += 1. * pow(dot(rd, ref / 2.), 2.);

                    // Relfection cube.
                    {
                        // For some reason, have to flip X, different handness, maybe?
                        // Also, have to flip Z to get actual reflection instead of "unflipped selfie".
                        half3 reflection = half3(-ref.x, ref.y, -ref.z);
                        /*If Roughness feature is not needed : UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflection) can be used instead.
                        It chooses the correct LOD value based on camera distance*/
                        half4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflection, _Roughness);
                        half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR); // This is done becasue the cubemap is stored HDR
                        color += skyColor * _Mirrorness;
                    }

                    alpha = 1.;
                }

                return fixed4(color, alpha);
            }

            fixed4 frag(VertexOutput i) : SV_Target
            {
                fixed2 uv = (i.uv-.5*1)/1;

                // ray origin
                half3 worldViewDir = UnityWorldSpaceViewDir(i.worldPos);
                fixed3 ro = -normalize(worldViewDir) * 5;

                // ray direction
                fixed3 rd = getRayDir(uv, ro, fixed3(0,0,0), 1.);

                return render(ro, rd, i.worldPos);
            }
            ENDCG
        }
    }
}
