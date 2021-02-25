Shader "ShaderMan/Reflection of cubemap z"
{
    Properties{
        _Mirrorness("Reflection Intensity", Range(0, 1)) = 1
        _Roughness("Roughness", Range(0, 2)) = 0
        _RayOriginScale("Scale applied to ray origin", Range(0.5, 16)) = 5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "AlphaTest" "DisableBatching" = "True" "IgnoreProject" = "True"}

        Pass
        {
            Cull Front
            ZTest Less
            Offset 0, -1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexInput {
                fixed4 vertex : POSITION;
            };

            struct VertexOutput {
                fixed4 pos : POSITION;
                fixed3 rayOrigin : TEXCOORD0;
                fixed3 rayDirection : TEXCOORD1;
            };

            //Variables
            sampler2D _MainTex;
            fixed _Roughness;
            fixed _Mirrorness;
            fixed _RayOriginScale;

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

                fixed3 objSpaceCameraPos = mul(unity_WorldToObject, fixed4(_WorldSpaceCameraPos.xyz, 1)).xyz;
                o.rayOrigin = objSpaceCameraPos * _RayOriginScale;
                o.rayDirection = normalize(v.vertex.xyz - objSpaceCameraPos);
                return o;
            }

            fixed4 render(fixed3 ro, fixed3 rd)
            {
                fixed3 rm = rayMarch(ro, rd);
                fixed d = rm[0];
                fixed info = rm[1];

                fixed color_bw = 0.;
                fixed3 colorBg = fixed3(.0,.0,.0);

                fixed3 color = fixed3(0., 0., 0.);

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
                    fixed3 ref = reflect(-rd, n);
                    // hiding sides of reflection on round
                    color += 1. * pow(dot(rd, ref / 2.), 2.);

                    // Relfection cube.
                    {
                        // For some reason Y is flipped.
                        fixed3 reflection = fixed3(ref.x, -ref.y, ref.z);
                        /*If Roughness feature is not needed : UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflection) can be used instead.
                        It chooses the correct LOD value based on camera distance*/
                        fixed4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflection, _Roughness);
                        fixed3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR); // This is done becasue the cubemap is stored HDR
                        color += skyColor * _Mirrorness;
                    }
                }
                else {
                    discard;
                }

                return fixed4(color, 1.);
            }

            fixed4 frag(VertexOutput i) : COLOR
            {
                return render(i.rayOrigin, i.rayDirection);
            }
            ENDCG
        }
    }
}
