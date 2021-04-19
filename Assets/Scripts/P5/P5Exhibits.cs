using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenC.P5
{
    /// <summary>
    /// Handles limiting the number of active works.
    /// </summary>
    internal sealed class P5Exhibits : MonoBehaviour
    {
        // Actually, the limit can be bigger. This number of 7 helps testing for now.
        [SerializeField] private byte _limit = 7;
        [Space]
        [SerializeField] private P5Slot[] _slots;
        [SerializeField] private P5Work[] _works;
        [SerializeField] private bool _shuffle = true;

        private readonly LinkedList<P5Slot> _invisible = new LinkedList<P5Slot>();
        private readonly List<P5Slot> _visible = new List<P5Slot>(64);

        internal void OnSlotBecameInvisible(P5Slot p5)
        {
            _invisible.AddLast(p5);
            _visible.Remove(p5);
        }

        internal void OnSlotBecameVisible(P5Slot p5)
        {
            _invisible.Remove(p5);
            _visible.Add(p5);
        }

        private void Start()
        {
            void swapWorks(int i, int j)
            {
                P5Work tmp = _works[i];
                _works[i] = _works[j];
                _works[j] = tmp;
            }

            if (_shuffle)
            {
                for (int i = _works.Length - 1; i >= 1; i--)
                {
                    int j = Random.Range(0, i - 1);
                    swapWorks(i, j);
                }
            }

            var count = Mathf.Min(_slots.Length, _works.Length);
            for (int i = 0; i < count; i++)
            {
                _slots[i].Reinit(_works[i]);
                _slots[i].enabled = true;
            }
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

                OnSlotBecameInvisible(farthest);
            }
        }
    }
}
