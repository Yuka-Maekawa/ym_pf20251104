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
    public class GachaMenuLineupUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroupSetter _canvasGroupSetter = null;
        [SerializeField] private Transform _parentTransform = null;
        [SerializeField] private GameObject _lineupItemObj = null;
        [SerializeField] private GameObject _rootObj = null;
        [SerializeField] private Scrollbar _scrollbar = null;

        private static readonly string _gachaInfoFilePath = "Database/Gacha/GachaInfo";
        private static readonly float _animationTime = 0.25f;

        private GameObject[] _itemObjs = null;
        private GachaMenuLineupItem[] _lineupItems = null;

        private GachaInfoParameter _gachaInfo = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync()
        {
            // ItemGroupのサイズ感が取れなくなるので初期化では最初にアルファを0にする
            _canvasGroupSetter.Hide();

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
            _canvasGroupSetter.KillAllSequence();

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

            _scrollbar.value = 1f;
            _canvasGroupSetter.PlayFadeInAnimation(_animationTime, Ease.InOutSine);

            await UniTask.WaitWhile(() => _canvasGroupSetter.IsPlayingFadeAnimation());
        }

        /// <summary>
        /// 閉じる（非同期）
        /// </summary>
        public async UniTask CloseAsync()
        {
            _canvasGroupSetter.PlayFadeOutAnimation(_animationTime, Ease.InOutSine);
            await UniTask.WaitWhile(() => _canvasGroupSetter.IsPlayingFadeAnimation());
            SetActive(false);
        }
    }
}