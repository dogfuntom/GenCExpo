using TheFirstPerson;
using UnityEngine;

namespace GenC
{
    [RequireComponent(typeof(Collider))]
    internal sealed class TfpRespawnOnFall : TFPExtension
    {
        private Collider _playerCollider;
        private Vector3 _initialPosition;

        public override void ExStart(ref TFPData data, TFPInfo info)
        {
            base.ExStart(ref data, info);
            _initialPosition = info.controller.transform.position;
            _playerCollider = info.controller.GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == _playerCollider)
            {
                _playerCollider.transform.position = _initialPosition;
            }
        }
    }
}
