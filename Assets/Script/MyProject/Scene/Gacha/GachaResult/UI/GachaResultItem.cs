using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyProject.Common.UI;
using MyProject.Systems.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Gacha.Result
{
    public class GachaResultItem : MonoBehaviour
    {
        [SerializeField] private GameObject _rootObj = null;
        [SerializeField] private CanvasGroupSetter _canvasGroupSetter = null;
        [SerializeField] private Image _bgImage = null;
        [SerializeField] private RawImage _thumbnailImage = null;
        [SerializeField] private TextMeshProUGUI _itemNameText = null;
        [SerializeField] private Vector3 _defaultScale = new Vector3(0.5f, 0.5f, 1f);

        private static readonly string _textureFilePath = "UI/Gacha/GachaResult/Texture/";

        private static readonly float _animationTime = 0.15f;

        private Texture2D _thumbnail = null;
        private string _thumbnailPath = string.Empty;

        private bool _isSetting = false;
        public bool IsSetting { get { return _isSetting; } }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            SetActive(true);
            Hide();
        }

        /// <summary>
        /// アイテム情報を設定（非同期）
        /// </summary>
        /// <param name="bgColor">背景色</param>
        /// <param name="texture">サムネイル</param>
        /// <param name="name">アイテム名</param>
        public async UniTask SetupItemAsync(Color bgColor, string textureName, string name)
        {
            _isSetting = true;

            SetBgColor(bgColor);
            SetItemNameText(name);

            _thumbnailPath = $"{_textureFilePath}{textureName}";

            Debug.Log(_thumbnailPath);

            await ResourceManager.Local.LoadAssetAsync<Texture2D>(_thumbnailPath);
            _thumbnail = ResourceManager.Local.GetAsset<Texture2D>(_thumbnailPath);
            SetThumbnail(_thumbnail);

            _isSetting = false;
        }

        /// <summary>
        /// 解放（非同期）
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            await UniTask.WaitWhile(() => _isSetting);

            _canvasGroupSetter.KillScaleSequence();
            Hide();

            SetItemNameText(string.Empty);
            SetThumbnail(null);
            SetBgColor(Color.white);

            if (_thumbnail != null)
            {
                Destroy(_thumbnail);
                _thumbnail = null;
            }

            ResourceManager.Local.UnloadAssets(_thumbnailPath);
            _thumbnailPath = string.Empty;
        }

        /// <summary>
        /// オブジェクトのアクティブ設定
        /// </summary>
        /// <param name="isActive">true: アクティブ, false: 非アクティブ</param>
        public void SetActive(bool isActive)
        {
            _rootObj.SetActive(isActive);
        }

        /// <summary>
        /// アイテムを表示
        /// </summary>
        public void View()
        {
            _canvasGroupSetter.View();

            _canvasGroupSetter.KillScaleSequence();
            _canvasGroupSetter.PlayScaleAnimation(Vector3.one, _animationTime, Ease.OutBack);
        }

        /// <summary>
        /// アイテムを非表示
        /// </summary>
        public void Hide()
        {
            _canvasGroupSetter.Hide();
            _canvasGroupSetter.SetLocalScale(_defaultScale);
        }

        /// <summary>
        /// アイテムのアニメーションを再生中？
        /// </summary>
        /// <returns>true: 再生中, false: 停止</returns>
        public bool IsPlayingScaleAnimation()
        {
            return _canvasGroupSetter.IsPlayingScaleAnimation();
        }

        /// <summary>
        /// CanvasGroupを表示
        /// </summary>
        public void ViewCanvasGroup()
        {
            _canvasGroupSetter.View();
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