using UnityEditor;
using UnityEngine;
using Corelib.Utils;
using Corelib.SUI;
using Core;
using Ingame;

namespace World
{
    public class WorldGeneratorWindow : EditorWindow
    {
        private const string PATH_WORLD_TEMPLATE = "Ingame/Templates/World";

        #region ========== Data ==========
        [SerializeField]
        private GameWorld gameWorld;
        private WorldGenerationPreset preset => gameWorld?.preset;
        private WorldPipelineAsset pipelineAsset => gameWorld?.pipelineAsset;
        #endregion =======================

        #region ========== UI ==========
        [SerializeField]
        private Vector2 scrollPosition;

        private WorldGenerationPreset prevPreset;

        private WorldGeneratorWindowVisualizer visualizerWindow;
        private WorldGeneratorWindowPipeline pipelineWindow;
        private WorldGeneratorWindowInfo infoWindow;
        private WorldGeneratorWindowOverlay overlayWindow;
        #endregion =====================

        [MenuItem("Tools/Game/World Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<WorldGeneratorWindow>("World Generator");
            window.SetIcon();
        }

        private void OnEnable()
        {
            SetIcon();
            EnsureGameWorld();

            pipelineWindow = new WorldGeneratorWindowPipeline(pipelineAsset);
            visualizerWindow = new WorldGeneratorWindowVisualizer(gameWorld);
            overlayWindow = new WorldGeneratorWindowOverlay(gameWorld);
            infoWindow = new WorldGeneratorWindowInfo(gameWorld?.worldData);

            ApplyPreset();
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if (gameWorld == null)
                EnsureGameWorld();

            SEditorGUILayout.Vertical()
            .Content(
                RenderDataPropertyPanel()
                + RenderWorldGeneration()
                + infoWindow.Render()
                + visualizerWindow.Render()
                + overlayWindow.Render(gameWorld)
            )
            .Render();

            EditorGUILayout.EndScrollView();
        }

        private void OnDisable()
        {
            overlayWindow?.OnDisable();
        }

        private SUIElement RenderDataPropertyPanel()
        {
            SUIElement content = SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("GameWorld", gameWorld)
                    .OnValueChanged(value => gameWorld = value as GameWorld)
                    + SEditorGUILayout.Button("R")
                    .OnClick(() => EnsureGameWorld())
                    .Width(24)
                );

            if (gameWorld != null)
            {
                content += SEditorGUILayout.Var("Preset", preset)
                .OnValueChanged(value => SetPreset(value as WorldGenerationPreset));
            }

            return SEditorGUILayout.Vertical("Box")
            .Content(
                content
                + SEditorGUILayout.Separator()
            );
        }

        private SUIElement RenderWorldGeneration()
        {
            if (preset == null) return SUIElement.Empty();

            return SEditorGUI.ChangeCheck(
                    preset,
                    SEditorGUILayout.Vertical()
                    .Content(
                        SEditorGUILayout.Group("Generation")
                        .Content(
                            SEditorGUILayout.Var("Theme", preset.theme)
                            .OnValueChanged(value => preset.theme = (WorldTheme)value)
                            + SEditorGUILayout.Var("Size", preset.size)
                            .OnValueChanged(value => preset.size = value)
                            + SEditorGUILayout.Var("Max Depth", preset.maxDepth)
                            .OnValueChanged(value => preset.maxDepth = value)
                            + SEditorGUILayout.Var("Min Cell Size", preset.minCellSize)
                            .OnValueChanged(value => preset.minCellSize = value)
                            + SEditorGUILayout.Var("Seed (-1 = random)", preset.seed)
                            .OnValueChanged(value => preset.seed = value)
                            + SEditorGUILayout.Button("Generate")
                            .OnClick(GenerateWorld)
                            + SEditorGUILayout.Button("Clear Entities")
                            .OnClick(() => EntitySystem.KillAllEntities())
                            + SEditorGUILayout.Var("Visible Mesh", WorldSystem.MeshVisible)
                            .OnValueChanged(_ => ToggleWorldMeshVisible())
                            + pipelineWindow.Render()
                        )
                    )
                );
        }

        private void SetPreset(WorldGenerationPreset newPreset)
        {
            gameWorld.SetGenerationPreset(newPreset);
            ApplyPreset();
        }

        private void ToggleWorldMeshVisible()
        {
            WorldSystem.MeshVisible = !WorldSystem.MeshVisible;
        }

        private async void GenerateWorld()
        {
            if (gameWorld == null)
            {
                Debug.LogWarning("worldGameObject not found");
                return;
            }

            await gameWorld.Generate();

            visualizerWindow?.UpdateContext(gameWorld);
            overlayWindow?.UpdateContext(gameWorld);
            infoWindow?.UpdateContext(gameWorld.worldData);

            SceneView.RepaintAll();
        }

        private void EnsureGameWorld()
        {
            if (gameWorld == null)
            {
                gameWorld = FindObjectOfType<GameWorld>();
                if (gameWorld == null)
                {
                    GameObject worldTemplate = Resources.Load<GameObject>(PATH_WORLD_TEMPLATE);
                    GameObject go;

#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        go = (GameObject)PrefabUtility.InstantiatePrefab(worldTemplate);
                    }
                    else
                    {
                        go = Instantiate(worldTemplate);
                    }
#else
                    go = Instantiate(worldTemplate);
#endif

                    go.transform.Reset();
                    go.name = "<World> World";

                    gameWorld = go.GetComponent<GameWorld>();
                }
            }

            visualizerWindow?.UpdateContext(gameWorld);
            overlayWindow?.UpdateContext(gameWorld);
            infoWindow?.UpdateContext(gameWorld.worldData);
        }

        private void SetIcon()
        {
            string iconName = EditorGUIUtility.isProSkin ? "d_UnityEditor.ConsoleWindow" : "UnityEditor.ConsoleWindow";
            titleContent.image = EditorGUIUtility.IconContent(iconName).image;
        }

        private void ApplyPreset()
        {
            if (preset == null || preset == prevPreset) return;
            if (preset.pipelineAsset != null)
            {
                pipelineWindow.UpdateContext(preset.pipelineAsset);
            }
            prevPreset = preset;
        }
    }
}
