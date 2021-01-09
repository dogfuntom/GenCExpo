using TheFirstPerson;
using UnityEngine;

[RequireComponent(typeof(FPSController))]
internal sealed class DisableMouseLockOnMobile : MonoBehaviour
{
    public bool DisableMouseLookCompletely = true;
    public bool Simulate;

    private void Awake()
    {
        if (!Simulate && !Application.isMobilePlatform)
            return;

        var fpsc = GetComponent<FPSController>();
        fpsc.mouseLockToggleEnabled = false;
        fpsc.startMouseLock = false;

        if (DisableMouseLookCompletely)
            fpsc.mouseLookEnabled = false;
    }
}
