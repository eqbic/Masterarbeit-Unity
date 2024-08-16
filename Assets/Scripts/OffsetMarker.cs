using System;
using Channels;
using UnityEngine;

public class OffsetMarker : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform _parentTransform;

    private GeoCoordChannel _focusViewChannel;
    private OnlineMaps _contextViewMap;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(GeoCoordChannel focusViewChannel, OnlineMaps contextViewMap)
    {
        _focusViewChannel = focusViewChannel;
        _contextViewMap = contextViewMap;
        _parentTransform = transform.parent.GetComponent<RectTransform>();
        
        _focusViewChannel.OnChange += UpdateOffset;
    }
    
    private void UpdateOffset(GeoCoord geoCoord)
    {
        var screenPosition = _contextViewMap.control.GetScreenPosition(geoCoord.Longitude, geoCoord.Latitude);
        var position = screenPosition - _parentTransform.anchoredPosition;
        _rectTransform.anchoredPosition = position;
    }
}