using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Systems.Resource;

namespace MyProject.Gacha.Lottery
{
    public class GachaLineupLottery : GachaLotteryBase
    {
        private class LineupInfoSummary
        {
            public string FilePath = string.Empty;
            public GachaLineupParameter LineupParameter = null;
        }

        private LineupInfoSummary[] _lineupParameters = null;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="gachaInfo">ガチャ情報</param>
        public  async UniTask InitializeAsync(GachaInfo gachaInfo)
        {
            base.Initialize();

            int rarityNum = System.Enum.GetValues(typeof(GachaRarityLottery.Rarity)).Length;
            _lineupParameters = new LineupInfoSummary[rarityNum];

            for(int i = 0; i < rarityNum; i++)
            {
                _lineupParameters[i] = new LineupInfoSummary();
            }

            _lineupParameters[(int)GachaRarityLottery.Rarity.Rare].FilePath = gachaInfo.RareLineupFilePath;
            _lineupParameters[(int)GachaRarityLottery.Rarity.SuperRare].FilePath = gachaInfo.SuperRareLineupFilePath;
            _lineupParameters[(int)GachaRarityLottery.Rarity.SpecialSuperRare].FilePath = gachaInfo.SpecialSuperRareLineupFilePath;

            await LoadDatabaseAsync(gachaInfo);
        }

        /// <summary>
        /// データベースを読み込み
        /// </summary>
        /// <param name="gachaInfo">ガチャ情報</param>
        private async UniTask LoadDatabaseAsync(GachaInfo gachaInfo)
        {
            var count = _lineupParameters.Length;
            var tasks = new UniTask[count];

            for (int i = 0; i < count; ++i)
            {
                tasks[i] = ResourceManager.Local.LoadAssetAsync<GachaLineupParameter>(_lineupParameters[i].FilePath);
            }

            await UniTask.WhenAll(tasks);

            for (int i = 0; i < count; ++i)
            {
                _lineupParameters[i].LineupParameter = ResourceManager.Local.GetAsset<GachaLineupParameter>(_lineupParameters[i].FilePath);
            }
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _lineupParameters = null;
            base.Release();
        }

        /// <summary>
        /// 抽選結果を取得
        /// </summary>
        /// <param name="lineupParameter">レアリティの情報</param>
        /// <returns>抽選結果</returns>
        public LineupInfo GetLotteryResult(GachaRarityLottery.Rarity rarity)
        {
            var table = _lineupParameters[(int)rarity].LineupParameter.Table;
            int dataNum = table.Count;

            var weights = new float[dataNum];
            for (int i = 0; i < dataNum; ++i)
            {
                weights[i] = table[i].Probability;
            }

            var lotteryIndex = GetLotteryResult(weights);

            return table[lotteryIndex];
        }
    }
}