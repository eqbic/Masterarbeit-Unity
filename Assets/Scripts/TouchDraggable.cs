using System;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class TouchDraggable : MonoBehaviour
{
    private ScreenTransformGesture _gesture;
    private Draggable _draggable;

    private void Awake()
    {
        SetupTransformGesture();
        _draggable = gameObject.AddComponent<Draggable>();
        _draggable.Gesture = _gesture;
    }

    private void SetupTransformGesture()
    {
        _gesture = gameObject.AddComponent<ScreenTransformGesture>();
        _gesture.Type = TransformGesture.TransformType.Translation;
        _gesture.MinScreenPointsDistance = 0.5f;
        _gesture.ScreenTransformThreshold = 0.1f;
        _gesture.MinPointers = 1;
        _gesture.MaxPointers = 1;
    }
}