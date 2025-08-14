using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class MonsterModelDataInspector
    {
        public static SUIElement Render(MonsterModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("sightRange", data.sightRange).OnValueChanged(v => data.sightRange = v)
                    + SEditorGUILayout.Var("attackRange", data.attackRange).OnValueChanged(v => data.attackRange = v)
                    + SEditorGUILayout.Var("attackCooldown", data.attackCooldown).OnValueChanged(v => data.attackCooldown = v)
                    + SEditorGUILayout.Var("dropTable", data.dropTable).OnValueChanged(v => data.dropTable = v)
                    + SEditorGUILayout.Var("enemyDetectionRange", data.enemyDetectionRange).OnValueChanged(v => data.enemyDetectionRange = v)
                );
        }

        public static SUIElement RenderGroup(MonsterModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("MonsterModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
