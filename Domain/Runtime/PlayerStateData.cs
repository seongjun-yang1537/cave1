using System.Collections.Generic;
using Ingame;
using Quest;
using UnityEngine;
using Corelib.Utils;
using Core;

namespace Domain
{
    public class PlayerStateData : InstanceScriptableObject
    {
        [SerializeField]
        public PlayerModelState playerModelState;

        [SerializeField]
        public List<QuestModel> activeQuests = new();
        [SerializeField]
        public int gold;

        public override object CreateInstance()
        {
            return null;
        }
    }
}