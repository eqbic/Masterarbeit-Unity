using System;
using UnityEngine;

public class ZoomSlider : MonoBehaviour
{
    [SerializeField] private ArcDrawer _arcDrawer;
    [SerializeField] private ZoomKnob _zoomKnob;
    [SerializeField] private float _angle = 90f;
    [SerializeField] private float _radius = 300f;

    private RectTransform _parent;
    private RectTransform _transform;
    private Joystick _joystick;
    private Vector2 _offset;

    public event Action<float> OnZoom;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        InitUI();
    }
    #endif

    private void Start()
    {
        InitUI();
    }

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _zoomKnob.OnZoom += Zoom;
    }

    private void OnDisable()
    {
        _zoomKnob.OnZoom -= Zoom;
    }

    private void Zoom(float normalizedZoom)
    {
        OnZoom?.Invoke(normalizedZoom);
    }

    public void Init(RectTransform parent, Joystick joystick, float zoom)
    {
        _parent = parent;
        _joystick = joystick;
    }
    
    private void Update()
    {
        _transform.anchoredPosition = _parent.anchoredPosition + Vector2.right * 300f + _joystick.Offset;
        // if (_direction != Vector2.zero)
        // {
        //     OnMove?.Invoke(_direction * (_maxSpeed * _directionFactor));
        // }
    }

    private void InitUI()
    {
        _arcDrawer.DrawArc(_angle, _radius);
        _zoomKnob.Init(_angle, _radius);
    }
}