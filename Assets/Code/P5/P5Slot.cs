using UnityEngine;
using UnityEngine.Profiling;

// TODO: Something is still not right, there's too much mess.
// Decisions:
// - Disabled works must be stopped, not paused. (Later should be configured per work probably.)
// - An invisible work should still be initialized and work in background unless disabled by count limit.
//   (Because so far it's better this way. Later it should be configurable per work.)
// - Thus, Slot itself doesn't care about Frustrum Culling alone, it's there only to be used by limiter.
// - Only refreshing texture gets special treatment and is dirtied by OnWillRenderObject() callback.
namespace GenC.P5
{
    /// <summary>
    /// The slot has no initiative, it must be used by <see cref="P5Exhibits"/> to work.
    /// </summary>
    // Life-cycle:
    // 1. Disabled (no work or disabled by director).
    // 2. Loading (waking up the work).
    // 3. Updating (everything is working under the hood; but no reason to refresh the visible texture).
    // 5. Rendering (the texture can and must be refreshed).
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

        // Think of it as an online media player.
        // It may be stopped, playing, paused.
        // And if it's playing but not rendered, it still consumes some CPU and memory resources.
        private P5Work _work;

        //private Stage _state;

        ///// <summary>
        ///// Returns the <see cref="Renderer.isVisible"/> value.
        ///// </summary>
        //public bool IsFrustrumVisible
        //{
        //    get
        //    {
        //        if (!_renderer)
        //            return false;
        //        return _renderer.isVisible;
        //    }
        //}

        public void Reinit(P5Work p5)
        {
            foreach (var p in _plaques)
            {
                p.text = p5.Author;
            }

            _restartSeconds = p5.RestartSeconds;
            _work = p5;

            //_state = Stage.Loading;
        }

        //private void LateUpdate()
        //{
        //    if (_state != Stage.Rendering)
        //        return;

        //    Profiler.BeginSample("get native texture pointer,");
        //    var ptr = _texture.GetNativeTexturePtr().ToInt32();
        //    Profiler.EndSample();

        //    Profiler.BeginSample("and get texture by native texture pointer.");
        //    _work.GetTexture(ptr);
        //    Profiler.EndSample();

        //    _state = Stage.Updating;
        //}

        //private void OnDisable() => _work?.Stop();

        private bool _isRendering;

        private void OnWillRenderObject()
        {
            //if (Stage.Updating == _state)
            //    _state = Stage.Rendering;
            _isRendering = true;
        }

        // We want to refresh texture often
        // but not more often than rendering
        // because it won't be visible anyway.
        // Using a flag achieves exactly that.
        private void LateUpdate()
        {
            if (!_isRendering)
                return;
            _isRendering = false;

            Profiler.BeginSample("get native texture pointer,");
            var ptr = _texture.GetNativeTexturePtr().ToInt32();
            Profiler.EndSample();

            Profiler.BeginSample("and get texture by native texture pointer.");
            _work.GetTexture(ptr);
            Profiler.EndSample();
        }

        private void Start()
        {
            // Prepare everything that doesn't depend on a particular work.
            _texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            _renderer.materials[_materialIndex].mainTexture = _texture;

            // Do nothing else while unused.
            if (_work == null)
                enabled = false;
        }

        private void Update()
        {
            var initialized = _work.Play();
            if (!initialized)
                return;

            Profiler.BeginSample("get width and height,");
            int width = _work.GetWidth();
            int height = _work.GetHeight();
            Profiler.EndSample();

            if (width != _texture.width || height != _texture.height)
            {
                Profiler.BeginSample("resize the texture,");
                _texture.Resize(width, height, TextureFormat.ARGB32, false);
                _texture.Apply();
                Profiler.EndSample();
            }

            if (_restartSeconds > 0 && _timer > _restartSeconds)
            {
                Profiler.BeginSample("Restart (by recreating).");
                _work.Recreate();
                _timer = 0;
                Profiler.EndSample();
            }

            _timer += Time.deltaTime;
        }

        //private void Update()
        //{
        //    var initialized = _work.Play();

        //    if (_state == Stage.Loading && initialized)
        //        _state = Stage.Updating;

        //    if (_state >= Stage.Updating)
        //    {
        //        Profiler.BeginSample("get width and height,");
        //        int width = _work.GetWidth();
        //        int height = _work.GetHeight();
        //        Profiler.EndSample();

        //        if (width != _texture.width || height != _texture.height)
        //        {
        //            Profiler.BeginSample("resize the texture,");
        //            _texture.Resize(width, height, TextureFormat.ARGB32, false);
        //            _texture.Apply();
        //            Profiler.EndSample();
        //        }

        //        if (_restartSeconds > 0 && _timer > _restartSeconds)
        //        {
        //            Profiler.BeginSample("Restart (by recreating).");
        //            _work.Recreate();
        //            _timer = 0;
        //            Profiler.EndSample();
        //        }

        //        _timer += Time.deltaTime;
        //    }
        //}

        #region Design-time logic
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
        #endregion

        //private enum Stage { Updating, Rendering }
    }
}
