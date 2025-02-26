using System;
using System.Collections;
using System.IO;
using ModelToPixelArt.Definition;
using ModelToPixelArt.Interface;
using ModelToPixelArt.Module;
using ModelToPixelArt.Module.PixelationProcess;
using ModelToPixelArt.Service.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ModelToPixelArt.Service
{
    public class ModelToPixelArtServiceController : MonoBehaviour, IUserRequestHandler
    {
        [SerializeField]
        private PixelationOptions pixelationOptions;

        private StudioUIView _studioUIView;
        private StudioShortcutController _studioShortcutController;
        private ViewportController _viewportController;
        private AnimationController _animationController;

        public int ViewportWidth
        {
            set => _viewportController.Width = value;
        }

        public int ViewportHeight
        {
            set => _viewportController.Height = value;
        }


        private void Awake()
        {
            _studioUIView = FindAnyObjectByType<StudioUIView>();
            _studioUIView.Initialization(this);

            _viewportController = FindAnyObjectByType<ViewportController>();
            _viewportController.OnFOVChanged += _studioUIView.SetViewportFOVText;
            _viewportController.OnViewportSizeChanged += _studioUIView.SetResolutionText;
            _viewportController.OnViewportModeChanged += _studioUIView.SetPixelationButtonStatus;
            _viewportController.Initialization();

            _animationController = FindAnyObjectByType<AnimationController>();
            _animationController.OnAnimationClipChanged += _studioUIView.SetAnimationClipName;
            _animationController.OnAnimationPlayStatusChanged += _studioUIView.SetAnimationControlButtons;
            _animationController.Initialization();
        }

        public void MoveViewport(Vector2 direction)
        {
            _viewportController.Move(direction);
        }

        public void AdjustViewportSize(float offset)
        {
            _viewportController.AdjustFOV(offset);
        }

        public void NextAnimation()
        {
            _animationController.NextClip();
        }

        public void PrevAnimation()
        {
            _animationController.PrevClip();
        }

        public void PlayAnimation()
        {
            _animationController.IsPlaying = true;
        }

        public void PauseAnimation()
        {
            _animationController.IsPlaying = false;
        }

        public void PixelationSingleImage()
        {
            Time.timeScale = 0;
            var inputTexture = _viewportController.Capture();
            PixelationController.Pixelation(inputTexture, pixelationOptions, OnPixelationComplete);

            void OnPixelationComplete(Texture2D outputTexture)
            {
                _viewportController.ResultTexture = outputTexture;
                _viewportController.ViewportMode = EViewportMode.Result;
            }
        }

        public void Rollback()
        {
            Time.timeScale = 1;
            _viewportController.ViewportMode = EViewportMode.Model;
        }

        // 애니메이션 픽셀화
        public void PixelationAnimation(int targetFrame, string fileName)
        {
            // 3단계를 반복하여 애니메이션을 프레임별로 픽셀화한다.
            // 애니메이션 시간 지정 -> 픽셀화 -> 저장

            // 애니메이션 시간은 정규화된 float 값으로 지정한다.
            // 목표 프레임을 낮추어 계산하여 클립의 시작과 끝을 캡쳐한다.
            targetFrame--;
            var timeUnit = 1 / (float)targetFrame;
            var currentFrame = 0;

            SetAnimationTime();

            void SetAnimationTime()
            {
                // 목표 프레임만큼 촬영한 뒤 로직은 종료된다.
                if (targetFrame < currentFrame)
                {
                    OnComplete();
                    return;
                }

                _animationController.IsPlaying = false;
                _animationController.SetAnimationTime(currentFrame * timeUnit, OnSetTimeComplete);
            }

            void OnSetTimeComplete()
            {
                var inputTexture = _viewportController.Capture();
                PixelationController.Pixelation(inputTexture, pixelationOptions, OnPixelationComplete);
            }

            void OnPixelationComplete(Texture2D outputTexture)
            {
                _viewportController.ResultTexture = outputTexture;
                _viewportController.ViewportMode = EViewportMode.Result;
                SaveImage($"{fileName}{currentFrame}", outputTexture, OnSaveComplete);
            }

            void OnSaveComplete()
            {
                currentFrame++;
                SetAnimationTime();
            }

            void OnComplete()
            {
                Rollback();
                _animationController.IsPlaying = true;
                Debug.Log("애니메이션 변환 완료");
            }
        }

        public void OpenSaveSingleImagePopup()
        {
            throw new NotImplementedException();
        }

        public void SaveSingleImage(string fileName)
        {
            SaveImage(fileName, _viewportController.ResultTexture);
        }

        public void OpenPixelationOption()
        {
            Selection.activeObject = pixelationOptions;
        }

        public void OpenSavePath()
        {
            FileSaver.OpenFolder();
        }

        private void SaveImage(string filename, Texture2D texture, Action onComplete = null)
        {
            FileSaver.SaveFileInPersistentDataPath($"{filename}.png", texture.EncodeToPNG(), OnSaveComplete);

            void OnSaveComplete()
            {
                Debug.Log($"파일 저장 완료 - 경로 : {Application.persistentDataPath}/{filename}.png");
                onComplete?.Invoke();
            }
        }
    }
}