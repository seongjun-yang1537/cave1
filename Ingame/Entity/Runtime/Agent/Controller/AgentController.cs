using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using System.Collections;
using Corelib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Ingame
{
    [RequireComponent(typeof(AgentScope))]
    public class AgentController : EntityController
    {
        protected UnityEvent<AgentController, float> onAttack = new();

        [Inject] private readonly IAimTargetProvider aimTargetProvider;
        [Inject] protected readonly IAgentAttackable agentAttackable;
        [Inject] protected readonly IAgentExplodable agentExplodable;
        [Inject] public readonly HandController handController;

        [ModelSourceBase]
        public AgentModel agentModel;
        public AgentView agentView;

        private Coroutine updateAimtargetCoroutine;
        private const float AIMTARGET_UPDATE_INTERVAL = 0.5f;

        public IReadOnlyList<IStatusEffect> StatusEffects => agentModel.StatusEffects;

        protected AgentController AimtargetController
        {
            get
            {
                if (agentModel.aimtargetID == -1) return null;
                return EntitySystem.FindControllerByID(agentModel.aimtargetID) as AgentController;
            }
        }

        #region LifeCycle
        protected override void Awake()
        {
            base.Awake();
            agentModel = (AgentModel)entityModel;
            agentView = (AgentView)entityView;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (updateAimtargetCoroutine != null) StopCoroutine(updateAimtargetCoroutine);
            updateAimtargetCoroutine = StartCoroutine(UpdateAimtargetRoutine());
        }

        protected override void Update()
        {
            base.Update();
            UpdateStatusEffects(Time.deltaTime);
        }

        protected void FixedUpdate()
        {
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (updateAimtargetCoroutine != null)
            {
                StopCoroutine(updateAimtargetCoroutine);
                updateAimtargetCoroutine = null;
            }
        }
        #endregion

        #region Action
        public int GetAimtargetID()
        {
            return aimTargetProvider?.GetAimtarget() ?? -1;
        }
        public EntityController GetAimtarget()
            => EntitySystem.FindControllerByID(GetAimtargetID());

        public virtual float Attack(float damage)
        {
            return Attack(AimtargetController, damage);
        }

        public virtual float Attack(AgentController target, float damage)
        {
            AttackContext info = AttackContext.CreateBuilder()
                .SetDamage(damage)
                .Build();

            damage = agentAttackable.Attack(this, target, info);
            onAttack.Invoke(target, damage);
            return damage;
        }

        public virtual void UseItem(ItemModel itemModel)
        {
            agentModel.UseItem(itemModel);
        }

        public virtual void AcquireItem(ItemModel itemModel)
        {
            agentModel.inventory.AddItem(itemModel);
        }

        public virtual void DiscardItem(ItemModel itemModel)
        {
            agentModel.inventory.RemoveItem(itemModel);
        }

        public virtual void ChangeHeldItem(InventorySlotModel itemSlot)
        {
            agentModel.SetHeldItem(itemSlot);
        }

        public virtual void DropItem(InventorySlotModel itemSlot)
        {
            agentModel.DropItem(itemSlot);
        }

        public virtual void ModifyItemCount(InventoryContainerType containerType, int slotID, int count)
        {
            agentModel.ModifyItemCount(containerType, slotID, count);
        }

        public virtual float TakeDamage(AgentModel other, float damage)
        {
            damage = agentModel.TakeDamage(other, damage);
            if (agentModel.isDead)
            {
                Dead(other);
            }
            return damage;
        }

        public virtual float Heal(float healAmount)
        {
            float deltaAmount = Mathf.Min(healAmount, agentModel.totalStat.lifeMax - agentModel.life);
            agentModel.SetLife(agentModel.life + deltaAmount);
            return deltaAmount;
        }

        public float HealRatio(float healRatio)
            => Heal(agentModel.totalStat.lifeMax * healRatio);

        public virtual void Dead(AgentModel other)
        {
            agentModel.Dead(other);
            Destroy(gameObject);
        }

        public virtual void Explode()
        {
            agentExplodable.Explode(this);
        }

        public virtual void AddStatusEffect(IStatusEffect effect)
        {
            if (effect == null) return;
            effect.OnApply(this);
            agentModel.AddStatusEffect(effect);
        }

        public virtual void RemoveStatusEffect(IStatusEffect effect)
        {
            if (effect == null) return;
            effect.OnRemove(this);
            agentModel.RemoveStatusEffect(effect);
        }

        protected virtual void UpdateStatusEffects(float dt)
        {
            for (int i = agentModel.StatusEffects.Count - 1; i >= 0; i--)
            {
                IStatusEffect effect = agentModel.StatusEffects[i];
                if (effect is IUpdateStatusEffect updater)
                    updater.Update(dt);

                if (effect.IsExpired())
                    RemoveStatusEffect(effect);
            }
        }
        #endregion

        #region Routines
        private IEnumerator UpdateAimtargetRoutine()
        {
            while (true)
            {
                agentModel.aimtargetID = GetAimtargetID();

                float randomDelay = Random.Range(AIMTARGET_UPDATE_INTERVAL * 0.8f, AIMTARGET_UPDATE_INTERVAL * 1.2f);
                yield return new WaitForSeconds(randomDelay);
            }
        }
        #endregion

        #region Event Callback
        [AutoSubscribe(nameof(onAttack))]
        protected virtual void OnAttack(AgentController other, float damage)
            => agentView.onAttack.Invoke(this, other, damage);

        [AutoModelSubscribe(nameof(AgentModel.onLife))]
        protected virtual void OnLife(float life)
            => agentView.onLife.Invoke(this, life);

        [AutoModelSubscribe(nameof(AgentModel.onLifeMax))]
        protected virtual void OnLifeMax(float lifeMax)
            => agentView.onLifeMax.Invoke(this, lifeMax);

        [AutoModelSubscribe(nameof(AgentModel.onTakeDamage))]
        protected virtual void OnTakeDamage(AgentModel other, float damage)
            => agentView.onTakeDamage.Invoke(this, other, damage);

        [AutoModelSubscribe(nameof(AgentModel.onUseItem))]
        protected virtual void OnUseItem(ItemModel itemModel)
            => agentView.onUseItem.Invoke(this, itemModel);

        [AutoModelSubscribe(nameof(AgentModel.onDead))]
        protected virtual void OnDead(AgentModel other)
            => agentView.onDead.Invoke(this, other);

        [AutoModelSubscribe(nameof(AgentModel.onApplyStatusEffect))]
        protected virtual void OnApplyStatusEffect(IStatusEffect effect)
            => agentView.onApplyStatusEffect.Invoke(this, effect);

        [AutoModelSubscribe(nameof(AgentModel.onRemoveStatusEffect))]
        protected virtual void OnRemoveStatusEffect(IStatusEffect effect)
            => agentView.onRemoveStatusEffect.Invoke(this, effect);

        [AutoModelSubscribe(nameof(AgentModel.onHeldItem))]
        protected virtual void OnHeldItem(InventorySlotModel itemSlot)
            => agentView.onHeldItem.Invoke(this, itemSlot.itemModel);

        [AutoModelSubscribe(nameof(AgentModel.onDropItem))]
        protected virtual void OnDropItem(ItemModel itemModel)
            => agentView.onDropItem.Invoke(this, itemModel);
        #endregion
    }
}