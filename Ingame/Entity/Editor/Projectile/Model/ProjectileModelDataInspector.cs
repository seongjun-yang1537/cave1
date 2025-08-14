using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ProjectileModelDataInspector
    {
        public static SUIElement Render(ProjectileModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("damage", data.damage).OnValueChanged(v => data.damage = v)
                    + SEditorGUILayout.Var("speed", data.speed).OnValueChanged(v => data.speed = v)
                    + SEditorGUILayout.Var("lifeTime", data.lifeTime).OnValueChanged(v => data.lifeTime = v)
                    + SEditorGUILayout.Var("range", data.range).OnValueChanged(v => data.range = v)
                    + SEditorGUILayout.Var("targetLayer", data.targetLayer).OnValueChanged(v => data.targetLayer = v)
                );
        }

        public static SUIElement RenderGroup(ProjectileModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ProjectileModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

