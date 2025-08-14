using System;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace VoxelEngine
{
    [CustomEditor(typeof(BitpackedScalarFieldConfig))]
    public class EditorBitpackedScalarFieldConfig : Editor
    {
        private Vector3Int newSize = new Vector3Int(32, 32, 32);

        private float noiseScale = 0.05f;
        private float terrainHeightMultiplier = 15f;
        private float baseGroundLevel = 5f;
        private int seed = -1;

        private long countOfOnes = -1;

        public override void OnInspectorGUI()
        {
            var config = (BitpackedScalarFieldConfig)target;

            var field = config.template;

            if (field == null) return;

            SEditorGUILayout.Vertical()
                .Content(
                    CreateFieldControlContent(config)

                    + (field != null
                        ? CreateInformationContent(field) + SEditorGUILayout.Space(5) + CreateGeneratorToolsContent(config)
                        : SEditorGUILayout.Label("Field data is not created yet."))
                )
                .Render();
        }

        private SUIElement CreateFieldControlContent(BitpackedScalarFieldConfig config)
        {
            return SEditorGUILayout.Label("Field Control")
                + SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Var("New Size", newSize)
                            .OnValueChanged(v => newSize = v)
                        + SEditorGUILayout.Button("Create / Resize Field")
                            .OnClick(() =>
                            {
                                config.CreateOrResize(newSize);
                                countOfOnes = -1; // 통계 리셋
                                EditorUtility.SetDirty(config);
                            })
                    );
        }

        private SUIElement CreateInformationContent(IScalarField field)
        {
            Vector3Int size = field.Size;
            long totalVoxels = (long)size.x * size.y * size.z;
            long bitsPerValue = 1; // Bit1 타입이므로
            long capacityInBytes = (totalVoxels * bitsPerValue + 7) / 8;

            var content = SEditorGUILayout.Label("Information")
                        + SEditorGUILayout.Vertical("box")
                            .Content(
                                SEditorGUILayout.Label($"Field Size: {size.x} x {size.y} x {size.z}")
                                + SEditorGUILayout.Label($"Total Voxels: {totalVoxels:N0}")
                                + SEditorGUILayout.Label($"Estimated Size: {FormatBytes(capacityInBytes)}")
                            );

            if (countOfOnes >= 0)
            {
                long countOfZeros = totalVoxels - countOfOnes;
                float percentOnes = (float)countOfOnes / totalVoxels * 100f;
                float percentZeros = 100f - percentOnes;

                content += SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Label($"Value 1: {countOfOnes:N0} ({percentOnes:F1}%)")
                        + SEditorGUILayout.Label($"Value 0: {countOfZeros:N0} ({percentZeros:F1}%)")
                    );
            }

            content += SEditorGUILayout.Button("Calculate Voxel Counts")
                .OnClick(() =>
                {
                    CalculateVoxelCounts(field);
                    Repaint(); // UI를 즉시 다시 그리도록 요청
                });

            return content;
        }

        private SUIElement CreateGeneratorToolsContent(BitpackedScalarFieldConfig config)
        {
            return SEditorGUILayout.Group("Terrain Generation Tools")
            .Content(
                SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Label("Perlin Noise Terrain")
                        + SEditorGUILayout.Var("Seed (-1 for random)", seed).OnValueChanged(v => seed = v)
                        + SEditorGUILayout.Var("Noise Scale", noiseScale).OnValueChanged(v => noiseScale = v)
                        + SEditorGUILayout.Var("Height Multiplier", terrainHeightMultiplier).OnValueChanged(v => terrainHeightMultiplier = v)
                        + SEditorGUILayout.Var("Base Ground Level", baseGroundLevel).OnValueChanged(v => baseGroundLevel = v)
                        + SEditorGUILayout.Button("Generate Perlin")
                            .OnClick(() =>
                            {
                                config.GeneratePerlinTerrain(seed, noiseScale, terrainHeightMultiplier, baseGroundLevel);
                                countOfOnes = -1;
                                EditorUtility.SetDirty(config);
                            })
                    )
                + SEditorGUILayout.Space(5)
                + SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Label("Random Terrain")
                        + SEditorGUILayout.Var("Seed (-1 for random)", seed).OnValueChanged(v => seed = v)
                        + SEditorGUILayout.Button("Generate Random")
                            .OnClick(() =>
                            {
                                config.GenerateRandomTerrain(seed);
                                countOfOnes = -1;
                                EditorUtility.SetDirty(config);
                            })
                    )
            );
        }

        private void CalculateVoxelCounts(IScalarField field)
        {
            countOfOnes = 0;
            Vector3Int size = field.Size;
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    for (int z = 0; z < size.z; z++)
                    {
                        if (field[x, y, z] == 1)
                        {
                            countOfOnes++;
                        }
                    }
        }

        private static string FormatBytes(long bytes)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB" };
            if (bytes == 0) return "0 " + suf[0];
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return num.ToString() + " " + suf[place];
        }
    }
}