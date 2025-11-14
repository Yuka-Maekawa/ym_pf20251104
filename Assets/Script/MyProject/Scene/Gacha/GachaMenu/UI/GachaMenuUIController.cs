using Cysharp.Threading.Tasks;
using MyProject.Common.UI;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _canvasGroupSetter = null;

        private static readonly string _onceResultScenePath = "Scene/Gacha/GachaOnceResult";
        private static readonly string _tenTimesResultScenePath = "Scene/Gacha/GachaTenTimesResult";

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
            _canvasGroupSetter.View();
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            _canvasGroupSetter.Hide();
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