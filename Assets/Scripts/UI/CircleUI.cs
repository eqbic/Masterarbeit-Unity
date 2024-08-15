using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class CircleUI : MonoBehaviour
    {
        [SerializeField] private Shader _shader;
        [Range(0f, 1f)] [SerializeField] private float _outlineThickness; 
        
        private static readonly int OutlineColor = Shader.PropertyToID("_Outline_Color");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");
        private Image _image;
        private RectTransform _rectTransform;
        public Material Material { get; private set; }
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
            _image = GetComponent<Image>();
        }

        public void Init()
        {
            Color = Random.ColorHSV(0f, 1f, 0.5f, 0.7f, 1f, 1f);
            SetupMaterial(_image, Color);
        }

        public void Init(Color color)
        {
            Color = color;
            SetupMaterial(_image, Color);
        }

        private void SetupMaterial(Image image, Color color)
        {
            Material = new Material(_shader);
            Material.SetColor(OutlineColor, color);
            Material.SetFloat(Thickness, _outlineThickness);
            image.material = Material;
        }
    }
}