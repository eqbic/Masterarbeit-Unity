using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.Serialization;

public class Joystick : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _knobGesture;
    [SerializeField] private ScreenTransformGesture _backgroundGesture;
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private bool _inverted = false;
    public event Action<Vector2> OnMove;

    private float _radius;
    private Vector2 _direction;
    private float _directionFactor = 1f;
    private RectTransform _knobTransform;
    private RectTransform _parent;
    private RectTransform _transform;
    private Vector2 _offset = Vector2.zero;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _knobTransform = _knobGesture.GetComponent<RectTransform>();
        var size = _knobTransform.rect.width;
        var parentSize = _backgroundTransform.rect.width;
        _radius = 0.5f * (parentSize - size);
        _directionFactor = _inverted ? -1f : 1f;
    }

    public void Init(RectTransform parent)
    {
        _parent = parent.parent as RectTransform;
    }

    private void Update()
    {
        _transform.anchoredPosition = _parent.anchoredPosition + Vector2.right * 300f + _offset;
        
        if (_direction != Vector2.zero)
        {
            OnMove?.Invoke(_direction * (_maxSpeed * _directionFactor));
        }
    }

    private void OnEnable()
    {
        _knobGesture.Transformed += MoveJoystick;
        _knobGesture.TransformCompleted += ResetKnob;
        _backgroundGesture.Transformed += MoveWidget;
    }

    private void OnDisable()
    {
        _knobGesture.Transformed -= MoveJoystick;
        _knobGesture.TransformCompleted -= ResetKnob;
        _backgroundGesture.Transformed -= MoveWidget;
    }

    private void MoveWidget(object sender, EventArgs e)
    {
        _offset += (Vector2)_backgroundGesture.DeltaPosition;
    }

    private void ResetKnob(object sender, EventArgs e)
    {
        _knobTransform.anchoredPosition = Vector2.zero;
        _direction = Vector2.zero;
    }

    private void MoveJoystick(object sender, EventArgs e)
    {
        var fingerVector = (_knobGesture.ScreenPosition - _transform.anchoredPosition) - 0.5f * _transform.rect.size;
        var newPosition = Vector2.ClampMagnitude(fingerVector, _radius);
        _knobTransform.anchoredPosition = newPosition;
        _direction = newPosition / _radius;
    }
}