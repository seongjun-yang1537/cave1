using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class DragonBoarModelDataInspector
    {
        public static SUIElement Render(DragonBoarModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("wanderRadius", data.wanderRadius).OnValueChanged(v => data.wanderRadius = v)
                );
        }

        public static SUIElement RenderGroup(DragonBoarModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("DragonBoarModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

