using System.Collections.Generic;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;
using Corelib.Utils;

namespace Quest
{
    [CustomEditor(typeof(QuestScheduleTable))]
    public class EditorQuestScheduleTable : Editor
    {
        QuestScheduleTable script;
        IntRange newDay = new IntRange(0, 0);
        QuestScheduleData newSchedule;
        void OnEnable()
        {
            script = (QuestScheduleTable)target;
            if (script.table == null) script.table = new List<QuestScheduleTable.Entry>();
        }
        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderInput()
                    + SEditorGUILayout.Separator()
                    + RenderEntries()
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Button("Sort").OnClick(() => { script.Sort(); EditorUtility.SetDirty(target); })
                )
            ).Render();
        }
        private SUIElement RenderInput()
        {
            return SEditorGUILayout.Horizontal()
            .LabelWidth(40f)
            .Content(
                SEditorGUILayout.Label("Day").Width(40)
                + SEditorGUILayout.Var("", newDay.Min).OnValueChanged(v => newDay.Min = v).Width(64)
                + SEditorGUILayout.Var("", newDay.Max).OnValueChanged(v => newDay.Max = v).Width(64)
                + SEditorGUILayout.Var("", newSchedule).OnValueChanged(v => newSchedule = (QuestScheduleData)v)
                + SEditorGUILayout.Button("+").Width(16).OnClick(() =>
                {
                    if (newSchedule == null) return;
                    var entry = script.table.Find(e => e.days.Equals(newDay));
                    if (entry == null)
                    {
                        entry = new QuestScheduleTable.Entry { days = new IntRange(newDay), schedules = new List<QuestScheduleData>() };
                        script.table.Add(entry);
                    }
                    entry.schedules.Add(newSchedule);
                    newSchedule = null;
                })
            );
        }
        private SUIElement RenderEntries()
        {
            SUIElement content = SUIElement.Empty();
            for (int i = 0; i < script.table.Count; i++)
            {
                int index = i;
                var entry = script.table[i];
                content += SEditorGUILayout.Vertical("HelpBox")
                .Content(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(40f)
                    .Content(
                        SEditorGUILayout.Label("Day").Width(40)
                        + SEditorGUILayout.Var("", entry.days.Min).OnValueChanged(v => entry.days.Min = v).Width(64)
                        + SEditorGUILayout.Var("", entry.days.Max).OnValueChanged(v => entry.days.Max = v).Width(64)
                        + SEditorGUILayout.Button("-").Width(16).OnClick(() => script.table.RemoveAt(index))
                    )
                    + RenderScheduleList(entry)
                );
            }
            return content;
        }
        private SUIElement RenderScheduleList(QuestScheduleTable.Entry entry)
        {
            SUIElement elements = SUIElement.Empty();
            for (int i = 0; i < entry.schedules.Count; i++)
            {
                int idx = i;
                var sched = entry.schedules[i];
                elements += SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("", sched).OnValueChanged(v => entry.schedules[idx] = (QuestScheduleData)v)
                    + SEditorGUILayout.Button("-").Width(16).OnClick(() => entry.schedules.RemoveAt(idx))
                );
            }
            return elements;
        }
    }
}
