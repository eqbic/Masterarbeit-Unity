using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _gesture;
    
    private RectTransform _rect;

    public event Action<Vector2> OnDrag; 

    private void Awake()
    {
        _gesture = GetComponent<ScreenTransformGesture>();
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
