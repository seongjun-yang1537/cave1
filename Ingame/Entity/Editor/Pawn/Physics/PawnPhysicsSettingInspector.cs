using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnPhysicsSettingInspector
    {
        private static bool foldoutPose;

        public static SUIElement Render(PawnPhysicsSetting setting)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Move Speed Constant", setting.MOVE_SPEED_CONSTANT)
                .OnValueChanged(v => setting.MOVE_SPEED_CONSTANT = v)
                + SEditorGUILayout.Var("Jump Force Constant", setting.JUMP_FORCE_CONSTANT)
                .OnValueChanged(v => setting.JUMP_FORCE_CONSTANT = v)
                + SEditorGUILayout.Var("Air Control Force", setting.AIR_CONTROL_FORCE)
                .OnValueChanged(v => setting.AIR_CONTROL_FORCE = v)
                + SEditorGUILayout.Var("Slope Limit", setting.slopeLimit)
                .OnValueChanged(value => setting.slopeLimit = value)
                + PawnPoseColliderPresetInspector.RenderGroup(setting.poseColliderPreset, foldoutPose, v => foldoutPose = v)
            );
        }

        public static SUIElement RenderGroup(PawnPhysicsSetting setting, bool fold, UnityAction<bool> setter)
        {
            if (setting == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pawn Physics Setting", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(setting)
                )
            );
        }
    }
}
