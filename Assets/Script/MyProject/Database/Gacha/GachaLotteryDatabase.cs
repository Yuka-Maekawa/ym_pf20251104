using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Database.Gacha
{
    public class GachaLotteryParameter : ScriptableObject
    {
        public List<LotteryInfo> Table = null;
    }

    [Serializable]
    public class LotteryInfo
    {
        public string Name = string.Empty;
        public float Probability = 0f;
    }
}