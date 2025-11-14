using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MyProject.Common.UI
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _canvasGroupSetter = null;

        private static readonly float _animationTime = 0.25f;

        /// <summary>
        /// フェードイン（非同期）
        /// </summary>
        public async UniTask PlayFadeInAsync()
        {
            _canvasGroupSetter.PlayFadeInAnimation(_animationTime, Ease.InSine);
            await UniTask.WaitWhile(() => _canvasGroupSetter.IsPlayingFadeAnimation());
        }

        /// <summary>
        /// フェードアウト（非同期）
        /// </summary>
        public async UniTask PlayFadeOutAsync()
        {
            _canvasGroupSetter.PlayFadeOutAnimation(_animationTime, Ease.InSine);
            await UniTask.WaitWhile(() => _canvasGroupSetter.IsPlayingFadeAnimation());
        }
    }
}