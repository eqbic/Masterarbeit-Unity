using System;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    private Tuio20Token _magnify;
    private Tuio20Token _joystick;

    private float _zoom = 17f;
    private float _zoomSpeed = 2f;

    private float _lastAngle;

    public void Init(Tuio20Object magnify)
    {
        _magnify = magnify.Token;
        _lastAngle = _magnify.Angle;
    }

    public void AddJoystick(Tuio20Object joystick)
    {
        _joystick = joystick.Token;
    }

    private void LateUpdate()
    {
        // UpdateZoom();
        UpdatePan();
    }

    private void UpdatePan()
    {
        // if (_joystick == null) return;
        // var v = (_joystick.Position - _magnify.Position).ToUnity();
        // v.y *= -1f;
        // var speed = Mathf.Clamp(v.magnitude - 0.15f, 0f, 1f);
        // if (speed > 0f)
        // {
        //     Pan(v.normalized * (speed * Time.deltaTime));
        // }
        Pan(Vector2.right * (0.2f * Time.deltaTime));
    }

    private void UpdateZoom()
    {
        var deltaAngle = DeltaAngle(_magnify.Angle);
        if (deltaAngle == 0f) return;
        _zoom += (deltaAngle) * _zoomSpeed;
        Zoom(_zoom);
    }

    private float DeltaAngle(float currentAngle)
    {
        var delta = currentAngle - _lastAngle;
        if (delta > Mathf.PI)
        {
            delta -= 2* Mathf.PI;
        }

        if (delta < -Mathf.PI)
        {
            delta += 2 * Mathf.PI;
        }

        _lastAngle = currentAngle;
        return delta;
    }
}