using Corelib.Utils;
using UnityEngine;
using Core;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ingame
{
    [RequireComponent(typeof(CreeplingerScope))]
    public class CreeplingerController : MonsterController
    {
        public CreeplingerModel creeplingerModel { get; private set; }
        public CreeplingerView creeplingerView;


        private float rerouteDelay;

        protected override void Awake()
        {
            base.Awake();
            creeplingerModel = (CreeplingerModel)monsterModel;
            creeplingerView = (CreeplingerView)monsterView;
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
                if (target != null && Vector3.Distance(transform.position, target.transform.position) <= creeplingerModel.sightRange)
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

                if (distance <= creeplingerModel.attackRange)
                {
                    ChangeState(MonsterState.Attack);
                    return;
                }

                if (distance > creeplingerModel.sightRange)
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
                if (distance > creeplingerModel.attackRange)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }

                transform.LookAt(target.transform);

                creeplingerView.onAnimationStartAttack.Invoke(this, target);
                creeplingerView.onAnimationEndAttack.AddListenerOnce(
                    () => Attack(target, creeplingerModel.totalStat.attack)
                );

                await UniTask.Delay((int)(creeplingerModel.attackCooldown * 1000f), cancellationToken: token);
            }
        }
    }
}