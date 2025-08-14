using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VoxelEngine;
using Ingame;
using Corelib.Utils;

namespace World
{
    [RequireComponent(typeof(WorldMap))]
    public class WorldSystem : Singleton<WorldSystem>
    {
        private ChunkedScalarField field;
        private WorldVSPGraph graph;

        public UnityEvent<PlayerController, Chunk> onPlayerEnterChunk = new();
        public UnityEvent<PlayerController, Chunk> onPlayerExitChunk = new();
        private readonly Dictionary<PlayerController, Vector3Int> playerChunks = new();

        [SerializeField]
        private Vector3Int activeChunkRadius = 2 * Vector3Int.one;
        private HashSet<Vector3Int> activeChunks = new();

        private WorldMap _worldMap = null;
        public WorldMap WorldMap { get => _worldMap ??= GetComponent<WorldMap>(); }

        public static bool MeshVisible
        {
            get => Instance.WorldMap?.MeshVisible ?? false;
            set
            {
                if (Instance.WorldMap != null)
                    Instance.WorldMap.MeshVisible = value;
            }
        }

        public static void SetWorldMap(ChunkedScalarField newField, WorldVSPGraph graph)
        {
            Instance.field = newField;
            Instance.graph = graph;
            Instance.WorldMap.SetWorldMap(newField, graph);
        }
        public static bool HasField() => Instance.field != null;

        public static void SetVoxel(Vector3Int position, int value)
            => Instance.WorldMap.SetVoxel(Instance.field, position, value);

        public static void AddVoxel(Vector3Int position)
            => Instance.WorldMap.SetVoxel(Instance.field, position, 1);
        public static void RemoveVoxel(Vector3Int position)
            => Instance.WorldMap.SetVoxel(Instance.field, position, 0);

        public static void SetBox(PBox box, int value)
            => Instance.WorldMap.SetBox(Instance.field, box, value);

        public static void FillBox(PBox box)
            => Instance.WorldMap.SetBox(Instance.field, box, 1);

        public static void CarveBox(PBox box)
            => Instance.WorldMap.SetBox(Instance.field, box, 0);

        public static void SetSphere(PSphere sphere, int value)
            => Instance.WorldMap.SetSphere(Instance.field, sphere, value);

        public static void FillSphere(PSphere sphere)
            => Instance.WorldMap.SetSphere(Instance.field, sphere, 1);

        public static void CarveSphere(PSphere sphere)
            => Instance.WorldMap.SetSphere(Instance.field, sphere, 0);

        protected virtual void OnEnable()
        {
            WorldMap.onRenderCompleteChunkMesh.AddListener(OnRenderCompleteChunkMesh);

            onPlayerEnterChunk.AddListener(OnPlayerEnterChunk);
            onPlayerExitChunk.AddListener(OnPlayerExitChunk);
        }

        protected virtual void OnDisable()
        {
            WorldMap.onRenderCompleteChunkMesh.RemoveListener(OnRenderCompleteChunkMesh);

            onPlayerEnterChunk.RemoveListener(OnPlayerEnterChunk);
            onPlayerExitChunk.RemoveListener(OnPlayerExitChunk);
        }

        private void OnRenderCompleteChunkMesh()
        {
            HashSet<Vector3Int> fullActiveChunk = new();
            foreach (var pos in field.GetLoadedChunkCoordinates())
                fullActiveChunk.Add(pos);
            Instance.activeChunks = fullActiveChunk;

            Instance.UpdatePlayerChunk();
            Instance.UpdateActiveChunks();
        }

        protected virtual void Update()
        {
            UpdatePlayerChunk();
        }

        private void UpdatePlayerChunk()
        {
            if (field == null) return;

            foreach (var player in PlayerSystem.Players)
            {
                Vector3Int voxel = Vector3Int.FloorToInt(player.transform.position);
                var (chunkCoord, _) = field.GetChunkAndLocalCoord(voxel.x, voxel.y, voxel.z);

                if (!playerChunks.TryGetValue(player, out var prevCoord))
                {
                    playerChunks[player] = chunkCoord;
                    var enterChunk = new Chunk(chunkCoord, field.GetChunk(chunkCoord));
                    onPlayerEnterChunk.Invoke(player, enterChunk);
                }
                else if (prevCoord != chunkCoord)
                {
                    var exitChunk = new Chunk(prevCoord, field.GetChunk(prevCoord));
                    onPlayerExitChunk.Invoke(player, exitChunk);

                    playerChunks[player] = chunkCoord;
                    var enterChunk = new Chunk(chunkCoord, field.GetChunk(chunkCoord));
                    onPlayerEnterChunk.Invoke(player, enterChunk);
                }
            }
        }

        private void OnPlayerEnterChunk(PlayerController playerController, Chunk chunk)
        {
            UpdateActiveChunks();
        }

        private void OnPlayerExitChunk(PlayerController playerController, Chunk chunk)
        {
            UpdateActiveChunks();
        }

        private void UpdateActiveChunks()
        {
            if (field == null) return;

            var newActive = new HashSet<Vector3Int>();

            foreach (var kvp in playerChunks)
            {
                Vector3Int center = kvp.Value;
                for (int dx = -activeChunkRadius.x; dx <= activeChunkRadius.x; dx++)
                    for (int dy = -activeChunkRadius.y; dy <= activeChunkRadius.y; dy++)
                        for (int dz = -activeChunkRadius.z; dz <= activeChunkRadius.z; dz++)
                        {
                            Vector3Int c = new Vector3Int(center.x + dx, center.y + dy, center.z + dz);
                            if (field.GetChunk(c) != null)
                                newActive.Add(c);
                        }
            }

            foreach (var coord in activeChunks)
                if (!newActive.Contains(coord))
                    WorldMap.SetChunkActive(coord, false);

            foreach (var coord in newActive)
                if (!activeChunks.Contains(coord))
                    WorldMap.SetChunkActive(coord, true);

            activeChunks = newActive;
        }
    }
}
