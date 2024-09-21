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
        [SerializeField] private PhysicalSize _physicalSize;
        [SerializeField] private Shader _shader;
        [SerializeField] private float _outlineThicknessMM;
        
        private static readonly int OutlineColor = Shader.PropertyToID("_Outline_Color");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");
        
        private MaskableGraphic _graphic;
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

        private void OnEnable()
        {
            _physicalSize.OnSizeSet += UpdateThickness;
        }

        private void OnDisable()
        {
            _physicalSize.OnSizeSet -= UpdateThickness;
        }

        private void UpdateThickness(Vector2 size)
        {
            Material.SetFloat(Thickness, GetThicknessNormalized(_outlineThicknessMM));
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
            Material = new Material(_shader);
            Material.SetColor(OutlineColor, color);
            graphic.material = Material;
        }

        private float GetThicknessNormalized(float thicknessMM)
        {
            var r = _rectTransform.rect.width * 0.5f;
            var thicknessPixel = DisplayManager.Instance.GetPixelSize(thicknessMM);
            return thicknessPixel / r;

        }
    }
}