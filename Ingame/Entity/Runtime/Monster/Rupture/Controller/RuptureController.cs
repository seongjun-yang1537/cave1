using Corelib.Utils;
using PathX;
using UnityEngine;
using Core;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ingame
{
    [RequireComponent(typeof(RuptureScope))]
    public class RuptureController : MonsterController
    {
        public RuptureModel ruptureModel { get; private set; }


        private float rerouteDelay;

        protected override void Awake()
        {
            base.Awake();
            ruptureModel = (RuptureModel)monsterModel;
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
                case MonsterState.Chase:
                    await ChaseState(token);
                    break;
                case MonsterState.Explode:
                    await ExplodeState(token);
                    break;
            }
        }

        private async UniTask IdleState(CancellationToken token)
        {
            EnterIdleState();
            while (!token.IsCancellationRequested)
            {
                if (AimtargetController != null)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }
                await UniTask.Delay(500, cancellationToken: token);
            }
        }

        private async UniTask ChaseState(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                AgentController target = AimtargetController;
                if (target == null)
                {
                    ChangeState(MonsterState.Idle);
                    return;
                }

                if (Vector3.Distance(transform.position, target.transform.position) <= ruptureModel.explosionRange)
                {
                    ChangeState(MonsterState.Explode);
                    return;
                }

                SetTargetPosition(target.transform.position);
                await UniTask.Delay((int)(rerouteDelay * 1000f), cancellationToken: token);
            }
        }

        private async UniTask ExplodeState(CancellationToken token)
        {
            StopMove();
            Attack(null, 0f);

            await UniTask.Delay((int)(ruptureModel.explosionFuseTime * 1000f), cancellationToken: token);

            Explode();
        }
    }
}