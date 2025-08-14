using Core;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(EntityScope), true)]
    public class EditorEntityScope : EditorScopeBase<EntityScope, EntityModel, EntityModelData, EntityModelState>
    {
        EntityScope script;
        protected override void OnEnable()
        {
            base.OnEnable();
            script = (EntityScope)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SEditorGUILayout.Horizontal()
            .LabelWidth(60)
            .Content(
                SEditorGUILayout.Label("Model Type")
                .Bold()
                + SEditorGUILayout.Vertical("helpbox")
                .Content(
                    SEditorGUILayout.Label($"{script.ModelType.Name}")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                )
            )
            .Render();
        }

        protected override EntityModel GetModel(EntityScope scope) => scope.entityModel;
        protected override EntityModelData GetModelData(EntityScope scope) => scope.entityModelData;
        protected override EntityModelState GetModelState(EntityScope scope) => scope.entityModelState;
    }
}