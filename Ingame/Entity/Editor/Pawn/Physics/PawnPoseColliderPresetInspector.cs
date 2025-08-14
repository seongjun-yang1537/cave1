using Corelib.SUI;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnPoseColliderPresetInspector
    {
        private static SUIElement RenderPreset(PawnPoseState state, CapsuleColliderPreset preset)
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Var("State", state)
                + CapsuleColliderPresetInspector.Render(preset)
            );
        }

        public static SUIElement Render(PawnPoseColliderPreset preset)
        {
            preset.EnsureContains();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Action(() =>
                {
                    foreach (var (state, preset) in preset.presets)
                        RenderPreset(state, preset).Render();
                })
            );
        }

        public static SUIElement RenderGroup(PawnPoseColliderPreset preset, bool fold, UnityAction<bool> setter)
        {
            if (preset == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Capsule Collider Preset", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(preset)
                )
            );
        }
    }
}