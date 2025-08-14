using System.Collections.Generic;
using Corelib.SUI;
using UnityEngine.Events;
using Quest;
using Core;

namespace Outgame
{
    public static class QuestModelDataInspector
    {
        public static SUIElement Render(QuestModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    // SEditorGUILayout.Var("type", data.type).OnValueChanged(v => data.type = v)
                    SEditorGUILayout.Var("objectiveType", data.objectiveType).OnValueChanged(v => data.objectiveType = v)
                    + SEditorGUILayout.Var("id", data.id).OnValueChanged(v => data.id = v)
                    + SEditorGUILayout.Var("title", data.title).OnValueChanged(v => data.title = v)
                    + SEditorGUILayout.Var("description", data.description).OnValueChanged(v => data.description = v)
                    + SEditorGUILayout.Var("durationInDays", data.durationInDays).OnValueChanged(v => data.durationInDays = v)
                    + SEditorGUILayout.Var("rewards", data.rewards).OnValueChanged(v => data.rewards = v)
                    + SEditorGUILayout.Var("requirements", data.requirements).OnValueChanged(v => data.requirements = v)
                );
        }

        public static SUIElement RenderGroup(QuestModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("QuestModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
