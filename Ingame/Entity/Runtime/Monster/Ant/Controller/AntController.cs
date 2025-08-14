using Corelib.Utils;
using PathX;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Core;

namespace Ingame
{
    [RequireComponent(typeof(AntScope))]
    public class AntController : MonsterController
    {
        public AntModel antModel { get; private set; }
        public AntView antView;

        private float rerouteDelay;

        protected void Awake()
        {
            base.Awake();
            antModel = (AntModel)monsterModel;
            antView = (AntView)monsterView;
            rerouteDelay = GameRng.Game.NextFloat(0.8f, 1.2f);
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
                case MonsterState.Attack:
                    await AttackState(token);
                    break;
            }
        }

        private async UniTask IdleState(CancellationToken token)
        {
            EnterIdleState();
            while (!token.IsCancellationRequested)
            {
                var target = AimtargetController;
                if (target != null && Vector3.Distance(transform.position, target.transform.position) <= antModel.sightRange)
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
                var target = AimtargetController;
                if (target == null)
                {
                    ChangeState(MonsterState.Idle);
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance <= antModel.attackRange)
                {
                    ChangeState(MonsterState.Attack);
                    return;
                }

                if (distance > antModel.sightRange)
                {
                    ChangeState(MonsterState.Idle);
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
                    ChangeState(MonsterState.Idle);
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > antModel.attackRange)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }

                transform.LookAt(target.transform);

                antView.onAnimationStartAttack.Invoke(this, target);
                antView.onAnimationEndAttack.AddListenerOnce(
                    () => Attack(target, antModel.totalStat.attack)
                );

                await UniTask.Delay((int)(antModel.attackCooldown * 1000f), cancellationToken: token);
            }
        }
    }
}