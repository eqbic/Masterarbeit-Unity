using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private UILineRendererList _lineRenderer;

    private RectTransform _focus;
    private RectTransform _rect;

    private float _offsetSize;
    private float _focusSize;
    
    private readonly Vector3[] _corners = new Vector3[4];

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _rect.GetWorldCorners(_corners);
        _offsetSize = (_corners[1] - _corners[0]).magnitude;
    }

    public void Init(RectTransform focusView, Color color)
    {
        _focus = focusView;
        _focus.GetWorldCorners(_corners);
        _focusSize = (_corners[1] - _corners[0]).magnitude;
        color.a = 0.5f;
        _lineRenderer.color = color;
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        Vector2 vFocus2 = _focus.position - _rect.position;
        Vector3 vFocus3 = vFocus2;
        Vector2 vNormal2 = Vector2.Perpendicular(vFocus2).normalized;
        Vector3 vNormal3 = vNormal2;

        
        
        var dPointOffsetCircle0 = _rect.position + vNormal3 * (_offsetSize * 0.5f);
        var dPointOffsetCircle1 = _rect.position - vNormal3 * (_offsetSize * 0.5f);
        var dPointFocusCircle0 = _focus.position + vNormal3 * (_focusSize * 0.5f);
        var dPointFocusCircle1 = _focus.position - vNormal3 * (_focusSize * 0.5f);

        Vector2 v = transform.InverseTransformVector(vFocus2);
        
        var pointOffsetCircle0 = vNormal2 * (0.5f * (_rect.rect.width - _lineRenderer.LineThickness));
        var pointOffsetCircle1 = -vNormal2 * (0.5f * (_rect.rect.width - _lineRenderer.LineThickness));
        var pointFocusCircle0 = v + vNormal2 * (0.5f * (_focus.rect.width - _lineRenderer.LineThickness));
        var pointFocusCircle1 = v - vNormal2 * (0.5f * (_focus.rect.width - _lineRenderer.LineThickness));

        _lineRenderer.Points[0] = pointOffsetCircle0;
        _lineRenderer.Points[1] = pointFocusCircle0;
        _lineRenderer.Points[2] = pointOffsetCircle1;
        _lineRenderer.Points[3] = pointFocusCircle1;
        _lineRenderer.SetAllDirty();
        
        Debug.DrawLine(_rect.position, _rect.position +  vFocus3, Color.red);
        Debug.DrawLine(_rect.position, dPointOffsetCircle0, Color.blue);
        Debug.DrawLine(dPointOffsetCircle0, dPointFocusCircle0, Color.green);
    }

    private void Update()
    {
        UpdatePoints();
    }
}