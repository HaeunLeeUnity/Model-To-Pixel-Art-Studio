using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModelToPixelArt.Service.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        private Image tooltipPanel;

        [SerializeField]
        private TextMeshProUGUI toolTipText;

        private bool _isVisible = false;
        private Vector2 _pivot;
        private Vector2 _screenMidPoint;

        private void Awake()
        {
            _screenMidPoint = new Vector2(Screen.width, Screen.height) / 2;
        }

        public void ShowTooltip(string text)
        {
            _isVisible = true;
            toolTipText.text = text;
            tooltipPanel.gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            _isVisible = false;
            tooltipPanel.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (!_isVisible) return;

            _pivot = Vector2.zero;

            if (_screenMidPoint.x < Input.mousePosition.x)
            {
                _pivot.x = 1;
            }

            if (_screenMidPoint.y < Input.mousePosition.y)
            {
                _pivot.x = 1;
            }

            tooltipPanel.rectTransform.pivot = _pivot;
            tooltipPanel.rectTransform.position = Input.mousePosition;
        }
    }
}