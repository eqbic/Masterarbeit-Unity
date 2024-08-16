using System.Collections.Generic;
using UnityEngine;

public class ViewFinderSpawner : MonoBehaviour
{
    [SerializeField] private OnlineMaps _contextViewMap;
    [SerializeField] private ViewFinder _viewFinderPrefab;
    [SerializeField] private RectTransform _viewParent;

    private readonly Dictionary<uint, ViewFinder> _viewFinders = new();

    public void SpawnViewFinder(uint id, Vector2 screenPosition)
    {
        var finder = Instantiate(_viewFinderPrefab, _viewParent);
        finder.Init(id, screenPosition, _contextViewMap);
        _viewFinders.Add(id, finder);
    }

    public void DestroyViewFinder(uint id)
    {
        if (_viewFinders.Remove(id, out var finder))
        {
            Destroy(finder.gameObject);
        }
    }
}