using UnityEngine;

namespace UI
{
    public class OffsetLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer _offsetLine;

        private RectTransform _startTransform;
        private RectTransform _endTransform;

        private float _startRadius;
        private float _endRadius;

        public void Init(RectTransform startTransform, RectTransform endTransform, Color color)
        {
            _startTransform = startTransform;
            _endTransform = endTransform;

            _startRadius = _startTransform.rect.width * 0.5f;
            _endRadius = _startTransform.rect.width * 0.5f;
            
            _offsetLine.material.color = color;
            _offsetLine.startWidth = 0.01f;
            _offsetLine.endWidth = 0.01f;
        }
        
        private void Update()
        {
            Vector2 line = _endTransform.position - _startTransform.position;
            var p0 = -line.normalized * _endRadius;
            Vector2 p1 = _endTransform.anchoredPosition - line.normalized * _startRadius;
            _offsetLine.SetPosition(0, p0);
            _offsetLine.SetPosition(1, -p1);
        }
    }
}