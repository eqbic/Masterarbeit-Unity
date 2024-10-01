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
    private Collider2D _collider;

    [SerializeField] private bool _inZone = false;
    [SerializeField] private float _timeInZone = 0f;
    [SerializeField] private float _startThreshold = 5f;
    [SerializeField] private float _endThreshold = 3f;

    private void OnTriggerStay2D(Collider2D other)
    {
        var otherBounds = other.bounds;
        var myBounds = _collider.bounds;
        
        if (otherBounds.Contains(myBounds.min) && otherBounds.Contains(myBounds.max))
        {
            if (_inZone) return;
            _timeInZone = 0f;
            _inZone = true;
            
            if (other.CompareTag("StartZone"))
            {
                StartCoroutine(CountTime(_startThreshold, StartRecord));
            }

            if (other.CompareTag("EndZone"))
            {
                StartCoroutine(CountTime(_endThreshold, StopRecord));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _inZone = false;
    }

    private void StartRecord()
    {
        print("Start Record");
    }

    private void StopRecord()
    {
        print("Stop Record");
    }

    IEnumerator CountTime(float timeThreshold, Action onFinish)
    {
        while (_timeInZone < timeThreshold && _inZone)
        {
            _timeInZone += Time.deltaTime;
            yield return null;
        }

        if (!(_timeInZone < timeThreshold))
        {
            onFinish?.Invoke();
        }
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<Collider2D>();
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
