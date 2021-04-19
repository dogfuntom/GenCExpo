using UnityEngine;
using UnityEngine.Profiling;

namespace GenC.P5
{
    internal sealed class P5Slot : MonoBehaviour
    {
        [SerializeField] private P5Exhibits _director;
        [Space]
        [SerializeField] private Renderer _renderer;
        [SerializeField] private byte _materialIndex;
        [Space]
        [SerializeField] private TMPro.TMP_Text[] _plaques;

        private byte _restartSeconds;
        private Texture2D _texture;
        private float _timer;

        private bool _isVisible = true;
        private bool _isStopped = true;

        private P5Work _work;

        public void Reinit(P5Work p5)
        {
            foreach (var p in _plaques)
            {
                p.text = p5.Author;
            }

            _restartSeconds = p5.RestartSeconds;

            _work = p5;
        }

        public void Stop()
        {
            _isStopped = true;
            _work.Stop();
        }

        public void Play()
        {
            _isStopped = false;
        }

        private void OnBecameInvisible()
        {
            _work.Pause();

            _isVisible = false;
            if (_director)
                _director.OnSlotBecameInvisible(this);
        }

        private void OnBecameVisible()
        {
            _isVisible = true;
            if (_director)
                _director.OnSlotBecameVisible(this);
        }

        private void OnValidate()
        {
            _director ??= GetComponentInParent<P5Exhibits>();
            _renderer ??= GetComponentInChildren<Renderer>();

            if (!GetComponent<Renderer>())
            {
                Debug.LogWarningFormat(
                    gameObject,
                    "{0} works only when there's a {1} on the same game object.",
                    nameof(P5Slot),
                    nameof(Renderer));
            }
        }

        private void Start()
        {
            _texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

            _renderer.materials[_materialIndex].mainTexture = _texture;

            if (_work == null)
                enabled = false;
        }

        private void Update()
        {
            if (_isStopped || !_isVisible)
                return;

            bool ready = _work.Play();
            if (!ready)
                return;

            Profiler.BeginSample("Get width and height");
            int width = _work.GetWidth();
            int height = _work.GetHeight();
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

            _work.GetTexture(ptr);

            if (_restartSeconds > 0 && _timer > _restartSeconds)
            {
                _work.Recreate();
                _timer = 0;
            }

            _timer += Time.deltaTime;
        }
    }
}
