using UnityEngine;
using System.Runtime.InteropServices;

public class NewBehaviourScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitInputForm();

    //[DllImport("__Internal")]
    //private static extern void HelloString(string str);

    //[DllImport("__Internal")]
    //private static extern void PrintFloatArray(float[] array, int size);

    //[DllImport("__Internal")]
    //private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string GetInputText();

    //[DllImport("__Internal")]
    //private static extern void BindWebGLTexture(int texture);
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture2(int texture);

    void Start()
    {
        {
            WebGLInput.captureAllKeyboardInput = false;
            InitInputForm();
        }

        var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        texture.Resize(1, 1, TextureFormat.ARGB32, false);
        texture.Apply();
        BindWebGLTexture2(texture.GetNativeTextureID());

        var rend = GetComponentInChildren<Renderer>();
        rend.sharedMaterial.mainTexture = texture;
    }

    string previousText = string.Empty;
    private void Update()
    {
        var text = GetInputText();
        Debug.Log(text);
        previousText = text;
    }
}