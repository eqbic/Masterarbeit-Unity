using System;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public abstract class TuiControlBase : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 400f;
    private float _directionFactor = -1f;
    protected Tuio20Token Magnify;
    protected Tuio20Token JoystickToken;
    protected Tuio20Token ZoomToken;
    protected FocusView FocusView;
    protected float ZoomSpeed;
    
    protected Action<float> Zoom;
    protected Action<float> Rotate;
    protected Action<Vector2> Pan;
    private TuiJoystickDeadzone _deadzone;
    private System.Numerics.Vector2 _joystickInitialPosition;
    private const float e = 2.71828f;
    private float _deadZoneRadiusPixel;
    private const float _shapeDiameterMM = 74f;


    public virtual void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate,
        Action<Vector2> pan)
    {
        ZoomSpeed = 2f;
        Magnify = magnify.Token;
        FocusView = focusView;
        Zoom = zoom;
        Rotate = rotate;
        Pan = pan;
    }

    public void AddJoystick(Tuio20Object joystick)
    {
        JoystickToken = joystick.Token;
        _joystickInitialPosition = JoystickToken.Position;
    }
    
    public abstract void AddZoomToken(Tuio20Object zoomToken);

    public void RemoveJoystick()
    {
        JoystickToken = null;
        Destroy(_deadzone.gameObject);
    }

    public void RemoveZoomToken()
    {
        ZoomToken = null;
    }

    protected abstract void UpdateZoom();
    protected abstract void UpdateRotation();
    
    public void SpawnDeadzone(TuiJoystickDeadzone deadzonePrefab)
    {
        _deadzone = Instantiate(deadzonePrefab, transform.parent.parent);
        _deadzone.Init(_joystickInitialPosition.ToUnity());
        _deadZoneRadiusPixel = DisplayManager.Instance.GetPixelSize((_deadzone.Diameter - _shapeDiameterMM) * 0.5f);
    }

    protected void UpdatePan()
    {
        if (JoystickToken == null) return;
        
        var v = (JoystickToken.Position - _joystickInitialPosition).ToUnity();
        v.x *= Screen.width;
        v.y *= -Screen.height;
        var distance = Mathf.Max(v.magnitude - _deadZoneRadiusPixel, 0f);
        // var speed = 600f * Mathf.Log(0.015f * distance + 1f);
        var speed = _maxSpeed / (1 + Mathf.Pow(e, -0.04f * (distance - 120)));
        // var speed = Mathf.Pow(5f / 100 * distance + 0.8f, 3) + 5f;
        // print($"distance: {distance} -> speed: {speed}");
        var direction = v.normalized * (_directionFactor * (speed * 1f * Time.deltaTime));
        if (distance > 0f)
        {
            Pan?.Invoke(direction);
        }
    }

    private void Update()
    {
        UpdateZoom();
        UpdateRotation();
        UpdatePan();
    }
    
    protected float DeltaAngle(float currentAngle, ref float lastAngle)
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