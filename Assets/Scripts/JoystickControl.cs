using System;
using TuioNet.Tuio20;
using UnityEngine;

public class JoystickControl : TuiControlBase
{
    private float _lastAngleRotation;
    private float _lastAngleZoom;

    public override void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate, Action<Vector2> pan)
    {
        base.Init(magnify, focusView, zoom, rotate, pan);
        _lastAngleRotation = Magnify.Angle;
    }

    public override void AddZoomToken(Tuio20Object zoomToken)
    {
        ZoomToken = zoomToken.Token;
        _lastAngleZoom = ZoomToken.Angle;
    }

    protected override void UpdateZoom()
    {
        if (ZoomToken == null) return;
        TokenZoom();
    }
    
    private void TokenZoom()
    {
        ZoomByAngle(ZoomToken.Angle);
    }
    
    private void ZoomByAngle(float angle)
    {
        var deltaAngle = DeltaAngle(angle, ref _lastAngleZoom);
        if(Mathf.Abs(deltaAngle) < 0.015f) return;
        var zoom = Mathf.Clamp(FocusView.CurrentZoom + (deltaAngle) * ZoomSpeed, FocusView.MinZoom, FocusView.MaxZoom);
        Zoom?.Invoke(zoom);
    }

    protected override void UpdateRotation()
    {
        var deltaAngle = DeltaAngle(Magnify.Angle, ref _lastAngleRotation);
        deltaAngle = -deltaAngle * Mathf.Rad2Deg;
        Rotate?.Invoke(deltaAngle);
    }
}