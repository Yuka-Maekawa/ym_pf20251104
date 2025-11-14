using UnityEngine;

namespace MyProject.Common.UI
{
    public class CanvasGroupSetter : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        private static readonly float _viewAlpha = 1f;
        private static readonly float _hideAlpha = 0f;

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
    }
}