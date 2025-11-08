using MyProject.Database.Gacha;

namespace MyProject.Gacha.Lottery
{
    public class GachaRarityLottery : GachaLotteryBase
    {
        public enum Rarity
        {
            R = 0,
            SR,
            SSR
        }


        private GachaLotteryParameter _lotteryParameter = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="lotteryParameter">レアリティの情報</param>
        public void Initialize(GachaLotteryParameter lotteryParameter)
        {
            base.Initialize();

            _lotteryParameter = lotteryParameter;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _lotteryParameter = null;
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
                0 => Rarity.R,
                1 => Rarity.SR,
                2 => Rarity.SSR,
                _ => Rarity.R,
            };
        }
    }
}