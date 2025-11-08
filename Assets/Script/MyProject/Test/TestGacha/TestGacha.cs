using Cysharp.Threading.Tasks;
using MyProject.Database.Gacha;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;

public class TestGacha : MonoBehaviour
{
    private static readonly string _lotteryFilePath = "Database/Gacha/GachaLottery";

    private GachaRarityLottery _rarityLottery = null;
    private GachaLotteryParameter _lotteryParameters = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        _rarityLottery = new GachaRarityLottery();

        InitializeAsync().Forget();
    }

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    public async UniTask InitializeAsync()
    {
        await ResourceManager.Local.LoadAssetAsync<GachaLotteryParameter>(_lotteryFilePath);
        _lotteryParameters = ResourceManager.Local.GetAsset<GachaLotteryParameter>(_lotteryFilePath);

        _rarityLottery.Initialize(_lotteryParameters);

        for(int i = 0; i < 1000; ++i)
        {
            LotteryRarity();
        }
    }

    /// <summary>
    /// 解放
    /// </summary>
    public void Release()
    {
        _lotteryParameters = null;
        ResourceManager.Local.UnloadAssets(_lotteryFilePath);
    }

    /// <summary>
    /// レアリティの抽選結果
    /// </summary>
    private void LotteryRarity()
    {
        var rarity = _rarityLottery.GetLotteryResult();
        Debug.Log($"{rarity}");
    }
}
