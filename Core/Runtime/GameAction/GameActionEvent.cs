using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine.Events;

namespace Core
{
    public struct GameActionEvent
    {
        public GameActionType Type { get; }
        public string JsonData { get; }

        public GameActionEvent(GameActionType type, string jsonData)
        {
            Type = type;
            JsonData = jsonData;
        }
    }
}