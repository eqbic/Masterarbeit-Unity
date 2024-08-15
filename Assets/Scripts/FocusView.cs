using Channels;
using UI;
using UnityEngine;

public class FocusView : MonoBehaviour
{
    [SerializeField] private FocusMapControl _focusMapControl;
    [SerializeField] private FocusMapUI _focusMapUI;
    [SerializeField] private CircleUI _ui;
    
    private GeoCoordChannel _viewFinderChannel;
    private GeoCoordChannel _focusViewChannel;

    private RectTransform _rect;

    private void OnEnable()
    {
        _focusMapControl.OnZoom += UpdateZoom;
        _focusMapControl.OnPan += UpdatePosition;
        _focusMapControl.OnRotate += UpdateRotation;
        _focusMapControl.OnResetView += ResetView;
    }

    private void OnDisable()
    {
        _focusMapControl.OnZoom -= UpdateZoom;
        _focusMapControl.OnPan -= UpdatePosition;
        _focusMapControl.OnRotate -= UpdateRotation;
        _focusMapControl.OnResetView -= ResetView;
    }

    private void ResetView()
    {
        var coords = _viewFinderChannel.Data;
        _focusMapUI.ResetView(coords);
        _focusViewChannel.RaiseEvent(coords);
    }

    public void Init(GeoCoordChannel viewFinderChannel, GeoCoordChannel focusViewChannel, Vector2 position, Color color)
    {
        _viewFinderChannel = viewFinderChannel;
        _focusViewChannel = focusViewChannel;
       
        _ui.Init(color, true);
        _focusMapUI.SetupTexture(512);
        
        _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = position;
        
        var coords = viewFinderChannel.Data;
        _focusMapUI.Init(coords);
        _focusViewChannel.RaiseEvent(coords);
        
        _viewFinderChannel.OnChange += UpdateCoordinate;
    }

    private void OnDestroy()
    {
        _viewFinderChannel.OnChange -= UpdateCoordinate;
    }

    private void UpdateRotation(float deltaDegree)
    {
        _rect.Rotate(Vector3.forward, deltaDegree);
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
}