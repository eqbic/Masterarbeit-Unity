using System;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class FocusMapControlTouch : FocusMapControlBase
{
    [SerializeField] private ScreenTransformGesture _zoomGesture;
    [SerializeField] private ScreenTransformGesture _panGesture;
    [SerializeField] private ScreenTransformGesture _rotateGesture;
    [SerializeField] private TapGesture _resetTap;
    [SerializeField] private Joystick _joystickPrefab;


    [SerializeField] private float _zoomSpeed = 2.0f;
    
    private float _zoom = 17f;
    private Joystick _joystick;

    private void Awake()
    {
        _joystick = Instantiate(_joystickPrefab, transform.parent.parent);
        _joystick.Init(transform as RectTransform);
    }

    private void OnEnable()
    {
        _zoomGesture.Transformed += Zoom;
        _panGesture.Transformed += Pan;
        _rotateGesture.Transformed += Rotate;
        _resetTap.Tapped += ResetView;
        _joystick.OnMove += Pan;
    }

    private void Zoom(object sender, EventArgs e)
    {
        _zoom += (_zoomGesture.DeltaScale - 1f) * _zoomSpeed;
        Zoom(_zoom);
    }

    private void Pan(object sender, EventArgs e)
    {
        Pan(_panGesture.DeltaPosition);
    }

    private void Rotate(object sender, EventArgs e)
    {
        Rotate(_rotateGesture.DeltaRotation);
    }

    private void ResetView(object sender, EventArgs e)
    {
        ResetView();
    }

    private void OnDisable()
    {
        _zoomGesture.Transformed -= Zoom;
        _panGesture.Transformed -= Pan;
        _rotateGesture.Transformed -= Rotate;
        _resetTap.Tapped -= ResetView;
        _joystick.OnMove -= Pan;
    }
}