using TuioNet.Tuio11;
using TuioUnity.Tuio11;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Magnify : MonoBehaviour
{
    [SerializeField] private Shader _magnifyShader;
    [SerializeField] private OnlineMaps _maps;

    public OnlineMaps BigMap { get; set; }
    private RectTransform _bigMapRectTransform;
    
    private Camera _camera;
    private static readonly int Zoom = Shader.PropertyToID("_Zoom");

    private Material _material;

    private Tuio11Object _tuio11Object;

    private Texture2D _texture;
    private RawImage _image;
    private Vector2 _normalizedPosition;
    private static readonly int MagnifyTexture = Shader.PropertyToID("_MagnifyTexture");

    private RectTransform _rectTransform;
    private Vector2 _lastPosition;
    public Vector4 NormalizedPosition { get; private set; }

    private float _angle;

    public void Zooming(float deltaZoom)
    {
        _maps.floatZoom += deltaZoom;
    }
    
    private void Awake()
    {
        _camera = Camera.main;
        _image = GetComponent<RawImage>();
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void Init(Tuio11Container container, OnlineMaps bigMap)
    {
        _material = new Material(_magnifyShader);
        _texture = new Texture2D(700, 700)
        {
            name = gameObject.name
        };
        _maps.SetTexture(_texture);
        _material.SetTexture(MagnifyTexture, _texture);
        BigMap = bigMap;
        _bigMapRectTransform = BigMap.GetComponent<RectTransform>();
        _image.material = _material;
        _tuio11Object = (Tuio11Object)container;
        _angle = _tuio11Object.Angle;
    }

    private void UpdateZoom()
    {
        var deltaAngle = _tuio11Object.Angle - _angle;
        if (Mathf.Abs(deltaAngle) > Mathf.PI)
        {
            deltaAngle %= (Mathf.PI * 2);
        }
        // deltaAngle = (deltaAngle + 2f * Mathf.PI) % (2f * Mathf.PI);
        _maps.floatZoom += deltaAngle * 2f;
        _material.SetFloat(Zoom, _maps.zoomFactor);
        _angle = _tuio11Object.Angle;
    }

    private void Update()
    {
        var pos = Vector2.zero;
        pos.x = _tuio11Object.Position.X;
        pos.y = 1.0f - _tuio11Object.Position.Y;
        pos = _camera.ViewportToScreenPoint(pos);
        UpdateZoom();
        if (_lastPosition == pos) return;
        _rectTransform.anchoredPosition = pos;
        UpdatePosition();
        _lastPosition = pos;
    }

    private void UpdatePosition()
    {
        var screenPosition = _camera.WorldToScreenPoint(transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bigMapRectTransform,
                screenPosition, _camera, out var localPos))
        {
            NormalizedPosition = Rect.PointToNormalized(_bigMapRectTransform.rect, localPos);
        }
        
        BigMap.control.GetCoords(screenPosition, out var lon, out var lat);
        _maps.SetPosition(lon, lat);
    }
}
