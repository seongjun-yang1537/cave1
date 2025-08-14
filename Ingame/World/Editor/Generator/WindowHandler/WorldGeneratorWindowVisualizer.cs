using System;
using Corelib.SUI;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowVisualizer
    {
        [SerializeField]
        private WorldGeneratorWindowVisualizerGraph graphWindow;
        [SerializeField]
        private WorldGeneratorWindowVisualizerTriangle triangleWindow;
        [SerializeField]
        private WorldGeneratorWindowStartEnd startEndWindow;
        [SerializeField]
        private WorldGeneratorWindowMonsterSpawnTrigger spawnTriggerWindow;

        public WorldGeneratorWindowVisualizer(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);
        }

        public SUIElement Render()
        {
            return SEditorGUILayout.Vertical()
            .Content(
                graphWindow.Render()
                + triangleWindow.Render()
                + startEndWindow.Render()
                + spawnTriggerWindow.Render()
            );
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            spawnTriggerWindow?.Dispose();
            graphWindow = new(gameWorld);
            triangleWindow = new(gameWorld);
            startEndWindow = new(gameWorld);
            spawnTriggerWindow = new(gameWorld);
        }
    }
}