using System;
using TuioNet.Tuio20;
using UnityEngine;

public class CarControl : TuiControlBase
{
    private float _lastAngleRotation = 0f;
    private float _lastAngleSpeed = 0f;
    private float _panInitialDistance;
    private float _currentSpeed = 0f;
    private float _lastAngleZoom;

    public override void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate, Action<Vector2> pan)
    {
        ZoomSpeed = 2f;
        base.Init(magnify, focusView, zoom, rotate, pan);
        _lastAngleZoom = Magnify.Angle;
    }

    // speed object
    public override void AddJoystick(Tuio20Object joystick)
    {
        JoystickToken = joystick.Token;
        _lastAngleSpeed = JoystickToken.Angle;
        if (ZoomToken == null) return;
        _panInitialDistance = GetPanDistance();
    }

    // rotation object
    public override void AddZoomToken(Tuio20Object zoomToken)
    {
        ZoomToken = zoomToken.Token;
        _lastAngleRotation = ZoomToken.Angle;
        if (JoystickToken == null) return;
        _panInitialDistance = GetPanDistance();
    }

    // speed object
    public override void RemoveJoystick()
    {
        JoystickToken = null;
    }
    
    // rotation object
    public override void RemoveZoomToken()
    {
        ZoomToken = null;
    }
    
    private float GetPanDistance()
    {
        return (TuioUtils.ToScreenPoint(JoystickToken.Position) - TuioUtils.ToScreenPoint(ZoomToken.Position)).magnitude;
    }
    
    protected override void UpdateZoom()
    {
        var deltaAngle = DeltaAngle(Magnify.Angle, ref _lastAngleZoom);
        var zoom = Mathf.Clamp(FocusView.CurrentZoom + deltaAngle * ZoomSpeed, FocusView.MinZoom, FocusView.MaxZoom);
        Zoom?.Invoke(zoom);
    }

    protected override void UpdatePan()
    {
        if (JoystickToken == null) return;
        var deltaSpeed = DeltaAngle(JoystickToken.Angle, ref _lastAngleSpeed);
        _currentSpeed = Mathf.Max(_currentSpeed + deltaSpeed, 0f);
        var direction = _currentSpeed * 10f * Vector2.down;
        Pan?.Invoke(direction);

    }

    protected override void UpdateRotation()
    {
        if (ZoomToken == null) return;
        var deltaAngle = DeltaAngle(ZoomToken.Angle, ref _lastAngleRotation);
        deltaAngle *= Mathf.Rad2Deg;
        Rotate?.Invoke(deltaAngle);
    }
}