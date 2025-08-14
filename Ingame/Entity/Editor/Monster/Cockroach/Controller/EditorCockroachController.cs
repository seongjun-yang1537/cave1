using System.Collections.Generic;
using Corelib.SUI;
using PathX;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(CockroachController))]
    public class EditorCockroachController : EditorMonsterController
    {
        CockroachController cockroachController;
        Vector3 selectedTargetPosition;

        protected void OnEnable()
        {
            base.OnEnable();
            cockroachController = (CockroachController)target;
            selectedTargetPosition = cockroachController.transform.position + cockroachController.transform.forward;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                RenderDebugPathFind()
            )
            .Render();
        }


        private SUIElement RenderDebugPathFind()
        {
            return SEditorGUILayout.Group("Pathfind Debug")
            .Content(
                SEditorGUILayout.Var("Selected Target Position", selectedTargetPosition)
                .OnValueChanged(value => selectedTargetPosition = value)
                + SEditorGUILayout.Button("PathFind")
                .OnClick(() =>
                {
                    cockroachController.SetTargetPosition(selectedTargetPosition);
                })
            );
        }
    }
}