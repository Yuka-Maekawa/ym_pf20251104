using MyProject.Gacha.Lottery;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatiorFileController
    {
        private string  _filePath = string.Empty;
        private static readonly string _dateTimeFormat = "yyyyMMddHHmm";

        /// <summary>
        /// 書き込み
        /// </summary>
        public void WriteSimulationResult(Dictionary<GachaRarityLottery.Rarity, int> rarityResult, Dictionary<string, GachaSimulatorMain.SimulationInfo> itemResult, int simulationNum)
        {
            var folderPath = Directory.GetParent(Application.dataPath).FullName;
            _filePath = $"{folderPath}/Tool/GachaSimulator";

            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }

            var filePath = GetFilePath();

            // レアリティの排出結果を書き込み
            WriteRarityLog(filePath, rarityResult, simulationNum);

            int[] rarityLotteryNums = new int[System.Enum.GetValues(typeof(GachaRarityLottery.Rarity)).Length];

            foreach (var rarity in rarityResult)
            {
                rarityLotteryNums[(int)rarity.Key] = rarity.Value;
            }

            WriteItemLog(filePath, itemResult, rarityLotteryNums);

            Debug.Log($"シミュレーション結果の出力が終了しました。\n保存先:{filePath}");
        }

        /// <summary>
        /// レアリティのシミュレーション結果を書き込み
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="rarityResult">レアリティの結果</param>
        /// <param name="simulationNum">シミュレーション回数</param>
        private void WriteRarityLog(string filePath, Dictionary<GachaRarityLottery.Rarity, int> rarityResult, int simulationNum)
        {
            // レアリティの結果を書き込む(上書き)
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine("レアリティ,確率");

                foreach (var item in rarityResult)
                {
                    var lotteryCount = item.Value;
                    writer.WriteLine($"{item.Key},{(float)(lotteryCount / (float)simulationNum) * 100}");
                }
            }
        }

        /// <summary>
        /// アイテム排出結果を書き込み
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="itemResult">レアリティの結果</param>
        /// <param name="simulationNum">シミュレーション回数</param>
        private void WriteItemLog(string filePath, Dictionary<string, GachaSimulatorMain.SimulationInfo> itemResult, int[] rarityNum)
        {
            // ログ用にソートする
            var itemLog = itemResult.OrderBy(x => itemResult[x.Key].Rarity)
                .ThenBy(x => x.Value.Id)
                .ToDictionary(x => x.Key, x => x.Value);

            // レアリティの結果を書き込む(上書き)
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("レアリティ,ID,アイテム名,確率");

                foreach (var item in itemLog)
                {
                    var lotteryCount = item.Value.Count;
                    var rarity = item.Value.Rarity;
                    var probability = (float)(lotteryCount / (float)rarityNum[(int)rarity]) * 100;
                    writer.WriteLine($"{rarity},{item.Value.Id},{item.Key},{probability}");
                }
            }
        }

        /// <summary>
        /// ファイルパスを取得
        /// </summary>
        /// <returns>ファイルパス</returns>
        private string GetFilePath()
        {
            var dateTimeStr = System.DateTime.Now.ToString(_dateTimeFormat);
            return $"{_filePath}/GachaSimulatorResult{dateTimeStr}.csv";
        }
    }
}