using UnityEngine;
using Ingame;

namespace UI
{
    public class GameCommandHandler : ICommandProcessor
    {
        [RuntimeInitializeOnLoadMethod]
        static void Register()
        {
            PlayerChatPopupUIController.commandProcessors.Add(new GameCommandHandler());
        }

        public bool OnProcessCommand(string cmd)
        {
            var parts = cmd.Split(' ');
            if (parts.Length == 3 && parts[0] == "give" && parts[1] == "gold" && int.TryParse(parts[2], out var value))
            {
                PlayerSystem.CurrentPlayer.GainGold(value);
                return true;
            }
            return false;
        }
    }
}
