using Corelib.Utils;
using UnityEngine;

namespace World
{
    [ExecuteAlways]
    public class DebugWorldDataVisualizer : MonoBehaviour
    {
        private GameWorld _gameWorld;
        private GameWorld gameWorld
        {
            get => _gameWorld ??= transform.GetComponentInSelfOrParent<GameWorld>();
        }

        private WorldVSPGraphVisualizer graphVisualizer;
        private TriangleVisualizer triangleVisualizer;

        protected virtual void OnEnable()
        {
            graphVisualizer = GetComponentInChildren<WorldVSPGraphVisualizer>();
            triangleVisualizer = GetComponentInChildren<TriangleVisualizer>();

            gameWorld.onWorldData.AddListener(UpdateWorldData);
        }

        protected virtual void OnDisable()
        {
            gameWorld.onWorldData.RemoveListener(UpdateWorldData);
        }

        public void UpdateWorldData(WorldData worldData)
        {
            graphVisualizer.SetGraph(worldData.graph, worldData.tree);
        }
    }
}