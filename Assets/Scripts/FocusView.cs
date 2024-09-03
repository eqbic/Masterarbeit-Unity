using Channels;
using TuioNet.Tuio20;
using TuioUnity.Tuio20;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class FocusView : MonoBehaviour
{
    private FocusMapControlBase _focusMapControl;
    [SerializeField] private FocusMapUI _focusMapUI;
    [SerializeField] private CircleUI _ui;
    [SerializeField] private ConnectionUI _connectionUI;

    [SerializeField] private Draggable _touchDrag;
    [SerializeField] private Tuio20TokenTransform _tokenTransform;

    [SerializeField] private FocusMapControlTouch _touchControl;
    [SerializeField] private FocusMapControlTui _tuiControl;
    
    private GeoCoordChannel _viewFinderChannel;
    private GeoCoordChannel _focusViewChannel;

    private RectTransform _rect;
    private RectTransform _mapRect;

    private void RegisterControls()
    {
        _focusMapControl.OnZoom += UpdateZoom;
        _focusMapControl.OnPan += UpdatePosition;
        _focusMapControl.OnRotate += UpdateRotation;
        _focusMapControl.OnResetView += ResetView;
    }

    // private void OnDisable()
    // {
    //     _focusMapControl.OnZoom -= UpdateZoom;
    //     _focusMapControl.OnPan -= UpdatePosition;
    //     _focusMapControl.OnRotate -= UpdateRotation;
    //     _focusMapControl.OnResetView -= ResetView;
    // }

    private void ResetView()
    {
        var coords = _viewFinderChannel.Data;
        _focusMapUI.ResetView(coords);
        _focusViewChannel.RaiseEvent(coords);
    }

    public void Init(ViewFinder viewFinder, Vector2 position)
    {
        _viewFinderChannel = viewFinder.ViewFinderChannel;
        _focusViewChannel = viewFinder.FocusViewChannel;

        _tokenTransform.UpdateRotation = false;
       
        _ui.Init(viewFinder.Color, true);
        _focusMapUI.SetupTexture(512);
        
        _rect = GetComponent<RectTransform>();
        _mapRect = _focusMapUI.GetComponent<RectTransform>();
        _rect.anchoredPosition = position;
        
        var coords = _viewFinderChannel.Data;
        _focusMapUI.Init(coords);
        _focusViewChannel.RaiseEvent(coords);
        
        _viewFinderChannel.OnChange += UpdateCoordinate;
        _connectionUI.Init(viewFinder.OffsetMarker, viewFinder.Color);
    }

    public void InitTouch()
    {
        _focusMapControl = Instantiate(_touchControl, transform);
        RegisterControls();
        Destroy(_tokenTransform);
    }

    public void InitTui(Tuio20Object tuioObject)
    {
        Destroy(_touchDrag);
        _focusMapControl = Instantiate(_tuiControl, transform);
        ((FocusMapControlTui)_focusMapControl).Init(tuioObject);
        RegisterControls();
        _tokenTransform.Initialize(tuioObject, RenderMode.ScreenSpaceCamera);
    }

    public void InitJoystick(Tuio20Object joystick)
    {
        ((FocusMapControlTui)_focusMapControl).AddJoystick(joystick);
    }

    private void OnDestroy()
    {
        _viewFinderChannel.OnChange -= UpdateCoordinate;
    }

    private void UpdateRotation(float deltaDegree)
    {
        _mapRect.Rotate(Vector3.forward, deltaDegree);
    }

    private void UpdatePosition(Vector2 delta)
    {
        var coords = _focusMapUI.Move(delta);
        _focusViewChannel.RaiseEvent(coords);
    }

    private void UpdateZoom(float zoom)
    {
        _focusMapUI.UpdateZoom(zoom);
    }

    private void UpdateCoordinate(GeoCoord geoCoord)
    {
        _focusMapUI.UpdateCoords(geoCoord);
        _focusViewChannel.RaiseEvent(geoCoord);
    }

    public void RemoveJoystick()
    {
        ((FocusMapControlTui)_focusMapControl).RemoveJoystick();
    }

    public void AddPanX(Tuio20Object panX)
    {
        ((FocusMapControlTui)_focusMapControl).AddPanX(panX);
    }

    public void RemovePanX()
    {
        ((FocusMapControlTui)_focusMapControl).RemovePanX();
    }
    
    public void RemovePanY()
    {
        ((FocusMapControlTui)_focusMapControl).RemovePanY();
    }
    
    public void AddPanY(Tuio20Object panY)
    {
        ((FocusMapControlTui)_focusMapControl).AddPanY(panY);
    }
}