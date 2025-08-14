using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Corelib.SUI;

namespace World
{
    public class WorldEntitiesWindow : EditorWindow
    {
        private const string PREFAB_PATH = "Assets/Resources/Ingame/Templates/World.prefab";

        [SerializeField] private Vector2 scrollPosition;
        private GameObject prefabInstance;
        private Transform entitiesRoot;
        private readonly Dictionary<string, bool> foldouts = new();

        [MenuItem("Tools/Game/World Entities Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<WorldEntitiesWindow>("World Entities");
            window.SetIcon();
        }

        private void OnEnable()
        {
            SetIcon();
            LoadEntities();
        }

        private void OnDisable()
        {
            if (prefabInstance != null)
                PrefabUtility.UnloadPrefabContents(prefabInstance);
        }

        private void SetIcon()
        {
            string iconName = EditorGUIUtility.isProSkin
                ? "d_UnityEditor.SceneHierarchyWindow"
                : "UnityEditor.SceneHierarchyWindow";
            titleContent.image = EditorGUIUtility.IconContent(iconName).image;
        }

        private void LoadEntities()
        {
            if (prefabInstance != null)
                PrefabUtility.UnloadPrefabContents(prefabInstance);

            prefabInstance = PrefabUtility.LoadPrefabContents(PREFAB_PATH);
            entitiesRoot = prefabInstance != null ? prefabInstance.transform.Find("Entities") : null;
            foldouts.Clear();
            if (entitiesRoot != null)
            {
                foreach (Transform child in entitiesRoot)
                {
                    if (!foldouts.ContainsKey(child.name))
                        foldouts.Add(child.name, false);
                }
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Button("Reload").OnClick(() => LoadEntities())
                + RenderEntities()
            )
            .Render();

            EditorGUILayout.EndScrollView();
        }

        private SUIElement RenderEntities()
        {
            if (entitiesRoot == null)
                return SEditorGUILayout.Label("Entities root not found");

            var elements = new List<SUIElement>();
            foreach (Transform category in entitiesRoot)
            {
                foldouts.TryGetValue(category.name, out var fold);

                var childElements = new List<SUIElement>
                {
                    SEditorGUILayout.Label($"Count: {category.childCount}")
                };

                foreach (Transform child in category)
                    childElements.Add(SEditorGUILayout.Label(child.name));

                elements.Add(
                    SEditorGUILayout.FoldGroup(category.name, fold)
                    .OnValueChanged(v => foldouts[category.name] = v)
                    .Content(childElements.Aggregate((a, b) => a + b))
                );
            }

            if (elements.Count == 0)
                return SEditorGUILayout.Label("No categories found");

            return elements.Aggregate((a, b) => a + b);
        }
    }
}
