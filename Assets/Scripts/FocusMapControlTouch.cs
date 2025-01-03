﻿using System;
using Extensions;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class FocusMapControlTouch : FocusMapControlBase
{
    [SerializeField] private TouchControl _controlType;
    [SerializeField] private ScreenTransformGesture _zoomGesture;
    [SerializeField] private ScreenTransformGesture _panGesture;
    [SerializeField] private ScreenTransformGesture _rotateGesture;
    [SerializeField] private TapGesture _resetTap;
    [SerializeField] private Joystick _joystickPrefab;
    [SerializeField] private ZoomSlider _zoomSliderPrefab;


    [SerializeField] private float _zoomSpeed = 2.0f;
    
    // private float _zoom = 17f; 
    private Joystick _joystick;
    private ZoomSlider _zoomSlider;

    // private float _minZoom = 8f;
    // private float _maxZoom = 21f;

    public override void Init(FocusView focusView)
    {
        base.Init(focusView);
        _controlType = focusView.CurrentUser.TouchControl;
        switch (_controlType)
        {
            case TouchControl.Gesture:
                RegisterGestures();
                break;
            case TouchControl.Joystick:
                CreateJoystick();
                RegisterJoystick();
                break;
        }

        _resetTap.Tapped += ResetView;
    }

    private void Start()
    {
        InputTypeCode = $"Touch_{_controlType.ToString()}";
    }

    private void CreateJoystick()
    {
        _joystick = Instantiate(_joystickPrefab, transform.parent.parent);
        _joystick.Init(transform.parent as RectTransform);

        _zoomSlider = Instantiate(_zoomSliderPrefab, transform.parent.parent);
        var normalizedZoom = FocusView.CurrentZoom.Remap(8, 21, 0, 1);
        _zoomSlider.Init(transform.parent as RectTransform, _joystick, normalizedZoom);
        _joystick.transform.SetAsLastSibling();
    }

    private void RegisterJoystick()
    {
        _joystick.OnMove += Pan;
        _zoomSlider.OnZoom += SliderZoom;
        _joystick.OnRotate += Rotate;
    }

    private void RegisterGestures()
    {
        _zoomGesture.Transformed += Zoom;
        _panGesture.Transformed += Pan;
        _rotateGesture.Transformed += Rotate;
    }

    private void SliderZoom(float normalizedZoom)
    {
        var zoom = Mathf.Lerp(FocusView.MinZoom, FocusView.MaxZoom, normalizedZoom);
        Zoom(zoom);
    }

    private void Zoom(object sender, EventArgs e)
    {
        var zoom = FocusView.CurrentZoom + (_zoomGesture.DeltaScale - 1f) * _zoomSpeed;
        Zoom(zoom);
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

    private void UnregisterJoystick()
    {
        _joystick.OnMove -= Pan;
        _zoomSlider.OnZoom -= SliderZoom;
        _joystick.OnRotate -= Rotate;
    }

    private void UnregisterGestures()
    {
        _zoomGesture.Transformed -= Zoom;
        _panGesture.Transformed -= Pan;
        _rotateGesture.Transformed -= Rotate;
    }

    private void OnDestroy()
    {
        switch (_controlType)
        {
            case TouchControl.Gesture:
                UnregisterGestures();
                break;
            case TouchControl.Joystick:
                UnregisterJoystick();
                Destroy(_joystick.gameObject);
                Destroy(_zoomSlider.gameObject);
                break;
        }
        _resetTap.Tapped -= ResetView;
    }
}