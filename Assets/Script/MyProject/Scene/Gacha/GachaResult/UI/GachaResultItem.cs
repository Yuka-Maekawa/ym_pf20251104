using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Gacha.Menu
{
    public class GachaResultItem : MonoBehaviour
    {
        [SerializeField] private Image _bgImage = null;
        [SerializeField] private RawImage _thumbnailImage = null;
        [SerializeField] private TextMeshProUGUI _itemNameText = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="bgColor">背景色</param>
        /// <param name="texture">サムネイル</param>
        /// <param name="name">アイテム名</param>
        public void Initialize(Color bgColor, Texture texture, string name)
        {
            SetBgColor(bgColor);
            SetThumbnail(texture);
            SetItemNameText(name);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            SetItemNameText(string.Empty);
            SetThumbnail(null);
            SetBgColor(Color.white);
        }

        /// <summary>
        /// 背景の色を設定
        /// </summary>
        /// <param name="color">背景色</param>
        public void SetBgColor(Color color)
        {
            _bgImage.color = color;
        }

        /// <summary>
        /// サムネイル設定
        /// </summary>
        /// <param name="texture"></param>
        public void SetThumbnail(Texture texture)
        {
            _thumbnailImage.texture = texture;
        }

        /// <summary>
        /// アイテム名を設定
        /// </summary>
        /// <param name="name">アイテム名</param>
        public void SetItemNameText(string name)
        {
            _itemNameText.SetText(name);
        }
    }
}