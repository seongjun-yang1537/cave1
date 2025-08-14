using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(EntityController))]
    public class EditorEntityController : Editor
    {
        EntityController entityController;

        protected void OnEnable()
        {
            entityController = (EntityController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}