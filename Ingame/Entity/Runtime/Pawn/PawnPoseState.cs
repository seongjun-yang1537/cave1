using System;

namespace Ingame
{
    [Flags]
    public enum PawnPoseState
    {
        None = 0,
        Standing = (1 << 0),
        Walk = (1 << 1),
        Run = (1 << 2),
        Crouch = (1 << 3),
        Prone = (1 << 4),
    }
}