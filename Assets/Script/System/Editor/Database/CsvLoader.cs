using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MyProject.Database
{
    public class CsvLoader
    {
        private static readonly string _skipCharacter = "#";

        /// <summary>
        /// CSVを読み込みます
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>CSVの中身</returns>
        public List<string[]> LoadCSV(string filePath)
        {
            var csvData = new List<string[]>();

            var separatorRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            using (var stream = new StreamReader(filePath))
            {
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();

                    if (string.IsNullOrEmpty(line)) { continue; }

                    var splitLine = separatorRegex.Split(line);

                    if (splitLine[0] == _skipCharacter) { continue; }

                    csvData.Add(splitLine);
                }
            }

            return csvData;
        }
    }
}
