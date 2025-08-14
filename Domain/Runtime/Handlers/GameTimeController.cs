using Corelib.Utils;
using GameTime;
using UnityEngine;
using UnityEngine.Events;

namespace Domain
{
    public class GameTimeController : ILifecycleInjectable
    {
        private readonly GameController controller;

        private GameCalendarModel calendarModel => controller.gameModel.calendarModel;

        public int Day => calendarModel.currentDay;
        public UnityEvent<int> OnNextDay => calendarModel.onNextDay;

        public GameTimeController(GameController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            OnNextDay.Invoke(calendarModel.currentDay);
        }

        public void NextDay(int amount = 1)
        {
            calendarModel.AdvanceDay(amount);
        }
    }
}