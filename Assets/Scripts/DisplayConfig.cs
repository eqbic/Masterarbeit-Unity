using UnityEngine;

[CreateAssetMenu(fileName = "DisplayConfig", menuName = "Masterarbeit/New Display Config", order = 0)]
public class DisplayConfig : ScriptableObject
{
    [SerializeField] private Vector2 _resolution = new Vector2(3840, 2160);
    [SerializeField] private float _diagonalInInch = 55;

    private float _aspectRatio = 0f;
    private float _ppi = 0f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _aspectRatio = _resolution.x / _resolution.y;
        var dPixel = Mathf.Sqrt(_resolution.x * _resolution.x + _resolution.y * _resolution.y);
        _ppi = dPixel / _diagonalInInch;
    }
#endif

    private const float InchToMm = 1f / 25.4f;

    public float MillimeterToPixel(float mm)
    {
        return mm * _ppi * InchToMm;
    }

    public Vector2 MillimeterToPixel(Vector2 mm)
    {
        return mm * _ppi * InchToMm;
    }
}