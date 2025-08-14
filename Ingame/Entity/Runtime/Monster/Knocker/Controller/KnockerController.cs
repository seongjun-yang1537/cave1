using Corelib.Utils;
using PathX;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ingame
{
    [RequireComponent(typeof(KnockerScope))]
    public class KnockerController : MonsterController
    {
        public KnockerModel knockerModel { get; private set; }
        public KnockerView knockerView;

        private Vector3 initialPosition;

        protected override void Awake()
        {
            base.Awake();
            knockerModel = (KnockerModel)monsterModel;
            knockerView = (KnockerView)monsterView;
            initialPosition = transform.position;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(MonsterState.Wander);
        }

        protected override async UniTask GetStateTask(MonsterState state, CancellationToken token)
        {
            switch (state)
            {
                case MonsterState.Wander:
                    await WanderState(token);
                    break;
                case MonsterState.Chase:
                    await ChaseState(token);
                    break;
                case MonsterState.Attack:
                    await AttackState(token);
                    break;
            }
        }

        private async UniTask WanderState(CancellationToken token)
        {
            float checkTargetDelay = 0.5f;
            float timeSinceLastCheck = 0f;

            while (!token.IsCancellationRequested)
            {
                timeSinceLastCheck += Time.deltaTime;
                if (timeSinceLastCheck >= checkTargetDelay)
                {
                    var target = AimtargetController;
                    if (target != null && Vector3.Distance(transform.position, target.transform.position) <= knockerModel.sightRange)
                    {
                        ChangeState(MonsterState.Chase);
                        return;
                    }
                    timeSinceLastCheck = 0f;
                }

                if (!HasPath)
                {
                    await UniTask.Delay((int)(Random.Range(2f, 5f) * 1000f), cancellationToken: token);

                    if (GetRandomPoint(initialPosition, 15f, out var randomPoint))
                    {
                        SetTargetPosition(randomPoint);
                    }
                }

                await UniTask.Yield(token);
            }
        }

        private async UniTask ChaseState(CancellationToken token)
        {
            float rerouteDelay = 1f;
            while (!token.IsCancellationRequested)
            {
                var target = AimtargetController;
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > knockerModel.sightRange)
                {
                    ChangeState(MonsterState.Wander);
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= knockerModel.attackRange)
                {
                    ChangeState(MonsterState.Attack);
                    return;
                }

                SetTargetPosition(target.transform.position);
                await UniTask.Delay((int)(rerouteDelay * 1000f), cancellationToken: token);
            }
        }

        private async UniTask AttackState(CancellationToken token)
        {
            StopMove();
            while (!token.IsCancellationRequested)
            {
                var target = AimtargetController;
                if (target == null)
                {
                    ChangeState(MonsterState.Wander);
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > knockerModel.attackRange)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }

                transform.LookAt(target.transform);

                knockerView.onAnimationStartAttack.Invoke(this, target);
                knockerView.onAnimationEndAttack.AddListenerOnce(
                    () => Attack(target, knockerModel.totalStat.attack)
                );

                await UniTask.Delay((int)(knockerModel.attackCooldown * 1000f), cancellationToken: token);
            }
        }

        private bool GetRandomPoint(Vector3 center, float radius, out Vector3 randomPoint)
        {
            PTriangle triangle = NavMesh.PickRandomTriangleNearby(center, radius);
            if (triangle == null)
            {
                randomPoint = center;
                return false;
            }
            else
            {
                randomPoint = triangle.GetRandomPointInside();
                return true;
            }
        }
    }
}