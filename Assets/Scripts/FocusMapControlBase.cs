using System;
using UnityEngine;

public abstract class FocusMapControlBase : MonoBehaviour
{
    public event Action<float> OnZoom;
    public event Action<Vector2> OnPan;
    public event Action<float> OnRotate;
    public event Action OnResetView;

    protected FocusView FocusView;
    public string InputTypeCode { get; protected set; }

    public virtual void Init(FocusView focusView)
    {
        FocusView = focusView;
    }
    
    protected void ResetView()
    {
        OnResetView?.Invoke();
    }
    
    protected void Rotate(float deltaRotation)
    {
        OnRotate?.Invoke(deltaRotation);
    }

    protected void Pan(Vector2 deltaPosition)
    {
        OnPan?.Invoke(deltaPosition);
    }

    protected void Zoom(float zoom)
    {
        OnZoom?.Invoke(zoom);
    }
}