using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelToPixelArt.Module
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private AnimationClip[] _clips;
        private int _currentClipIndex = 0;
        private bool _isPlaying = true;

        public event Action<bool> OnAnimationPlayStatusChanged;
        public event Action<string> OnAnimationClipChanged;

        public bool IsPlaying
        {
            set
            {
                _isPlaying = value;
                OnAnimationPlayStatusChanged?.Invoke(_isPlaying);
            }
        }

        public void Initialization()
        {
            OnAnimationClipChanged += SetAnimationClip;
            OnAnimationPlayStatusChanged += SetAnimationPlayStatus;

            StartCoroutine(Co_FindAnimator());
        }

        public void NextClip()
        {
            if (_animator == null || _clips == null)
            {
                OnAnimationClipChanged?.Invoke("애니메이션 클립 없음");
                return;
            }

            _currentClipIndex++;

            if (_clips.Length <= _currentClipIndex)
            {
                _currentClipIndex = 0;
            }

            var clipName = GetAnimationClipName(_currentClipIndex);
            OnAnimationClipChanged?.Invoke(clipName);
        }

        public void PrevClip()
        {
            if (_animator == null || _clips == null)
            {
                OnAnimationClipChanged?.Invoke("애니메이션 클립 없음");
                return;
            }

            _currentClipIndex--;

            if (_currentClipIndex < 0)
            {
                _currentClipIndex = _clips.Length - 1; // 마지막 인덱스로 이동
            }

            var clipName = GetAnimationClipName(_currentClipIndex);
            OnAnimationClipChanged?.Invoke(clipName);
        }

        // 애니메이션 재생 지점 설정
        // 렌더가 완료될 때 까지 대기 후 완료 콜백을 호출한다.
        public void SetAnimationTime(float normalizedTime, Action onComplete)
        {
            if (_animator == null) return;
            StartCoroutine(Co_SetAnimationTime(normalizedTime, onComplete));
        }

        private IEnumerator Co_SetAnimationTime(float normalizedTime, Action onComplete)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            _animator.Play(stateInfo.fullPathHash, 0, normalizedTime);
            yield return null;
            onComplete?.Invoke();
        }

        private void SetAnimationPlayStatus(bool isPlaying)
        {
            if (_animator == null) return;
            _animator.speed = isPlaying ? 1 : 0;
        }

        private void SetAnimationClip(string clipName)
        {
            if (_animator == null || _clips == null)
            {
                return;
            }

            _animator.Play(clipName);
        }

        private string GetAnimationClipName(int index)
        {
            if (_animator == null || _clips == null)
            {
                return null;
            }

            Debug.Assert(0 <= index && index <= _clips.Length, $"Animation Index 오류 {index}");
            return _clips[_currentClipIndex].name;
        }
        
        private void DisableTransitionExitTime(Animator animator)
        {
            var disabledAnimatorController = animator.runtimeAnimatorController as AnimatorController;

            if (disabledAnimatorController == null) return;
            
            foreach (var layer in disabledAnimatorController.layers)
            {
                var stateMachine = layer.stateMachine;

                foreach (var state in stateMachine.states)
                {
                    foreach (var transition in state.state.transitions)
                    {
                        transition.hasExitTime = false;
                    }
                }
            }

            animator.runtimeAnimatorController = disabledAnimatorController;
        }

        private IEnumerator Co_FindAnimator()
        {
            var findDelay = new WaitForSecondsRealtime(0.3f);

            while (true)
            {
                if (_animator == null)
                {
                    _animator = FindAnyObjectByType<Animator>();

                    if (_animator != null)
                    {
                        DisableTransitionExitTime(_animator);
                        _clips = _animator.runtimeAnimatorController.animationClips;
                        var clipName = GetAnimationClipName(_currentClipIndex);

                        OnAnimationClipChanged?.Invoke(clipName);
                        OnAnimationPlayStatusChanged?.Invoke(_isPlaying);
                    }
                    else
                    {
                        OnAnimationClipChanged?.Invoke(null);
                    }
                }

                yield return findDelay;
            }
        }
    }
}