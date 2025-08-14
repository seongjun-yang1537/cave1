using Corelib.SUI;
using UnityEditor;

namespace Outgame
{
    [CustomEditor(typeof(ShopItemModelData))]
    public class EditorShopItemModelData : Editor
    {
        ShopItemModelData script;
        private void Awake()
        {
            script = (ShopItemModelData)target;
        }

        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                script,
                ShopItemModelDataInspector.Render(script)
            )
            .Render();
        }
    }
}