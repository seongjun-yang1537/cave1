using System;
using Corelib.Utils;
using UnityEngine;

namespace VoxelEngine
{
    [Serializable]
    public class VSPCube : PBoxInt
    {
        public VSPCube(Vector3Int topLeft, Vector3Int bottomRight) : base(topLeft, bottomRight)
        {
        }
    }
}