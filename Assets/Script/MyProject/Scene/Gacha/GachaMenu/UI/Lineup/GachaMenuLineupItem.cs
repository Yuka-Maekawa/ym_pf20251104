using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _myObjRectTransform = null;
        [SerializeField] private RectTransform _titleTextRectTransform = null;
        [SerializeField] private RectTransform _itemGroupRectTransform = null;

        [SerializeField] private GachaMenuLineupTitle _lineupTitle = null;
        [SerializeField] private GachaMenuLineupGroup _lineupGroup = null;
        [SerializeField] private Color[] _rarityColors = null;

        private static readonly float _itemGroupSpece = 15f;

        private GachaLineupParameter _lineupParameter = null;
        private string _filePath = string.Empty;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="rarity">レアリティ</param>
        /// <param name="filePath">ラインナップデータのファイルパス</param>
        public async UniTask InitializeAsync(GachaRarityLottery.Rarity rarity, string filePath)
        {
            _filePath = filePath;

            await ResourceManager.Local.LoadAssetAsync<GachaLineupParameter>(_filePath);
            _lineupParameter = ResourceManager.Local.GetAsset<GachaLineupParameter>(_filePath);

            var rarityColor = _rarityColors[(int)rarity];

            _lineupTitle.SetupTitle($"{rarity}", rarityColor);

            var table = _lineupParameter.Table;
            int tableNum = table.Count;
            _lineupGroup.Initialize(tableNum);

            var task = new UniTask[tableNum];
            for (int i = 0; i < tableNum; ++i)
            {
                var itemInfo = table[i];
                task[i] = _lineupGroup.SetupItemAsync(i, rarityColor, itemInfo.ThumbnailName, itemInfo.Name);
            }

            await UniTask.WhenAll(task);

            // ラインナップの数に合わせて、LineupItemのサイズを設定
            var size = new Vector2(_myObjRectTransform.sizeDelta.x, _titleTextRectTransform.sizeDelta.y + _itemGroupRectTransform.sizeDelta.y + _itemGroupSpece);
            _myObjRectTransform.sizeDelta = size;
        }

        /// <summary>
        /// 解放（非同期）
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            ResourceManager.Local.UnloadAssets(_filePath);
            _filePath = string.Empty;

            await _lineupGroup.ReleaseAsync();
        }
    }
}