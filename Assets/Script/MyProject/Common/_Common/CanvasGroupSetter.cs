using DG.Tweening;
using UnityEngine;

namespace MyProject.Common.UI
{
    public class CanvasGroupSetter : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        private static readonly float _viewAlpha = 1f;
        private static readonly float _hideAlpha = 0f;

        private Sequence _fadeSequence = null;
        private Sequence _scaleSequence = null;

        private bool _isPlayingFadeAnimation = false;
        private bool _isPlayingScaleAnimation = false;

        public void Reset()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = this.GetComponentInChildren<CanvasGroup>();
            }
        }

        /// <summary>
        /// CanvasGroupを取得
        /// </summary>
        /// <returns>CanvasGroup</returns>
        public CanvasGroup GetCanvasGroup()
        {
            return _canvasGroup;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public void View()
        {
            SetAlpha(_viewAlpha);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            SetAlpha(_hideAlpha);
        }

        /// <summary>
        /// CanvasGroupのアルファ設定
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        /// <summary>
        /// Transformを取得
        /// </summary>
        /// <returns>Transform</returns>
        public Transform GetTransform()
        {
            return _canvasGroup.transform;
        }

        /// <summary>
        /// LocalScaleの値を設定
        /// </summary>
        /// <param name="localScale">スケール値</param>
        public void SetLocalScale(Vector3 localScale)
        {
            transform.localScale = localScale;
        }

        /// <summary>
        /// シーケンスを全てKill
        /// </summary>
        public void KillAllSequence()
        {
            KillFadeSequence();
            KillScaleSequence();
        }

        /// <summary>
        /// フェードのSequenceをKill
        /// </summary>
        public void KillFadeSequence()
        {
            if (_fadeSequence != null)
            {
                _isPlayingFadeAnimation = false;
                _fadeSequence.Kill();
                _fadeSequence = null;
            }
        }

        /// <summary>
        /// スケールアニメーションのSequenceをKill
        /// </summary>
        public void KillScaleSequence()
        {
            if (_scaleSequence != null)
            {
                _isPlayingScaleAnimation = false;
                _scaleSequence.Kill(true);
                _scaleSequence = null;
            }
        }

        /// <summary>
        /// フェードアニメーションを再生
        /// </summary>
        /// <param name="animationTime">アニメーションの時間</param>
        /// <param name="ease">Ease</param>
        public void PlayFadeAnimation(float animationTime, Ease ease)
        {
            KillFadeSequence();

            _isPlayingScaleAnimation = true;
            _fadeSequence = DOTween.Sequence();
            _fadeSequence.Append(_canvasGroup.DOFade(_viewAlpha, animationTime).SetEase(Ease.InOutSine))
                .OnComplete(() => { _isPlayingFadeAnimation = false; });
        }

        /// <summary>
        /// フェードアニメーションを再生
        /// </summary>
        /// <param name="scale">変更するサイズ</param>
        /// <param name="animationTime">アニメーションの時間</param>
        /// <param name="ease">Ease</param>
        public void PlayScaleAnimation(Vector3 scale, float animationTime, Ease ease)
        {
            KillScaleSequence();

            _isPlayingScaleAnimation = true;
            _scaleSequence = DOTween.Sequence();
            _scaleSequence.Append(transform.DOScale(scale, animationTime).SetEase(ease))
                .OnComplete(() => { _isPlayingScaleAnimation = false; });
        }

        /// <summary>
        /// フェードアニメーションを再生中？
        /// </summary>
        /// <returns>true: 再生中, false: 停止</returns>
        public bool IsPlayingFadeAnimation()
        {
            return _isPlayingFadeAnimation;
        }

        /// <summary>
        /// スケールアニメーションを再生中？
        /// </summary>
        /// <returns>true: 再生中, false: 停止</returns>
        public bool IsPlayingScaleAnimation()
        {
            return _isPlayingScaleAnimation;
        }
    }
}