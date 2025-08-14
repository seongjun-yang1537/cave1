using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Pawn Physics Setting", menuName = "Game/Config/Physics/Pawn Setting")]
    public class PawnPhysicsSetting : ScriptableObject
    {
        public float MOVE_SPEED_CONSTANT = 3.0f;
        public float JUMP_FORCE_CONSTANT = 500.0f;
        public float AIR_CONTROL_FORCE = 1.0f;

        public float slopeLimit;

        [SerializeField, AutoCreateScriptableObject]
        public PawnPoseColliderPreset poseColliderPreset;
    }
}