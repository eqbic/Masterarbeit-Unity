using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class OutlineUI : MonoBehaviour
    {
        [SerializeField] private Sprite _spritePrefab;
        private Image _image;
        private RectTransform _rectTransform;
        private Sprite _sprite;
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
            _sprite = Instantiate(_spritePrefab);
            _image.sprite = _sprite;
        }

        private void Start()
        {
            _rectTransform.SetAsLastSibling();
        }

        public void Init()
        {
            Color = Random.ColorHSV(0.15f, 0.75f, 0.5f, 0.7f, 1f, 1f);
            SetupMaterial(_image, Color);
        }

        public void Init(Color color)
        {
            Color = color;
            SetupMaterial(_image, Color);
        }

        private void SetupMaterial(MaskableGraphic graphic, Color color)
        {
            graphic.color = color;
        }
    }
}