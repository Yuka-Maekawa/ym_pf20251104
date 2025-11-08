using UnityEngine;

namespace MyProject.Gacha.Lottery
{
    public class WeightRandomLottery
    {
        /// <summary>
        /// 抽選結果
        /// </summary>
        /// <param name="weights">重み</param>
        /// <returns>抽選結果</returns>
        public int LotteryResults(float[] weights)
        {
            if(weights == null ) { return -1; }

            return LotteryWeight(weights, GetTotalWeight(weights));
        }

        /// <summary>
        /// 配列の重みの合計値を取得する
        /// </summary>
        /// <param name="weights">重み</param>
        /// <returns>重みの合計</returns>
        private float GetTotalWeight(float[] weights)
        {
            var totalWeight = 0f;

            int count  = weights.Length;
            for(int i = 0; i < count; ++i )
            {
                totalWeight += weights[i];
            }

            return totalWeight;
        }

        /// <summary>
        /// 重み付き抽選
        /// </summary>
        /// <param name="weights">重み(配列)</param>
        /// <param name="totalWeight">重みの合計値</param>
        /// <returns>抽選結果</returns>
        private int LotteryWeight(float[] weights, float totalWeight)
        {
            var random = Random.Range(0f, totalWeight);

            var weight = 0f;
            int count = weights.Length;
            for(int i = 0; i < count; ++i )
            {
                weight += weights[i];

                if ( weight >= random )
                {
                    return i;
                }
            }

            return -1;
        }
    }
}