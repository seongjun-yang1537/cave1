using System.Collections.Generic;

namespace World
{
    public interface IDfsTransitionController
    {
        public void OnEnter(WorldVSPGraph graph, WorldVSPGraphNode currentNode);
        void OnTraverse(WorldVSPGraph graph, WorldVSPGraphEdge edge);
        public void OnExit(WorldVSPGraph graph, WorldVSPGraphNode currentNode);
        public IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode);
    }
}