using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(EntityView), true)]
    public class EditorEntityView : Editor
    {
        EntityView entityView;

        Transform body;
        private static string PATH_TEMPLATES = "Editor/Template";

        protected virtual void OnEnable()
        {
            entityView = (EntityView)target;
            UpdateReference();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawAnchors();
            DrawValidation();
        }

        protected virtual void DrawValidation()
        {
            SEditorGUILayout.Group("Validation")
            .Content(
                DrawValidationElement()
                + SEditorGUILayout.Button("Reload Reference")
                .OnClick(() => UpdateReference())
            )
            .Render();
        }

        private void DrawAnchors()
        {
            var anchors = entityView.rigAnchors;
            SEditorGUILayout.Group("Anchors")
            .Content(
                RenderHead(anchors)
                + RenderBody(anchors)
                + SEditorGUILayout.Button("Reload")
                .OnClick(() => anchors.Reload())
            )
            .Render();
        }

        private SUIElement RenderHead(RigAnchors anchors)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("Head", anchors.head)
                    + SEditorGUILayout.Button("Create")
                    .Width(60)
                    .Where(() => anchors.head == null)
                    .OnClick(() => CreateRigObject(anchors, "rig@head"))
                )
                + SEditorGUILayout.HelpBox($"Missing Rig Named 'rig@head' with PSphereArea component", MessageType.Error)
                .Where(() => anchors.head == null)
            );
        }

        private SUIElement RenderBody(RigAnchors anchors)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("Body", anchors.body)
                    + SEditorGUILayout.Button("Create")
                    .Width(60)
                    .Where(() => anchors.body == null)
                    .OnClick(() => CreateRigObject(anchors, "rig@body"))
                )
                + SEditorGUILayout.HelpBox($"Missing Rig Named 'rig@body' with PCapsuleArea component", MessageType.Error)
                .Where(() => anchors.body == null)
            );
        }

        private void CreateRigObject(RigAnchors anchors, string rigName)
        {
            var template = Resources.Load($"{PATH_TEMPLATES}/{rigName}");
            GameObject instance;
            if (template == null)
            {
                Debug.LogWarning($"Template not found at 'Resources/{PATH_TEMPLATES}/{rigName}'. Creating empty GameObject.");
                instance = new GameObject(rigName);
                instance.transform.SetParent(anchors.root);
                instance.transform.localPosition = Vector3.zero;
            }
            else
            {
                instance = (GameObject)PrefabUtility.InstantiatePrefab(template, anchors.root);
                instance.name = rigName;
            }
            anchors.Reload();
        }

        protected virtual SUIElement DrawValidationElement()
        {
            return SEditorGUILayout.Vertical()
            .Content(
                EditorValidationHelper.CreateValidationField(nameof(body), body, entityView.transform, UpdateReference)
            );
        }

        protected virtual void UpdateReference()
        {
            body = entityView.transform.FindInChild(nameof(body));
            if (body != null)
            {
                entityView.rigAnchors.Init(body);
            }
        }

        protected virtual bool CheckValidation()
        {
            if (body == null)
                return false;

            if (entityView.rigAnchors.head == null || entityView.rigAnchors.body == null)
                return false;

            return true;
        }
    }
}

