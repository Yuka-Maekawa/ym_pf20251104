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
        public string RareLineupFilePath = string.Empty;
        public string SuperRareLineupFilePath = string.Empty;
        public string SpecialSuperRareLineupFilePath = string.Empty;
    }
}