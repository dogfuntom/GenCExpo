using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class NewBehaviourScript : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool HasP5Canvas();
#else
    private static bool HasP5Canvas() => false;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture3(int texture);
#else
    private static void BindWebGLTexture3(int texture) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(BindWebGLTexture3));
#endif

    private Renderer rend;
    private Texture texture;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        const int width = 400;
        const int height = 400;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        rend.sharedMaterial.mainTexture = texture;
    }

    private void Update()
    {
        if (HasP5Canvas()) {
            BindWebGLTexture3(texture.GetNativeTexturePtr().ToInt32());
        }
    }
}