using Corelib.Utils;
using UnityEngine;
using Core;
using PathX;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ingame
{
    [RequireComponent(typeof(CockroachScope))]
    public class CockroachController : MonsterController
    {
        public CockroachModel cockroachModel { get; private set; }

        private const float wanderRadius = 5f;
        private float rerouteDelay;

        protected override void Awake()
        {
            base.Awake();
            cockroachModel = (CockroachModel)monsterModel;
            rerouteDelay = GameRng.Game.NextFloat(1.5f, 2.5f);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(MonsterState.Idle);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        protected override async UniTask GetStateTask(MonsterState state, CancellationToken token)
        {
            switch (state)
            {
                case MonsterState.Idle:
                    await IdleState(token);
                    break;
            }
        }

        private async UniTask IdleState(CancellationToken token)
        {
            EnterIdleState();
            while (!token.IsCancellationRequested)
            {
                if (!HasPath)
                {
                    await UniTask.Delay((int)(rerouteDelay * 1000f), cancellationToken: token);

                    if (GetRandomPoint(transform.position, wanderRadius, out var randomPoint))
                    {
                        SetTargetPosition(randomPoint);
                    }
                }

                await UniTask.Yield(token);
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