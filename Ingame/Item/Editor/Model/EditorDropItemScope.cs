using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(WorldItemScope))]
    public class EditorWorldItemScope : Editor
    {
        WorldItemScope worldItemScope;

        ItemModel itemModel
        {
            get => worldItemScope.itemModel ?? ItemModelFactory.Create(worldItemScope.itemModelData, worldItemScope.itemModelState);
        }

        protected void OnEnable()
        {
            worldItemScope = (WorldItemScope)target;
        }

        bool foldItemModel = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                RenderItemModel(itemModel)
            )
            .Render();
        }

        protected SUIElement RenderItemModel(ItemModel itemModel)
            => ItemModelInspector.RenderGroup(itemModel,
                    foldItemModel,
                    value => foldItemModel = value
                );
    }
}