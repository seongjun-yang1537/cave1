using Core;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(PlayerScope))]
    public class PlayerController : PawnController
    {
        #region ========== Constant ==========
        private const float COMBAT_DURATION = 5f;
        private const float MONSTER_COMBAT_CHECK_RANGE = 20f;
        #endregion ====================

        #region ========== MVC ==========
        [field: SerializeField]
        public PlayerModel playerModel { get; private set; }
        [field: SerializeField]
        public PlayerView playerView { get; private set; }
        #endregion ====================

        #region ========== Event ==========
        public UnityEvent<EntityController> onInteractTargetEnter = new();
        public UnityEvent<EntityController> onInteractTargetExit = new();
        public UnityEvent<bool> onViewLockChanged => viewHandler.onViewLockChanged;
        #endregion ====================

        [Inject] public PlayerInputContext inputContext = new();
        [Inject] public PlayerPhysics playerPhysics;

        private PlayerInputConfig inputConfig { get => playerModel.inputConfig; }
        private JetpackModel jetpackModel { get => playerModel.jetpackModel; }

        #region ========== Jetpack ==========
        private JetpackController jetpackController;
        private float jetpackDisableTimer;
        #endregion ====================

        #region ========== State ==========
        [field: SerializeField]
        public bool inCombat { get; private set; }
        private float combatTime;
        #endregion ====================

        #region ========== Handler ==========
        [LifecycleInject]
        private PlayerJetpackHandler jetpackHandler;
        [LifecycleInject]
        private HandActionHandler handActionHandler;
        [LifecycleInject]
        private PlayerPoseHandler poseHandler;
        [LifecycleInject]
        private PlayerItemMagnet itemMagnet;
        [LifecycleInject]
        private PlayerInteractionHandler interactionHandler;
        [LifecycleInject]
        private PlayerViewHandler viewHandler;
        #endregion ====================

        public bool CanUseJetpack() => jetpackDisableTimer <= 0f;

        private float oxygenPerSecond;
        public void SetOxygenPerSecond(float value)
        {
            oxygenPerSecond = value;
        }

        protected override void Awake()
        {
            base.Awake();
            playerModel = (PlayerModel)pawnModel;
            playerView = (PlayerView)pawnView;
            jetpackController = GetComponentInChildren<JetpackController>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            jetpackModel.onState.AddListener(OnStateJetpack);

            playerPhysics.ResetFallState();
        }

        protected override void Update()
        {
            base.Update();

            UpdateJetpackDisableTimer();

            poseHandler.Update(inputContext);
            UpdateOxygen();
            UpdateJump();
            UpdateInventory();

            jetpackHandler.Update(inputContext);
            handActionHandler.Update(inputContext);
            interactionHandler.Update(inputContext);
            itemMagnet.Update();

            float fallDamage = playerPhysics.ProcessFallDamage();
            if (fallDamage > 0f)
                TakeDamage(null, fallDamage);

            UpdateCombatState();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            jetpackModel.onState.RemoveListener(OnStateJetpack);
        }

        #region ========== View Context ==========
        public bool viewLocked => viewHandler.viewLocked;
        public void SetActionEnabled(bool enabled) => viewHandler.SetActionEnabled(enabled);

        public void SetMoveAndViewEnabled(bool enabled) => viewHandler.SetMoveAndViewEnabled(enabled);

        public void SetViewLocked(bool locked) => viewHandler.SetViewLocked(locked);
        #endregion ====================

        #region Action
        #region ========== Gold ==========
        public void GainGold(int amount)
        {
            playerModel.AddGold(amount);
        }

        public void SpendGold(int amount)
        {
            playerModel.AddGold(-amount);
        }
        #endregion ====================

        public override void AcquireItem(ItemModel itemModel)
        {
            base.AcquireItem(itemModel);
            GameActionEventBus.Publish(GameActionType.PlayerAcquiredItem, $"{{\"playerId\":{playerModel.entityID},\"itemId\":{(int)itemModel.itemID},\"count\":{itemModel.count}}}");
        }

        public void Refuel(float deltaFuel)
        {
            playerModel.Refuel(deltaFuel);
        }

        public void RefuelRatio(float ratio)
        {
            playerModel.RefuelRatio(ratio);
        }

        public void RefillOxygen(float deltaOxygen)
        {
            playerModel.RefillOxygen(deltaOxygen);
        }

        public void RefillOxygenRatio(float deltaOxygenRatio)
        {
            playerModel.RefillOxygenRatio(deltaOxygenRatio);
        }

        public void SetPosition(Vector3 newPosition)
        {
            moveable.ClearVelocity();
            characterController.SetPosition(newPosition);
        }
        #endregion

        float delayOxygenDamage;

        private void UpdateOxygen()
        {
            playerModel.ConsumeOxygen(oxygenPerSecond * Time.deltaTime);

            delayOxygenDamage -= Time.deltaTime;
            if (playerModel.oxygen <= 0f)
            {
                if (delayOxygenDamage <= 0f)
                {
                    TakeDamage(null, 10f);
                    delayOxygenDamage = 1f;
                }
            }
        }

        private void UpdateInventory()
        {
            if (inputContext.GetKeyDown(KeyCode.Q))
            {
                DropItem(playerModel.heldItemSlot);
            }
        }

        private void UpdateJump()
        {
            if (playerModel.jetpackModel.state == JetpackState.On)
                return;

            if (inputContext.GetKeyDown(inputConfig.jumpKey))
            {
                Jump();
            }
        }

        private void UpdateJetpackDisableTimer()
        {
            jetpackDisableTimer -= Time.deltaTime;
        }


        private void UpdateCombatState()
        {
            combatTime -= Time.deltaTime;
            if (IsTargetOfMonster())
                combatTime = COMBAT_DURATION;

            bool combat = combatTime > 0f;
            if (combat != inCombat)
            {
                inCombat = combat;
            }
        }

        private bool IsTargetOfMonster()
        {
            foreach (var entity in EntitySystem.Entities)
            {
                if (entity is MonsterController monster)
                {
                    if (monster.GetAimtargetID() == entityModel.entityID)
                    {
                        float sqrDist = (monster.transform.position - transform.position).sqrMagnitude;
                        if (sqrDist <= MONSTER_COMBAT_CHECK_RANGE * MONSTER_COMBAT_CHECK_RANGE)
                            return true;
                    }
                }
            }
            return false;
        }

        private void OnStateJetpack(JetpackState state)
        {
            switch (state)
            {
                case JetpackState.On:
                    OnEnableJetpack();
                    break;
                case JetpackState.Off:
                    OnDisableJetpack();
                    break;
            }
        }

        private void OnEnableJetpack()
        {
            jetpackController.OnEnableJetpack();
        }

        private void OnDisableJetpack()
        {
            jetpackController.OnDisableJetpack();
        }

        public override void MoveTo(Vector3 newPosition)
        {
            base.MoveTo(newPosition);
            playerPhysics.ResetFallState();
        }

        public override float Attack(float damage)
        {
            return Attack(AimtargetController, damage);
        }

        public override float Attack(AgentController target, float damage)
        {
            if (target is MonsterController)
                combatTime = COMBAT_DURATION;
            return base.Attack(target, damage);
        }

        public override float TakeDamage(AgentModel other, float damage)
        {
            if (other is MonsterModel)
            {
                combatTime = COMBAT_DURATION;
                jetpackDisableTimer = 3f;
                if (jetpackModel.state == JetpackState.On)
                    jetpackModel.SetState(JetpackState.Off);
            }
            return base.TakeDamage(other, damage);
        }

        [AutoSubscribe(nameof(onInteractTargetEnter))]
        private void OnInteractTargetEnter(EntityController targetEntityController)
            => playerView.onInteractTargetEnter.Invoke(this, targetEntityController);

        [AutoSubscribe(nameof(onInteractTargetExit))]
        private void OnInteractTargetExit(EntityController targetEntityController)
            => playerView.onInteractTargetExit.Invoke(this, targetEntityController);
    }
}