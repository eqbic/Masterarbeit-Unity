using System;
using TuioNet.Tuio20;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    private Tuio20Token _magnify;

    private float _zoom = 17f;
    private float _zoomSpeed = 2f;

    private float _lastAngle;

    public void Init(Tuio20Object magnify)
    {
        _magnify = magnify.Token;
        _lastAngle = _magnify.Angle;
    }

    private void Update()
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