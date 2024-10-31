using System;
using System.Collections;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    [SerializeField] private TuiControl _tuiControlType = TuiControl.Joystick;
    [SerializeField] private bool _invertJoystick = true;
    [SerializeField] private TuiJoystickDeadzone _deadzonePrefab;
    private Tuio20Token _magnify;
    
    private Tuio20Token _joystick;
    private Tuio20Token _zoomToken;
    
    private float _lastAngleRotation;
    private float _lastAngleZoom;
    private float _lastPanXRotation;
    private float _lastPanYRotation;

    private float _panInitialDistance;


    private float _directionFactor;

    private System.Numerics.Vector2 _joystickInitialPosition;

    private TuiJoystickDeadzone _deadzone;
    private float _deadZoneRadiusPixel;
    private float _shapeDiameterMM = 74f;

    private TuiControlBase _tuiControl;

    private void Awake()
    {
        switch (_tuiControlType)
        {
            case TuiControl.Joystick:
                _tuiControl = gameObject.AddComponent<JoystickControl>();
                break;
            case TuiControl.Car:
                _tuiControl = gameObject.AddComponent<CarControl>();
                break;
        }
    }

    public void Init(Tuio20Object magnify)
    {
        _tuiControl.Init(magnify, FocusView, Zoom, Rotate, Pan);
        InputTypeCode = $"TUI";
    }
    
    public void AddJoystick(Tuio20Object joystick)
    {
        _tuiControl.AddJoystick(joystick);
        _deadzone = Instantiate(_deadzonePrefab, transform.parent.parent);
        _deadzone.Init(_joystickInitialPosition.ToUnity());
        _deadZoneRadiusPixel = DisplayManager.Instance.GetPixelSize((_deadzone.Diameter - _shapeDiameterMM) * 0.5f);
    }

    public void AddZoomToken(Tuio20Object zoomToken)
    {
        _tuiControl.AddZoomToken(zoomToken);
    }

    public void RemoveJoystick()
    {
       _tuiControl.RemoveJoystick();
        Destroy(_deadzone.gameObject);
    }

    public void RemoveZoomToken()
    {
       _tuiControl.RemoveZoomToken();
    }

    // private void PanZoom()
    // {
    //     if (_panX == null || _panY == null) return;
    //     var currentDistance = GetPanDistance();
    //     _zoom += (currentDistance - _panInitialDistance) * 0.01f;
    //     _panInitialDistance = currentDistance;
    //     Zoom(_zoom);
    // }

    private float DeltaAngle(float currentAngle, ref float lastAngle)
    {
        var delta = currentAngle - lastAngle;
        if (delta > Mathf.PI)
        {
            delta -= 2* Mathf.PI;
        }

        if (delta < -Mathf.PI)
        {
            delta += 2 * Mathf.PI;
        }

        lastAngle = currentAngle;
        return delta;
    }
    
}