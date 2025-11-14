using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyProject.Common.UI;
using MyProject.Database.Gacha;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _menuCanvasGroupSetter = null;
        [SerializeField] private CanvasGroupSetter _parentCanvasGroupSetter = null;
        [SerializeField] private CanvasGroupSetter _scrollbarCanvasGroupSetter = null;
        [SerializeField] private Transform _parentTransform = null;
        [SerializeField] private GameObject _lineupItemObj = null;
        [SerializeField] private GameObject _rootObj = null;
        [SerializeField] private Scrollbar _scrollbar = null;

        private static readonly string _gachaInfoFilePath = "Database/Gacha/GachaInfo";
        private static readonly float _scrollbarMaxValue = 1f;

        private static readonly Vector3 _defaultScale = new Vector3(1f, 0f, 1f);
        private static readonly Vector3 _animationScale = Vector3.one;

        private static readonly float _animationTime = 0.25f;
        private static readonly float _menuAnimationTime = _animationTime * 0.5f;

        private GameObject[] _itemObjs = null;
        private GachaMenuLineupItem[] _lineupItems = null;

        private GachaInfoParameter _gachaInfo = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync()
        {
            // ItemGroupのサイズ感が取れなくなるので初期化では最初にアルファを0にする
            _menuCanvasGroupSetter.Hide();
            _menuCanvasGroupSetter.SetLocalScale(_defaultScale);
            CloseLineup();

            _lineupItemObj.SetActive(false);

            int rarityNum = System.Enum.GetValues(typeof(GachaRarityLottery.Rarity)).Length;
            _itemObjs = new GameObject[rarityNum];
            _lineupItems = new GachaMenuLineupItem[rarityNum];

            await ResourceManager.Local.LoadAssetAsync<GachaInfoParameter>(_gachaInfoFilePath);
            _gachaInfo = ResourceManager.Local.GetAsset<GachaInfoParameter>(_gachaInfoFilePath);

            var filePaths = _gachaInfo.Table[0].RarityLineupFilePaths;

            for (int i = 0; i < rarityNum; ++i)
            {
                var obj = Instantiate(_lineupItemObj, _parentTransform);
                obj.SetActive(true);

                var item = obj.GetComponent<GachaMenuLineupItem>();
                await item.InitializeAsync((GachaRarityLottery.Rarity)i, filePaths[i]);

                _lineupItems[i] = item;
                _itemObjs[i] = obj;
            }

            SetActive(false);
        }

        /// <summary>
        /// 解放（非同期）
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            _menuCanvasGroupSetter.KillAllSequence();

            int count = _lineupItems.Length;
            for (int i = 0; i < count; ++i)
            {
                await _lineupItems[i].ReleaseAsync();
                _lineupItems[i] = null;

                Destroy(_itemObjs[i]);
                _itemObjs[i] = null;
            }

            _lineupItems = null;
            _itemObjs = null;
        }

        /// <summary>
        /// アクティブ設定
        /// </summary>
        /// <param name="isActive">true: 表示, false: 非表示</param>
        private void SetActive(bool isActive)
        {
            _rootObj.SetActive(isActive);
        }

        /// <summary>
        /// 開く（非同期）
        /// </summary>
        public async UniTask OpenAsync()
        {
            SetActive(true);

            await OpenWindowAsync();
            await OpenLineupAsync();
        }

        /// <summary>
        /// ウィンドウを開く（非同期）
        /// </summary>
        public async UniTask OpenWindowAsync()
        {
            _menuCanvasGroupSetter.PlayFadeOutAnimation(_animationTime, Ease.InOutSine);
            _menuCanvasGroupSetter.PlayScaleAnimation(_animationScale, _animationTime, Ease.InOutSine);

            await UniTask.WaitWhile(() => _menuCanvasGroupSetter.IsPlayingAnimation());
        }

        /// <summary>
        /// メニューを開く（非同期）
        /// </summary>
        public async UniTask OpenLineupAsync()
        {
            _scrollbar.value = _scrollbarMaxValue;
            _scrollbarCanvasGroupSetter.PlayFadeOutAnimation(_menuAnimationTime, Ease.InOutSine);
            _parentCanvasGroupSetter.PlayFadeOutAnimation(_menuAnimationTime, Ease.InOutSine);

            await UniTask.WaitWhile(() => _parentCanvasGroupSetter.IsPlayingFadeAnimation());
        }

        /// <summary>
        /// 閉じる（非同期）
        /// </summary>
        public async UniTask CloseAsync()
        {
            _menuCanvasGroupSetter.PlayFadeInAnimation(_animationTime, Ease.InOutSine);
            _menuCanvasGroupSetter.PlayScaleAnimation(_defaultScale, _animationTime, Ease.InOutSine);

            await UniTask.WaitWhile(() => _menuCanvasGroupSetter.IsPlayingAnimation());

            CloseLineup();
            SetActive(false);
        }

        /// <summary>
        /// ラインナップを閉じる
        /// </summary>
        public void CloseLineup()
        {
            _scrollbarCanvasGroupSetter.Hide();
            _parentCanvasGroupSetter.Hide();
        }
    }
}