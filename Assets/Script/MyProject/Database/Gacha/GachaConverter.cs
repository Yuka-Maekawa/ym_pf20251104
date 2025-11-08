using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyProject.Database.Gacha
{
    public class GachaConverter : CsvConverterBase
    {
        private enum Lottery
        {
            SkipCharacter = 0,
            Name,
            Probability
        }

        private enum Lineup
        {
            SkipCharacter = 0,
            Id,
            Name,
            Probability
        }

        private static readonly string _gachaFilePath = $"{_databaseFilePath}Gacha/";
        private static readonly string _gachaCsvFilePath = $"{_csvFilePath}Gacha/";

        /// <summary>
        /// ガチャのデータ情報をデータベース化
        /// </summary>
        [MenuItem("MyProject/Database/GachaConverter")]
        public static void ConvertGachaData()
        {
            var csvLoader = new CsvLoader();
            LotteryConverter(csvLoader, $"GachaLottery");
            LineupConverter(csvLoader, "LineupRare");
            LineupConverter(csvLoader, "LineupSuperRare");
            LineupConverter(csvLoader, "LineupSpecialSuperRare");
        }

        /// <summary>
        /// 抽選データを取得
        /// </summary>
        /// <param name="csvLoader">CsvLoader</param>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="fileName">ファイル名</param>
        private static void LotteryConverter(CsvLoader csvLoader, string fileName)
        {
            var csvLine = csvLoader.LoadCSV(GetCsvFilePath(fileName));

            if (csvLine == null) { return; }

            int count = csvLine.Count;

            var database = ScriptableObject.CreateInstance<GachaLotteryParameter>();
            database.Table = new List<LotteryInfo>(count);

            for (int i = 0; i < count; ++i)
            {
                var line = csvLine[i];

                var data = new LotteryInfo();
                data.Name = line[(int)Lottery.Name];

                if (float.TryParse(line[(int)Lottery.Probability], out float probability))
                {
                    data.Probability = probability;
                }

                database.Table.Add(data);
            }

            CreateScriptableObject<GachaLotteryParameter>(database, GetScriptableObjectFilePath(fileName));
        }

        /// <summary>
        /// ガチャのラインナップのデータをコンバート
        /// </summary>
        /// <param name="csvLoader">CsvLoader</param>
        /// <param name="fileName">ファイル名</param>
        private static void LineupConverter(CsvLoader csvLoader,  string fileName)
        {
            var csvLine = csvLoader.LoadCSV(GetCsvFilePath(fileName));

            if (csvLine == null) { return; }

            int count = csvLine.Count;

            var database = ScriptableObject.CreateInstance<GachaLineupParameter>();
            database.Table = new List<LineupInfo>(csvLine.Count);

            for (int i = 0; i < count; ++i)
            {
                var line = csvLine[i];

                var data = new LineupInfo();

                if (int.TryParse(line[(int)Lineup.Id], out int id))
                {
                    data.Id = id;
                }

                data.Name = line[(int)Lineup.Name];

                if (float.TryParse(line[(int)Lineup.Probability], out float probability))
                {
                    data.Probability = probability;
                }

                database.Table.Add(data);
            }

            CreateScriptableObject<GachaLineupParameter>(database, GetScriptableObjectFilePath(fileName));
        }

        /// <summary>
        /// CSVのファイルパスを取得
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>CSVのファイルパス</returns>
        private static string GetCsvFilePath(string fileName)
        {
            return $"{_gachaCsvFilePath}{fileName}{_csvExtension}";
        }

        /// <summary>
        /// ScriptableObjectのファイルパスを取得
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ScriptableObjectのファイルパス</returns>
        private static string GetScriptableObjectFilePath(string fileName) 
        {
            return $"{_gachaFilePath}{fileName}{_assetExtension}";
        }
    }
}