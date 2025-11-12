using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultBackSceneButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        private static readonly float _viewAlpha = 1f;
        private static readonly float _hideAlpha = 0f;

        /// <summary>
        /// 表示
        /// </summary>
        public void View()
        {
            SetCanvasGroupAlpha(_viewAlpha);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            SetCanvasGroupAlpha(_hideAlpha);
        }

        /// <summary>
        /// CanvasGroupのアルファを変更
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        public void SetCanvasGroupAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }
    }
}