using Corelib.SUI;
using UnityEditor;

namespace Quest
{
    [CustomEditor(typeof(QuestScheduleData))]
    public class EditorQuestScheduleData : Editor
    {
        QuestScheduleData script;
        QuestModelData newFixed;
        void OnEnable()
        {
            script = (QuestScheduleData)target;
            if (script.fixedQuests == null) script.fixedQuests = new();
            if (script.randomQuests == null) script.randomQuests = new();
        }
        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderFixed()
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Label("Random Quests")
                )
            ).Render();
        }
        private SUIElement RenderFixed()
        {
            SUIElement elements = SUIElement.Empty();
            elements += SEditorGUILayout.Label("Fixed Quests");
            for (int i = 0; i < script.fixedQuests.Count; i++)
            {
                int index = i;
                var quest = script.fixedQuests[i];
                elements += SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("", quest)
                    .OnValueChanged(v => script.fixedQuests[index] = v)
                    + SEditorGUILayout.Button("-").Width(20)
                    .OnClick(() => script.fixedQuests.RemoveAt(index))
                );
            }
            elements += SEditorGUILayout.Horizontal()
            .Content(
                SEditorGUILayout.Var("", newFixed)
                .OnValueChanged(v => newFixed = v)
                + SEditorGUILayout.Button("+").Width(20)
                .OnClick(() =>
                {
                    if (newFixed != null)
                    {
                        script.fixedQuests.Add(newFixed);
                        newFixed = null;
                    }
                })
            );
            return SEditorGUILayout.Vertical().Content(elements);
        }
    }
}
