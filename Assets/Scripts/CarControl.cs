using System;
using TuioNet.Tuio20;
using UnityEngine;

public class CarControl : TuiControlBase
{
    private float _lastAngleRotation = 0f;
    private float _panInitialDistance;
    private float _lastAngleZoom;
    private float _lastAngleSpeed;
    private float _currentSpeed;
    private float _maxSpeed = 0.5f * Mathf.PI;
    private SpeedMeter _speedMeter;

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
    }

    // rotation object
    public override void AddZoomToken(Tuio20Object zoomToken)
    {
        ZoomToken = zoomToken.Token;
        _lastAngleRotation = ZoomToken.Angle;
    }

    // speed object
    public override void RemoveJoystick()
    {
        _currentSpeed = 0f;
        Destroy(_speedMeter.gameObject);
        _speedMeter = null;
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
        _currentSpeed = Mathf.Clamp(_currentSpeed + deltaSpeed, 0f, _maxSpeed);
        _speedMeter.SetNormalizedSpeed(_currentSpeed / _maxSpeed);
        var speed = Mathf.Pow(2f * _currentSpeed, 3);
        var direction = speed * Vector2.down;
        Pan?.Invoke(direction);

    }

    protected override void UpdateRotation()
    {
        if (ZoomToken == null) return;
        var deltaAngle = DeltaAngle(ZoomToken.Angle, ref _lastAngleRotation);
        deltaAngle *= Mathf.Rad2Deg;
        Rotate?.Invoke(deltaAngle);
    }

    public void SpawnSpeedMeter(SpeedMeter speedMeterPrefab)
    {
        _speedMeter = Instantiate(speedMeterPrefab, transform.parent.parent);
        _speedMeter.Init(JoystickToken);
    }
}