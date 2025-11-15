using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatiorProgressGage : MonoBehaviour
    {
        [SerializeField] private GameObject _rootObject= null;
        [SerializeField] private TextMeshProUGUI _progressText = null;
        [SerializeField] private Slider _slider = null;

        private static readonly float _barMinValue = 0f;

        private int _taskMaxNum = 0;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            ResetProgress();
            Hide();
        }
        
        /// <summary>
        /// 進捗状況をリセット
        /// </summary>
        private void ResetProgress()
        {
            SetText(string.Empty);
            SetSlider(_barMinValue);
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="taskMaxNum">仕事量</param>
        public void View(int taskMaxNum)
        {
            _rootObject.SetActive(true);
            ResetProgress();

            _taskMaxNum = taskMaxNum;
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            _rootObject.SetActive(false);
        }

        /// <summary>
        /// 進捗更新
        /// </summary>
        /// <param name="progress">進捗の数</param>
        public void UpdateProgress(int progressNum)
        {
            SetText($"{progressNum} / {_taskMaxNum}");
            SetSlider((float)(progressNum / (float)_taskMaxNum));
        }

        /// <summary>
        /// テキスト設定
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            _progressText.SetText(text);
        }

        /// <summary>
        /// スライドの値設定
        /// </summary>
        /// <param name="value">値</param>
        private void SetSlider(float value)
        {
            _slider.value = value;
        }
    }
}