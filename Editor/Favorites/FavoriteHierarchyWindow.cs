using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using Core;
using Corelib.SUI;

namespace ProjectEditor
{
    public class FavoriteHierarchyWindow : EditorWindow
    {
        SceneFavoriteData asset;
        SerializedObject serialized;
        SerializedProperty objectsProperty;
        SerializedProperty attributeObjectsProperty;
        SearchField searchField;
        string search = string.Empty;
        GameObject addTarget;

        [MenuItem("Window/Favorite Hierarchy")]
        static void Open()
        {
            var window = GetWindow<FavoriteHierarchyWindow>();
            var icon = EditorGUIUtility.IconContent("Favorite").image as Texture2D;
            window.titleContent = new GUIContent("Favorite", icon);
        }

        void OnEnable()
        {
            searchField = new SearchField();
            EditorSceneManager.sceneOpened += (s, m) => LoadAsset();
            LoadAsset();
        }

        void LoadAsset()
        {
            asset = FavoriteHierarchyUtility.LoadOrCreate();
            serialized = new SerializedObject(asset);
            objectsProperty = serialized.FindProperty("objects");
            attributeObjectsProperty = serialized.FindProperty("attributeObjects");
        }

        void OnGUI()
        {
            serialized.Update();
            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Horizontal().LabelWidth(0).Content(
                    SEditorGUILayout.Var("", addTarget).OnValueChanged(v => addTarget = v as GameObject)
                    + SEditorGUILayout.Button("+").Width(20).OnClick(() =>
                    {
                        if (addTarget)
                        {
                            FavoriteHierarchyUtility.Add(asset, addTarget);
                            addTarget = null;
                            LoadAsset();
                        }
                    })
                )
                + SEditorGUILayout.Separator()
                + SEditorGUILayout.Action(() =>
                {
                    if (searchField != null)
                        search = searchField.OnGUI(search);
                    else
                        search = EditorGUILayout.TextField(search);
                })
                + SEditorGUILayout.Label(FavoriteHierarchyUtility.GetCurrentAssetPath())
                + SEditorGUILayout.Object("Data", asset, typeof(SceneFavoriteData)).OnValueChanged(o => {})
                + RenderList(objectsProperty, true)
                + SEditorGUILayout.Separator()
                + RenderList(attributeObjectsProperty, false)
            ).Render();
            serialized.ApplyModifiedProperties();
        }

        SUIElement RenderList(SerializedProperty property, bool removable)
        {
            var elements = new List<SUIElement>();
            for (int i = 0; i < property.arraySize; i++)
            {
                var p = property.GetArrayElementAtIndex(i);
                var id = p.stringValue;
                var obj = FavoriteHierarchyUtility.Find(id);
                var name = obj != null ? obj.name : id;
                if (!string.IsNullOrEmpty(search) && name.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                var row = SEditorGUILayout.Horizontal().Content(
                    SEditorGUILayout.Object("", obj, typeof(GameObject)).OnValueChanged(o =>
                    {
                        if (o == null)
                            p.stringValue = string.Empty;
                        else
                            p.stringValue = FavoriteHierarchyUtility.GetId(o as GameObject);
                    })
                );
                if (removable)
                {
                    int index = i;
                    row.Content(
                        SEditorGUILayout.Button("-").Width(20).OnClick(() =>
                        {
                            property.DeleteArrayElementAtIndex(index);
                            FavoriteHierarchyUtility.Remove(asset, id);
                        })
                    );
                }
                elements.Add(row);
            }
            if (elements.Count == 0) return SUIElement.Empty();
            var container = SEditorGUILayout.Vertical();
            container.Content(elements.Aggregate((a, b) => a + b));
            return container;
        }
    }
}
