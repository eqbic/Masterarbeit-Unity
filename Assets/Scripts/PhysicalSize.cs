using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PhysicalSize : MonoBehaviour
{
    [SerializeField] private Vector2 _sizeInMM;
    public Vector2 SizeInMm
    {
        get => _sizeInMM;
        set
        {
            _sizeInMM = value;
            SetSize(_sizeInMM);
        }
    }

    private RectTransform _rectTransform;

    public event Action<Vector2> OnSizeSet;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetSize(SizeInMm);
    }

    private void SetSize(Vector2 sizeInMM)
    {
        var sizeInPixel = DisplayManager.Instance.GetPixelSize(sizeInMM);
        _rectTransform.sizeDelta = sizeInPixel;
        OnSizeSet?.Invoke(sizeInPixel);
    }
}