using System;
using TouchScript.Gestures;
using UnityEngine;

public class TouchDestroy : MonoBehaviour
{
    private LongPressGesture _longPress;

    public event Action OnTouchDestroy;

    private void Awake()
    {
        SetupDestroyOnLongPress();
    }

    private void SetupDestroyOnLongPress()
    {
        _longPress = gameObject.AddComponent<LongPressGesture>();
        _longPress.TimeToPress = 2f;
        _longPress.DistanceLimit = 0.2f;
        _longPress.MinPointers = 1;
        _longPress.MaxPointers = 1;
        _longPress.LongPressed += DestroyViewFinder;
    }

    private void DestroyViewFinder(object sender, EventArgs e)
    {
        OnTouchDestroy?.Invoke();
    }
 private void OnDestroy()
    {
        _longPress.LongPressed -= DestroyViewFinder;
    }
}
   