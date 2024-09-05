using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class ZoomKnob : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _gesture;
    private RectTransform _transform;

    private float _angle;
    private float _radius;
    private float _startAngle;

    private RectTransform _parent;

    public event Action<float> OnZoom;

    private RectTransform RectTransform
    {
        get
        {
            if (_transform == null)
            {
                _transform = GetComponent<RectTransform>();
                _parent = transform.parent as RectTransform;
            }

            return _transform;
        }
    }

    private void OnEnable()
    {
        _gesture.Transformed += MoveAlongArc;
    }

    private void OnDisable()
    {
        _gesture.Transformed -= MoveAlongArc;
    }

    private void MoveAlongArc(object sender, EventArgs e)
    {
        var fingerPosition = _gesture.ScreenPosition;
        var direction = (fingerPosition - _parent.anchoredPosition - 0.5f * _parent.rect.size).normalized;
        var angle = 180 - Mathf.Clamp(Vector2.SignedAngle(Vector2.down, direction), _startAngle, _startAngle + _angle);
        direction = Quaternion.AngleAxis(angle, Vector3.back) * Vector2.up; 
        RectTransform.anchoredPosition = direction * _radius;
        var normalizedZoom = (angle - _startAngle) / (_angle);
        OnZoom?.Invoke(normalizedZoom);
    }

    public void Init(float angle, float radius)
    {
        _angle = angle;
        _radius = radius;
        _startAngle = (180f - angle) * 0.5f;
        var direction = Quaternion.AngleAxis(_startAngle, Vector3.back) * Vector2.up; 
        RectTransform.anchoredPosition = direction * radius;
    }
}