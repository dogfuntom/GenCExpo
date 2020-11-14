using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace GenCExpo
{
    public class P5Manager: MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool HasP5Canvas();
#else
        private static bool HasP5Canvas() => false;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void GetP5CanvasTexture(int texture);
#else
        private static void GetP5CanvasTexture(int texture) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(GetP5CanvasTexture));
#endif

        private Renderer _rend;
        private Texture _texture;

        void Start()
        {
            _rend = GetComponentInChildren<Renderer>();

            const int width = 400;
            const int height = 400;
            _texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            _rend.sharedMaterial.mainTexture = _texture;
        }

        private void Update()
        {
            if (HasP5Canvas())
            {
                GetP5CanvasTexture(_texture.GetNativeTexturePtr().ToInt32());
            }
        }
    }
}