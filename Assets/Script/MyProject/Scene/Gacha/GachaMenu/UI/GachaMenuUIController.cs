using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuUIController : MonoBehaviour
    {
        /// <summary>
        /// 単発ボタンを押下
        /// </summary>
        public void PushOneGachaButton()
        {
            NextSceneAsync("Scene/Gacha/GachaOneceResult").Forget();
        }

        /// <summary>
        /// 10回ボタンを押下
        /// </summary>
        public void PushTenTimeGachaButton()
        {
            NextSceneAsync("Scene/Gacha/GachaTenTimeResult").Forget();
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