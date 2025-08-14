using Corelib.Utils;
using UnityEngine;

namespace World
{
    [ExecuteAlways]
    public class TriangleVisualizer : MonoBehaviour
    {
        [LifecycleInject]
        public MeshVisualizer meshVisualizer;

        private void OnEnable()
        {
            meshVisualizer = new MeshVisualizer(this);
            meshVisualizer.OnEnable();
        }

        private void OnDisable()
        {
            meshVisualizer?.OnDisable();
            meshVisualizer?.HideMeshAll();
        }
    }
}
