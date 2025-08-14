using Corelib.SUI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ingame
{
    [CustomEditor(typeof(PawnPhysicsSetting))]
    public class EditorPawnPhysicsSetting : Editor
    {
        PawnPhysicsSetting script;
        protected void OnEnable()
        {
            script = (PawnPhysicsSetting)target;
        }

        bool foldPawnPhysicsSetting = true;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    PawnPoseColliderPresetInspector.RenderGroup(script.poseColliderPreset,
                        foldPawnPhysicsSetting,
                        value => foldPawnPhysicsSetting = value
                    )
                )
            )
            .Render();
        }
    }
}