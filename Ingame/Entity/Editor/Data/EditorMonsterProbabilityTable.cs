using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(MonsterProbabilityTable))]
    public class EditorMonsterProbabilityTable : Editor
    {
        private MonsterProbabilityTable script;

        private void OnEnable()
        {
            script = (MonsterProbabilityTable)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            SEditorGUILayout.Button("Normalize Weights")
                .OnClick(() =>
                {
                    script.NormalizeWeights();
                    EditorUtility.SetDirty(script);
                })
                .Render();

            serializedObject.ApplyModifiedProperties();

            for (int i = 0; i < script.entries.Count; i++)
            {
                if (script.entries[i].countRange == null)
                {
                    var e = script.entries[i];
                    e.countRange = new IntRange(1, 1);
                    script.entries[i] = e;
                    EditorUtility.SetDirty(script);
                }
            }
        }
    }
}
