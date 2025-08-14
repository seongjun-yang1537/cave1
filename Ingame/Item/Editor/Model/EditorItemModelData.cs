using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(ItemModelData))]
    public class EditorItemModelData : Editor
    {
        ItemModelData script;
        protected virtual void OnEnable()
        {
            script = (ItemModelData)target;
        }

        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                script,
                ItemModelDataInspector.Render(script)
            )
            .Render();
        }
    }
}