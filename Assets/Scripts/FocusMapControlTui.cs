using System.Collections;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    
    [SerializeField] private bool _invertJoystick = true;
    private Tuio20Token _magnify;
    private Tuio20Token _joystick;
    private Tuio20Token _panX;
    private Tuio20Token _panY;

    private float _zoom = 17f;
    private float _zoomSpeed = 2f;

    private float _lastAngleRotation;
    private float _lastAngleZoom;
    private float _lastPanXRotation;
    private float _lastPanYRotation;

    private float _panInitialDistance;

    private float _targetZoom;

    private float _directionFactor;

    private System.Numerics.Vector2 _joystickInitialPosition;

    public void Init(Tuio20Object magnify)
    {
        _magnify = magnify.Token;
        _lastAngleRotation = _magnify.Angle;
        _targetZoom = _zoom;
        _directionFactor = _invertJoystick ? -1f : 1f;
    }

    public void AddJoystick(Tuio20Object joystick)
    {
        _joystick = joystick.Token;
        _joystickInitialPosition = _joystick.Position;
        _lastAngleZoom = _joystick.Angle;
    }

    public void RemoveJoystick()
    {
        _joystick = null;
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

    private void PanControl()
    {
        if (_panX == null || _panY == null) return;
        var deltaX = DeltaAngle(_panX.Angle, ref _lastPanXRotation);
        var deltaY = DeltaAngle(_panY.Angle, ref _lastPanYRotation);
        var direction = new Vector2(deltaX, deltaY) * 100f;
        Pan(direction);
    }

    private void JoystickControl()
    {
        if (_joystick == null) return;
        
        var v = (_joystick.Position - _joystickInitialPosition).ToUnity();
        v.x *= Screen.width;
        v.y *= -Screen.height;
        var speed = v.magnitude;
        if (speed > 0f)
        {
            Pan(v.normalized * (_directionFactor * (speed * 1f * Time.deltaTime)));
        }
    }

    private void MagnifyZoom()
    {
        var deltaAngle = DeltaAngle(_magnify.Angle, ref _lastAngleZoom);
        if (deltaAngle == 0f) return;
        _zoom += (deltaAngle) * _zoomSpeed;
        Zoom(_zoom);
    }

    private void JoystickZoom()
    {
        if(_joystick == null) return;
        var deltaAngle = DeltaAngle(_joystick.Angle, ref _lastAngleZoom);
        if(Mathf.Abs(deltaAngle) < 0.015f) return;
        _zoom += (deltaAngle) * _zoomSpeed;
        Zoom(_zoom);
    }

    private void PanZoom()
    {
        if (_panX == null || _panY == null) return;
        var currentDistance = GetPanDistance();
        _zoom += (currentDistance - _panInitialDistance) * 0.01f;
        _panInitialDistance = currentDistance;
        Zoom(_zoom);
    }

    private void UpdateZoom()
    {
        // PanZoom();
        JoystickZoom();
        // MagnifyZoom();
    }

    private IEnumerator AnimateZoom()
    {
        float alpha = 0f;
        float startZoom = _zoom;
        while (alpha < 1f)
        {
            _zoom = Mathf.Lerp(startZoom, _targetZoom, alpha);
            alpha += 10f * Time.deltaTime;
            Zoom(_zoom);
            yield return null;
        }
    }

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

    public void AddPanX(Tuio20Object panX)
    {
        _panX = panX.Token;
        _lastPanXRotation = _panX.Angle;
        if (_panY == null) return;
        _panInitialDistance = GetPanDistance();
    }

    public void RemovePanX()
    {
        _panX = null;
    }

    public void RemovePanY()
    {
        _panY = null;
    }

    private float GetPanDistance()
    {
        return (TuioUtils.ToScreenPoint(_panX.Position) - TuioUtils.ToScreenPoint(_panY.Position)).magnitude;
    }
    
    public void AddPanY(Tuio20Object panY)
    {
        _panY = panY.Token;
        _lastPanYRotation = _panY.Angle;
        if (_panX == null) return;
        _panInitialDistance = GetPanDistance();
    }
}