using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class MonsterModelStateInspector
    {
        public static SUIElement Render(MonsterModelState monsterModelState)
        {
            if (monsterModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Sight Range", monsterModelState.sightRange)
                    .OnValueChanged(value => monsterModelState.sightRange = value)
                + SEditorGUILayout.Var("Attack Range", monsterModelState.attackRange)
                    .OnValueChanged(value => monsterModelState.attackRange = value)
                + SEditorGUILayout.Var("Attack Cooldown", monsterModelState.attackCooldown)
                    .OnValueChanged(value => monsterModelState.attackCooldown = value)
                + SEditorGUILayout.Var("Drop Table", monsterModelState.dropTable)
                    .OnValueChanged(value => monsterModelState.dropTable = value as ItemDropTable)
            );
        }

        public static SUIElement RenderGroup(MonsterModelState monsterModelState, bool fold, UnityAction<bool> setter)
        {
            if (monsterModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Monster Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(monsterModelState)
                )
            );
        }
    }
}
