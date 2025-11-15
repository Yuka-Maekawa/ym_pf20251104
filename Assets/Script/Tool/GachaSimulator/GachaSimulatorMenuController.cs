using UnityEngine;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatorMenuController : MonoBehaviour
    {
        [SerializeField] private GachaSimulatorInputFieldController _simulationIdText = null;
        [SerializeField] private GachaSimulatorInputFieldController _simulationCountText = null;
        [SerializeField] private GachaSimulatorGachaType _simulationType = null;

        private static readonly int _defaultId = 1;
        private static readonly int _defaultSimulationCount = 1;

        /// <summary>
        ///  初期化
        /// </summary>
        public void Initialize()
        {
            _simulationIdText.SetText($"{_defaultId}");
            _simulationCountText.SetText($"{_defaultSimulationCount}");

            _simulationType.Initialize();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            ResetAllText(string.Empty);
        }

        /// <summary>
        /// シミュレーションするIdを取得
        /// </summary>
        /// <returns>Id(数値)</returns>
        public int GetSimulationId()
        {
            var text = _simulationIdText.GetInputFieldText();

            if(int.TryParse(text, out int id))
            {
                return id;
            }

            Debug.LogError("数値以外の文字が入力されています");

            return -1;
        }

        /// <summary>
        /// シミュレーションする回数を取得
        /// </summary>
        /// <returns>回数</returns>
        public int GetSimulationCount()
        {
            var text = _simulationCountText.GetInputFieldText();

            if (int.TryParse(text, out int playCount))
            {
                return playCount;
            }

            Debug.LogError("数値以外の文字が入力されています");

            return -1;
        }

        /// <summary>
        /// すべてのテキストをリセット
        /// </summary>
        /// <param name="text">テキスト</param>
        private void ResetAllText(string text)
        {
            _simulationIdText.SetText(text);
            _simulationCountText.SetText(text);
        }

        /// <summary>
        /// シミュレーションタイプの < ボタンを押下
        /// </summary>
        public void PushSimulationTypeLeftButton()
        {
            _simulationType.PushRightButton();
        }

        /// <summary>
        /// シミュレーションタイプの > ボタンを押下
        /// </summary>
        public void PushSimulationTypeRightButton()
        {
            _simulationType.PushRightButton();
        }

        /// <summary>
        /// 選択中のLotteryTypeを取得
        /// </summary>
        /// <returns>LotteryType</returns>
        public GachaSimulatorMain.LotteryType GetSelectLotteryType()
        {
            return _simulationType.GetSelectLotteryType();
        }
    }
}