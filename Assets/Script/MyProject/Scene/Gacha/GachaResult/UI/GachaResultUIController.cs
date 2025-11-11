using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultUIController : MonoBehaviour
    {
        [SerializeField] private GachaResultMenu _resultMenu = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="itemNum">アイテム数</param>
        public void Initialize(int itemNum)
        {
            _resultMenu.Initialize(itemNum);
        }

        /// <summary>
        /// 解放(非同期)
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            await _resultMenu.ReleaseAsync();
        }

        /// <summary>
        /// アイテム設定
        /// </summary>
        public async UniTask SetupItemsAsync(GachaLotteryControllerBase.ItemInfo itemInfo, int index)
        {
            await _resultMenu.SetupItemsAsync(itemInfo, index);
        }

        /// <summary>
        /// アイテム設定中か？
        /// </summary>
        /// <returns>true: 設定中, false: 設定終了</returns>
        public bool IsMenuItemsSetting()
        {
            return _resultMenu.IsMenuItemsSetting();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void ViewAllItem()
        {
            _resultMenu.ViewAllItem();
        }

        /// <summary>
        /// 単発ボタンを押下
        /// </summary>
        public void PushOneGachaButton()
        {
            NextSceneAsync("Scene/Gacha/GachaMenu").Forget();
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