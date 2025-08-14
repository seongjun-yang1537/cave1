using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World;

namespace Realm
{
    [CreateAssetMenu(fileName = "New Realm Pipeline Profile", menuName = "Game/Realm/Generation Profile")]
    public class RealmGenerationProfile : ScriptableObject
    {
        [SerializeField]
        public WorldGenerationPreset testbedPreset;
        [SerializeField]
        public List<RealmProfile> profiles;

        public void SortProfiles()
        {
            profiles.Sort();
        }

        public RealmProfile GetProfileForDepth(float depth)
        {
            return profiles.FirstOrDefault(p => p.Contains(depth));
        }

        public RealmProfile this[float depth]
        {
            get => GetProfileForDepth(depth);
        }
    }
}