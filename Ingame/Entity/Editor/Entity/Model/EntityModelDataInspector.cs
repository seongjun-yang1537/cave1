using Corelib.SUI;
using UnityEngine.Events;
using PathX;
using Core;
using UnityEngine;
using UnityEditor;

namespace Ingame
{
    public static class EntityModelDataInspector
    {
        public static Object undoTarget;

        public static SUIElement Render(EntityModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("entityType", data.entityType).OnValueChanged(v =>
                    {
                        var target = undoTarget != null ? undoTarget : Selection.activeObject;
                        if (target != null)
                        {
                            Undo.RecordObject(target, "Change Entity Model Data");
                            data.entityType = v;
                        }
                    })
                    + SEditorGUILayout.Var("navDomain", data.navDomain).OnValueChanged(v =>
                    {
                        var target = undoTarget != null ? undoTarget : Selection.activeObject;
                        if (target != null)
                        {
                            Undo.RecordObject(target, "Change Entity Model Data");
                            data.navDomain = v;
                        }
                    })
                );
        }

        public static SUIElement RenderGroup(EntityModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("EntityModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
