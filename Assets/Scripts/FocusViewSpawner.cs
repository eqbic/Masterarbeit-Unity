using Channels;
using UnityEngine;

public class FocusViewSpawner : MonoBehaviour
{
    [SerializeField] private FocusView _focusViewPrefab;

    public FocusView Spawn(Transform parent, GeoCoordChannel viewFinderChannel, GeoCoordChannel focusViewChannel, Vector2 position, Material material)
    {
        var focus = Instantiate(_focusViewPrefab, parent);
        focus.Init(viewFinderChannel, focusViewChannel, position, material);
        return focus;
    }
}