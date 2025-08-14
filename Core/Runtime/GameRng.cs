using Corelib.Utils;
using UnityEngine;

namespace Core
{
    public static class GameRng
    {
        public static int gameSeed = -1;
        public static int worldSeed = -1;
        public static int uiSeed = -1;

        private static MT19937 game;
        public static MT19937 Game
        {
            get => game ??= gameSeed == -1 ? MT19937.Create() : MT19937.Create(gameSeed);
        }

        private static MT19937 world;
        public static MT19937 World
        {
            get => world ??= worldSeed == -1 ? MT19937.Create() : MT19937.Create(worldSeed);
        }

        private static MT19937 ui;
        public static MT19937 UI
        {
            get => ui ??= uiSeed == -1 ? MT19937.Create() : MT19937.Create(uiSeed);
        }

        static GameRng()
        {
            game = world = ui = null;
        }

        public static void SetGameSeed(int seed)
        {
            gameSeed = seed;
            game = MT19937.Create(seed);
        }

        public static void SetWorldSeed(int seed)
        {
            worldSeed = seed;
            world = MT19937.Create(seed);
        }

        public static void SetUISeed(int seed)
        {
            uiSeed = seed;
            ui = MT19937.Create(seed);
        }
    }
}
