using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyProject.Common.UI;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _canvasGroupSetter = null;
        [SerializeField] private CanvasGroupSetter _bgCanvasGroupSetter = null;
        [SerializeField] private Transform _baseParent = null;
        [SerializeField] private GameObject _baseItemObj = null;
        [SerializeField] private Color[] _bgColors = null;

        private static readonly Vector3 _defaultScale = new Vector3(1f, 0f, 1f);
        private static readonly Vector3 _animationScale = new Vector3(1f, 1f, 1f);
        private static readonly float _animationTime = 0.5f;

        private static readonly float _viewAlpha = 1f;

        private GameObject[] _itemObjs = null;
        private GachaResultItem[] _items = null;

        private Sequence _fadeSequence = null;
        private Sequence _scaleSequence = null;

        private bool _isFadeAnimation = false;
        private bool _isScaleAnimation = false;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="itemNum">アイテム数</param>
        public void Initialize(int itemNum)
        {
            Close();

            _baseItemObj.SetActive(false);

            _itemObjs = new GameObject[itemNum];
            _items = new GachaResultItem[itemNum];
            for (int i = 0; i < itemNum; ++i)
            {
                var obj = Instantiate(_baseItemObj, _baseParent);
                obj.SetActive(true);

                var item = obj.GetComponent<GachaResultItem>();
                item.Initialize();

                _items[i] = item;
                _itemObjs[i] = obj;
            }
        }

        /// <summary>
        /// 解放(非同期)
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            KillFadeSequence();
            KillScaleSequence();
            await UnloadItemsAsync();
        }

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        public void Open()
        {
            KillFadeSequence();
            KillScaleSequence();

            _canvasGroupSetter.View();

            _fadeSequence = DOTween.Sequence();
            var bgCanvasGroup = _bgCanvasGroupSetter.GetCanvasGroup();
            _fadeSequence.Append(bgCanvasGroup.DOFade(_viewAlpha, _animationTime).SetEase(Ease.InOutSine))
                .OnComplete(() => { _isFadeAnimation = true; });

            _scaleSequence = DOTween.Sequence();
            var bgCanvasGroupTransform = _bgCanvasGroupSetter.GetTransform();
            _scaleSequence.Append(bgCanvasGroupTransform.DOScale(_animationScale, _animationTime).SetEase(Ease.InOutBack))
                .OnComplete(() => { _isScaleAnimation = true; });
        }

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        public void Close()
        {
            _canvasGroupSetter.Hide();
            _bgCanvasGroupSetter.Hide();
            _bgCanvasGroupSetter.SetLocalScale(_defaultScale);
        }

        /// <summary>
        /// アイテム情報を開放
        /// </summary>
        public async UniTask UnloadItemsAsync()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                await _items[i].ReleaseAsync();
                _items[i] = null;

                Destroy(_itemObjs[i]);
                _itemObjs[i] = null;
            }

            _items = null;
            _itemObjs = null;
        }

        /// <summary>
        /// アイテム設定
        /// </summary>
        public async UniTask SetupItemsAsync(GachaLotteryControllerBase.ItemInfo itemInfo, int index)
        {
            var item = itemInfo.Item;
            await _items[index].SetupItemAsync(_bgColors[(int)itemInfo.Rarity], item.ThumbnailName, item.Name);
        }

        /// <summary>
        /// アイテム設定中か？
        /// </summary>
        /// <returns>true: 設定中, false: 設定終了</returns>
        public bool IsMenuItemsSetting()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                if (_items[i].IsSetting)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// アイテムを全て表示
        /// </summary>
        public async UniTask ViewAllItemAsync()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                await ViewItemAsync(i);
            }
        }

        /// <summary>
        /// アイテム表示(非同期)
        /// </summary>
        /// <param name="index">インデックス値</param>
        private async UniTask ViewItemAsync(int index)
        {
            _items[index]?.View();
            await UniTask.WaitUntil(() => _items[index].IsViewItem());
        }

        /// <summary>
        /// フェードのSequenceをKill
        /// </summary>
        private void KillFadeSequence()
        {
            if (_fadeSequence != null)
            {
                _isFadeAnimation = false;
                _fadeSequence.Kill();
                _fadeSequence = null;
            }
        }

        /// <summary>
        /// 拡縮のSequenceをKill
        /// </summary>
        private void KillScaleSequence()
        {
            if (_scaleSequence != null)
            {
                _isScaleAnimation = false;
                _scaleSequence.Kill(true);
                _scaleSequence = null;
            }
        }

        /// <summary>
        /// ウィンドウが開くアニメーションが終了しているか？
        /// </summary>
        /// <returns>true: 終了, false: 再生中</returns>
        public bool IsEndOpenAnimation()
        {
            return _isFadeAnimation && _isScaleAnimation;
        }
    }
}