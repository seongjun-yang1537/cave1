using Corelib.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace VoxelEngine
{
    [ExecuteAlways]
    public class ChunkedScalarFieldMeshRenderer : MonoBehaviour
    {
        public UnityEvent onRenderCompleteAllMeshes = new();
        public UnityEvent<Vector3Int> onRenderCompleteChunkMesh = new();

        [Header("Meshing Parameters")]
        [SerializeField, Range(0f, 1f)]
        protected float isolevel = 0.1f;

        public Material material { get; protected set; }

        private bool visible;
        public bool Visible { get => visible; set => SetVisible(value); }

        protected ChunkMesher chunkMesher;
        protected MeshVisualizer meshVisualizer;

        protected Dictionary<Vector3Int, Mesh> meshCache = new();
        private bool waitingForAllMeshes;

        #region Unity Lifecycle & Core Logic

        protected virtual void OnEnable()
        {
            chunkMesher = new ChunkMesher();
            meshVisualizer = new MeshVisualizer(this, new VoxelChunkMeshCreator());
            meshVisualizer.OnEnable();
        }

        protected virtual void OnDestroy()
        {
            chunkMesher?.Dispose();

            foreach (var mesh in meshCache.Values)
            {
                if (mesh == null) continue;
                if (Application.isPlaying) Destroy(mesh);
                else DestroyImmediate(mesh);
            }
            meshCache.Clear();

            meshVisualizer?.Dispose();
        }

        protected void Update()
        {
            chunkMesher.CheckForCompletedJobs(ApplyMeshFromJobResult);

            if (waitingForAllMeshes && !chunkMesher.HasRunningJobs)
            {
                waitingForAllMeshes = false;
                onRenderCompleteAllMeshes.Invoke();
            }
        }
        #endregion

        public GameObject GetChunkObject(Vector3Int chunkCoord)
        {
            string key = ChunkKey(chunkCoord);
            if (string.IsNullOrEmpty(key)) return null;

            if (meshVisualizer.childMeshes.TryGetValue(key, out var mesh))
            {
                return mesh.gameObject;
            }

            return null;
        }

        public void SetMaterial(Material newMaterial)
        {
            material = newMaterial;
            meshVisualizer.SetMaterial(newMaterial);
        }

        public void SetPropertyTexture(string property, Texture newTexture)
        {
            meshVisualizer.SetPropertyTexture(property, newTexture);
        }

        private void SetVisible(bool newVisible)
        {
            visible = newVisible;
            meshVisualizer.SetActiveMeshRoot(newVisible);
        }

        public virtual void RenderField(ChunkedScalarField scalarField)
        {
            ClearAllMeshes();

            meshCache?.Clear();
            meshVisualizer?.HideMeshAll();

            RenderChunks(scalarField, scalarField.GetLoadedChunkCoordinates());
        }

        public void RenderChunks(ChunkedScalarField scalarField, IEnumerable<Vector3Int> chunkCoords)
        {
            bool scheduled = false;
            foreach (var chunkCoord in chunkCoords)
            {
                meshCache.Remove(chunkCoord);
                scheduled |= chunkMesher.ScheduleMeshingJob(chunkCoord, scalarField, isolevel);
            }

            if (scheduled)
            {
                waitingForAllMeshes = true;
            }
        }

        private void ApplyMeshFromJobResult(Vector3Int chunkCoord, NativeList<Vector3> vertices, NativeList<int> triangles)
        {
            if (vertices.Length == 0 || triangles.Length == 0)
            {
                HideMesh(chunkCoord);
                return;
            }

            var mesh = new Mesh
            {
                indexFormat = vertices.Length > 65535
                    ? UnityEngine.Rendering.IndexFormat.UInt32
                    : UnityEngine.Rendering.IndexFormat.UInt16
            };

            mesh.SetVertices(vertices.AsArray());
            mesh.SetTriangles(triangles.AsArray().ToArray(), 0);

            mesh.uv = new Vector2[vertices.Length];

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            var verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++) verts[i] -= Vector3.one;
            mesh.vertices = verts;

            meshCache[chunkCoord] = mesh;
            ApplyMesh(chunkCoord, mesh);

            onRenderCompleteChunkMesh.Invoke(chunkCoord);
        }

        private string ChunkKey(Vector3Int chunkCoord)
            => $"Chunk_{chunkCoord.x}_{chunkCoord.y}_{chunkCoord.z}";

        private void ApplyMesh(Vector3Int chunkCoord, Mesh mesh)
        {
            string chunkKey = ChunkKey(chunkCoord);
            Material mat = material;
            if (mat == null)
            {
                mat = Resources.Load<Material>("VoxelEngine/DefaultMaterial");
            }
            GameObject chunkObject = meshVisualizer.ShowMesh(chunkKey, mesh, mat);
            if (chunkObject != null)
            {
                chunkObject.transform.position = (Vector3)chunkCoord * ChunkedScalarField.CHUNK_SIZE;
                MeshCollider meshCollider = chunkObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.sharedMesh = null;
                    meshCollider.sharedMesh = mesh;
                }
            }
        }

        public void ShowMesh(Vector3Int chunkCoord)
        {
            meshVisualizer.ShowMesh(ChunkKey(chunkCoord));
        }

        public void HideMesh(Vector3Int chunkCoord)
        {
            meshVisualizer.HideMesh(ChunkKey(chunkCoord));
        }

        [ContextMenu("Clear All Meshes")]
        public void ClearAllMeshes()
        {
            meshCache.Clear();
            meshVisualizer?.ClearMeshAll();
        }

        public bool IsRendering => waitingForAllMeshes || chunkMesher.HasRunningJobs;

        public async Cysharp.Threading.Tasks.UniTask WaitForAllMeshesAsync()
        {
            await Cysharp.Threading.Tasks.UniTask
                .WaitUntil(() => !waitingForAllMeshes && !chunkMesher.HasRunningJobs);
        }
    }
}