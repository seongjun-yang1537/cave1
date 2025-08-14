using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Core;
using VoxelEngine;
using PathX;

namespace World
{
    [ExecuteAlways]
    [RequireComponent(typeof(WorldMap))]
    public class WorldMapMeshRenderer : VoxelMapMeshRenderer
    {
        private const string PATH_WORLD_MATERIAL = "Ingame/World/WorldMaterial";

        private Material worldMaterial;

        protected WorldMap worldMap;
        private Dictionary<Vector3Int, Texture3D> chunkBlendMaps = new();
        private Dictionary<Vector3Int, Texture3D> storedBlendMaps = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            onRenderCompleteAllMeshes.AddListener(OnRenderCompleteAllMeshes);
            onRenderCompleteChunkMesh.AddListener(OnRenderCompleteChunkMesh);

            worldMap = GetComponent<WorldMap>();
            worldMaterial = Resources.Load<Material>(PATH_WORLD_MATERIAL);
            SetMaterial(worldMaterial);

            worldMap.onRenderWorldMap.AddListener(OnRenderWorldMap);
            worldMap.onChunkActive.AddListener(OnChunkActive);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onRenderCompleteAllMeshes.RemoveListener(OnRenderCompleteAllMeshes);
            onRenderCompleteChunkMesh.RemoveListener(OnRenderCompleteChunkMesh);

            worldMap.onRenderWorldMap.RemoveListener(OnRenderWorldMap);
            worldMap.onChunkActive.RemoveListener(OnChunkActive);

        }

        protected virtual void OnRenderWorldMap(ChunkedScalarField field, WorldVSPGraph graph)
        {
            onRenderCompleteChunkMesh.RemoveListener(OnChunkMeshCompleted);
            chunkBlendMaps = CreateChunkBlendMaps(field, graph);
            storedBlendMaps = new Dictionary<Vector3Int, Texture3D>(chunkBlendMaps);
            onRenderCompleteChunkMesh.AddListener(OnChunkMeshCompleted);

            RenderField(field);
        }

        protected virtual void OnRenderCompleteChunkMesh(Vector3Int chunkCoord)
        {
            GameObject chunkObject = GetChunkObject(chunkCoord);
            if (chunkObject == null) return;

            WorldBoundsController wbc = chunkObject.GetComponent<WorldBoundsController>();
            wbc.ApplyShaderProperty();
        }

        protected virtual void OnRenderCompleteAllMeshes()
        {
            worldMap.onRenderCompleteChunkMesh.Invoke();
        }

        protected virtual void OnChunkActive(Vector3Int chunkCoord, bool active)
        {
            if (active) ShowMesh(chunkCoord);
            else HideMesh(chunkCoord);
        }

        private void OnChunkMeshCompleted(Vector3Int chunkCoord)
        {
            Texture3D map = null;

            if (chunkBlendMaps != null && chunkBlendMaps.TryGetValue(chunkCoord, out map))
            {
                storedBlendMaps[chunkCoord] = map;
                chunkBlendMaps.Remove(chunkCoord);
                if (chunkBlendMaps.Count == 0)
                {
                    onRenderCompleteChunkMesh.RemoveListener(OnChunkMeshCompleted);
                }
            }
            else if (!storedBlendMaps.TryGetValue(chunkCoord, out map))
            {
                return;
            }

            ApplyBlendMap(chunkCoord, map);
        }

        public void ApplyBlendMap(Vector3Int chunkCoord, Texture3D blendMap)
        {
            if (blendMap == null) return;
            string key = $"Chunk_{chunkCoord.x}_{chunkCoord.y}_{chunkCoord.z}";
            meshVisualizer.SetMaterial(key, worldMaterial);
            meshVisualizer.SetPropertyTexture(key, "_BlendMap", blendMap);
        }

        [Unity.Burst.BurstCompile]
        private struct FillBiomeJob : IJob
        {
            [NativeDisableParallelForRestriction] public NativeArray<Color32> colors;
            [ReadOnly] public Vector3Int fieldSize;
            [ReadOnly] public Vector3Int regionTL;
            [ReadOnly] public Vector3Int regionBR;
            [ReadOnly] public NativeArray<float3> centers;
            [ReadOnly] public NativeArray<float> radii;
            [ReadOnly] public Color32 color;

            private int Index(int x, int y, int z)
                => z + y * fieldSize.z + x * fieldSize.z * fieldSize.y;

            public void Execute()
            {
                for (int x = regionTL.x; x < regionBR.x; x++)
                    for (int y = regionTL.y; y < regionBR.y; y++)
                        for (int z = regionTL.z; z < regionBR.z; z++)
                        {
                            float3 p = new float3(x + 0.5f, y + 0.5f, z + 0.5f);
                            bool inside = false;
                            for (int i = 0; i < centers.Length; i++)
                            {
                                float3 d = p - centers[i];
                                if (math.lengthsq(d) <= radii[i] * radii[i])
                                {
                                    inside = true;
                                    break;
                                }
                            }
                            if (!inside) continue;
                            colors[Index(x, y, z)] = color;
                        }
            }
        }

        private void GenerateSpheres(WorldVSPGraphNode node, out NativeArray<float3> centers, out NativeArray<float> radii)
        {
            MT19937 rng = GameRng.World;
            int volume = node.size.Area();
            int count = Mathf.Clamp(volume / 128, 4, 12);

            centers = new NativeArray<float3>(count, Allocator.TempJob);
            radii = new NativeArray<float>(count, Allocator.TempJob);

            float minDim = Mathf.Min(node.lenX, Mathf.Min(node.lenY, node.lenZ));
            float baseRadius = Mathf.Max(1f, minDim * 0.4f);

            for (int i = 0; i < count; i++)
            {
                float x = rng.NextFloat(node.topLeft.x, node.bottomRight.x);
                float y = rng.NextFloat(node.topLeft.y, node.bottomRight.y);
                float z = rng.NextFloat(node.topLeft.z, node.bottomRight.z);
                centers[i] = new float3(x, y, z);
                radii[i] = rng.NextFloat(baseRadius * 0.5f, baseRadius);
            }
        }

        private Dictionary<Vector3Int, Texture3D> CreateChunkBlendMaps(ChunkedScalarField field, WorldVSPGraph graph)
        {
            int voxelCount = field.Size.Area();
            var colorsArr = new NativeArray<Color32>(voxelCount, Allocator.TempJob);

            JobHandle chain = default;
            var sphereCenters = new List<NativeArray<float3>>();
            var sphereRadii = new List<NativeArray<float>>();

            foreach (var node in graph.nodes)
            {
                GenerateSpheres(node, out var centers, out var radii);

                Color32 color = node.biome switch
                {
                    BiomeType.Hollow => new Color32(255, 0, 0, 0),
                    BiomeType.Threadway => new Color32(0, 255, 0, 0),
                    BiomeType.Behemire => new Color32(0, 0, 255, 0),
                    _ => new Color32(0, 0, 0, 0)
                };

                var job = new FillBiomeJob
                {
                    colors = colorsArr,
                    fieldSize = field.Size,
                    regionTL = node.topLeft,
                    regionBR = node.bottomRight,
                    centers = centers,
                    radii = radii,
                    color = color
                };

                chain = job.Schedule(chain);

                sphereCenters.Add(centers);
                sphereRadii.Add(radii);
            }

            chain.Complete();

            var colors = new List<Color32>(voxelCount);
            colors.AddRange(colorsArr.ToArray());

            foreach (var c in sphereCenters) c.Dispose();
            foreach (var r in sphereRadii) r.Dispose();
            colorsArr.Dispose();

            colors = BlendMapGaussianBlur.Apply(field.Size, colors, iteration: 3, customRadius: 5);

            int padded = ChunkedScalarField.CHUNK_SIZE + 2;
            var blendMaps = new Dictionary<Vector3Int, Texture3D>();
            Vector3Int chunkSize = new Vector3Int(padded, padded, padded);

            foreach (var chunkCoord in field.GetLoadedChunkCoordinates())
            {
                Texture3D tex = Texture3DGenerator.GenerateTexture3D(
                    chunkSize,
                    localCoord =>
                    {
                        Vector3Int global = chunkCoord * ChunkedScalarField.CHUNK_SIZE + localCoord - Vector3Int.one;
                        if (field.IsInBounds(global.x, global.y, global.z))
                            return colors[field.CalculateIndex(global)];
                        else
                            return new Color32(0, 0, 0, 0);
                    });
                blendMaps[chunkCoord] = tex;
            }

            return blendMaps;
        }
    }
}