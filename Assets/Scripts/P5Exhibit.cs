﻿using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace GenCExpo
{
    [RequireComponent(typeof(Renderer))]
    public class P5Exhibit : MonoBehaviour
    {
        private const byte PlaqueDistance = 4;

        [SerializeField] private string _name;
        [SerializeField] private byte _materialIndex;
        [SerializeField] private byte _restartSeconds;

        [Tooltip("Can be null.")]
        [SerializeField] private GameObject _plaque;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool InitP5Instance(string name);
#else
        private static bool InitP5Instance(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GetP5CanvasTexture(string name, int texture);
#else
        private static void GetP5CanvasTexture(string name, int texture) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(GetP5CanvasTexture));
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int GetP5CanvasTextureWidth(string name);
#else
        private static int GetP5CanvasTextureWidth(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int GetP5CanvasTextureHeight(string name);
#else
        private static int GetP5CanvasTextureHeight(string name) => default;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void RecreateP5Instance(string name);
#else
        private static void RecreateP5Instance(string name) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(RecreateP5Instance));
#endif

        private Texture2D _texture;
        private float _timer;

        private bool _isVisible = true;

        private void OnBecameVisible() => _isVisible = true;
        private void OnBecameInvisible() => _isVisible = false;

        private void Start()
        {
            if (string.IsNullOrEmpty(_name))
                return;

            _texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

            var rend = GetComponent<Renderer>();
            rend.materials[_materialIndex].mainTexture = _texture;
        }

        private void ShowPlaqueIfAimedAt()
        {
            Profiler.BeginSample(nameof(ShowPlaqueIfAimedAt));
            var ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
            _plaque.SetActive(GetComponent<Collider>().Raycast(ray, out _, PlaqueDistance));
            Profiler.EndSample();
        }

        private void Update()
        {
            if (_plaque)
                ShowPlaqueIfAimedAt();

            if (string.IsNullOrEmpty(_name))
                return;

            Profiler.BeginSample(nameof(InitP5Instance));
            var ready = InitP5Instance(_name);
            Profiler.EndSample();

            if (!ready || !_isVisible)
                return;

            Profiler.BeginSample("Get width and height");
            var width = GetP5CanvasTextureWidth(name);
            var height = GetP5CanvasTextureHeight(name);
            Profiler.EndSample();

            if (width != _texture.width || height != _texture.height)
            {
                Profiler.BeginSample(nameof(Texture2D.Resize));
                _texture.Resize(width, height, TextureFormat.ARGB32, false);
                _texture.Apply();
                Profiler.EndSample();
            }

            Profiler.BeginSample("Get native texture pointer");
            var ptr = _texture.GetNativeTexturePtr().ToInt32();
            Profiler.EndSample();

            Profiler.BeginSample(nameof(GetP5CanvasTexture));
            GetP5CanvasTexture(_name, ptr);
            Profiler.EndSample();

            if (_restartSeconds > 0 && _timer > _restartSeconds)
            {
                Profiler.BeginSample(nameof(RecreateP5Instance));
                RecreateP5Instance(_name);
                Profiler.EndSample();

                _timer = 0;
            }

            _timer += Time.deltaTime;
        }
    }
}