using System;
using UnityEngine;
using System.Collections.Generic;
using Quest;
using Ingame;

namespace Domain
{
    [Serializable]
    public class GamePlayerModelData : ScriptableObject
    {
        public PlayerModelData playerModel;
        public List<QuestModelData> activeQuests = new();
    }
}

