using System;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class JoystickControl : TuiControlBase
{
    private const bool InvertJoystick = true;
    private float _lastAngleRotation;
    private float _directionFactor;
    private System.Numerics.Vector2 _joystickInitialPosition;
    private float _lastAngleZoom;
    private float _deadZoneRadiusPixel;
    private TuiJoystickDeadzone _deadzone;
    private float _shapeDiameterMM = 74f;
    private const float e = 2.71828f;

    public override void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate, Action<Vector2> pan)
    {
        base.Init(magnify, focusView, zoom, rotate, pan);
        ZoomSpeed = 2f;
        _lastAngleRotation = Magnify.Angle;
        _directionFactor = InvertJoystick ? -1f : 1f;
    }

    public override void AddJoystick(Tuio20Object joystick)
    {
        JoystickToken = joystick.Token;
        _joystickInitialPosition = JoystickToken.Position;
        _lastAngleZoom = JoystickToken.Angle;
    }

    public void SpawnDeadzone(TuiJoystickDeadzone deadzonePrefab)
    {
        _deadzone = Instantiate(deadzonePrefab, transform.parent.parent);
        _deadzone.Init(_joystickInitialPosition.ToUnity());
        _deadZoneRadiusPixel = DisplayManager.Instance.GetPixelSize((_deadzone.Diameter - _shapeDiameterMM) * 0.5f);
    }

    public override void AddZoomToken(Tuio20Object zoomToken)
    {
        ZoomToken = zoomToken.Token;
        _lastAngleZoom = ZoomToken.Angle;
    }

    public override void RemoveJoystick()
    {
        JoystickToken = null;
        Destroy(_deadzone.gameObject);
    }

    public override void RemoveZoomToken()
    {
        if(JoystickToken != null)
            _lastAngleZoom = JoystickToken.Angle;
        ZoomToken = null;
    }

    protected override void UpdateZoom()
    {
        // PanZoom();
        if (ZoomToken == null)
        {
            JoystickZoom();
        }
        else
        {
            TokenZoom();
        }
    }
    
    private void JoystickZoom()
    {
        if(JoystickToken == null) return;
        ZoomByAngle(JoystickToken.Angle);
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

    protected override void UpdatePan()
    {
        if (JoystickToken == null) return;
        
        var v = (JoystickToken.Position - _joystickInitialPosition).ToUnity();
        v.x *= Screen.width;
        v.y *= -Screen.height;
        var distance = Mathf.Max(v.magnitude - _deadZoneRadiusPixel, 0f);
        // var speed = 600f * Mathf.Log(0.015f * distance + 1f);
        var speed = 800f / (1 + Mathf.Pow(e, -0.04f * (distance - 120)));
        // var speed = Mathf.Pow(5f / 100 * distance + 0.8f, 3) + 5f;
        print($"distance: {distance} -> speed: {speed}");
        if (distance > 0f)
        {
            Pan?.Invoke(v.normalized * (_directionFactor * (speed * 1f * Time.deltaTime)));
        }
    }
}