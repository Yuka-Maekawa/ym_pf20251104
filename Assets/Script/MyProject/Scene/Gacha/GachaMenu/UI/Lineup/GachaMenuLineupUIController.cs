using Cysharp.Threading.Tasks;
using MyProject.Common.UI;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _buttonCanvasGroupSetter = null;
        [SerializeField] private GachaMenuLineupWindow _lineupWindow = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync()
        {
            _buttonCanvasGroupSetter.Hide();
            await _lineupWindow.InitializeAsync();
        }

        /// <summary>
        /// 解放（非同期）
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            await _lineupWindow.ReleaseAsync();
        }

        /// <summary>
        /// 開く（非同期）
        /// </summary>
        public async UniTask OpenAsync()
        {
            await _lineupWindow.OpenAsync();
            _buttonCanvasGroupSetter.View();
        }

        /// <summary>
        /// 閉じる（非同期）
        /// </summary>
        public async UniTask CloseAsync()
        {
            _buttonCanvasGroupSetter.Hide();

            await _lineupWindow.CloseAsync();
        }
    }
}