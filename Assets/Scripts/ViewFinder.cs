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
        material.SetColor("_Outline_Color", Random.ColorHSV(0f, 1f, 0.5f, 0.7f, 1f, 1f));
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