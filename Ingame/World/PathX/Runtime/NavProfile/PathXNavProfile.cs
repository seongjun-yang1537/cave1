using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public class PathXNavProfile
    {
        [SerializeField]
        public readonly TriangleDomain domain;
        [NonSerialized]
        public readonly IMeshTriangleExtractor extractor;

        [SerializeField]
        public PathXNavMesh navSurface;

        [SerializeField]
        public PathXNavProfileConfig config { get; private set; } = new();
        [SerializeField]
        public PathXNavProfileConfig appliedConfig;

        public bool NeedsReload => navSurface == null || config == null || !config.Equals(appliedConfig);

        public PathXNavProfile(TriangleDomain domain, IMeshTriangleExtractor extractor)
        {
            this.domain = domain;
            this.extractor = extractor;
        }

        public void Reload(Mesh mesh)
        {
            List<PTriangle> triangles = extractor.Extract(mesh);
            config.triangles = triangles;

            appliedConfig = config.Clone();
            navSurface = new PathXNavMesh(config);

            Debug.Log($"<b><color=#00FFFF>[PathX]</color> <color=lime>Engine Reloaded</color></b> → " +
                      $"<color=orange>Profile:</color> {domain}, " +
                      $"<color=orange>Mesh:</color> {mesh.name}, " +
                      $"<color=orange>Triangles:</color> {triangles.Count}, " +
                      $"<color=orange>ChunkSize:</color> {config.chunkSize}");
        }

        public void ApplyConfig(PathXNavProfileConfig newConfig)
        {
            appliedConfig = newConfig;
            navSurface = new PathXNavMesh(appliedConfig);
        }
    }
}