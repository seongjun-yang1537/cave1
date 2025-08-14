using Corelib.SUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace Ingame
{
    public static class ItemResourceInspector
    {
        public static SUIElement Render(ItemResource resource, Object undoTarget, System.Action<ItemResource> onValueChange)
        {
            if (resource == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Prefab", resource.prefab)
                .OnValueChanged(value =>
                {
                    Undo.RecordObject(undoTarget, "Change Item Resource");
                    resource.prefab = value as GameObject;
                    onValueChange?.Invoke(resource);
                })
                + SEditorGUILayout.Var("Model Data", resource.modelData)
                .OnValueChanged(value =>
                {
                    Undo.RecordObject(undoTarget, "Change Item Resource");
                    resource.modelData = value as ItemModelData;
                    onValueChange?.Invoke(resource);
                })
                + SEditorGUILayout.Var("Icon Texture", resource.iconSprite)
                .OnValueChanged(value =>
                {
                    Undo.RecordObject(undoTarget, "Change Item Resource");
                    resource.iconSprite = value as Sprite;
                    onValueChange?.Invoke(resource);
                })
                + SEditorGUILayout.Var("Icon Texture", resource.iconTexture)
                .OnValueChanged(value =>
                {
                    Undo.RecordObject(undoTarget, "Change Item Resource");
                    resource.iconTexture = value as Texture2D;
                    onValueChange?.Invoke(resource);
                })
            );
        }

        public static SUIElement RenderGroup(ItemResource resource, bool fold, UnityAction<bool> setter, Object undoTarget, System.Action<ItemResource> onValueChange)
        {
            if (resource == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Item Resource", fold)
                .OnValueChanged(setter)
                .Content(Render(resource, undoTarget, onValueChange))
            );
        }
    }
}
