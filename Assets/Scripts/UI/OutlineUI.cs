using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class OutlineUI : MonoBehaviour
    {
        
        private MaskableGraphic _graphic;
        private RectTransform _rectTransform;
        public Color Color { get; private set; }

        public RectTransform RectTransform
        {
            get => _rectTransform;
            private set
            {
                _rectTransform = value;
                Radius = _rectTransform.rect.width * 0.5f;
            }
        }

        public float Radius { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _graphic = GetComponent<MaskableGraphic>();
        }

        public void Init()
        {
            Color = Random.ColorHSV(0f, 1f, 0.5f, 0.7f, 1f, 1f);
            SetupMaterial(_graphic, Color);
        }

        public void Init(Color color)
        {
            Color = color;
            SetupMaterial(_graphic, Color);
        }

        private void SetupMaterial(MaskableGraphic graphic, Color color)
        {
            graphic.color = color;
        }
    }
}