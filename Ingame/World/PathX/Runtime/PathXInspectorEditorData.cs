using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class PathXInspectorEditorData
    {
        public bool enableChunkIndexHandle;
        public Vector3 chunkIndexHandlePosition;
        public Vector3Int selectedChunkIndex;

        public bool enablePointLocationHandle;
        public Vector3 pointLocationHandlePosition;

        public bool enablePathfindHandle;
        public Vector3 pathfindHandlePositionStart;
        public Vector3 pathfindHandlePositionEnd;
        public List<PTriangleGraphNode> pathfindNodes;
        public List<NavPortal> pathfindPortals;
        public List<Vector3> pathfindLine;
        public List<Vector3> pathfindNormals;
    }
}