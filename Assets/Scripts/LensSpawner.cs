using System.Collections.Generic;
using TuioNet.Tuio20;
using UnityEngine;

public class LensSpawner : MonoBehaviour
{
    [SerializeField] private OnlineMaps _contextViewMap;
    [SerializeField] private ViewFinder _viewFinderPrefab;
    [SerializeField] private FocusView _focusViewPrefab;
    [SerializeField] private RectTransform _viewParent;

    private readonly Dictionary<uint, ViewFinder> _viewFinders = new();
    private readonly Dictionary<uint, FocusView> _focusViews = new();

    public ViewFinder SpawnViewFinder(uint id, Vector2 screenPosition, InputType inputType, Tuio20Object tuioObject = null)
    {
        var finder = Instantiate(_viewFinderPrefab, _viewParent);
        finder.Init(id, screenPosition, _contextViewMap);
        switch (inputType)
        {
            case InputType.Touch:
                finder.InitTouch();
                break;
            case InputType.Tui:
                finder.InitTui(tuioObject);
                break;
        }
        _viewFinders.Add(id, finder);
        return finder;
    }

    public void DestroyViewFinder(uint id)
    {
        if (_viewFinders.Remove(id, out var finder))
        {
            Destroy(finder.gameObject);
        }
    }

    public FocusView SpawnFocusView(uint id, Vector2 position, ViewFinder viewFinder, InputType inputType, Tuio20Object tuioObject = null)
    {
        var focus = Instantiate(_focusViewPrefab, viewFinder.RectTransform.parent);
        focus.Init(viewFinder, position);
        switch (inputType)
        {
            case InputType.Touch:
                focus.InitTouch();
                break;
            case InputType.Tui:
                focus.InitTui(tuioObject);
                break;
        }
        _focusViews.Add(id, focus);
        return focus;
    }

    public void AddJoystick(uint focusId, Tuio20Object joystick)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.InitJoystick(joystick);
        }
    }

    public void DestroyFocusView(uint id)
    {
        if (_focusViews.Remove(id, out var focus))
        {
            Destroy(focus.gameObject);
        }
    }

    public void RemoveJoystick(uint focusId)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.RemoveJoystick();
        }
    }

    public void AddPanX(uint focusId, Tuio20Object panX)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.AddPanX(panX);
        }
    }
    
    public void RemovePanX(uint focusId)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.RemovePanX();
        }
    }
    
    public void RemovePanY(uint focusId)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.RemovePanY();
        }
    }

    public void AddPanY(uint focusId, Tuio20Object panY)
    {
        if (_focusViews.TryGetValue(focusId, out var focusView))
        {
            focusView.AddPanY(panY);
        }
    }
}