using System;
using System.Collections.Generic;
using GameTime;
using Quest;
using UnityEngine;

namespace Domain
{
    [Serializable]
    public class GameModelState
    {
        [SerializeField]
        public GamePlayerModelState gamePlayer;

        [SerializeField]
        public GameCalendarModelState calendar;

        [SerializeField]
        public List<QuestModelState> availableQuests;

        [SerializeField]
        public List<string> chatLogs;
    }
}