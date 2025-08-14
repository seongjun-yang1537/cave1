using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Game/World/Step Graph/Graph", fileName = "Graph Step Graph")]
    public class GraphStepGraph : WorldgenStepGraph
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

            var vsp = nodes.OfType<GenerateVSPTreeStepNode>().FirstOrDefault();
            if (vsp == null)
                vsp = AddNode(BaseNode.CreateFromType(typeof(GenerateVSPTreeStepNode), Vector2.right * 150)) as GenerateVSPTreeStepNode;

            var rooms = nodes.OfType<GenerateRoomsStepNode>().FirstOrDefault();
            if (rooms == null)
                rooms = AddNode(BaseNode.CreateFromType(typeof(GenerateRoomsStepNode), Vector2.right * 300)) as GenerateRoomsStepNode;

            ConnectIfMissing(vsp, "input", startNode, "output");
            ConnectIfMissing(rooms, "input", vsp, "output");
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
