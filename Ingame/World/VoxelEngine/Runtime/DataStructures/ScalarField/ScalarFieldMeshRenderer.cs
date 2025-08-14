using Corelib.Utils;
using UnityEngine;

namespace VoxelEngine
{
    [ExecuteAlways]
    public class ScalarFieldMeshRenderer : MonoBehaviour
    {
        public BitpackedScalarFieldConfig bitpackedScalarField;
        public ChunkedScalarFieldConfig chunkScalarField;

        public IScalarField scalarField
        {
            get
            {
                if (bitpackedScalarField?.template != null) return bitpackedScalarField.template;
                if (chunkScalarField?.template != null) return chunkScalarField.template;
                return null;
            }
        }

        private const string DEBUG_MESH_KEY = "MarchingCubes_Result";
        [SerializeField] private float isolevel = 0.5f;

        [LifecycleInject]
        public MeshVisualizer meshVisualizer;

        public void OnEnable()
        {
            LifecycleInjectionUtil.ConstructLifecycleObjects(this);
            LifecycleInjectionUtil.OnEnable(this);
        }

        public void OnDisable()
        {
            LifecycleInjectionUtil.OnDisable(this);
        }

        public void HideMesh()
        {
            meshVisualizer?.HideMesh(DEBUG_MESH_KEY);
        }
    }
}