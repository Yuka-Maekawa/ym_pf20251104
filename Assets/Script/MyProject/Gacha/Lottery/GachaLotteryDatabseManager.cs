using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Systems.Resource;

namespace MyProject.Gacha.Lottery
{
    public class GachaLotteryDatabseManager
    {
        private static readonly string _gachaInfoFilePath = "Database/Gacha/GachaInfo";

        // データベース
        private GachaInfo _gachaInfo = null;
        private GachaLineupParameter _rareLineupLotteryParameter = null;
        private GachaLineupParameter _superRareLineupLotteryParameter = null;
        private GachaLineupParameter _specialSuperRareLineupLotteryParameter = null;

        private GachaLotteryParameter _rarityLotteryParameter = null;
        public GachaLotteryParameter RarityLotteryParameter { get { return _rarityLotteryParameter ; } }

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        /// <param name="id">GachaInfoParameterのId</param>
        public async UniTask InitializeAsync(int id)
        {
            await ResourceManager.Local.LoadAssetAsync<GachaInfoParameter>(_gachaInfoFilePath);
            var gachaInfoParameter = ResourceManager.Local.GetAsset<GachaInfoParameter>(_gachaInfoFilePath);

            // データ読み込み
            _gachaInfo = FindGachaInfoById(gachaInfoParameter, id);
            var lotteryTask = ResourceManager.Local.LoadAssetAsync<GachaLotteryParameter>(_gachaInfo.LotteryFilePath);
            var rareTask = ResourceManager.Local.LoadAssetAsync<GachaLineupParameter>(_gachaInfo.RareLineupFilePath);
            var superRareTask = ResourceManager.Local.LoadAssetAsync<GachaLineupParameter>(_gachaInfo.SuperRareLineupFilePath);
            var specialSuperRareTask =  ResourceManager.Local.LoadAssetAsync<GachaLineupParameter>(_gachaInfo.SpecialSuperRareLineupFilePath);

            await UniTask.WhenAll(lotteryTask, rareTask, superRareTask, specialSuperRareTask);

            _rarityLotteryParameter  = ResourceManager.Local.GetAsset<GachaLotteryParameter>(_gachaInfo.LotteryFilePath);
            _rareLineupLotteryParameter = ResourceManager.Local.GetAsset<GachaLineupParameter>(_gachaInfo.RareLineupFilePath);
            _superRareLineupLotteryParameter = ResourceManager.Local.GetAsset<GachaLineupParameter>(_gachaInfo.SuperRareLineupFilePath);
            _specialSuperRareLineupLotteryParameter = ResourceManager.Local.GetAsset<GachaLineupParameter>(_gachaInfo.SpecialSuperRareLineupFilePath);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            _specialSuperRareLineupLotteryParameter = null;
            _superRareLineupLotteryParameter = null;
            _rareLineupLotteryParameter = null;
            _rarityLotteryParameter  = null;

            ResourceManager.Local.UnloadAssets(_gachaInfo.LotteryFilePath);
            ResourceManager.Local.UnloadAssets(_gachaInfo.RareLineupFilePath);
            ResourceManager.Local.UnloadAssets(_gachaInfo.SuperRareLineupFilePath);
            ResourceManager.Local.UnloadAssets(_gachaInfo.SpecialSuperRareLineupFilePath);

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

            for(int i = 0; i < count; ++i)
            {
                var data = table[i];
                if (data .Id== id)
                {
                    return data;
                }
            }

            return null;
        }

        /// <summary>
        /// 確率の重み付けを取得
        /// </summary>
        /// <returns>重みの配列</returns>
        public float[] GetGachaLotteryWeights()
        {
            var table = _rarityLotteryParameter .Table;
            int count = table.Count;

            var weights = new float[count];

            for(int i = 0; i < count; ++i)
            {
                weights[i] = table[i].Probability;
            }

            return weights;
        }

        /// <summary>
        /// 確率の重み付けを取得
        /// </summary>
        /// <returns>重みの配列</returns>
        public float[] GetGachaLotteryWeights(GachaRarityLottery.Rarity rarity)
        {
            var data = GetLineupParameter(rarity);
            var table = data.Table;
            int count = table.Count;

            var weights = new float[count];

            for (int i = 0; i < count; ++i)
            {
                weights[i] = table[i].Probability;
            }

            return weights;
        }

        /// <summary>
        /// ラインナップのデータを取得
        /// </summary>
        /// <param name="rarity">レアリティ</param>
        /// <returns>GachaLineupParameter</returns>
        public GachaLineupParameter GetLineupParameter(GachaRarityLottery.Rarity rarity)
        {
            return rarity switch
            {
                GachaRarityLottery.Rarity.Rare => _rareLineupLotteryParameter,
                GachaRarityLottery.Rarity.SuperRare => _superRareLineupLotteryParameter,
                GachaRarityLottery.Rarity.SpecialSuperRare => _specialSuperRareLineupLotteryParameter,
                _ => null
            };
        }
    }
}