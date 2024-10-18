using System;
using UnityEngine;
using UnityEngine.UI;

public class RecordSign : MonoBehaviour
{
    [SerializeField] private Shader _shader;
    [SerializeField] private float _width = 0.05f;
    [SerializeField] private float _rotation = 0.25f;
    [SerializeField] private Image _image;

    private Material _material;
    private float _progress;
    private static readonly int ProgressID = Shader.PropertyToID("_Progress");
    private static readonly int Width = Shader.PropertyToID("_Width");
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private Color _imageColor;

    public float Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            _material.SetFloat(ProgressID, _progress);
            _imageColor.a = _progress < 1f ? 0.5f : 1f;
            _image.color = _imageColor;
        }
    }

    private void Awake()
    {
        _material = new Material(_shader);
        _material.SetFloat(Width, _width);
        _material.SetFloat(Rotation, _rotation);
        _image.material = _material;
        _imageColor = _image.color;
    }
}