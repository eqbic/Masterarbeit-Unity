using System;
using TuioNet.Tuio20;
using TuioUnity.Utils;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private RecordSign _recordSign;

    private Tuio20Token _parent;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    

    public void SetNormalizedSpeed(float speed)
    {
        _recordSign.Progress = speed;
    }

    public void Init(Tuio20Token token)
    {
        _parent = token;
    }

    private void Update()
    {
        var x = _parent.Position.X * Screen.width;
        var y = (1.0f -_parent.Position.Y) * Screen.height;
        _rectTransform.anchoredPosition = new Vector2(x, y);
    }
}