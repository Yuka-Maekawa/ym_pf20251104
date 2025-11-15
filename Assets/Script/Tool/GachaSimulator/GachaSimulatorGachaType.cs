using MyProject.Gacha.Lottery;
using TMPro;
using UnityEngine;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatorGachaType : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lotteryTypeText = null;

        private static readonly int _selectLotteryTypeNum = System.Enum.GetValues(typeof(GachaSimulatorMain.LotteryType)).Length;
        private static readonly int _cursorValue = 1;

        private GachaSimulatorMain.LotteryType _selectLotteryType = GachaSimulatorMain.LotteryType.Default;

        /// <summary>
        ///  初期化
        /// </summary>
        public void Initialize()
        {
            _selectLotteryType = GachaSimulatorMain.LotteryType.Default;
            SetText(_selectLotteryType);
        }

        /// <summary>
        /// 選択中のLotteryTypeを取得
        /// </summary>
        /// <returns>LotteryType</returns>
        public GachaSimulatorMain.LotteryType GetSelectLotteryType()
        {
            return _selectLotteryType;
        }

        /// <summary>
        /// < ボタンを押下
        /// </summary>
        public void PushLeftButton()
        {
            SetSelectLotteryType(-_cursorValue);
            SetText(_selectLotteryType);
        }

        /// <summary>
        /// > ボタンを押下
        /// </summary>
        public void PushRightButton()
        {
            SetSelectLotteryType(_cursorValue);
            SetText(_selectLotteryType);
        }

        /// <summary>
        /// 選択
        /// </summary>
        /// <param name="addValue">加算値</param>
        private void SetSelectLotteryType(int addValue)
        {
            _selectLotteryType = (GachaSimulatorMain.LotteryType)Mathf.Repeat((int)_selectLotteryType + addValue, _selectLotteryTypeNum);
        }

        /// <summary>
        /// テキストを設定
        /// </summary>
        /// <param name="lotteryType">LotteryType</param>
        private void SetText(GachaSimulatorMain.LotteryType lotteryType)
        {
            _lotteryTypeText.SetText($"{lotteryType}");
        }
    }
}