using Cysharp.Threading.Tasks;

namespace MyProject.Gacha.Lottery
{
    public class GachaLotteryMultipleTimeController : GachaLotteryControllerBase
    {
        private GachaRarityLottery _lastRarityLottery = null;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="gachaInfoParameterId">GachaInfoParameterのId</param>
        public override async UniTask InitializeAsync(int gachaInfoParameterId)
        {
            await base.InitializeAsync(gachaInfoParameterId);

            // 最後の1回の抽選確率データ
            _lastRarityLottery = new GachaRarityLottery();
            await _lastRarityLottery.InitializeAsync(_gachaInfo.LastLotteryFilePath);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _lastRarityLottery.Release();
            _lastRarityLottery = null;

            base.Release();
        }

        /// <summary>
        /// 最後の抽選結果を取得
        /// </summary>
        public ItemInfo GetLastLotteryResult()
        {
            // 1. レアリティ抽選
            var rarity = _lastRarityLottery.GetLotteryResult();

            // 2. アイテム抽選
            return GetLotteryLineupItem(rarity);
        }
    }
}