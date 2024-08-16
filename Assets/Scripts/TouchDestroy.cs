using System;
using TouchScript.Gestures;
using UnityEngine;

public class TouchDestroy : MonoBehaviour
{
    private LongPressGesture _longPress;

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
        Destroy(gameObject);
    }
 private void OnDestroy()
    {
        _longPress.LongPressed -= DestroyViewFinder;
    }
}
   