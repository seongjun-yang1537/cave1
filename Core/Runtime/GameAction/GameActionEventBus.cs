using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine.Events;

namespace Core
{
    public class GameActionEventBus : Singleton<GameActionEventBus>
    {
        private UnityEvent<GameActionEvent> onPublish = new();
        public static UnityEvent<GameActionEvent> OnPublish => Instance.onPublish;

        private readonly List<GameActionEvent> _eventQueue = new List<GameActionEvent>();
        public IReadOnlyList<GameActionEvent> EventQueue => _eventQueue.AsReadOnly();

        public static void Publish(GameActionType type, string jsonData)
        {
            var newEvent = new GameActionEvent(type, jsonData);
            Instance._eventQueue.Add(newEvent);
            OnPublish.Invoke(newEvent);
        }
    }
}