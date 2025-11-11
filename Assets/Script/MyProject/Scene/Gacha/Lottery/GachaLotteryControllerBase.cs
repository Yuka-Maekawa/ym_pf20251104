using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Systems.Resource;

namespace MyProject.Gacha.Lottery
{
    public class GachaLotteryControllerBase
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

        protected GachaInfo _gachaInfo = null;
        protected GachaRarityLottery _defaultRarityLottery = null;
        private GachaLineupLottery _lineupLottery = null;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="gachaInfoParameterId">GachaInfoParameterのId</param>
        public virtual async UniTask InitializeAsync(int gachaInfoParameterId)
        {
            // 使用するガチャのデータベースの情報まとめ
            await ResourceManager.Local.LoadAssetAsync<GachaInfoParameter>(_gachaInfoFilePath);
            var gachaInfoParameter = ResourceManager.Local.GetAsset<GachaInfoParameter>(_gachaInfoFilePath);
            _gachaInfo = FindGachaInfoById(gachaInfoParameter, gachaInfoParameterId);

            // 抽選管理
            _defaultRarityLottery = new GachaRarityLottery();
            await _defaultRarityLottery.InitializeAsync(_gachaInfo.LotteryFilePath);

            _lineupLottery = new GachaLineupLottery();
            await _lineupLottery.InitializeAsync(_gachaInfo);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public virtual void Release()
        {
            _lineupLottery?.Release();
            _lineupLottery = null;

            _defaultRarityLottery?.Release();
            _defaultRarityLottery = null;

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
        public ItemInfo GetDefaultLotteryResult()
        {
            // 1. レアリティ抽選
            var rarity = _defaultRarityLottery.GetLotteryResult();

            // 2. アイテム抽選
            return GetLotteryLineupItem(rarity);
        }

        /// <summary>
        /// ラインナップの抽選結果を取得
        /// </summary>
        /// <param name="rarity">GachaRarityLottery.Rarity</param>
        /// <returns>抽選結果</returns>
        protected ItemInfo GetLotteryLineupItem(GachaRarityLottery.Rarity rarity)
        {
            var item = _lineupLottery.GetLotteryResult(rarity);

            return new ItemInfo(rarity, item);
        }
    }
}