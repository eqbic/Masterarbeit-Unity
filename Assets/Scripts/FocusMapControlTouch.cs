using System;
using Extensions;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.Serialization;

public class FocusMapControlTouch : FocusMapControlBase
{
    [SerializeField] private ScreenTransformGesture _zoomGesture;
    [SerializeField] private ScreenTransformGesture _panGesture;
    [SerializeField] private ScreenTransformGesture _rotateGesture;
    [SerializeField] private TapGesture _resetTap;
    [SerializeField] private Joystick _joystickPrefab;
    [SerializeField] private ZoomSlider _zoomSliderPrefab;


    [SerializeField] private float _zoomSpeed = 2.0f;
    
    private float _zoom = 17f;
    private Joystick _joystick;
    private ZoomSlider _zoomSlider;

    private float _minZoom = 8f;
    private float _maxZoom = 21f;

    private void Awake()
    {
        _joystick = Instantiate(_joystickPrefab, transform.parent.parent);
        _joystick.Init(transform.parent as RectTransform);

        _zoomSlider = Instantiate(_zoomSliderPrefab, transform.parent.parent);
        _zoomSlider.Init(transform.parent as RectTransform, _joystick, _zoom);
        _joystick.transform.SetAsLastSibling();
    }

    private void OnEnable()
    {
        _zoomGesture.Transformed += Zoom;
        _panGesture.Transformed += Pan;
        _rotateGesture.Transformed += Rotate;
        _resetTap.Tapped += ResetView;
        _joystick.OnMove += Pan;
        _zoomSlider.OnZoom += SliderZoom;
    }

    private void SliderZoom(float normalizedZoom)
    {
        var zoom = Mathf.Lerp(_minZoom, _maxZoom, normalizedZoom);
        Zoom(zoom);
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
        _zoomSlider.OnZoom -= SliderZoom;
    }
}