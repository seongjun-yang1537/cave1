using UnityEngine;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(MonsterController), true)]
    public class EditorMonsterController : EditorPawnController
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}