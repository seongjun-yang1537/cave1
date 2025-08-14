using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class VFXRuptureBlast : ViewBehaviour<RuptureView>
    {
        List<ParticleSystem> particles;

        protected override void Awake()
        {
            base.Awake();
            particles = transform.Cast<Transform>()
                .Select(t => t.GetComponent<ParticleSystem>())
                .Where(p => p != null)
                .ToList();

            Play();
        }

        public void Play()
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }

        public void Stop()
        {
            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }

        public void Render() { }
    }
}