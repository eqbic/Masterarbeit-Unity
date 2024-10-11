using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Mover : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _gesture;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
        _rectTransform.anchoredPosition += (Vector2)_gesture.DeltaPosition;
        Physics.SyncTransforms();
    }
}
