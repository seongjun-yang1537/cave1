using Corelib.SUI;
using Ingame;
using UnityEditor;

namespace Quest
{
    public static class QuestRequirementGenerationConfigInspector
    {
        public static SUIElement Render(QuestRequirementGenerationConfig config)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Probability", config.probability).OnValueChanged(v => config.probability = v)
                + SEditorGUILayout.Var("Type", config.type).OnValueChanged(v => config.type = (QuestRequirementType)v)
                + RenderSpecific(config)
                + RenderCountRange(config)
            );
        }
        static SUIElement RenderSpecific(QuestRequirementGenerationConfig config)
        {
            if (config.type == QuestRequirementType.COLLECT_ITEM)
                return SEditorGUILayout.Var("Item", config.itemID).OnValueChanged(v => config.itemID = (ItemID)v);
            if (config.type == QuestRequirementType.KILL_ENEMY)
                return SEditorGUILayout.Var("Entity", config.entityType).OnValueChanged(v => config.entityType = (EntityType)v);
            return SUIElement.Empty();
        }
        static SUIElement RenderCountRange(QuestRequirementGenerationConfig config)
        {
            return SEditorGUILayout.Horizontal()
            .Content(
                SEditorGUILayout.Label("Count Range")
                + SEditorGUILayout.Var("", config.countRange.x).OnValueChanged(v => config.countRange.x = v).Width(64)
                + SEditorGUILayout.Label("~").Width(8)
                + SEditorGUILayout.Var("", config.countRange.y).OnValueChanged(v => config.countRange.y = v).Width(64)
            );
        }
    }
}
