using System;
using ModelToPixelArt.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModelToPixelArt.Service
{
    public class StudioShortcutController : MonoBehaviour
    {
        [SerializeField]
        private Button nextAnimationButton;

        [SerializeField]
        private Button prevAnimationButton;


        [SerializeField]
        private Button PlayButton;

        [SerializeField]
        private Button PauseButton;

        [SerializeField]
        private Button pixelationButton;

        [SerializeField]
        private Button rollbackButton;

        [SerializeField]
        private Button saveButton;

        [SerializeField]
        private Button pixelationAnimationButton;

        [SerializeField]
        private Button saveConfirmButton;

        [SerializeField]
        private Button animationSaveConfirmButton;

        [SerializeField]
        private Button pixelationOptionButton;

        private readonly PointerEventData _pointerEventData = new(EventSystem.current);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanExecute(PauseButton))
                {
                    FunctionExecute(PauseButton);
                    return;
                }

                if (CanExecute(PlayButton))
                {
                    FunctionExecute(PlayButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                if (CanExecute(prevAnimationButton))
                {
                    FunctionExecute(prevAnimationButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                if (CanExecute(nextAnimationButton))
                {
                    FunctionExecute(nextAnimationButton);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (CanExecute(pixelationOptionButton))
                {
                    FunctionExecute(pixelationOptionButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (CanExecute(pixelationButton))
                {
                    FunctionExecute(pixelationButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (CanExecute(rollbackButton))
                {
                    FunctionExecute(rollbackButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (CanExecute(saveConfirmButton))
                {
                    FunctionExecute(saveConfirmButton);
                    return;
                }

                if (CanExecute(animationSaveConfirmButton))
                {
                    FunctionExecute(animationSaveConfirmButton);
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                if (CanExecute(pixelationAnimationButton))
                {
                    FunctionExecute(pixelationAnimationButton);
                    return;
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                if (CanExecute(saveButton))
                {
                    FunctionExecute(saveButton);
                    return;
                }
            }
        }

        private bool CanExecute(Button button)
        {
            if (!button.gameObject.activeInHierarchy) return false;
            if (!button.interactable) return false;
            return true;
        }

        private void FunctionExecute(Button button)
        {
            button.OnPointerClick(_pointerEventData);
        }
    }
}