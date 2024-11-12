using Channels;
using TuioNet.Tuio20;
using TuioUnity.Tuio20;
using UI;
using UnityEngine;

public class FocusView : ViewBase
{
    private FocusMapControlBase _focusMapControl;
    [SerializeField] private FocusMapUI _focusMapUI;
    [SerializeField] private OutlineUI _ui;
    [SerializeField] private ConnectionUI _connectionUI;

    [SerializeField] private Draggable _touchDrag;
    [SerializeField] private Tuio20TokenTransform _tokenTransform;

    [SerializeField] private FocusMapControlTouch _touchControl;
    [SerializeField] private FocusMapControlTui _tuiControl;

    [SerializeField] private RecordActivator _recordActivator;
    [field:SerializeField] public UserData CurrentUser { get; private set; }
    
    private GeoCoordChannel _viewFinderChannel;
    private GeoCoordChannel _focusViewChannel;

    public GeoCoordChannel FocusViewChannel => _focusViewChannel;
    public GeoCoordChannel ViewFinderChannel => _viewFinderChannel;

    private RectTransform _rect;
    private RectTransform _mapRect;

    private float _defaultZoom = 17f;
    public float CurrentZoom { get; private set; } = 17f;
    public float MinZoom { get; } = 8f;
    public float MaxZoom { get; } = 21f;

    public FocusMapControlBase FocusMapControl => _focusMapControl;

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
       
        _ui.Init(viewFinder.Color);
        _focusMapUI.SetupTexture(512);
        
        _rect = GetComponent<RectTransform>();
        _mapRect = _focusMapUI.GetComponent<RectTransform>();
        _rect.anchoredPosition = position;
        
        var coords = _viewFinderChannel.Data;
        coords.Elevation = CurrentZoom;
        _focusMapUI.Init(coords);
        _focusViewChannel.RaiseEvent(coords);
        
        _viewFinderChannel.OnChange += UpdateCoordinate;
        _connectionUI.Init(viewFinder.OffsetMarker, viewFinder.Color);
        Loaded(_focusMapUI.Map);
        
        _recordActivator.Init(_focusViewChannel);
    }

    public void InitTouch()
    {
        _focusMapControl = Instantiate(_touchControl, transform);
        _focusMapControl.Init(this);
        _focusMapControl.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        RegisterControls();
        Destroy(_tokenTransform);
    }

    public void InitTui(Tuio20Object tuioObject)
    {
        Destroy(_touchDrag);
        _focusMapControl = Instantiate(_tuiControl, transform);
        _focusMapControl.Init(this);
        _focusMapControl.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        ((FocusMapControlTui)_focusMapControl).Init(tuioObject);
        RegisterControls();
        _tokenTransform.Initialize(tuioObject, RenderMode.ScreenSpaceCamera);
    }

    public void InitJoystick(Tuio20Object joystick)
    {
        ((FocusMapControlTui)_focusMapControl).AddJoystick(joystick);
    }

    public void InitZoomToken(Tuio20Object zoom)
    {
        ((FocusMapControlTui)_focusMapControl).AddZoomToken(zoom);
    }

    private void OnDestroy()
    {
        _viewFinderChannel.OnChange -= UpdateCoordinate;
        Destroy(_focusMapControl);
    }

    private void UpdateRotation(float deltaDegree)
    {
        _mapRect.Rotate(Vector3.forward, deltaDegree);
    }

    private void UpdatePosition(Vector2 delta)
    {
        var coords = _focusMapUI.Move(delta);
        coords.Elevation = CurrentZoom;
        _focusViewChannel.RaiseEvent(coords);
    }

    private void UpdateZoom(float zoom)
    {
        CurrentZoom = zoom;
        _focusMapUI.UpdateZoom(zoom);
    }

    private void UpdateCoordinate(GeoCoord geoCoord)
    {
        geoCoord.Elevation = CurrentZoom;
        _focusMapUI.UpdateCoords(geoCoord);
        _focusViewChannel.RaiseEvent(geoCoord);
    }

    public void RemoveJoystick()
    {
        ((FocusMapControlTui)_focusMapControl).RemoveJoystick();
    }
    
    public void RemoveZoomToken()
    {
        ((FocusMapControlTui)_focusMapControl).RemoveZoomToken();
    }

    // public void AddPanX(Tuio20Object panX)
    // {
    //     ((FocusMapControlTui)_focusMapControl).AddPanX(panX);
    // }
    //
    // public void RemovePanX()
    // {
    //     ((FocusMapControlTui)_focusMapControl).RemovePanX();
    // }
    //
    // public void RemovePanY()
    // {
    //     ((FocusMapControlTui)_focusMapControl).RemovePanY();
    // }
    //
    // public void AddPanY(Tuio20Object panY)
    // {
    //     ((FocusMapControlTui)_focusMapControl).AddPanY(panY);
    // }

}