using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.Serialization;

public class FocusMapControl : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _zoomGesture;
    [SerializeField] private ScreenTransformGesture _panGesture;
    // [SerializeField] private ScreenTransformGesture _rotateGesture;

    private float _zoom = 17f;

    public event Action<float> OnZoom;
    public event Action<Vector2> OnPan;
    public event Action<float> OnRotate; 

    private void OnEnable()
    {
        _zoomGesture.Transformed += Zoom;
        _panGesture.Transformed += Pan;
    }

    private void OnDisable()
    {
        _zoomGesture.Transformed -= Zoom;
        _panGesture.Transformed -= Pan;
    }

    private void Pan(object sender, EventArgs e)
    {
        var delta = new Vector2(_panGesture.DeltaPosition.x, _panGesture.DeltaPosition.y);
        OnPan?.Invoke(delta);
    }

    private void Zoom(object sender, EventArgs e)
    {
        _zoom *= _zoomGesture.DeltaScale;
        OnZoom?.Invoke(_zoom);
    }
}