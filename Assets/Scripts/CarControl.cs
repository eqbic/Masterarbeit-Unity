using System;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class CarControl : TuiControlBase
{
    private float _lastAngleRotation = 0f;
    private float _panInitialDistance;
    private float _lastAngleZoom;
    private float _currentSpeed;
    private System.Numerics.Vector2 _joystickInitialPosition;

    public override void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate, Action<Vector2> pan)
    {
        base.Init(magnify, focusView, zoom, rotate, pan);
        _lastAngleZoom = Magnify.Angle;
    }
    
    // rotation object
    public override void AddZoomToken(Tuio20Object zoomToken)
    {
        ZoomToken = zoomToken.Token;
        _lastAngleRotation = ZoomToken.Angle;
    }

    protected override void UpdateZoom()
    {
        var deltaAngle = DeltaAngle(Magnify.Angle, ref _lastAngleZoom);
        var zoom = Mathf.Clamp(FocusView.CurrentZoom + deltaAngle * ZoomSpeed, FocusView.MinZoom, FocusView.MaxZoom);
        Zoom?.Invoke(zoom);
    }

    protected override void UpdateRotation()
    {
        if (ZoomToken == null) return;
        var deltaAngle = DeltaAngle(ZoomToken.Angle, ref _lastAngleRotation);
        deltaAngle *= Mathf.Rad2Deg;
        Rotate?.Invoke(deltaAngle);
    }
    
    protected override Vector2 CalculateMoveVector(Vector2 direction, float speed)
    {
        var angle = Vector2.SignedAngle(Vector2.right, direction);
        direction = angle > 0 ? Vector2.up : Vector2.down;
        return direction * (-1f * (speed  * 1f * Time.deltaTime));        
    }

    protected override float GetSpeed(Vector2 direction)
    {
        var distance = Mathf.Max(direction.magnitude - _deadZoneRadiusPixel, 0);
        var v = direction.normalized * distance;
        return _maxSpeed / ActivationFunction(Mathf.Abs(v.y));
    }
}