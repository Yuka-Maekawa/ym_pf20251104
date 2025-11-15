using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatorInputFieldController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputFieldText = null;

        /// <summary>
        /// テキスト設定
        /// </summary>
        /// <param name="text">テキスト</param>
        public void SetText(string text)
        {
            _inputFieldText.SetTextWithoutNotify(text);
        }

        /// <summary>
        /// InputFieldのテキストを取得
        /// </summary>
        /// <returns>テキスト</returns>
        public string GetInputFieldText()
        {
            return _inputFieldText.text;
        }
    }
}