using System;
using System.Collections.Generic;
using Corelib.SUI;
using PathX;
using UnityEditor;
using UnityEngine;
using World;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowVisualizerTriangle
    {
        [NonSerialized]
        private TriangleVisualizer triangleVisualizer;
        private readonly Dictionary<(BiomeType biome, TriangleDomain domain), Mesh> triangleMeshCache = new();
        private readonly Dictionary<(BiomeType biome, TriangleDomain domain), Mesh> triangleWireMeshCache = new();

        [SerializeField]
        private bool showTriangles = false;
        [SerializeField]
        private bool triangleWireframe = false;
        [SerializeField]
        private TriangleDomain triangleDomain = TriangleDomain.All;
        [SerializeField]
        private BiomeFlags triangleBiomes = BiomeFlags.All;

        public WorldGeneratorWindowVisualizerTriangle(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);
        }

        public SUIElement Render()
        {
            if (triangleVisualizer == null) return SUIElement.Empty();

            return SEditorGUILayout.Group("Visualizer")
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Label("Triangle")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Var("Visible", showTriangles)
                        .OnValueChanged(v => { showTriangles = v; RenderTriangles(); })
                    + SEditorGUILayout.Var("Wireframe", triangleWireframe)
                        .OnValueChanged(v => { triangleWireframe = v; RenderTriangles(); })
                    + SEditorGUILayout.Var("Domain", triangleDomain)
                        .OnValueChanged(v => { triangleDomain = (TriangleDomain)v; RenderTriangles(); })
                    + SEditorGUILayout.EnumFlags("Biomes", triangleBiomes)
                        .OnValueChanged(v => { triangleBiomes = (BiomeFlags)v; RenderTriangles(); })
                    + SEditorGUILayout.Separator()
                )
            );
        }

        private void RenderTriangles()
        {
            if (!showTriangles)
            {
                triangleVisualizer.meshVisualizer?.HideMeshAll();
                return;
            }
            triangleVisualizer.meshVisualizer?.HideMeshAll();
            foreach (BiomeType biome in Enum.GetValues(typeof(BiomeType)))
            {
                if (biome == BiomeType.None) continue;
                if (!triangleBiomes.HasFlag(biome.ToFlag())) continue;
                var cache = triangleWireframe ? triangleWireMeshCache : triangleMeshCache;
                if (cache.TryGetValue((biome, triangleDomain), out var mesh))
                {
                    string key = $"{biome}_{triangleDomain}";
                    triangleVisualizer.meshVisualizer.ShowMesh(key, mesh);
                }
            }
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            this.triangleVisualizer = gameWorld?.GetComponentInChildren<TriangleVisualizer>();
            CacheTriangleMeshes(gameWorld?.worldData);
        }

        private void CacheTriangleMeshes(WorldData worldData)
        {
            triangleMeshCache.Clear();
            triangleWireMeshCache.Clear();

            var data = worldData;
            if (data?.trianglesByBiome == null) return;
            foreach (var biomeKvp in data.trianglesByBiome)
            {
                foreach (var domainKvp in biomeKvp.Value)
                {
                    var tris = domainKvp.Value;
                    var mesh = MeshGenerator.CreateMeshFromTriangles(tris);
                    var wire = MeshGenerator.CreateWireframeMeshFromTriangles(tris);
                    triangleMeshCache[(biomeKvp.Key, domainKvp.Key)] = mesh;
                    triangleWireMeshCache[(biomeKvp.Key, domainKvp.Key)] = wire;
                }
            }
        }
    }
}