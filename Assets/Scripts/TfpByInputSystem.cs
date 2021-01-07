using TheFirstPerson;
using UnityEngine.InputSystem;
using UnityEngine;

public class TfpByInputSystem : TFPExtension
{
    public InputActionProperty Move;
    public InputActionProperty Look;
    public float LookSensitivity = 8;

    public override void ExStart(ref TFPData data, TFPInfo info)
    {
        base.ExStart(ref data, info);

        if (Move != null && Move.reference == null)
            Move.action.Enable();
        if (Look != null && Look.reference == null)
            Look.action.Enable();

        Debug.LogFormat("isMobilePlatform = {0}", Application.isMobilePlatform);
    }

    public override void ExPostInput(ref TFPData data, TFPInfo info)
    {
        base.ExPostInput(ref data, info);

        if (Move != null)
        {
            var vec = Move.action.ReadValue<Vector2>();
            if (vec != Vector2.zero)
            {
                data.xIn = vec.x;
                data.yIn = vec.y;
                data.moving = true;
            }
        }
    }

    public override void ExPreMove(ref TFPData data, TFPInfo info)
    {
        if (Look == null)
            return;

        var vec = Look.action.ReadValue<Vector2>();
        if (vec == Vector2.zero)
            return;

        Vector2 lastLook = GetLastLook(data, info);

        float horLook = lastLook.y + vec.x * Time.deltaTime * LookSensitivity;
        float verLook = lastLook.x - vec.y * Time.deltaTime * LookSensitivity;

        transform.eulerAngles = new Vector3(0.0f, horLook, 0.0f);
        info.cam.localEulerAngles = new Vector3(verLook, 0.0f, 0.0f);
    }

    Vector2 GetLastLook(TFPData data, TFPInfo info)
    {
        return new Vector2(info.cam.localEulerAngles.x, transform.eulerAngles.y);
    }
}
