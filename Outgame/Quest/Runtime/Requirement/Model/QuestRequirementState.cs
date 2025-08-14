using System;
using Ingame;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestRequirementState
    {
        public QuestRequirementID requirementID;

        public int count;
    }
}