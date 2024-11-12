using System;
using UnityEngine;

public class FindCamera : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    private void Start()
    {
        _canvas.worldCamera = Camera.main;
    }
}