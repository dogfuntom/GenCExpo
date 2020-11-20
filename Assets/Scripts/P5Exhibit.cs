﻿using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace GenCExpo
{
    [RequireComponent(typeof(Renderer))]
    public class P5Exhibit: MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private byte _materialIndex;
        [SerializeField] private byte _restartSeconds;

        [SerializeField] private GameObject _plaque;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool HasP5Instance(string name);
#else
        private static bool HasP5Instance(string name) => false;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void RecreateP5Instance(string name);
#else
        private static void RecreateP5Instance(string name) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(RecreateP5Instance));
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GetP5CanvasTexture(string name, int texture);
#else
        private static void GetP5CanvasTexture(string name, int texture) => Debug.LogWarningFormat("{0}() is called but ignored because it's supported only on WebGL.", nameof(GetP5CanvasTexture));
#endif

        private Texture _texture;
        private float _timer;

        private void Start()
        {
            const int width = 400;
            const int height = 400;
            _texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            var rend = GetComponent<Renderer>();
            rend.materials[_materialIndex].mainTexture = _texture;
        }

        private void Update()
        {
            if (HasP5Instance(_name))
            {
                GetP5CanvasTexture(_name, _texture.GetNativeTexturePtr().ToInt32());

                if (_restartSeconds > 0 && _timer > _restartSeconds)
                {
                    RecreateP5Instance(_name);
                    _timer = 0;
                }
                _timer += Time.deltaTime;
            }

            ShowPlaqueIfAimedAt();
        }

        private void ShowPlaqueIfAimedAt()
        {
            var ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
            if (GetComponent<Collider>().Raycast(ray, out var result, 8))
            {
                _plaque.SetActive(true);
            }
            else
            {
                _plaque.SetActive(false);
            }
        }
    }
}