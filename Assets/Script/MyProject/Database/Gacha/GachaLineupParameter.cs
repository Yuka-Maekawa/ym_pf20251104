using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Database.Gacha
{
    public class GachaLineupParameter : ScriptableObject
    {
        public List<LineupInfo> Table = null;
    }

    [Serializable]
    public class LineupInfo
    {
        public int Id = 0;
        public string Name = string.Empty;
        public float Probability = 0f;
        public string ThumbnailName = string.Empty;
    }
}

