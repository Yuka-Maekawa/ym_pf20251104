using Cysharp.Threading.Tasks;
using MyProject.Common.UI;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultUIController : MonoBehaviour
    {
        [SerializeField] private GachaResultMenu _resultMenu = null;
        [SerializeField] private CanvasGroupSetter _buttonCanvasGroupSetter = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="itemNum">アイテム数</param>
        public void Initialize(int itemNum)
        {
            _resultMenu.Initialize(itemNum);
            _buttonCanvasGroupSetter.Hide();
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
        /// ウィンドウを閉じる（非同期）
        /// </summary>
        public async UniTask CloseAsync()
        {
            _buttonCanvasGroupSetter.Hide();
            await _resultMenu.CloseAsync();
        }

        /// <summary>
        /// ウィンドウアニメーションを再生中
        /// </summary>
        /// <returns>true: 再生中, false: 停止</returns>
        public bool IsPlayingWindowAnimation()
        {
            return _resultMenu.IsPlayingWindowAnimation();
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

        /// <summary>
        /// 前のシーンに戻るボタンを表示
        /// </summary>
        public void ViewBackSceneButton()
        {
            _buttonCanvasGroupSetter.View();
        }
    }
}