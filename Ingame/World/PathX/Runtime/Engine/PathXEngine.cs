using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class PathXEngine
    {
        [Serializable]
        public class EngineProfileAgentDictionary : SerializableDictionary<TriangleDomain, PathXNavProfile> { }

        [SerializeField]
        private EngineProfileAgentDictionary profiles = new();

        public IEnumerable<PathXNavProfile> Profiles => profiles.Values;
        public IEnumerable<PathXNavMesh> NavMeshes => Profiles.Select(agent => agent.navSurface);

        public IEnumerable<TriangleDomain> ProfileKeys => profiles.Keys;

        public bool NeedsReload => Profiles.All(agent => agent.NeedsReload);

        public PathXNavMesh this[TriangleDomain navDomain]
        {
            get => profiles[navDomain].navSurface;
        }

        public void Reload(Mesh mesh)
        {
            foreach (var profileAgent in Profiles)
                profileAgent.Reload(mesh);
        }

        public bool AddProfileAgent(PathXNavProfile profileAgent)
        {
            if (profiles.ContainsKey(profileAgent.domain)) return false;
            profiles.Add(profileAgent.domain, profileAgent);
            return true;
        }

        public bool ReplaceProfileAgent(PathXNavProfile profileAgent)
        {
            if (!profiles.ContainsKey(profileAgent.domain)) return false;
            profiles[profileAgent.domain] = profileAgent;
            return true;
        }

        public bool RemoveProfileAgent(PathXNavProfile profileAgent)
        {
            return profiles.Remove(profileAgent.domain);
        }

        public bool TryGetProfileAgent(TriangleDomain domain, out PathXNavProfile profileAgent)
        {
            return profiles.TryGetValue(domain, out profileAgent);
        }

        public void RemoveAllProfile()
        {
            profiles.Clear();
        }

        public bool HasProfile(TriangleDomain domain) => profiles.ContainsKey(domain);
    }
}
