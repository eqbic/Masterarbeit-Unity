using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RawImage), typeof(OnlineMaps))]
    public class FocusMapUI : MonoBehaviour
    {
        [SerializeField] private float _defaultZoom = 17f;
        
        private RawImage _image;
        private OnlineMaps _map;
        private OnlineMapsGeoRect _bounds;

        public OnlineMaps Map => _map;
        private GeoCoord _currentCoords;
        
        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _map = GetComponent<OnlineMaps>();
        }

        public void Init(GeoCoord initialCoords)
        {
            _map.SetPositionAndZoom(initialCoords.Longitude, initialCoords.Latitude, _defaultZoom);
            _currentCoords = initialCoords;
            _bounds = ContextView.ContextBounds;
        }

        public void ResetView(GeoCoord coord)
        {
            _map.SetPositionAndZoom(coord.Longitude, coord.Latitude, _defaultZoom);
            _currentCoords = coord;
        }
        
        public GeoCoord Move(Vector2 delta)
        {
            _map.GetPosition(out var lng, out var lat);
            var screenPosition = _map.control.GetScreenPosition(lng, lat);
            screenPosition -= delta;
            lng = Math.Clamp(lng, _bounds.left, _bounds.right);
            lat = Math.Clamp(lat, _bounds.bottom, _bounds.top);
            _map.control.GetCoords(screenPosition, out lng, out lat);
            _map.SetPosition(lng, lat);
            return new GeoCoord(lng, lat);
        }

        public void SetupTexture(int size)
        {
            var texture = new Texture2D(size, size);
            _map.texture = texture;
            _image.texture = texture;
        }

        public void UpdateCoords(GeoCoord coords)
        {
            _map.SetPosition(coords.Longitude, coords.Latitude);
        }

        public void UpdateZoom(float zoom)
        {
            _map.floatZoom = zoom;
        }
    }
}