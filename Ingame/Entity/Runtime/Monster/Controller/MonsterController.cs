using Core;
using Corelib.Utils;
using PathX;
using UnityEngine;
using VContainer;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Ingame
{
    [RequireComponent(typeof(MonsterScope))]
    public class MonsterController : PawnController
    {
        public MonsterModel monsterModel { get; private set; }
        public MonsterView monsterView;

        [Inject] protected readonly IMonsterActiveCondition activeCondition;

        protected CancellationTokenSource stateCts;
        public MonsterState CurrentState { get; private set; }

        private bool IsStunned => agentModel.StatusEffects.Any(e => e is StunStatusEffect);

        private bool fsmPaused;

        protected bool HasPath { get => monsterModel.pathProgress?.plannedPath?.Count > 0; }

        protected override void Update()
        {
            base.Update();

            if (activeCondition != null && !activeCondition.IsActive())
            {
                CancelStateRoutine();
            }

            if (IsStunned)
            {
                if (!fsmPaused)
                {
                    fsmPaused = true;
                    CancelStateRoutine();
                    StopMove();
                }
            }
            else if (fsmPaused)
            {
                fsmPaused = false;
                ChangeState(MonsterState.Idle);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            monsterModel = (MonsterModel)pawnModel;
            monsterView = (MonsterView)pawnView;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CancelStateRoutine();
        }

        private void CancelStateRoutine()
        {
            if (stateCts != null)
            {
                stateCts.Cancel();
                stateCts.Dispose();
                stateCts = null;
            }
        }

        protected void ChangeState(MonsterState newState)
        {
            if (activeCondition != null && !activeCondition.IsActive())
                return;

            if (IsStunned)
                return;

            CurrentState = newState;

            CancelStateRoutine();

            stateCts = new CancellationTokenSource();
            var token = stateCts.Token;
            GetStateTask(newState, token).Forget();
        }

        protected virtual async UniTask GetStateTask(MonsterState state, CancellationToken token)
        {
            await UniTask.Yield(token);
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = targetPosition;

            PathfindingSettings settings = new PathfindingSettings.Builder()
                .WithFallbackToClosest(true)
                .Build();

            SetTargetPositionAsync(startPosition, endPosition, settings).Forget();
        }

        private async UniTask SetTargetPositionAsync(Vector3 startPosition, Vector3 endPosition, PathfindingSettings settings)
        {
            var path = await PathXSystem.PathfindAsync(entityModel.navDomain, startPosition, endPosition, settings);
            monsterModel.pathProgress.SetCurrentPath(path);
        }

        public void EnterIdleState()
        {
            StopMove();
        }

        public void StopMove()
        {
            monsterModel.ClearPath();
        }

        public override void Dead(AgentModel other)
        {
            if (monsterModel.dropTable != null)
            {
                List<ItemModel> drops = monsterModel.dropTable.GenerateDrops();
                foreach (var drop in drops)
                {
                    WorldItemController controller = ItemSystem.SpawnWorldItem(transform.position, drop, WorldItemMode.Drop);
                    controller.Leap();
                }
            }
            if (other is PlayerModel player)
                GameActionEventBus.Publish(GameActionType.PlayerKilledEnemy, $"{{\"playerId\":{player.entityID},\"enemyId\":{monsterModel.entityID}}}");
            base.Dead(other);
        }

        [AutoModelSubscribe(nameof(AgentModel.onApplyStatusEffect))]
        protected override void OnApplyStatusEffect(IStatusEffect effect)
        {
            base.OnApplyStatusEffect(effect);
            if (effect is StunStatusEffect)
            {
                CancelStateRoutine();
                StopMove();
                fsmPaused = true;
            }
        }

        [AutoModelSubscribe(nameof(AgentModel.onRemoveStatusEffect))]
        protected override void OnRemoveStatusEffect(IStatusEffect effect)
        {
            base.OnRemoveStatusEffect(effect);
            if (effect is StunStatusEffect && !IsStunned)
            {
                fsmPaused = false;
                ChangeState(MonsterState.Idle);
            }
        }
    }
}