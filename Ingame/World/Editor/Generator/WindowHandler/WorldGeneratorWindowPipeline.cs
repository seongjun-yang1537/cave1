using System;
using Corelib.SUI;
using PathX;
using UnityEngine;
using UnityEditor;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowPipeline
    {
        [NonSerialized]
        private WorldPipelineAsset pipelineAsset;
        [NonSerialized]
        private WorldPipelineAsset prevPipelineAsset;

        [NonSerialized]
        private Editor pipelineEditor;

        [SerializeField]
        private bool foldPipeline = true;

        public WorldGeneratorWindowPipeline(WorldPipelineAsset pipelineAsset)
        {
            UpdateContext(pipelineAsset);
        }

        public SUIElement Render()
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pipeline", foldPipeline)
                .OnValueChanged(value => foldPipeline = value)
                .Content(
                    SEditorGUILayout.Var("Pipeline Asset", pipelineAsset)
                    + SEditorGUILayout.Action(() =>
                    {
                        if (prevPipelineAsset != pipelineAsset)
                        {
                            pipelineEditor = pipelineAsset != null ? Editor.CreateEditor(pipelineAsset) : null;
                            prevPipelineAsset = pipelineAsset;
                        }
                        pipelineEditor?.OnInspectorGUI();
                    })
                )
            );
        }

        public void UpdateContext(WorldPipelineAsset pipelineAsset)
        {
            this.pipelineAsset = pipelineAsset;
            this.prevPipelineAsset = pipelineAsset;
            this.pipelineEditor = pipelineAsset != null ? Editor.CreateEditor(pipelineAsset) : null;
        }
    }
}