using UnityEngine;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(PlayerController), true)]
    public class EditorPlayerController : EditorPawnController
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}