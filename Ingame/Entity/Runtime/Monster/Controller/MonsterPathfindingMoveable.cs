using PathX;
using UnityEngine;
using VContainer.Unity;

namespace Ingame
{
    public class MonsterPathfindingMoveable : MonsterMoveable, IInitializable
    {
        private float turnSpeed = 10f;

        private Vector3? nextPosition;
        private Quaternion? targetRotation;
        private Quaternion nextRotation;

        public override void Update()
        {
            base.Update();

            nextPosition = null;
            targetRotation = null;
            if (monsterModel.pathProgress != null && monsterModel.pathProgress.plannedPath != null)
            {
                NavSurfacePoint nextPoint = monsterModel.pathProgress.Advance(monsterModel.pawnTotalStat.moveSpeed * Time.deltaTime);
                if (nextPoint != null)
                {
                    nextPosition = nextPoint.point;
                    Vector3 moveDirection = nextPosition.Value - transform.position;
                    float threshold = 0.0001f;
                    if (moveDirection.sqrMagnitude > threshold * threshold)
                    {
                        targetRotation = Quaternion.LookRotation(moveDirection.normalized, nextPoint.normal);
                    }
                }
            }
            nextRotation = transform.rotation;
            if (targetRotation != null)
            {
                nextRotation = Quaternion.Slerp(nextRotation, targetRotation.Value, turnSpeed * Time.deltaTime);
            }
        }

        public override Vector3? GetNextPosition()
        {
            return nextPosition;
        }

        public override Quaternion? GetNextRotation()
        {
            return nextRotation;
        }
    }
}