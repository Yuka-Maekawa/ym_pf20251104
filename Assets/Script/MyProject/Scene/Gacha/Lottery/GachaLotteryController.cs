using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Systems.Resource;

namespace MyProject.Gacha.Lottery
{
    public class GachaLotteryController
    {
        public class ItemInfo
        {
            public GachaRarityLottery.Rarity Rarity = GachaRarityLottery.Rarity.Rare;
            public LineupInfo Item = null;

            public ItemInfo(GachaRarityLottery.Rarity rarity, LineupInfo lineupInfo)
            {
                Rarity = rarity;
                Item = lineupInfo;
            }
        }

        private static readonly string _gachaInfoFilePath = "Database/Gacha/GachaInfo";

        private GachaInfo _gachaInfo = null;
        private GachaRarityLottery _rarityLottery = null;
        private GachaLineupLottery _lineupLottery = null;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="gachaInfoParameterId">GachaInfoParameterのId</param>
        public async UniTask InitializeAsync(int gachaInfoParameterId)
        {
            // 使用するガチャのデータベースの情報まとめ
            await ResourceManager.Local.LoadAssetAsync<GachaInfoParameter>(_gachaInfoFilePath);
            var gachaInfoParameter = ResourceManager.Local.GetAsset<GachaInfoParameter>(_gachaInfoFilePath);
            _gachaInfo = FindGachaInfoById(gachaInfoParameter, gachaInfoParameterId);

            // 抽選管理
            _rarityLottery = new GachaRarityLottery();
            await _rarityLottery.InitializeAsync(_gachaInfo.LotteryFilePath);

            _lineupLottery = new GachaLineupLottery();
            await _lineupLottery.InitializeAsync(_gachaInfo);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            _lineupLottery?.Release();
            _lineupLottery = null;

            _rarityLottery?.Release();
            _rarityLottery = null;

            _gachaInfo = null;
            ResourceManager.Local.UnloadAssets(_gachaInfoFilePath);
        }

        /// <summary>
        /// Idを元に対象のGachaInfoを取得
        /// </summary>
        /// <param name="gachaInfoParameter">GachaInfoParameter</param>
        /// <param name="id">Id</param>
        /// <returns>GachaInfo</returns>
        private GachaInfo FindGachaInfoById(GachaInfoParameter gachaInfoParameter, int id)
        {
            var table = gachaInfoParameter.Table;
            int count = table.Count;

            for (int i = 0; i < count; ++i)
            {
                var data = table[i];
                if (data.Id == id)
                {
                    return data;
                }
            }

            return null;
        }

        /// <summary>
        /// 抽選結果を取得
        /// </summary>
        public ItemInfo GetLotteryResult()
        {
            // 1. レアリティ抽選
            var rarity = _rarityLottery.GetLotteryResult();

            // 2. アイテム抽選
            var item = _lineupLottery.GetLotteryResult(rarity);

            return new ItemInfo(rarity, item);
        }
    }
}