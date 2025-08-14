using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureModelDataInspector
    {
        public static SUIElement Render(RuptureModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("explosionRange", data.explosionRange).OnValueChanged(v => data.explosionRange = v)
                    + SEditorGUILayout.Var("explosionFuseTime", data.explosionFuseTime).OnValueChanged(v => data.explosionFuseTime = v)
                );
        }

        public static SUIElement RenderGroup(RuptureModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("RuptureModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

