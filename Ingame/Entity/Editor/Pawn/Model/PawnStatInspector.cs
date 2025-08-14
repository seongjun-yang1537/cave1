using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnStatInspector
    {
        public static SUIElement Render(PawnStat stat)
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Var("Move Speed", stat.moveSpeed)
                .OnValueChanged(value => stat.moveSpeed = value)
                + SEditorGUILayout.Var("Rotation Speed", stat.rotationSpeed)
                .OnValueChanged(value => stat.rotationSpeed = value)
                + SEditorGUILayout.Var("Sprint Speed", stat.sprintSpeed)
                .OnValueChanged(value => stat.sprintSpeed = value)
                + SEditorGUILayout.Var("Jump Force", stat.jumpForce)
                .OnValueChanged(value => stat.jumpForce = value)
            );
        }

        public static SUIElement RenderGroup(PawnStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pawn Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}