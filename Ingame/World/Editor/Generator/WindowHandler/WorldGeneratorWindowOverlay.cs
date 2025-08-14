using System;
using Corelib.SUI;
using UnityEngine;
using VoxelEngine;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowOverlay
    {
        [NonSerialized]
        private WorldRegionInsertOverlay regionOverlay = new();
        [NonSerialized]
        private WorldDebugOverlay debugOverlay = new();
        [NonSerialized]
        private SpawnEntityDebugOverlay spawnOverlay = new();

        [SerializeField]
        private bool showRegionOverlay = false;
        [SerializeField]
        private bool showDebugOverlay = false;
        [SerializeField]
        private bool showSpawnOverlay = false;

        [NonSerialized]
        private WorldData prevWorldData;
        [NonSerialized]
        private GameWorld prevGameWorld;

        public WorldGeneratorWindowOverlay(GameWorld gameWorld)
        {
            regionOverlay = new();
            debugOverlay = new();
            spawnOverlay = new();
            UpdateContext(gameWorld);
        }

        public SUIElement Render(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);

            return SEditorGUILayout.Group("Overlays")
            .Content(
                SEditorGUILayout.Var("Region Insert", showRegionOverlay)
                    .OnValueChanged(v => { showRegionOverlay = v; regionOverlay.Visible = v; })
                + SEditorGUILayout.Var("Debug Tools", showDebugOverlay)
                    .OnValueChanged(v => { showDebugOverlay = v; debugOverlay.Visible = v; })
                + SEditorGUILayout.Var("Spawn Entity", showSpawnOverlay)
                    .OnValueChanged(v => { showSpawnOverlay = v; spawnOverlay.Visible = v; })
            );
        }

        public void OnDisable()
        {
            regionOverlay?.Dispose();
            debugOverlay?.Dispose();
            spawnOverlay?.Dispose();
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            if (gameWorld == null) return;

            var worldData = gameWorld.worldData;
            if (prevWorldData == worldData && prevGameWorld == gameWorld)
            {
                return;
            }

            prevWorldData = worldData;
            prevGameWorld = gameWorld;

            WorldVSPGraphVisualizer visualizer = gameWorld?.GetComponentInChildren<WorldVSPGraphVisualizer>();
            TriangleVisualizer triangleVisualizer = gameWorld?.GetComponentInChildren<TriangleVisualizer>();

            regionOverlay = new();
            debugOverlay = new();
            spawnOverlay = new();

            spawnOverlay.worldData = worldData;
            spawnOverlay.triangleVisualizer = triangleVisualizer;

            regionOverlay.graph = worldData?.graph;
            regionOverlay.visualizer = visualizer;

            UpdateOverlayVisible();
        }

        private void UpdateOverlayVisible()
        {
            regionOverlay.Visible = showRegionOverlay;
            debugOverlay.Visible = showDebugOverlay;
            spawnOverlay.Visible = showSpawnOverlay;
        }
    }
}