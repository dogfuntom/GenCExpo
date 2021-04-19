using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace GenCExpo
{
    internal sealed class DeactivateOnDesktop : MonoBehaviour
    {
        void Start()
        {
            if (!Application.isMobilePlatform)
                gameObject.SetActive(false);
        }
    }
}
