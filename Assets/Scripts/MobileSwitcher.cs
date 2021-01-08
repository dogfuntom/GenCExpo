using UnityEngine;

namespace GenC
{
    public class MobileSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activateOnMobile;
        [SerializeField] private GameObject[] _deactivateOnMobile;

        void Awake()
        {
            if (!Application.isMobilePlatform)
                return;

            foreach (var item in _deactivateOnMobile)
            {
                item.SetActive(false);
            }

            foreach (var item in _activateOnMobile)
            {
                item.SetActive(true);
            }
        }
    }
}
