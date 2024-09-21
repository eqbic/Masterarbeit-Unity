using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PhysicalSize : MonoBehaviour
{
    [SerializeField] private Vector2 _sizeInMm;

    private RectTransform _rectTransform;

    public event Action<Vector2> OnSizeSet;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        var sizeInPixel = DisplayManager.Instance.GetPixelSize(_sizeInMm);
        _rectTransform.sizeDelta = sizeInPixel;
        OnSizeSet?.Invoke(sizeInPixel);
    }
}