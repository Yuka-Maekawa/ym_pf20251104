using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupItem : MonoBehaviour
    {
        [SerializeField] private GachaMenuLineupTitle _lineupTitle = null;
        [SerializeField] private GachaMenuLineupGroup _lineupGroup = null;
        [SerializeField] private Color[] _rarityColors = null;

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

            for (int i = 0; i < tableNum; ++i)
            {
                var itemInfo = table[i];
                await _lineupGroup.SetupItemAsync(i, rarityColor, itemInfo.ThumbnailName, itemInfo.Name);
            }
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