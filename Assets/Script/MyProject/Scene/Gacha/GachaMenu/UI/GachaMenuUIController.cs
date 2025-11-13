using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        private static readonly string _onceResultScenePath = "Scene/Gacha/GachaOnceResult";
        private static readonly string _tenTimesResultScenePath = "Scene/Gacha/GachaTenTimesResult";
        private static readonly float _viewAlpha = 1f;
        private static readonly float _hideAlpha = 0f;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            Close();
        }

        /// <summary>
        /// 開く
        /// </summary>
        public void Open()
        {
            _canvasGroup.alpha = _viewAlpha;
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            _canvasGroup.alpha = _hideAlpha;
        }

        /// <summary>
        /// 単発ボタンを押下
        /// </summary>
        public void PushOneGachaButton()
        {
            NextSceneAsync(_onceResultScenePath).Forget();
        }

        /// <summary>
        /// 10回ボタンを押下
        /// </summary>
        public void PushTenTimeGachaButton()
        {
            NextSceneAsync(_tenTimesResultScenePath).Forget();
        }

        /// <summary>
        /// 次のシーンへ移動
        /// </summary>
        /// <param name="nextScenePath">シーンのファイルパス</param>
        private async UniTask NextSceneAsync(string nextScenePath)
        {
            await SceneLoader.SceneLoad.LoadSceneAsync(nextScenePath);
        }
    }
}