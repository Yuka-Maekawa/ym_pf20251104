using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
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
        /// ウィンドウを開く
        /// </summary>
        public void Open()
        {
            _resultMenu.Open();
        }

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        public void Close()
        {
            _resultMenu.Close();
        }

        /// <summary>
        /// ウィンドウが開くアニメーションが終了しているか？
        /// </summary>
        /// <returns>true: 終了, false: 再生中</returns>
        public bool IsEndOpenAnimation()
        {
            return _resultMenu.IsEndOpenAnimation();
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
        /// 全てを表示(非同期)
        /// </summary>
        public async UniTask ViewAllItemAsync()
        {
            await _resultMenu.ViewAllItemAsync();
        }
    }
}