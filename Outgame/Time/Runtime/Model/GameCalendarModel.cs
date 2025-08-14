using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameTime
{
    [Serializable]
    public class GameCalendarModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<int> onNextDay = new();
        #endregion ====================

        #region ========== Data ==========
        private readonly GameCalendarModelData data;
        #endregion ====================

        #region ========== State ==========
        [field: SerializeField]
        public int currentDay { get; private set; }
        #endregion ====================

        public GameCalendarModel(GameCalendarModelData data)
        {
            this.data = data;
            currentDay = data?.currentDay ?? 0;
        }

        public void AdvanceDay(int amount = 1)
        {
            if (amount <= 0) return;

            currentDay += amount;
            onNextDay?.Invoke(currentDay);
        }
    }
}