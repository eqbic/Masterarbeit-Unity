using UnityEngine;

public class FocusViewSpawner : MonoBehaviour
{
    [SerializeField] private FocusView _focusViewPrefab;

    public FocusView Spawn(ViewFinder viewFinder)
    {
        var offset = Random.insideUnitCircle.normalized;
        var focusPosition = viewFinder.RectTransform.anchoredPosition + offset * 500f;
        var focus = Instantiate(_focusViewPrefab, viewFinder.RectTransform.parent);
        focus.Init(viewFinder.ViewFinderChannel, viewFinder.FocusViewChannel, focusPosition, viewFinder.Color);
        return focus;
    }
}