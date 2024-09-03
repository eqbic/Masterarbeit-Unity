using UnityEngine;

public class ContextView : MonoBehaviour
{
    [SerializeField] private double _latitude;
    [SerializeField] private double _longitude;
    [SerializeField] private float _zoom;
    [SerializeField] private OnlineMaps _maps;

    public static OnlineMapsGeoRect ContextBounds { get; private set; }

    private void Start()
    {
        SetupMaps();
    }

    private void SetupMaps()
    {
        _maps.SetPositionAndZoom(_longitude, _latitude, _zoom);
        ContextBounds = _maps.bounds;
    }
}