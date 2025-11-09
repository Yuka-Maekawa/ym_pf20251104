using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
using UnityEngine;

public class TestGacha : MonoBehaviour
{
    private static readonly int _useId = 1;

    private GachaLotteryController _gachaLottery = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        _gachaLottery = new GachaLotteryController();

        InitializeAsync().Forget();
    }

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    public async UniTask InitializeAsync()
    {
        await _gachaLottery.InitializeAsync(_useId);

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
        _gachaLottery?.Release();
        _gachaLottery = null;
    }

    /// <summary>
    /// 抽選結果
    /// </summary>
    private void LotteryRarity()
    {
        var data = _gachaLottery.GetLotteryResult();
        Debug.Log($"{data.Rarity}： {data.Item.Name}");
    }
}
