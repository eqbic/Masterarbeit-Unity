using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _gesture;

    public ScreenTransformGesture Gesture
    {
        get => _gesture;
        set
        {
            _gesture = value;
            _gesture.Transformed -= Move;
            _gesture.Transformed += Move;
        }
    }
    
    private RectTransform _rect;

    public event Action<Vector2> OnDrag; 

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _gesture.Transformed += Move;
    }

    private void OnDisable()
    {
        _gesture.Transformed -= Move;
    }

    private void Move(object sender, EventArgs e)
    {
        Vector2 delta = new Vector2(_gesture.DeltaPosition.x, _gesture.DeltaPosition.y);
        _rect.anchoredPosition += delta;
        OnDrag?.Invoke(_rect.anchoredPosition);
    }
}
