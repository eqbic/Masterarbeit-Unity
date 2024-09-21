using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.Serialization;

public class Joystick : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _knobGesture;
    [SerializeField] private ScreenTransformGesture _backgroundGesture;
    [SerializeField] private ScreenTransformGesture _rotateGesture;
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private PhysicalSize _physicalSize;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private bool _inverted = false;
    public event Action<Vector2> OnMove;
    public event Action<float> OnRotate;
    public event Action<Vector2> OnSizeSet;

    private Vector2 _lastRotationVector;
    private float _radius;
    private Vector2 _direction;
    private float _directionFactor = 1f;
    private RectTransform _knobTransform;
    private RectTransform _parent;
    private RectTransform _transform;
    public Vector2 Offset { get; private set; } = Vector2.zero;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _knobTransform = _knobGesture.GetComponent<RectTransform>();
        _directionFactor = _inverted ? -1f : 1f;
    }

    public void Init(RectTransform parent)
    {
        _parent = parent;
    }

    private void Update()
    {
        _transform.anchoredPosition = _parent.anchoredPosition + Vector2.right * 300f + Offset;
        if (_direction != Vector2.zero)
        {
            OnMove?.Invoke(_direction * (_maxSpeed * _directionFactor));
        }
    }

    private void OnEnable()
    {
        _physicalSize.OnSizeSet += UpdateSize;
        _knobGesture.Transformed += MoveJoystick;
        _knobGesture.TransformCompleted += ResetKnob;
        _backgroundGesture.Transformed += MoveWidget;
        _rotateGesture.TransformStarted += InitRotation;
        _rotateGesture.Transformed += UpdateRotation;
    }

    private void OnDisable()
    {
        _physicalSize.OnSizeSet -= UpdateSize;
        _knobGesture.Transformed -= MoveJoystick;
        _knobGesture.TransformCompleted -= ResetKnob;
        _backgroundGesture.Transformed -= MoveWidget;
        _rotateGesture.TransformStarted -= InitRotation;
        _rotateGesture.Transformed -= UpdateRotation;
    }

    private void UpdateSize(Vector2 parentSize)
    {
        var size = _knobTransform.rect.width;
        _radius = 0.5f * (parentSize.x - 2 * size);
        OnSizeSet?.Invoke(_transform.rect.size);
    }

    private void UpdateRotation(object sender, EventArgs e)
    {
        var fingerVector = ((_rotateGesture.ScreenPosition - _transform.anchoredPosition) - 0.5f * _transform.rect.size).normalized;
        var deltaAngle = Vector2.SignedAngle(_lastRotationVector, fingerVector);
        _lastRotationVector = fingerVector;
        OnRotate?.Invoke(deltaAngle);
    }

    private void InitRotation(object sender, EventArgs e)
    {
        _lastRotationVector = ((_rotateGesture.ScreenPosition - _transform.anchoredPosition) - 0.5f * _transform.rect.size).normalized;
    }

    private void MoveWidget(object sender, EventArgs e)
    {
        Offset += (Vector2)_backgroundGesture.DeltaPosition;
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