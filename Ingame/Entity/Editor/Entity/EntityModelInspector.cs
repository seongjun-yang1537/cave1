using Corelib.SUI;
using PathX;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public static class EntityModelInspector
    {
        public static SUIElement Render(EntityModel entityModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                EntityModelDataInspector.Render(entityModel.Data)
                + SEditorGUILayout.Var("Is Spawned", entityModel.isSpanwed)
                    .OnValueChanged(value => entityModel.isSpanwed = value)
                + SEditorGUILayout.Var("Entity Type", entityModel.entityType)
            );
        }

        public static SUIElement RenderGroup(EntityModel entitymodel, bool fold, UnityAction<bool> setter)
        {
            if (entitymodel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Entity Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(entitymodel)
                )
            );
        }
    }
}