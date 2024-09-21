using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DisplayConfig", menuName = "Masterarbeit/New Display Config", order = 0)]
public class DisplayConfig : ScriptableObject
{
    public Vector2 Resolution = new Vector2(3840, 2160);
    public float DiagonalInInch = 55;

    private float _aspectRatio = 0f;
    private float _ppi = 0f;

    private float PPI
    {
        get
        {
            if (_ppi != 0f) return _ppi;
            _aspectRatio = Resolution.x / Resolution.y;
            var dPixel = Mathf.Sqrt(Resolution.x * Resolution.x + Resolution.y * Resolution.y);
            _ppi = dPixel / DiagonalInInch;

            return _ppi;
        }
    }

    private const float InchToMm = 1f / 25.4f;

    public float MillimeterToPixel(float mm)
    {
        return mm * PPI * InchToMm;
    }

    public Vector2 MillimeterToPixel(Vector2 mm)
    {
        return mm * PPI * InchToMm;
    }
}