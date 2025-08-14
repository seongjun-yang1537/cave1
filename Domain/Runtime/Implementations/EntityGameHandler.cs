using System;
using Core;
using Ingame;

namespace Domain
{
    [Serializable]
    public class EntityGameHadler : IEntityGameHandler
    {
        private GameTimeController GameTime => GameController.Time;

        public void NextDay()
        {
            GameTime.NextDay();
            ScreenUISystem.TriggerDayUI(GameTime.Day);

            foreach (var player in PlayerSystem.Players)
                player.SetActionEnabled(false);

            ScreenUISystem.RegisterOnNextDayUIEndOnce(() =>
            {
                foreach (var player in PlayerSystem.Players)
                    player.SetActionEnabled(true);
            });
        }
    }
}