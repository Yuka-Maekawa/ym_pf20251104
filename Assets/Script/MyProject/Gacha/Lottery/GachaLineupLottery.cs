using MyProject.Database.Gacha;

namespace MyProject.Gacha.Lottery
{
    public class GachaLineupLottery : GachaLotteryBase
    {
        private GachaLineupParameter _lineupParameter = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="lotteryParameter">レアリティの情報</param>
        public void Initialize(GachaLineupParameter lineupParameter)
        {
            base.Initialize();

            _lineupParameter = lineupParameter;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _lineupParameter = null;
            base.Release();
        }

        /// <summary>
        /// 抽選結果を取得
        /// </summary>
        /// <returns>抽選結果</returns>
        public int GetLotteryResult()
        {
            var table = _lineupParameter.Table;
            int dataNum = table.Count;

            var weights = new float[dataNum];
            for (int i = 0; i < dataNum; ++i)
            {
                weights[i] = table[i].Probability;
            }

            return GetLotteryResult(weights);
        }
    }
}