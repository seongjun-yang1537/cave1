using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(AgentController))]
    public class EditorAgentController : Editor
    {
        protected AgentController agentController;

        protected void OnEnable()
        {
            agentController = (AgentController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Agent View", agentController.agentView)
            // Add AgentController specific properties here
            // SEditorGUILayout.Label("Current State: " + agentController.CurrentState.ToString())
            )
            .Render();
        }
    }
}
