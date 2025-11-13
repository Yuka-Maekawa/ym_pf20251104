using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Database.Gacha
{
    public class GachaInfoParameter : ScriptableObject
    {
        public List<GachaInfo> Table = null;
    }

    [Serializable]
    public class GachaInfo
    {
        public int Id = 0;
        public string LotteryFilePath = string.Empty;
        public string LastLotteryFilePath = string.Empty;
        public string[] RarityLineupFilePaths = null;
    }
}