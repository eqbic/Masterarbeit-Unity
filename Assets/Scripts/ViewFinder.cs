using System;
using Channels;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ViewFinder : MonoBehaviour
{
    [SerializeField] private RectTransform _offsetMarker;
    [SerializeField] private Draggable _draggable;
    [SerializeField] private FocusViewSpawner _focusViewSpawner;
    [SerializeField] private Image _image;
    [SerializeField] private Shader _shader;
    [SerializeField] private LongPressGesture _longPress;
    [SerializeField] private ConnectionUI _connectionUI;
    [SerializeField] private LineRenderer _offsetLine;
    
    private GeoCoordChannel _viewFinderChannel;
    private GeoCoordChannel _focusViewChannel;
    
    private OnlineMaps _contextViewMap;
    private RectTransform _rect;
    private GeoCoord _coords;

    private FocusView _focus;

    public void Init(Vector2 position, OnlineMaps contextViewMap)
    {
        _viewFinderChannel = new GeoCoordChannel();
        _focusViewChannel = new GeoCoordChannel();

        var material = new Material(_shader);
        var color = Random.ColorHSV(0f, 1f, 0.5f, 0.7f, 1f, 1f);
        material.SetColor("_Outline_Color", color);
        _image.material = material;
        _offsetMarker.GetComponent<Image>().material = material;
        _focusViewChannel.OnChange += UpdateOffset;
        _draggable.OnDrag += UpdateViewFinderCoords;
        _contextViewMap = contextViewMap;
        _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = position;
        _coords = ScreenToCoords(_rect.anchoredPosition);
        _viewFinderChannel.RaiseEvent(_coords);
        var offset = Random.insideUnitCircle.normalized;
        var focusPosition = _rect.anchoredPosition + offset * 500f;
        _focus = _focusViewSpawner.Spawn(transform.parent, _viewFinderChannel, _focusViewChannel, focusPosition, material);
        _longPress.LongPressed += DestroyViews;
        _connectionUI.Init(_focus.transform as RectTransform, color);

        _offsetLine.material.color = color;
        _offsetLine.startWidth = 0.01f;
        _offsetLine.endWidth = 0.01f;
    }

    private void Update()
    {
        Vector2 line = _offsetMarker.position - _rect.position;
        var p0 = -line.normalized * (_offsetMarker.rect.width * 0.5f);
        Vector2 p1 = _offsetMarker.anchoredPosition - line.normalized * (_rect.rect.width * 0.5f);
        _offsetLine.SetPosition(0, p0);
        _offsetLine.SetPosition(1, -p1);
    }

    private void OnDestroy()
    {
        _focusViewChannel.OnChange -= UpdateOffset;
        _draggable.OnDrag -= UpdateViewFinderCoords;
        _longPress.LongPressed -= DestroyViews;
    }

    private void DestroyViews(object sender, EventArgs e)
    {
        Destroy(_focus.gameObject);
        Destroy(gameObject);
    }

    private GeoCoord ScreenToCoords(Vector2 screenPosition)
    {
        _contextViewMap.control.GetCoords(screenPosition, out var lng, out var lat);
        return new GeoCoord(lng, lat);
    }

    private void UpdateViewFinderCoords(Vector2 screenPosition)
    {
        _contextViewMap.control.GetCoords(screenPosition, out var lng, out var lat);
        _coords.Latitude = lat;
        _coords.Longitude = lng;
        _viewFinderChannel.RaiseEvent(_coords);
    }

    private void UpdateOffset(GeoCoord geoCoord)
    {
        var screenPosition = _contextViewMap.control.GetScreenPosition(geoCoord.Longitude, geoCoord.Latitude);
        var position = screenPosition - _rect.anchoredPosition;
        _offsetMarker.anchoredPosition = position;
    }
}