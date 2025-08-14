using UnityEditor.SceneManagement;
using UnityEditor;

namespace ProjectEditor
{
    [InitializeOnLoad]
    public static class FavoriteHierarchyInitializer
    {
        static FavoriteHierarchyInitializer()
        {
            EditorSceneManager.sceneOpened += (s, m) => FavoriteHierarchyUtility.LoadOrCreate();
        }
    }
}
