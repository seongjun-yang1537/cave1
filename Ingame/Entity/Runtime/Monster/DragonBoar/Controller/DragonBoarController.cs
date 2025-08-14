using Corelib.Utils;
using PathX;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Core;

namespace Ingame
{
    [RequireComponent(typeof(DragonBoarController))]
    public class DragonBoarController : MonsterController
    {
        public DragonBoarModel dragonBoarModel { get; private set; }
        public DragonBoarView dragonBoarView;

        private Vector3 initialPosition;

        protected override void Awake()
        {
            base.Awake();
            dragonBoarModel = (DragonBoarModel)monsterModel;
            dragonBoarView = (DragonBoarView)monsterView;
            initialPosition = transform.position;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(MonsterState.Wander);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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
            float nextCheckTime = Time.time + checkTargetDelay;

            while (!token.IsCancellationRequested)
            {
                if (Time.time >= nextCheckTime)
                {
                    var target = AimtargetController;
                    if (target != null && Vector3.Distance(transform.position, target.transform.position) <= dragonBoarModel.sightRange)
                    {
                        ChangeState(MonsterState.Chase);
                        return;
                    }
                    nextCheckTime = Time.time + checkTargetDelay;
                }

                if (!HasPath)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(GameRng.Game.NextFloat(2f, 5f)), cancellationToken: token);

                    if (GetRandomPoint(initialPosition, dragonBoarModel.wanderRadius, out var randomPoint))
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
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > dragonBoarModel.sightRange)
                {
                    ChangeState(MonsterState.Wander);
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= dragonBoarModel.attackRange)
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
                if (distance > dragonBoarModel.attackRange)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }

                transform.LookAt(target.transform);

                dragonBoarView.onAnimationStartAttack.Invoke(this, target);
                dragonBoarView.onAnimationEndAttack.AddListenerOnce(
                    () => Attack(target, dragonBoarModel.totalStat.attack)
                );

                await UniTask.Delay((int)(dragonBoarModel.attackCooldown * 1000f), cancellationToken: token);
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