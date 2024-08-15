using System;
using TouchScript.Gestures;
using UnityEngine;

public class ViewFinderSpawner : MonoBehaviour
{
    [SerializeField] private OnlineMaps _contextViewMap;
    [SerializeField] private TapGesture _tapGesture;
    [SerializeField] private ViewFinder _viewFinderPrefab;
    [SerializeField] private RectTransform _viewParent;

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
        // _contextViewMap.control.GetCoords(_tapGesture.ScreenPosition, out var lng, out var lat);
        // print($"Spawned at: {_tapGesture.ScreenPosition} -> {lng}, {lat}");

        var finder = Instantiate(_viewFinderPrefab, _viewParent);
        finder.Init(_tapGesture.ScreenPosition, _contextViewMap);
    }
}