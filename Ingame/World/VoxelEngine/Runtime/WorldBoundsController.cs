using UnityEngine;

namespace VoxelEngine
{
    [ExecuteInEditMode]
    public class WorldBoundsController : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock propertyBlock;

        private Vector3 _calculatedCenter;
        private Vector3 _calculatedSize;

        private static readonly int BoundsCenterID = Shader.PropertyToID("_WorldBoundsCenter");
        private static readonly int BoundsSizeID = Shader.PropertyToID("_WorldBoundsSize");

        protected virtual void OnEnable()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
        }

        public void ApplyShaderProperty()
        {
            if (meshRenderer == null)
                return;

            CalculateWorldBounds();
            UpdateShaderProperties();
        }

        void CalculateWorldBounds()
        {
            MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshFilter.sharedMesh.vertexCount == 0)
            {
                _calculatedCenter = transform.position;
                _calculatedSize = Vector3.one;
                return;
            }

            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            Vector3 min = transform.TransformPoint(vertices[0]);
            Vector3 max = min;

            for (int i = 1; i < vertices.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(vertices[i]);
                min = Vector3.Min(min, worldPos);
                max = Vector3.Max(max, worldPos);
            }

            _calculatedCenter = (min + max) * 0.5f;
            _calculatedSize = max - min;
        }

        void UpdateShaderProperties()
        {
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector(BoundsCenterID, _calculatedCenter);
            propertyBlock.SetVector(BoundsSizeID, _calculatedSize);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        protected virtual void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && transform != null)
            {
                CalculateWorldBounds();
            }
#endif
            if (_calculatedSize != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(_calculatedCenter, _calculatedSize);
            }
        }
    }
}
