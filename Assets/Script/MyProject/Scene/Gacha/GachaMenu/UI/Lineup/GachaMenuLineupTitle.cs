using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rarityText = null;
        [SerializeField] private Image _lineImage = null;

        /// <summary>
        /// タイトルUIのテキスト設定
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="color">線の色</param>
        public void SetupTitle(string text, Color color)
        {
            SetRarityText(text);
            SetLineColor(color);
        }

        /// <summary>
        /// レアリティのテキスト設定
        /// </summary>
        /// <param name="text">テキスト</param>
        private void SetRarityText(string text)
        {
            _rarityText.SetText(text);
        }

        /// <summary>
        /// 線の色を設定
        /// </summary>
        /// <param name="color">線の色</param>
        private void SetLineColor(Color color)
        {
            _lineImage.color = color;
        }
    }
}