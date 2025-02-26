using ModelToPixelArt.Definition;
using ModelToPixelArt.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ModelToPixelArt.Service.UI
{
    public class StudioUIView : MonoBehaviour
    {
        private IUserRequestHandler _userRequestHandler;

        [Header("뷰포트 설정")]
        [SerializeField]
        private TextMeshProUGUI targetFOVText;

        [SerializeField]
        private TMP_InputField targetWidthInputField;

        [SerializeField]
        private TMP_InputField targetHeightInputField;

        [Header("애니메이션 설정")]
        [SerializeField]
        private Button prevAnimationButton;

        [SerializeField]
        private Button nextAnimationButton;

        [SerializeField]
        private Button playAnimationButton;

        [SerializeField]
        private Button pauseAnimationButton;

        [SerializeField]
        private TextMeshProUGUI animationClipNameText;

        [Header("픽셀화")]
        [SerializeField]
        private Button pixelationSingleImageButton;

        [SerializeField]
        private Button pixelationAnimationButton;

        [SerializeField]
        private Button rollbackButton;

        [SerializeField]
        private Button saveButton;

        [Header("저장 팝업")]
        [SerializeField]
        private GameObject saveSingleImagePopup;

        [SerializeField]
        private TMP_InputField singleImageFileNameInputField;

        [SerializeField]
        private GameObject animationPixelationPopup;

        [SerializeField]
        private TMP_InputField animationImageFileNameInputField;

        [SerializeField]
        private TMP_InputField animationTargetFrameInputField;


        public void Initialization(IUserRequestHandler userRequestHandler)
        {
            _userRequestHandler = userRequestHandler;
            RegisterUIInputHandler();
        }

        #region UGUI Output

        public void SetViewportFOVText(float fov)
        {
            targetFOVText.text = fov.ToString("F1");
        }

        public void SetResolutionText(int width, int height)
        {
            targetWidthInputField.text = width.ToString("F0");
            targetHeightInputField.text = height.ToString("F0");
        }

        public void SetPixelationButtonStatus(EViewportMode viewportMode)
        {
            switch (viewportMode)
            {
                case EViewportMode.Model:
                    rollbackButton.interactable = false;
                    saveButton.interactable = false;
                    break;
                case EViewportMode.Result:
                    rollbackButton.interactable = true;
                    saveButton.interactable = true;
                    break;
            }
        }

        public void SetAnimationClipName(string clipName)
        {
            if (!string.IsNullOrEmpty(clipName))
            {
                animationClipNameText.text = clipName;
                prevAnimationButton.interactable = true;
                nextAnimationButton.interactable = true;
                playAnimationButton.interactable = true;
                pauseAnimationButton.interactable = true;
                pixelationAnimationButton.interactable = true;
            }
            else
            {
                animationClipNameText.text = "애니메이션 클립 없음";
                prevAnimationButton.interactable = false;
                nextAnimationButton.interactable = false;
                playAnimationButton.interactable = false;
                pauseAnimationButton.interactable = false;
                pixelationAnimationButton.interactable = false;
            }
        }

        public void SetAnimationControlButtons(bool isPlaying)
        {
            if (isPlaying)
            {
                playAnimationButton.gameObject.SetActive(false);
                pauseAnimationButton.gameObject.SetActive(true);
            }
            else
            {
                playAnimationButton.gameObject.SetActive(true);
                pauseAnimationButton.gameObject.SetActive(false);
            }
        }

        public void OpenSaveSingleImagePopup()
        {
            saveSingleImagePopup.SetActive(true);
        }

        public void OpenPixelationAnimationPopup()
        {
            animationPixelationPopup.SetActive(true);
        }

        #endregion

        #region UGUI Input

        private void RegisterUIInputHandler()
        {
            targetWidthInputField.onValueChanged.AddListener(SetViewportWidth);
            targetHeightInputField.onValueChanged.AddListener(SetViewportHeight);
        }

        public void MoveViewportUp()
        {
            _userRequestHandler.MoveViewport(Vector2.up);
        }

        public void MoveViewportDown()
        {
            _userRequestHandler.MoveViewport(Vector2.down);
        }

        public void MoveViewportLeft()
        {
            _userRequestHandler.MoveViewport(Vector2.left);
        }

        public void MoveViewportRight()
        {
            _userRequestHandler.MoveViewport(Vector2.right);
        }

        public void AdjustViewportSize(float offset)
        {
            _userRequestHandler.AdjustViewportSize(offset);
        }

        public void PixelationSingleImage()
        {
            _userRequestHandler.PixelationSingleImage();
        }

        public void PixelationAnimation()
        {
            if (!int.TryParse(animationTargetFrameInputField.text, out var targetFrame))
            {
                targetFrame = 5;
            }

            var fileName = "Animation_";

            if (!string.IsNullOrEmpty(animationImageFileNameInputField.text))
            {
                fileName = animationImageFileNameInputField.text;
            }

            _userRequestHandler.PixelationAnimation(targetFrame, fileName);
            animationPixelationPopup.SetActive(false);
            animationImageFileNameInputField.text = string.Empty;
        }

        public void SaveSingleImage()
        {
            var fileName = "Image";

            if (!string.IsNullOrEmpty(singleImageFileNameInputField.text))
            {
                fileName = singleImageFileNameInputField.text;
            }

            _userRequestHandler.SaveSingleImage(fileName);
            saveSingleImagePopup.SetActive(false);
            singleImageFileNameInputField.text = string.Empty;
        }

        public void Rollback()
        {
            _userRequestHandler.Rollback();
        }

        public void OpenPixelationOption()
        {
            _userRequestHandler.OpenPixelationOption();
        }

        public void OpenSavePath()
        {
            _userRequestHandler.OpenSavePath();
        }

        private void SetViewportWidth(string input)
        {
            if (int.TryParse(input, out var width))
            {
                _userRequestHandler.ViewportWidth = width;
            }
            else
            {
                Debug.LogError("Viewport Width 변환 실패");
            }
        }

        private void SetViewportHeight(string input)
        {
            if (int.TryParse(input, out var height))
            {
                _userRequestHandler.ViewportHeight = height;
            }
            else
            {
                Debug.LogError("Viewport Height 변환 실패");
            }
        }

        public void NextAnimation()
        {
            _userRequestHandler.NextAnimation();
        }

        public void PrevAnimation()
        {
            _userRequestHandler.PrevAnimation();
        }

        public void PlayAnimation()
        {
            _userRequestHandler.PlayAnimation();
        }

        public void PauseAnimation()
        {
            _userRequestHandler.PauseAnimation();
        }

        #endregion
    }
}