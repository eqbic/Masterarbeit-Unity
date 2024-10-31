using System.Collections;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    [SerializeField] private TuiControl _tuiControl;
    [SerializeField] private bool _invertJoystick = true;
    [SerializeField] private TuiJoystickDeadzone _deadzonePrefab;
    private Tuio20Token _magnify;
    
    private Tuio20Token _joystick;
    private Tuio20Token _zoomToken;
    
    // private Tuio20Token _panX;
    // private Tuio20Token _panY;

    private readonly float _zoomSpeed = 2f;
    private const float e = 2.71828f;

    private float _lastAngleRotation;
    private float _lastAngleZoom;
    private float _lastPanXRotation;
    private float _lastPanYRotation;

    private float _panInitialDistance;

    private float _targetZoom;

    private float _directionFactor;

    private System.Numerics.Vector2 _joystickInitialPosition;

    private TuiJoystickDeadzone _deadzone;
    private float _deadZoneRadiusPixel;
    private float _shapeDiameterMM = 74f;

    public void Init(Tuio20Object magnify)
    {
        _magnify = magnify.Token;
        _lastAngleRotation = _magnify.Angle;
        _targetZoom = FocusView.CurrentZoom;
        _directionFactor = _invertJoystick ? -1f : 1f;
        InputTypeCode = $"TUI";
    }

    public void AddJoystick(Tuio20Object joystick)
    {
        _joystick = joystick.Token;
        _joystickInitialPosition = _joystick.Position;
        _lastAngleZoom = _joystick.Angle;
        _deadzone = Instantiate(_deadzonePrefab, transform.parent.parent);
        _deadzone.Init(_joystickInitialPosition.ToUnity());
        _deadZoneRadiusPixel = DisplayManager.Instance.GetPixelSize((_deadzone.Diameter - _shapeDiameterMM) * 0.5f);
    }

    public void AddZoomToken(Tuio20Object zoom)
    {
        _zoomToken = zoom.Token;
        _lastAngleZoom = _zoomToken.Angle;
    }

    public void RemoveJoystick()
    {
        _joystick = null;
        Destroy(_deadzone.gameObject);
    }

    public void RemoveZoomToken()
    {
        if(_joystick != null)
            _lastAngleZoom = _joystick.Angle;
        _zoomToken = null;
    }

    private void Update()
    {
        UpdateZoom();
        UpdateRotation();
        UpdatePan();
    }

    private void UpdateRotation()
    {
        var deltaAngle = DeltaAngle(_magnify.Angle, ref _lastAngleRotation);
        deltaAngle = -deltaAngle * 180f / Mathf.PI;
        Rotate(deltaAngle);
    }

    private void UpdatePan()
    {
        JoystickControl();
        // PanControl();
    }

    // private void PanControl()
    // {
    //     if (_panX == null || _panY == null) return;
    //     var deltaX = DeltaAngle(_panX.Angle, ref _lastPanXRotation);
    //     var deltaY = DeltaAngle(_panY.Angle, ref _lastPanYRotation);
    //     var direction = new Vector2(deltaX, deltaY) * 100f;
    //     Pan(direction);
    // }

    private void JoystickControl()
    {
        if (_joystick == null) return;
        
        var v = (_joystick.Position - _joystickInitialPosition).ToUnity();
        v.x *= Screen.width;
        v.y *= -Screen.height;
        var distance = Mathf.Max(v.magnitude - _deadZoneRadiusPixel, 0f);
        // var speed = 600f * Mathf.Log(0.015f * distance + 1f);
        var speed = 800f / (1 + Mathf.Pow(e, -0.04f * (distance - 120)));
        // var speed = Mathf.Pow(5f / 100 * distance + 0.8f, 3) + 5f;
        print($"distance: {distance} -> speed: {speed}");
        if (distance > 0f)
        {
            Pan(v.normalized * (_directionFactor * (speed * 1f * Time.deltaTime)));
        }
    }

    private void MagnifyZoom()
    {
        ZoomByAngle(_magnify.Angle);
    }

    private void ZoomByAngle(float angle)
    {
        var deltaAngle = DeltaAngle(angle, ref _lastAngleZoom);
        if(Mathf.Abs(deltaAngle) < 0.015f) return;
        var zoom = Mathf.Clamp(FocusView.CurrentZoom + (deltaAngle) * _zoomSpeed, FocusView.MinZoom, FocusView.MaxZoom);
        Zoom(zoom);
    }

    private void JoystickZoom()
    {
        if(_joystick == null) return;
        ZoomByAngle(_joystick.Angle);
    }

    // private void PanZoom()
    // {
    //     if (_panX == null || _panY == null) return;
    //     var currentDistance = GetPanDistance();
    //     _zoom += (currentDistance - _panInitialDistance) * 0.01f;
    //     _panInitialDistance = currentDistance;
    //     Zoom(_zoom);
    // }

    private void UpdateZoom()
    {
        // PanZoom();
        if (_zoomToken == null)
        {
            JoystickZoom();
        }
        else
        {
            TokenZoom();
        }
        // MagnifyZoom();
    }

    private void TokenZoom()
    {
        ZoomByAngle(_zoomToken.Angle);
    }

    // private IEnumerator AnimateZoom()
    // {
    //     float alpha = 0f;
    //     float startZoom = _zoom;
    //     while (alpha < 1f)
    //     {
    //         _zoom = Mathf.Lerp(startZoom, _targetZoom, alpha);
    //         alpha += 10f * Time.deltaTime;
    //         Zoom(_zoom);
    //         yield return null;
    //     }
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

    // public void AddPanX(Tuio20Object panX)
    // {
    //     _panX = panX.Token;
    //     _lastPanXRotation = _panX.Angle;
    //     if (_panY == null) return;
    //     _panInitialDistance = GetPanDistance();
    // }

    // public void RemovePanX()
    // {
    //     _panX = null;
    // }
    //
    // public void RemovePanY()
    // {
    //     _panY = null;
    // }

    // private float GetPanDistance()
    // {
    //     return (TuioUtils.ToScreenPoint(_panX.Position) - TuioUtils.ToScreenPoint(_panY.Position)).magnitude;
    // }
    //
    // public void AddPanY(Tuio20Object panY)
    // {
    //     _panY = panY.Token;
    //     _lastPanYRotation = _panY.Angle;
    //     if (_panX == null) return;
    //     _panInitialDistance = GetPanDistance();
    // }
}