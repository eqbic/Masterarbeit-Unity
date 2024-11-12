using TuioNet.Tuio20;
using UnityEngine;

public class FocusMapControlTui : FocusMapControlBase
{
    [SerializeField] private TuiControl _tuiControlType = TuiControl.Joystick;
    [SerializeField] private TuiJoystickDeadzone _deadzonePrefab;
    [SerializeField] private SpeedMeter _speedMeterPrefab;

    private TuiControlBase _tuiControl;

    public void Init(Tuio20Object magnify)
    {
        _tuiControl.Init(magnify, FocusView, Zoom, Rotate, Pan);
        InputTypeCode = $"TUI_{_tuiControlType.ToString()}";
    }

    public override void Init(FocusView focusView)
    {
        base.Init(focusView);
        _tuiControlType = focusView.CurrentUser.TuioControl;
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

    public void AddJoystick(Tuio20Object joystick)
    {
        _tuiControl.AddJoystick(joystick);
        if (_tuiControlType == TuiControl.Joystick)
        {
            ((JoystickControl)_tuiControl).SpawnDeadzone(_deadzonePrefab);
        }

        if (_tuiControlType == TuiControl.Car)
        {
            ((CarControl)_tuiControl).SpawnSpeedMeter(_speedMeterPrefab);
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
}