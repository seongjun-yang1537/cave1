using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Game/World/Step Graph/Rasterize", fileName = "Rasterize Step Graph")]
    public class RasterizeStepGraph : WorldgenStepGraph
    {
        [SerializeReference]
        public List<WorldPipelineStep> steps = new();

        System.Action<GraphChanges> changeHandler;

        protected override void OnEnable()
        {
            base.OnEnable();
            changeHandler = _ => EnsureFixedConnections();
            onGraphChanges += changeHandler;
            EnsureFixedConnections();
        }

        protected override void OnDisable()
        {
            onGraphChanges -= changeHandler;
            base.OnDisable();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EnsureFixedConnections();
        }
#endif

        void EnsureFixedConnections()
        {
            EnsureStartAndEndNodes();

            var render = nodes.OfType<RenderWorldStepNode>().FirstOrDefault();
            if (render == null)
                render = AddNode(BaseNode.CreateFromType(typeof(RenderWorldStepNode), Vector2.right * 100)) as RenderWorldStepNode;

            var tri = nodes.OfType<GenerateTriangleLayerStepNode>().FirstOrDefault();
            if (tri == null)
                tri = AddNode(BaseNode.CreateFromType(typeof(GenerateTriangleLayerStepNode), Vector2.right * 250)) as GenerateTriangleLayerStepNode;

            var nav = nodes.OfType<GeneratePathXNavMeshStepNode>().FirstOrDefault();
            if (nav == null)
                nav = AddNode(BaseNode.CreateFromType(typeof(GeneratePathXNavMeshStepNode), Vector2.right * 400)) as GeneratePathXNavMeshStepNode;

            ConnectIfMissing(tri, "input", render, "output");
            ConnectIfMissing(nav, "input", tri, "output");
            ConnectIfMissing(endNode, "input", nav, "output");
        }

        void ConnectIfMissing(BaseNode input, string inputField, BaseNode output, string outputField)
        {
            if (!edges.Any(e => e.inputNode == input && e.inputFieldName == inputField && e.outputNode == output && e.outputFieldName == outputField))
            {
                var inPort = input.GetPort(inputField, null);
                var outPort = output.GetPort(outputField, null);
                if (inPort != null && outPort != null)
                    Connect(inPort, outPort);
            }
        }

        public void RefreshSteps()
        {
            steps.Clear();
            foreach (var step in EnumerateSteps())
            {
                steps.Add(step);
            }
        }
    }
}
