using UnityEngine;

public class ContextView : MonoBehaviour
{
    [SerializeField] private double _latitude;
    [SerializeField] private double _longitude;
    [SerializeField] private float _zoom;
    [SerializeField] private OnlineMaps _maps;

    private void Start()
    {
        SetupMaps();
    }

    private void SetupMaps()
    {
        _maps.SetPositionAndZoom(_longitude, _latitude, _zoom);
    }
}