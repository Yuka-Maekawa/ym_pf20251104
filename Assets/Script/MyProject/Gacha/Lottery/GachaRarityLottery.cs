using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Systems.Resource;

namespace MyProject.Gacha.Lottery
{
    public class GachaRarityLottery : GachaLotteryBase
    {
        public enum Rarity
        {
            Rare = 0,
            SuperRare,
            SpecialSuperRare
        }

        private GachaLotteryParameter _lotteryParameter = null;

        private string _filePath = string.Empty;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public async UniTask InitializeAsync(string filePath)
        {
            base.Initialize();

            _filePath = filePath;

            await ResourceManager.Local.LoadAssetAsync<GachaLotteryParameter>(_filePath);
            _lotteryParameter = ResourceManager.Local.GetAsset<GachaLotteryParameter>(_filePath);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _lotteryParameter = null;

            ResourceManager.Local.UnloadAssets(_filePath);
            _filePath = string.Empty;

            base.Release();
        }

        /// <summary>
        /// 抽選結果を取得
        /// </summary>
        /// <returns>抽選結果</returns>
        public Rarity GetLotteryResult()
        {
            var table = _lotteryParameter.Table;
            int dataNum = table.Count;

            var weights = new float[dataNum];
            for (int i = 0; i < dataNum; ++i)
            {
                weights[i] = table[i].Probability;
            }

            var index = GetLotteryResult(weights);

            return GetRarity(index);
        }

        /// <summary>
        /// レアリティを取得
        /// </summary>
        /// <param name="index">データベースのインデックス値</param>
        /// <returns>レアリティ</returns>
        private Rarity GetRarity(int index)
        {
            return index switch
            {
                0 => Rarity.Rare,
                1 => Rarity.SuperRare,
                2 => Rarity.SpecialSuperRare,
                _ => Rarity.Rare,
            };
        }
    }
}