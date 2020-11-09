using UnityEngine;
using System.Runtime.InteropServices;

// TODO: Use conditionals to allow running in editor.
public class NewBehaviourScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitInputForm();

    //[DllImport("__Internal")]
    //private static extern void HelloString(string str);

    //[DllImport("__Internal")]
    //private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern bool HasP5Canvas();

    [DllImport("__Internal")]
    private static extern string GetInputText();

    //[DllImport("__Internal")]
    //private static extern void BindWebGLTexture(int texture);
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture2(int texture);
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture3(int texture);

    private Renderer rend;
    private Texture texture;

    void Start()
    {
        //{
        //    WebGLInput.captureAllKeyboardInput = false;
        //    InitInputForm();
        //}

        // var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        // texture.Resize(1, 1, TextureFormat.ARGB32, false);
        // texture.Apply();
        // BindWebGLTexture2(texture.GetNativeTextureID());

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