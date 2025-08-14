using System.Collections.Generic;
using Corelib.SUI;
using UnityEditor;

namespace Quest
{
    [CustomEditor(typeof(QuestGenerationConfig), true)]
    public class EditorQuestGenerationConfig : Editor
    {
        QuestGenerationConfig script;
        void OnEnable()
        {
            script = (QuestGenerationConfig)target;
            if (script.requirements == null) script.requirements = new List<QuestRequirementGenerationConfig>();
            if (script.rewards == null) script.rewards = new List<QuestRewardGenerationConfig>();
        }
        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("Quest Type", script.questType).OnValueChanged(v => script.questType = (QuestCategory)v)
                    + SEditorGUILayout.Var("Objective Type", script.objectiveType).OnValueChanged(v => script.objectiveType = (QuestObjectiveType)v)
                    + SEditorGUILayout.Var("Title", script.title).OnValueChanged(v => script.title = v)
                    + SEditorGUILayout.Var("Description", script.description).OnValueChanged(v => script.description = v)
                    + RenderRequirements()
                    + RenderRewards()
                )
            ).Render();
        }
        private SUIElement RenderRequirements()
        {
            SUIElement elements = SUIElement.Empty();
            elements += SEditorGUILayout.Label("Requirements");
            for (int i = 0; i < script.requirements.Count; i++)
            {
                int index = i;
                var config = script.requirements[i];
                elements += SEditorGUILayout.Vertical()
                .Content(
                    QuestRequirementGenerationConfigInspector.Render(config)
                    + SEditorGUILayout.Button("-").Width(20).OnClick(() =>
                    {
                        script.requirements.RemoveAt(index);
                        EditorUtility.SetDirty(target);
                    })
                );
            }
            elements += SEditorGUILayout.Button("+").Width(20).OnClick(() =>
            {
                script.requirements.Add(new QuestRequirementGenerationConfig());
                EditorUtility.SetDirty(target);
            });
            return SEditorGUILayout.Vertical().Content(elements);
        }
        private SUIElement RenderRewards()
        {
            SUIElement elements = SUIElement.Empty();
            elements += SEditorGUILayout.Label("Rewards");
            for (int i = 0; i < script.rewards.Count; i++)
            {
                int index = i;
                var config = script.rewards[i];
                elements += SEditorGUILayout.Vertical()
                .Content(
                    QuestRewardGenerationConfigInspector.Render(config)
                    + SEditorGUILayout.Button("-").Width(20).OnClick(() =>
                    {
                        script.rewards.RemoveAt(index);
                        EditorUtility.SetDirty(target);
                    })
                );
            }
            elements += SEditorGUILayout.Button("+").Width(20).OnClick(() =>
            {
                script.rewards.Add(new QuestRewardGenerationConfig());
                EditorUtility.SetDirty(target);
            });
            return SEditorGUILayout.Vertical().Content(elements);
        }
    }
}
