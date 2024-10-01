using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class TouchTester : MonoBehaviour
{
    [SerializeField] private ScreenTransformGesture _gesture;

    private void OnEnable()
    {
        _gesture.Transformed += PrintName;
    }

    private void OnDisable()
    {
        _gesture.Transformed -= PrintName;
    }

    private void Start()
    {
        print($"out: {DisplayManager.Instance.GetPixelSize(193)}");
        print($"in: {DisplayManager.Instance.GetPixelSize(161)}");
    }

    private void PrintName(object sender, EventArgs e)
    {
        print(gameObject.name);
    }
}
