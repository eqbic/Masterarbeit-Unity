using System;
using Channels;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class FocusView : MonoBehaviour
{
    [SerializeField] private OnlineMaps _focusViewMap;
    [SerializeField] private FocusMapControl _focusMapControl;
    [SerializeField] private TapGesture _resetTap;
    [SerializeField] private RawImage _image;
    
    private GeoCoordChannel _viewFinderChannel;
    private GeoCoordChannel _focusViewChannel;

    private GeoCoord _coords;
    private RectTransform _rect;
    
   public void Init(GeoCoordChannel viewFinderChannel, GeoCoordChannel focusViewChannel, Vector2 position, Material material)
   {
        var texture = new Texture2D(512, 512);
        _focusViewMap.texture = texture;
        _image.texture = texture;
        var mat = new Material(material);
        mat.SetInt("_HasTexture", 1);
        mat.SetFloat("_Thickness", 0.95f);
        _image.material = mat;
        
        _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = position;
        _viewFinderChannel = viewFinderChannel;
        _focusViewChannel = focusViewChannel;
        _coords = viewFinderChannel.Data;
        _focusViewMap.SetPositionAndZoom(_coords.Longitude, _coords.Latitude, 17f);
        _focusViewChannel.RaiseEvent(_coords);
        _viewFinderChannel.OnChange += UpdateCoordinate;
        
        _focusMapControl.OnZoom += UpdateZoom;
        _focusMapControl.OnPan += UpdatePosition;
        _resetTap.Tapped += ResetOffset;
        _focusMapControl.OnRotate += UpdateRotation;
   }

    private void OnDestroy()
    {
        _viewFinderChannel.OnChange -= UpdateCoordinate;
        
        _focusMapControl.OnZoom -= UpdateZoom;
        _focusMapControl.OnPan -= UpdatePosition;
        _resetTap.Tapped -= ResetOffset;
        _focusMapControl.OnRotate -= UpdateRotation;

    }

    private void UpdateRotation(float deltaDegree)
    {
        _rect.Rotate(Vector3.forward, deltaDegree);
    }

    private void ResetOffset(object sender, EventArgs e)
    {
        _coords = _viewFinderChannel.Data;
        _focusViewMap.SetPositionAndZoom(_coords.Longitude, _coords.Latitude, 17f);
        _focusViewChannel.RaiseEvent(_coords);
    }

    private void UpdatePosition(Vector2 delta)
    {
        double lng, lat;
        _focusViewMap.GetPosition(out lng, out lat);
        var screenPosition = _focusViewMap.control.GetScreenPosition(lng, lat);
        screenPosition -= delta;
        _focusViewMap.control.GetCoords(screenPosition, out lng, out lat);
        _coords.Latitude = lat;
        _coords.Longitude = lng;
        
        _focusViewMap.SetPosition(lng, lat);
        _focusViewChannel.RaiseEvent(_coords);
    }

    private void UpdateZoom(float zoom)
    {
        _focusViewMap.floatZoom = zoom;
    }

    private void UpdateCoordinate(GeoCoord geoCoord)
    {
        _coords = geoCoord;
        _focusViewMap.SetPosition(geoCoord.Longitude, geoCoord.Latitude);
        _focusViewChannel.RaiseEvent(_coords);
    }
}