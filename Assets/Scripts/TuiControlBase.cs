using System;
using TuioNet.Tuio20;
using UnityEngine;

public abstract class TuiControlBase : MonoBehaviour
{
    protected Tuio20Token Magnify;
    protected Tuio20Token JoystickToken;
    protected Tuio20Token ZoomToken;
    protected FocusView FocusView;
    protected float ZoomSpeed;
    
    protected Action<float> Zoom;
    protected Action<float> Rotate;
    protected Action<Vector2> Pan;


    public virtual void Init(Tuio20Object magnify, FocusView focusView, Action<float> zoom, Action<float> rotate,
        Action<Vector2> pan)
    {
        Magnify = magnify.Token;
        FocusView = focusView;
        Zoom = zoom;
        Rotate = rotate;
        Pan = pan;
    }
    
    public abstract void AddJoystick(Tuio20Object joystick);
    public abstract void AddZoomToken(Tuio20Object zoomToken);

    public abstract void RemoveJoystick();
    public abstract void RemoveZoomToken();

    protected abstract void UpdateZoom();
    protected abstract void UpdateRotation();
    protected abstract void UpdatePan();

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