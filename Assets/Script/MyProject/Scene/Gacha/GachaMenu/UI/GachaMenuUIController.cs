using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuUIController : MonoBehaviour
    {
        private static readonly string _onceResultScenePath = "Scene/Gacha/GachaOnceResult";
        private static readonly string _tenTimesResultScenePath = "Scene/Gacha/GachaTenTimesResult";

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