using System;
using TouchScript.Gestures;
using UnityEngine;
using Random = UnityEngine.Random;

public class TouchSpawner : MonoBehaviour
{
    [SerializeField] private TapGesture _tapGesture;
    [SerializeField] private LensSpawner _spawner;

    private uint _currentId = 0;

    private void OnEnable()
    {
        _tapGesture.Tapped += SpawnViewFinder;
    }

    private void OnDisable()
    {
        _tapGesture.Tapped -= SpawnViewFinder;
    }

    private void SpawnViewFinder(object sender, EventArgs e)
    {
        var viewFinder = _spawner.SpawnViewFinder(_currentId, _tapGesture.ScreenPosition, InputType.Touch);
        var offset = Random.insideUnitCircle.normalized;
        var focusPosition = viewFinder.RectTransform.anchoredPosition + offset * 500f;
        _spawner.SpawnFocusView(_currentId, focusPosition, viewFinder, InputType.Touch);
        _currentId += 1;
    }
}