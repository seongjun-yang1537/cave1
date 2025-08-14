using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;
using System.Collections.Generic;

namespace ProjectEditor
{
    public class UIHierarchyWindow : EditorWindow
    {
        UIHierarchyTreeView tree;
        [MenuItem("Window/UI Hierarchy")]
        static void Open()
        {
            var window = GetWindow<UIHierarchyWindow>();
            var icon = EditorGUIUtility.IconContent("d_UnityEditor.SceneHierarchyWindow").image as Texture2D;
            window.titleContent = new GUIContent("UI Hierarchy", icon);
        }
        void OnEnable()
        {
            tree = new UIHierarchyTreeView(new TreeViewState());
            tree.Reload();
        }
        void OnGUI()
        {
            tree.OnGUI(new Rect(0, 0, position.width, position.height));
        }

        class UIHierarchyTreeView : TreeView
        {
            public UIHierarchyTreeView(TreeViewState state) : base(state) { }
            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = 0, depth = -1 };
                var scene = SceneManager.GetActiveScene();
                foreach (var go in scene.GetRootGameObjects())
                    Add(go.transform, 0, root);
                if (root.children == null || root.children.Count == 0)
                    root.children = new List<TreeViewItem> { new TreeViewItem { id = -1, depth = 0, displayName = "" } };
                return root;
            }
            void Add(Transform t, int depth, TreeViewItem parent)
            {
                if (Include(t.gameObject))
                {
                    var content = EditorGUIUtility.ObjectContent(t.gameObject, typeof(GameObject));
                    var item = new TreeViewItem { id = t.gameObject.GetInstanceID(), depth = depth, displayName = t.gameObject.name, icon = (Texture2D)content.image };
                    parent.AddChild(item);
                    foreach (Transform c in t)
                        Add(c, depth + 1, item);
                }
                else
                {
                    foreach (Transform c in t)
                        Add(c, depth, parent);
                }
            }
            bool Include(GameObject go)
            {
                if (go.GetComponent<Canvas>() != null)
                    return true;
                var comps = go.GetComponents<Component>();
                for (int i = 0; i < comps.Length; i++)
                    if (comps[i] is UIMonoBehaviour)
                        return true;
                if (go.name.StartsWith("<ref>"))
                    return true;
                return false;
            }
            protected override void SingleClickedItem(int id)
            {
                Selection.activeObject = EditorUtility.InstanceIDToObject(id);
            }
        }
    }
}

