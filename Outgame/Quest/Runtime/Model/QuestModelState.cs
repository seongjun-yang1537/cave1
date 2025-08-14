using System;
using UnityEngine;
using System.Collections.Generic;

namespace Quest
{
    [Serializable]
    public class QuestModelState
    {
        public QuestID id;
        public int remainDays;
        public QuestPhase phase;
    }
}

