using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnTotalStatInspector
    {
        public static SUIElement Render(PawnTotalStat stat)
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Var("Move Speed", stat.moveSpeed)
                + SEditorGUILayout.Var("Rotation Speed", stat.rotationSpeed)
                + SEditorGUILayout.Var("Sprint Speed", stat.sprintSpeed)
                + SEditorGUILayout.Var("Jump Force", stat.jumpForce)
            );
        }

        public static SUIElement RenderGroup(PawnTotalStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pawn Total Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}