using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ArcDrawer : MonoBehaviour
{
    [SerializeField] private UILineRendererList _lineRenderer;

    private const int AngleBetweenPoints = 2;

    public void DrawArc(float angle, float radius)
    {
        var pointCount = (int)angle / AngleBetweenPoints;
        angle = Mathf.Deg2Rad * angle;
        var angleOffset = (Mathf.PI - angle) * 0.5f;
        _lineRenderer.ClearPoints();
        var angleStep = angle / pointCount;
        for (var i = 0; i < pointCount; i++)
        {
            var x = Mathf.Sin(angleStep * i + angleOffset) * radius;
            var y = Mathf.Cos(angleStep * i + angleOffset) * radius;
            _lineRenderer.AddPoint(new Vector2(x, y));
        }
        _lineRenderer.SetAllDirty();
    }
}