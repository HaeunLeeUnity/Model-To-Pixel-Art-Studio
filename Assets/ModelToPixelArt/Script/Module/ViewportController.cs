using System;
using ModelToPixelArt.Definition;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ModelToPixelArt.Module
{
    public class ViewportController : MonoBehaviour
    {
        public event Action<float> OnFOVChanged;
        public event Action<int, int> OnViewportSizeChanged;
        public event Action<EViewportMode> OnViewportModeChanged;

        [SerializeField]
        private RawImage modelIndicator;

        [SerializeField]
        private Image resultIndicator;

        [SerializeField]
        private Image viewportBorder;

        [SerializeField]
        private Camera captureCamera;

        private RenderTexture _renderTexture;
        private Texture2D _resultTexture;


        private int _width = 1024;
        private int _height = 1024;
        private float _fov = 2.4f;
        private EViewportMode _viewportMode = EViewportMode.Model;

        public int Width
        {
            set
            {
                _width = value;
                OnViewportSizeChanged?.Invoke(_width, _height);
            }
        }

        public int Height
        {
            set
            {
                _height = value;
                OnViewportSizeChanged?.Invoke(_width, _height);
            }
        }

        public float FOV
        {
            set
            {
                _fov = value;
                OnFOVChanged?.Invoke(_fov);
            }
        }

        public Texture2D ResultTexture
        {
            set
            {
                _resultTexture = value;
                var sprite = Sprite.Create(_resultTexture, new Rect(0, 0, _resultTexture.width, _resultTexture.height),
                    new Vector2(0.5f, 0.5f));
                resultIndicator.sprite = sprite;
            }
            get => _resultTexture;
        }

        public EViewportMode ViewportMode
        {
            set
            {
                _viewportMode = value;
                OnViewportModeChanged?.Invoke(_viewportMode);
            }
        }

        public void Initialization()
        {
            OnFOVChanged += ApplyFOV;
            OnViewportSizeChanged += CreateRenderTexture;
            OnViewportModeChanged += SetIndicator;

            _fov = captureCamera.orthographicSize;
            
            OnFOVChanged?.Invoke(_fov);
            OnViewportSizeChanged?.Invoke(_width, _height);
            OnViewportModeChanged?.Invoke(_viewportMode);
        }


        public void Move(Vector2 direction)
        {
            captureCamera.transform.Translate(direction);
        }

        public void AdjustFOV(float offset)
        {
            FOV = _fov + offset;
        }

        private void SetIndicator(EViewportMode viewportMode)
        {
            switch (viewportMode)
            {
                case EViewportMode.Model:
                    modelIndicator.gameObject.SetActive(true);
                    resultIndicator.gameObject.SetActive(false);
                    break;
                case EViewportMode.Result:
                    modelIndicator.gameObject.SetActive(false);
                    resultIndicator.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void CreateRenderTexture(int width, int height)
        {
            _renderTexture = new RenderTexture(width, height, 24);
            _renderTexture.Create();
            captureCamera.targetTexture = _renderTexture;
            modelIndicator.texture = _renderTexture;

            if (_width > _height)
            {
                viewportBorder.rectTransform.sizeDelta = new Vector2(1024, (float)_height / _width * 1024);
            }
            else
            {
                viewportBorder.rectTransform.sizeDelta = new Vector2((float)_width / _height * 1024, 1024);
            }
        }

        private void ApplyFOV(float fov)
        {
            captureCamera.orthographicSize = fov;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector2.left * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(Vector2.right * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Move(Vector2.up * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Move(Vector2.down * Time.deltaTime);
            }
        }

        public Texture2D Capture()
        {
            RenderTexture.active = _renderTexture;
            var capturedTexture =
                new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
            capturedTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            capturedTexture.Apply();
            RenderTexture.active = null;
            return capturedTexture;
        }
    }
}