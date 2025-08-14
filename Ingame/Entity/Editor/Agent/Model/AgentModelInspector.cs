using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AgentModelInspector
    {
        private static bool foldBaseStat = false;
        private static bool foldTotalStat = false;

        public static SUIElement Render(AgentModel agentModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                AgentModelDataInspector.Render(agentModel.Data)
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Life", agentModel.life)
                    .OnValueChanged(value => agentModel.life = value)
                    + AgentStatInspector.RenderGroup(
                        agentModel.baseStat,
                        foldBaseStat, value =>
                        foldBaseStat = value
                    )
                    + AgentTotalStatInspector.RenderGroup(
                        agentModel.totalStat,
                        foldTotalStat,
                        value => foldTotalStat = value
                    )
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Aimtarget", EntitySystem.FindControllerByID(agentModel.aimtargetID))
                )
            );
        }

        public static SUIElement RenderGroup(AgentModel agentModel, bool fold, UnityAction<bool> setter)
        {
            if (agentModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Agent Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(agentModel)
                )
            );
        }
    }
}