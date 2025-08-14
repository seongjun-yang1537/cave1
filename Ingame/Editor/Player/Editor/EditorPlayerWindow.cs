using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    public class EditorPlayerWindow : EditorWindow
    {
        [MenuItem("Tools/Game/Player Window")]
        public static void ShowWindow()
        {
            GetWindow<EditorPlayerWindow>("Player Window");
        }

        private PlayerController playerController => PlayerSystem.CurrentPlayer;
        protected PlayerModel playerModel { get => playerController?.playerModel; }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
        }

        private void OnPlayModeStateChange(PlayModeStateChange state)
        {
            Repaint();
        }

        private void OnGUI()
        {
            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Group("Inventory")
                .Content(
                    InventoryModelInspector.Render(playerModel?.inventory)
                )
            )
            .Render();
        }
    }
}
