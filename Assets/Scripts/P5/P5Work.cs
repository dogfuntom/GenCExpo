using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace GenC.P5
{
    [System.Serializable]
    internal sealed class P5Work
    {
        [SerializeField]
        private string _key;
        [SerializeField]
        private string _author;
        [SerializeField]
        private byte _restartSeconds;

        public string Key { get => _key; private set => _key = value; }

        public string Author { get => _author; private set => _author = value; }
        public byte RestartSeconds { get => _restartSeconds; private set => _restartSeconds = value; }

        public void Pause() => PauseP5(Key);
        public void Stop() => StopP5(Key);
        public bool Play() => PlayP5(Key);
        public int GetWidth() => GetP5Width(Key);
        public int GetHeight() => GetP5Height(Key);
        public void GetTexture(int ptr) => GetP5Texture(Key, ptr);
        public void Recreate() => RecreateP5(Key);

        #region DllImport
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool PlayP5(string name);
#else
        private static bool PlayP5(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GetP5Texture(string name, int texture);
#else
        private static void GetP5Texture(string name, int texture) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(GetP5Texture));
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int GetP5Width(string name);
#else
        private static int GetP5Width(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int GetP5Height(string name);
#else
        private static int GetP5Height(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void RecreateP5(string name);
#else
        private static void RecreateP5(string name) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(RecreateP5));
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void PauseP5(string name);
#else
        private static void PauseP5(string name) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(PauseP5));
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void StopP5(string name);
#else
        private static void StopP5(string name) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(StopP5));
#endif
        #endregion
    }
}
