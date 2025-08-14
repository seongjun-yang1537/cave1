using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Corelib.Utils;

namespace World
{
    [ExecuteAlways]
    public class WorldVSPGraphVisualizer : MonoBehaviour
    {
        private WorldVSPGraph graph;
        private WorldVSPTree tree;

        [FormerlySerializedAs("graphShowNodes")]
        public bool showGraphNodes = false;
        public bool showGraphEdges = false;
        public bool showTreeNodes = false;
        public bool showTreeEdges = false;
        public bool showBiomes = false;
        public bool showRooms = false;

        public Dictionary<BiomeType, bool> biomeVisibility = new();

        [LifecycleInject]
        public MeshVisualizer meshVisualizer;

        private static Mesh sphereMesh;

        private void OnEnable()
        {
            meshVisualizer = new MeshVisualizer(this);
            meshVisualizer.OnEnable();
            InitTemplates();
            InitBiomeVisibility();
            Render();
        }

        private void OnDisable()
        {
            meshVisualizer?.OnDisable();
            meshVisualizer?.HideMeshAll();
        }

        private static void InitTemplates()
        {
            if (sphereMesh == null)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphereMesh = go.GetComponent<MeshFilter>().sharedMesh;
                if (Application.isPlaying)
                    GameObject.Destroy(go);
                else
                    GameObject.DestroyImmediate(go);
            }
        }

        public void SetGraph(WorldVSPGraph newGraph, WorldVSPTree newTree)
        {
            this.tree = newTree;
            this.graph = newGraph;
            Render();
        }

        public void SetGraph(WorldVSPGraph newGraph)
        {
            this.graph = newGraph;
            Render();
        }

        private void InitBiomeVisibility()
        {
            foreach (BiomeType biome in Enum.GetValues(typeof(BiomeType)))
            {
                if (biome == BiomeType.None) continue;
                if (!biomeVisibility.ContainsKey(biome))
                    biomeVisibility[biome] = true;
            }
        }

        public void Render()
        {
            if (meshVisualizer == null)
                return;

            meshVisualizer.HideMeshAll();

            if ((showGraphNodes || showGraphEdges || showBiomes || showRooms) && graph != null)
                RenderGraphMesh();

            if ((showTreeNodes || showTreeEdges) && tree != null)
                RenderTreeMesh();
        }

        private void RenderGraphMesh()
        {
            InitBiomeVisibility();
            if (showGraphNodes && graph.nodes != null && graph.nodes.Count > 0)
            {
                var nodeInstances = new List<CombineInstance>();
                foreach (var node in graph.nodes)
                {
                    float radius = Mathf.Min(node.size.x, node.size.y, node.size.z) * 0.5f;
                    var inst = new CombineInstance
                    {
                        mesh = sphereMesh,
                        transform = Matrix4x4.TRS(node.center, Quaternion.identity, Vector3.one * radius * 2f)
                    };
                    nodeInstances.Add(inst);
                }

                Mesh mesh = new();
                // Allow large graphs by switching to 32 bit indices before combining
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.CombineMeshes(nodeInstances.ToArray(), true, true);
                Material mat = CreateColoredMaterial(new Color(0.1f, 0.4f, 1f, 0.75f));
                meshVisualizer.ShowMesh("GraphNodes", mesh, mat);
            }

            if (showGraphEdges && graph.edges != null && graph.edges.Count > 0)
            {
                var vertices = new List<Vector3>();
                var indices = new List<int>();

                for (int i = 0; i < graph.edges.Count; i++)
                {
                    var edge = graph.edges[i];
                    if (edge.from >= graph.nodes.Count || edge.to >= graph.nodes.Count) continue;

                    Vector3 from = graph.nodes[edge.from].center;
                    Vector3 to = graph.nodes[edge.to].center;

                    vertices.Add(from);
                    vertices.Add(to);
                    indices.Add(vertices.Count - 2);
                    indices.Add(vertices.Count - 1);
                }

                Mesh mesh = new();
                if (vertices.Count > 65535)
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.SetVertices(vertices);
                mesh.SetIndices(indices, MeshTopology.Lines, 0);
                Material mat = CreateColoredMaterial(new Color(1f, 1f, 1f, 0.6f));
                meshVisualizer.ShowMesh("GraphEdges", mesh, mat);
            }

            if (showBiomes && graph.nodes != null && graph.nodes.Count > 0)
            {
                var biomeGroups = new Dictionary<BiomeType, List<CombineInstance>>();
                foreach (var node in graph.nodes)
                {
                    if (node.biome == BiomeType.None) continue;
                    if (!IsBiomeVisible(node.biome)) continue;

                    var inst = new CombineInstance
                    {
                        mesh = CreateCubeMesh((Vector3)node.size * 0.98f),
                        transform = Matrix4x4.TRS(node.center, Quaternion.identity, Vector3.one)
                    };

                    if (!biomeGroups.TryGetValue(node.biome, out var list))
                    {
                        list = new List<CombineInstance>();
                        biomeGroups[node.biome] = list;
                    }
                    list.Add(inst);
                }

                foreach (var kvp in biomeGroups)
                {
                    Mesh mesh = new();
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                    mesh.CombineMeshes(kvp.Value.ToArray(), true, true);
                    Color c = GetBiomeColor(kvp.Key);
                    Material mat = CreateColoredMaterial(new Color(c.r, c.g, c.b, 0.25f));
                    meshVisualizer.ShowMesh($"Biome_{kvp.Key}", mesh, mat);
                }
            }

            if (showRooms && graph.nodes != null)
            {
                var groups = new Dictionary<BiomeType, List<CombineInstance>>();
                foreach (var node in graph.nodes)
                {
                    if (node.room == null) continue;
                    if (!groups.TryGetValue(node.biome, out var list))
                    {
                        list = new List<CombineInstance>();
                        groups[node.biome] = list;
                    }
                    var inst = new CombineInstance
                    {
                        mesh = CreateCubeMesh((Vector3)node.room.size),
                        transform = Matrix4x4.TRS(node.room.center, Quaternion.identity, Vector3.one)
                    };
                    list.Add(inst);
                }

                foreach (var kvp in groups)
                {
                    Mesh mesh = new();
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                    mesh.CombineMeshes(kvp.Value.ToArray(), true, true);
                    Color c = GetBiomeColor(kvp.Key);
                    Material mat = CreateColoredMaterial(new Color(c.r, c.g, c.b, 0.25f));
                    meshVisualizer.ShowMesh($"Rooms_{kvp.Key}", mesh, mat);
                }
            }
        }

        private void RenderTreeMesh()
        {
            if (tree == null || tree.root == null || meshVisualizer == null)
                return;

            List<WorldVSPNode> allNodes = tree.GetNodes();
            List<WorldVSPNode> leafNodes = tree.GetLeafs();
            int maxDepth = 0;
            foreach (var n in allNodes)
                if (n.depth > maxDepth) maxDepth = n.depth;
            Material sphereMat = CreateColoredMaterial(new Color(0f, 1f, 0f, 0.75f));

            if (showTreeNodes)
            {
                for (int i = 0; i < leafNodes.Count; i++)
                {
                    WorldVSPNode node = leafNodes[i];
                    Mesh mesh = CreateWireCubeMesh((Vector3)node.size * 0.98f);
                    float t = maxDepth == 0 ? 0f : (float)node.depth / maxDepth;
                    Color c = Color.Lerp(Color.green, Color.red, t);
                    GameObject go = meshVisualizer.ShowMesh($"TreeLeaf_{i}", mesh, CreateColoredMaterial(new Color(c.r, c.g, c.b, 0.8f)));
                    if (go != null)
                        go.transform.position = node.center;
                }

                var sphereInstances = new List<CombineInstance>();
                for (int i = 0; i < leafNodes.Count; i++)
                {
                    WorldVSPNode node = leafNodes[i];
                    float radius = Mathf.Min(node.size.x, node.size.y, node.size.z) * 0.25f;
                    var inst = new CombineInstance
                    {
                        mesh = sphereMesh,
                        transform = Matrix4x4.TRS(node.center, Quaternion.identity, Vector3.one * radius * 2f)
                    };
                    sphereInstances.Add(inst);
                }

                Mesh spheres = new Mesh();
                spheres.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                spheres.CombineMeshes(sphereInstances.ToArray(), true, true);
                meshVisualizer.ShowMesh("TreeNodeSpheres", spheres, sphereMat);
            }

            if (showTreeEdges)
            {
                var vertices = new List<Vector3>();
                var indices = new List<int>();
                for (int i = 0; i < allNodes.Count; i++)
                {
                    var node = allNodes[i];
                    if (node.parent == null) continue;
                    vertices.Add(node.center);
                    vertices.Add(node.parent.center);
                    indices.Add(vertices.Count - 2);
                    indices.Add(vertices.Count - 1);
                }
                Mesh lines = new Mesh();
                if (vertices.Count > 65535)
                    lines.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                lines.SetVertices(vertices);
                lines.SetIndices(indices, MeshTopology.Lines, 0);
                meshVisualizer.ShowMesh("TreeEdges", lines, CreateColoredMaterial(new Color(1f, 1f, 1f, 0.6f)));
            }

        }

        private static Mesh CreateCubeMesh(Vector3 size)
        {
            Vector3 hs = size * 0.5f;
            Vector3[] vertices =
            {
                new(-hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, hs.z),
                new(-hs.x, -hs.y, hs.z),
                new(-hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, hs.z),
                new(-hs.x, hs.y, hs.z)
            };
            int[] triangles =
            {
                0,2,1,0,3,2,
                4,5,6,4,6,7,
                4,7,3,4,3,0,
                1,2,6,1,6,5,
                3,7,6,3,6,2,
                4,0,1,4,1,5
            };
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        private static Mesh CreateLineMesh(Vector3 from, Vector3 to)
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(new List<Vector3> { from, to });
            mesh.SetIndices(new int[] { 0, 1 }, MeshTopology.Lines, 0);
            return mesh;
        }

        private static Mesh CreateWireCubeMesh(Vector3 size)
        {
            Vector3 hs = size * 0.5f;
            Vector3[] vertices =
            {
                new(-hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, hs.z),
                new(-hs.x, -hs.y, hs.z),
                new(-hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, hs.z),
                new(-hs.x, hs.y, hs.z)
            };
            int[] indices =
            {
                0,1, 1,2, 2,3, 3,0,
                4,5, 5,6, 6,7, 7,4,
                0,4, 1,5, 2,6, 3,7
            };
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            return mesh;
        }

        private static Color GetBiomeColor(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.Hollow => new Color(0.6f, 0.6f, 0.6f),
                BiomeType.Threadway => Color.yellow,
                BiomeType.Behemire => Color.red,
                BiomeType.Veinreach => Color.blue,
                BiomeType.Fangspire => Color.cyan,
                BiomeType.Silken => Color.magenta,
                _ => Color.gray,
            };
        }

        public bool IsBiomeVisible(BiomeType biome)
        {
            if (biomeVisibility.TryGetValue(biome, out var visible))
                return visible;
            return false;
        }

        public void SetBiomeVisible(BiomeType biome, bool visible)
        {
            biomeVisibility[biome] = visible;
        }

        private static Material CreateColoredMaterial(Color color)
        {
            var mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            mat.SetColor("_Color", color);
            return mat;
        }
    }
}