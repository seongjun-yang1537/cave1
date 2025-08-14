using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class VFXJetpack : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particleSystems;
        [SerializeField] private float gravityModifier = 0.5f;
        [SerializeField] private float duration = 1.0f;

        public JetpackState State { get; private set; }

        public void Start()
        {
            Render(JetpackState.Off);
        }

        public void On() => Render(JetpackState.On);
        public void Off() => Render(JetpackState.Off);

        public void Render(JetpackState state)
        {
            State = state;

            foreach (ParticleSystem ps in particleSystems)
                ps.gameObject.SetActive(state == JetpackState.On);
            // KillAllJetpackTweens();

            // float targetModifier = (state == JetpackState.On) ? gravityModifier : 0f;

            // foreach (var ps in particleSystems)
            // {
            //     AnimateGravityModifier(ps, targetModifier, duration);

            //     var emission = ps.emission;
            //     emission.enabled = (state == JetpackState.On);

            //     if (state == JetpackState.On)
            //     {
            //         if (!ps.isPlaying)
            //             ps.Play();
            //     }
            //     else
            //     {
            //         DOTween.Sequence()
            //             .AppendInterval(duration)
            //             .AppendCallback(() =>
            //             {
            //                 if (ps != null && ps.isPlaying)
            //                     ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //             })
            //             .SetId($"Stop_{ps.GetInstanceID()}");
            //     }
            // }
        }


        private void AnimateGravityModifier(ParticleSystem ps, float targetValue, float duration)
        {
            var gravity = ps.main.gravityModifier.constant;

            DOTween.To(
                () => gravity,
                x =>
                {
                    gravity = x;
                    var main = ps.main;
                    var curve = new ParticleSystem.MinMaxCurve(gravity);
                    main.gravityModifier = curve;
                },
                targetValue,
                duration
            )
            .SetEase(Ease.InOutSine)
            .SetId(ps);
        }

        private void KillAllJetpackTweens()
        {
            foreach (var ps in particleSystems)
                DOTween.Kill(ps);
        }
    }
}
