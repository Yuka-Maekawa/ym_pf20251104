using UnityEngine;
using MyProject.Common.Lottery;

public class TestGacha : MonoBehaviour
{
    public class TestInfo
    {
        public string Name = string.Empty;
        public float Weight = 0f;
    }

    private WeightRandomLottery _wightRandomLottery = null;

    private TestInfo[] _testLineup = null;

    private void Start()
    {
        _wightRandomLottery = new WeightRandomLottery();

        _testLineup = new TestInfo[]
         {
             new TestInfo{ Name = "テスト1", Weight = 15f},
             new TestInfo{ Name = "テスト2", Weight = 5f},
             new TestInfo{ Name = "テスト3", Weight = 7.5f},
             new TestInfo{ Name = "テスト4", Weight = 13f},
             new TestInfo{ Name = "テスト5", Weight = 5f},
             new TestInfo{ Name = "テスト6", Weight = 15f},
             new TestInfo{ Name = "テスト7", Weight = 10.5f},
             new TestInfo{ Name = "テスト8", Weight = 12f},
             new TestInfo{ Name = "テスト9", Weight = 11.9f},
             new TestInfo{ Name = "テスト10", Weight = 5.1f},
         };

        TestWeightLottery();
    }

    private void OnDestroy()
    {
        _testLineup = null;
        _wightRandomLottery = null;
    }

    /// <summary>
    /// 重み付き抽選のテスト
    /// </summary>
    private void TestWeightLottery()
    {
        int lineupNum = _testLineup.Length;

        var weight = new float[lineupNum];
        for (int i = 0; i < lineupNum; ++i)
        {
            weight[i] = _testLineup[i].Weight;
        }

        for(int i = 0; i < 10000; ++i )
        {
            var result = _wightRandomLottery.LotteryResults(weight);
            Debug.Log($"抽選結果:{_testLineup[result].Name}");
        }
    }
}
