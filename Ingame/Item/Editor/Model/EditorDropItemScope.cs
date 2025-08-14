using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(DropItemScope))]
    public class EditorDropItemScope : Editor
    {
        DropItemScope dropItemScope;

        ItemModel itemModel
        {
            get => dropItemScope.itemModel ?? ItemModelFactory.Create(dropItemScope.itemModelData, dropItemScope.itemModelState);
        }

        protected void OnEnable()
        {
            dropItemScope = (DropItemScope)target;
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