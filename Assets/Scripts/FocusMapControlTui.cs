﻿using System;
using System.Collections;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    [SerializeField] private TuiControl _tuiControlType = TuiControl.Joystick;
    [SerializeField] private bool _invertJoystick = true;
    [SerializeField] private TuiJoystickDeadzone _deadzonePrefab;

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
        InputTypeCode = $"TUI_{_tuiControlType.ToString()}";
    }
    
    public void AddJoystick(Tuio20Object joystick)
    {
        _tuiControl.AddJoystick(joystick);
        if (_tuiControlType == TuiControl.Joystick)
        {
            ((JoystickControl)_tuiControl).SpawnDeadzone(_deadzonePrefab);
        }
    }

    public void AddZoomToken(Tuio20Object zoomToken)
    {
        _tuiControl.AddZoomToken(zoomToken);
    }

    public void RemoveJoystick()
    {
       _tuiControl.RemoveJoystick();
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