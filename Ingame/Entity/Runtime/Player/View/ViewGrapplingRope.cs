using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class ViewGrapplingRope : MonoBehaviour
    {
        private GrapplingSpring spring;
        private LineRenderer lr;
        private Vector3 currentGrapplePosition;
        public GrapplingController grapplingController;
        public int quality;
        public float damper;
        public float strength;
        public float velocity;
        public float waveCount;
        public float waveHeight;
        public AnimationCurve affectCurve;

        void Awake()
        {
            lr = GetComponent<LineRenderer>();
            spring = new GrapplingSpring();
            spring.SetTarget(0);
        }

        void LateUpdate()
        {
            DrawRope();
        }

        void DrawRope()
        {
            if (!grapplingController.IsGrappling())
            {
                currentGrapplePosition = grapplingController.transform.position;
                spring.Reset();
                if (lr.positionCount > 0)
                    lr.positionCount = 0;
                return;
            }

            if (lr.positionCount == 0)
            {
                spring.SetVelocity(velocity);
                lr.positionCount = quality + 1;
            }

            spring.SetDamper(damper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);

            var grapplePoint = grapplingController.GetGrapplePoint();
            var gunTipPosition = grapplingController.transform.position;
            var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

            for (var i = 0; i < quality + 1; i++)
            {
                var delta = i / (float)quality;
                var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                             affectCurve.Evaluate(delta);

                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }
    }
}
