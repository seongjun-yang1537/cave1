using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Utility to simply display a list of <see cref="WorldPipelineStep"/> instances.
    /// </summary>
    internal class WorldPipelineListDrawer
    {
        List<WorldPipelineStep> steps;

        public WorldPipelineListDrawer(List<WorldPipelineStep> steps)
        {
            SetSteps(steps);
        }

        public void SetSteps(List<WorldPipelineStep> newSteps)
        {
            steps = newSteps ?? new List<WorldPipelineStep>();
        }

        public void OnGUI()
        {
            if (steps == null || steps.Count == 0) return;

            foreach (var step in steps)
            {
                if (step == null) continue;
                DrawStep(step);
                EditorGUILayout.Space();
            }
        }

        void DrawStep(WorldPipelineStep step)
        {
            EditorGUILayout.LabelField(step.GetType().Name, EditorStyles.boldLabel);
            DrawFields(step);
        }

        void DrawFields(object obj)
        {
            var fields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            EditorGUI.indentLevel++;
            foreach (var field in fields)
            {
                DrawField(ObjectNames.NicifyVariableName(field.Name), field.GetValue(obj));
            }
            EditorGUI.indentLevel--;
        }

        void DrawField(string label, object value)
        {
            if (value == null)
            {
                EditorGUILayout.LabelField(label, "null");
                return;
            }

            switch (value)
            {
                case int i:
                    EditorGUILayout.IntField(label, i);
                    break;
                case float f:
                    EditorGUILayout.FloatField(label, f);
                    break;
                case bool b:
                    EditorGUILayout.Toggle(label, b);
                    break;
                case Enum e:
                    EditorGUILayout.EnumPopup(label, e);
                    break;
                case Vector3Int v3i:
                    EditorGUILayout.Vector3IntField(label, v3i);
                    break;
                case Vector3 v3:
                    EditorGUILayout.Vector3Field(label, v3);
                    break;
                case string s:
                    EditorGUILayout.TextField(label, s);
                    break;
                default:
                    EditorGUILayout.LabelField(label, value.ToString());
                    break;
            }
        }
    }
}

