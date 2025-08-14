using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Pawn Pose Collider Preset", menuName = "Game/Config/Physics/Pawn Pose Colliders")]
    public class PawnPoseColliderPreset : ScriptableObject
    {
        [Serializable]
        public class CapsuleColliderPresetDictionary : SerializableDictionary<PawnPoseState, CapsuleColliderPreset> { }
        [SerializeField]
        public CapsuleColliderPresetDictionary presets = new();

        public CapsuleColliderPreset this[PawnPoseState poseState]
        {
            get => presets[poseState];
        }

        public void EnsureContains()
        {
            List<PawnPoseState> ensures = new()
            {
                PawnPoseState.Standing,
                PawnPoseState.Crouch,
                PawnPoseState.Prone,
            };

            foreach (PawnPoseState state in ensures)
            {
                if (presets.ContainsKey(state)) continue;
                presets.Add(state, new());
            }
        }
    }
}