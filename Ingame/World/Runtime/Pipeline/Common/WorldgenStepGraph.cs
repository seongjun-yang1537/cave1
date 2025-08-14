using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace World
{
    public class WorldgenStepGraph : BaseGraph
    {
        public World.Common.WorldgenStepStartNode startNode;
        public World.Common.WorldgenStepEndNode endNode;

        protected override void OnEnable()
        {
            base.OnEnable();
            EnsureStartAndEndNodes();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EnsureStartAndEndNodes();
        }
#endif

        public void EnsureStartAndEndNodes()
        {
            var startNodes = nodes.OfType<World.Common.WorldgenStepStartNode>().ToList();
            var endNodes = nodes.OfType<World.Common.WorldgenStepEndNode>().ToList();

            if (startNodes.Count == 0)
            {
                var node = BaseNode.CreateFromType(typeof(World.Common.WorldgenStepStartNode), UnityEngine.Vector2.zero);
                startNode = AddNode(node) as World.Common.WorldgenStepStartNode;
                startNodes.Add(startNode);
            }
            if (endNodes.Count == 0)
            {
                var node = BaseNode.CreateFromType(typeof(World.Common.WorldgenStepEndNode), UnityEngine.Vector2.right * 300);
                endNode = AddNode(node) as World.Common.WorldgenStepEndNode;
                endNodes.Add(endNode);
            }

            // Remove extra nodes if any
            for (int i = 1; i < startNodes.Count; i++)
                RemoveNode(startNodes[i]);
            for (int i = 1; i < endNodes.Count; i++)
                RemoveNode(endNodes[i]);

            startNode = startNodes.First();
            endNode = endNodes.First();
        }

        /// <summary>
        /// Enumerate pipeline step assets in this graph.
        /// Steps are collected by processing the graph from the start node to the end node.
        /// </summary>
        public IEnumerable<WorldPipelineStep> EnumerateSteps()
        {
            if (startNode == null || endNode == null)
                return Enumerable.Empty<WorldPipelineStep>();

            startNode.SetInputSteps(new List<WorldPipelineStep>());
            var processor = new ProcessGraphProcessor(this);
            processor.Run();
            return endNode.input ?? Enumerable.Empty<WorldPipelineStep>();
        }
    }
}
