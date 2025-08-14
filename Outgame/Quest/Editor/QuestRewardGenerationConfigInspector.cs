using Corelib.SUI;
using Ingame;
using UnityEditor;

namespace Quest
{
    public static class QuestRewardGenerationConfigInspector
    {
        public static SUIElement Render(QuestRewardGenerationConfig config)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Type", config.type).OnValueChanged(v => config.type = (QuestRewardType)v)
                + RenderSpecific(config)
                + RenderCountRange(config)
            );
        }
        static SUIElement RenderSpecific(QuestRewardGenerationConfig config)
        {
            if (config.type == QuestRewardType.ITEM)
                return SEditorGUILayout.Var("Item", config.itemID).OnValueChanged(v => config.itemID = (ItemID)v);
            return SUIElement.Empty();
        }
        static SUIElement RenderCountRange(QuestRewardGenerationConfig config)
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
