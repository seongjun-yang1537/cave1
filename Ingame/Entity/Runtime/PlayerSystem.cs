using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class PlayerSystem : Singleton<PlayerSystem>
    {
        public UnityEvent<PlayerController> onPlayerAdded = new();
        public static UnityEvent<PlayerController> OnPlayerAdded => Instance.onPlayerAdded;

        public UnityEvent onPlayersInitialized = new();
        public static UnityEvent OnPlayersInitialized => Instance.onPlayersInitialized;

        private readonly List<PlayerController> players = new();

        public static IReadOnlyList<PlayerController> Players => Instance.players;
        public static PlayerController CurrentPlayer { get; private set; }

        protected void Start()
        {
            players.Clear();
            GetComponentsInChildren(players);

            if (players.Count > 0)
                CurrentPlayer = players[0];

            foreach (PlayerController pc in players)
                onPlayerAdded.Invoke(pc);

            OnPlayersInitialized.Invoke();
        }
    }
}
