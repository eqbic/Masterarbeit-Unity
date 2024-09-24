using System;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

public class TuiJoystickDeadzone : MonoBehaviour
{
    [field: SerializeField] public float Diameter { get; private set; } = 95f;
    [SerializeField] private PhysicalSize _physicalSize;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _physicalSize.SizeInMm = Vector2.one * Diameter;
    }

    public void Init(Vector2 normalizedPosition)
    {
        normalizedPosition.x *= Screen.width;
        normalizedPosition.y = (1.0f - normalizedPosition.y) *  Screen.height;
        _rectTransform.anchoredPosition = normalizedPosition;
    }
}