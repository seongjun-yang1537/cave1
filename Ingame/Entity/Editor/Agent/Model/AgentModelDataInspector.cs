using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AgentModelDataInspector
    {
        static bool foldBaseStat;

        public static SUIElement Render(AgentModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    AgentStatInspector.RenderGroup(data.baseStat, foldBaseStat, v => foldBaseStat = v)
                    + SEditorGUILayout.Var("isInvincible", data.isInvincible).OnValueChanged(v => data.isInvincible = v)
                );
        }

        public static SUIElement RenderGroup(AgentModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("AgentModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
