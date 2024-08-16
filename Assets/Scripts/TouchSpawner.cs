using System;
using TouchScript.Gestures;
using UnityEngine;

public class TouchSpawner : MonoBehaviour
{
    [SerializeField] private TapGesture _tapGesture;
    [SerializeField] private ViewFinderSpawner _spawner;

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
        _spawner.SpawnViewFinder(_currentId, _tapGesture.ScreenPosition);
        _currentId += 1;
    }
}