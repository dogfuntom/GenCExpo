using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenCExpo
{
    internal sealed class P5Pool : MonoBehaviour
    {
        [SerializeField] private byte _limit = 7;

        private readonly LinkedList<P5Exhibit> _invisible = new LinkedList<P5Exhibit>();
        private readonly List<P5Exhibit> _visible = new List<P5Exhibit>(64);

        public void OnP5BecameInvisible(P5Exhibit p5)
        {
            _invisible.AddLast(p5);
            _visible.Remove(p5);
        }

        public void OnP5BecameVisible(P5Exhibit p5)
        {
            _invisible.Remove(p5);
            _visible.Add(p5);
        }

        private void Update()
        {
            if (_visible.Count <= _limit)
            {
                while (_visible.Count + _invisible.Count > _limit)
                {
                    var first = _invisible.First.Value;
                    _invisible.RemoveFirst();
                    first.Stop();
                    // Debug.Log("Stopped", first);
                }

                foreach (var item in _visible)
                {
                    item.Play();
                }
            }
            else
            {
                // This is a tricky situation: more visible sketches than the limit.
                // We can try to juggle the visible sketches somehow
                // but it's probably best to keep it simple:
                // mostly just "freeze" by doing nothing until the situation improves.

                // Find the farthest and pretend that it's invisible.
                var farthest =
                    (from p5 in _visible
                    let dist = Vector3.SqrMagnitude(Camera.main.transform.position - p5.transform.position)
                    orderby dist
                    select p5).Last();
                OnP5BecameInvisible(farthest);
            }
        }
    }
}