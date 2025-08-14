using System;
using System.Collections.Generic;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;
using Ingame;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowMonsterSpawnTrigger : IDisposable
    {
        [NonSerialized]
        private List<MonsterSpawnTrigger> triggers;
        [SerializeField]
        private bool showTriggers = false;

        public WorldGeneratorWindowMonsterSpawnTrigger(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);
        }

        public void Dispose()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public SUIElement Render()
        {
            if (triggers == null)
                return SUIElement.Empty();

            return SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Label("Monster Spawn Triggers")
                        .Bold()
                        .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Var("Show", showTriggers)
                        .OnValueChanged(v =>
                        {
                            showTriggers = v;
                            UpdateSceneHook();
                            RenderTriggers();
                        })
                );
        }

        private void UpdateSceneHook()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            if (showTriggers)
                SceneView.duringSceneGui += OnSceneGUI;
        }


        private void RenderTriggers()
        {
            SceneView.RepaintAll();
        }

        private static string FormatMonsters(List<EntityType> monsters)
        {
            if (monsters == null || monsters.Count == 0)
                return "None";

            Dictionary<EntityType, int> counts = new();
            foreach (var m in monsters)
            {
                counts.TryGetValue(m, out int c);
                counts[m] = c + 1;
            }

            List<string> parts = new();
            foreach (var kvp in counts)
            {
                string part = kvp.Key.ToString();
                if (kvp.Value > 1)
                    part += $" (x{kvp.Value})";
                parts.Add(part);
            }
            return string.Join(", ", parts);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!showTriggers || triggers == null) return;
            Handles.color = Color.white;
            foreach (var trigger in triggers)
            {
                Handles.Label(trigger.center, FormatMonsters(trigger.monsters));
                Handles.DrawWireDisc(trigger.center, Vector3.up, trigger.radius);
                Handles.DrawWireDisc(trigger.center, Vector3.right, trigger.radius);
                Handles.DrawWireDisc(trigger.center, Vector3.forward, trigger.radius);
            }
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            SceneView.duringSceneGui -= OnSceneGUI;

            if (gameWorld == null)
            {
                triggers = null;
                RenderTriggers();
                UpdateSceneHook();
                return;
            }

            triggers = gameWorld.worldData?.monsterSpawnTriggers;
            RenderTriggers();
            UpdateSceneHook();
        }
    }
}
