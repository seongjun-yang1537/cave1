using Corelib.SUI;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public static class CapsuleColliderPresetInspector
    {
        public static SUIElement Render(CapsuleColliderPreset preset)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Center", preset.center)
                .OnValueChanged(value => preset.center = value)
                + SEditorGUILayout.Var("Height", preset.height)
                .OnValueChanged(value => preset.height = value)
                + SEditorGUILayout.Var("Radius", preset.radius)
                .OnValueChanged(value => preset.radius = value)
            );
        }

        public static SUIElement RenderGroup(CapsuleColliderPreset preset, bool fold, UnityAction<bool> setter)
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