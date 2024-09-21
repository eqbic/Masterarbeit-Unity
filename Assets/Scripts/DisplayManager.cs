using System;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private DisplayConfig _config;

    public static DisplayManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float GetPixelSize(float mm)
    {
        return _config.MillimeterToPixel(mm);
    }

    public Vector2 GetPixelSize(Vector2 mm)
    {
        return _config.MillimeterToPixel(mm);
    }
}