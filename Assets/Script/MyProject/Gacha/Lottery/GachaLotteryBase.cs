using MyProject.Common.Lottery;

namespace MyProject.Gacha.Lottery
{
    public class GachaLotteryBase
    {
        private WeightRandomLottery _weightRandomLottery = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            _weightRandomLottery = new WeightRandomLottery();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public virtual void Release()
        {
            _weightRandomLottery = null;
        }

        /// <summary>
        /// 抽選結果を取得
        /// </summary>
        /// <param name="weights">抽選の重み</param>
        /// <returns>抽選結果</returns>
        public virtual int GetLotteryResult(float[] weights)
        {
            return _weightRandomLottery.GetLotteryResults(weights);
        }
    }
}
