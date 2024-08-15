using UnityEngine;

[ExecuteInEditMode]
public class MagnifyPosition : MonoBehaviour
{
    [SerializeField] private Material _material;

    private Camera _camera;
    private static readonly int ObjectScreenPosition = Shader.PropertyToID("_ObjectScreenPosition");

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var screenPixel = _camera.WorldToScreenPoint(transform.position);
        screenPixel.x /= Screen.width;
        screenPixel.y /= Screen.height;
        _material.SetVector(ObjectScreenPosition, screenPixel);
    }
}
