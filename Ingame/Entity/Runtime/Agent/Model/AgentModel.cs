// Ingame/AgentModel.cs

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditorInternal;

namespace Ingame
{
    [Serializable]
    public class AgentModel : EntityModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent onStatChanged = new();

        [NonSerialized]
        public readonly UnityEvent<float> onLife = new();
        [NonSerialized]
        public readonly UnityEvent<float> onLifeMax = new();

        [NonSerialized]
        public readonly UnityEvent<AgentModel, float> onTakeDamage = new();
        [NonSerialized]
        public readonly UnityEvent<ItemModel> onUseItem = new();

        [NonSerialized]
        public readonly UnityEvent<AgentModel> onDead = new();

        [NonSerialized]
        public readonly UnityEvent<IStatusEffect> onApplyStatusEffect = new();

        [NonSerialized]
        public readonly UnityEvent<IStatusEffect> onRemoveStatusEffect = new();

        [NonSerialized]
        public readonly UnityEvent<InventorySlotModel> onHeldItem = new();
        [NonSerialized]
        public readonly UnityEvent<ItemModel> onDropItem = new();
        #endregion ====================

        #region ========== Data ==========
        public new AgentModelData Data => base.Data as AgentModelData;

        [SerializeField]
        public AgentStat baseStat;
        public AgentTotalStat totalStat;

        public bool isInvincible => Data.isInvincible;
        #endregion ====================

        #region ========== State ==========
        public float life;
        public float exp;

        private List<IStatusEffect> statusEffects = new();
        public IReadOnlyList<IStatusEffect> StatusEffects => statusEffects;

        [SerializeField]
        public InventoryModel inventory;

        [field: SerializeField]
        public InventorySlotModel heldItemSlot { get; private set; }

        public int aimtargetID;
        #endregion ====================

        public bool isDead { get => life < 0; }

        public AgentModel(AgentModelData data, AgentModelState state = null) : base(data, state)
        {
            baseStat = new AgentStat(data.baseStat);
            totalStat = new AgentTotalStat(this);

            inventory = new();
            foreach (var slotModel in inventory)
                slotModel.itemModel = ItemModel.Empty;

            heldItemSlot = inventory.GetItemSlot(QuickSlotID.Slot0);
            life = totalStat.lifeMax;
            exp = 0f;
            aimtargetID = 0;

            if (state != null)
            {
                inventory = JsonUtility.FromJson<InventoryModel>(JsonUtility.ToJson(state.inventory));
                heldItemSlot = state.heldItemSlot;
                life = state.life;
                exp = state.exp;
                aimtargetID = state.aimtargetID;
            }

            onStatChanged.AddListener(OnStatChanged);
            inventory.onChangedEquipment.AddListener(_ => onStatChanged.Invoke());
            inventory.onChangedQuickSlot.AddListener(OnInventoryChanged);
            inventory.onChangedBag.AddListener(OnInventoryChanged);
        }

        private void OnInventoryChanged(InventorySlotModel changedSlot)
        {
            if (changedSlot == heldItemSlot)
            {
                onHeldItem.Invoke(heldItemSlot);
            }
        }

        protected void Awake()
        {

        }

        public void SetLife(float newLife)
        {
            life = newLife;
            onLife.Invoke(newLife);
        }

        public virtual float TakeDamage(AgentModel other, float damage)
        {
            SetLife(life - damage);
            if (life <= 0f)
                Dead(other);

            if (damage > 0f)
                onTakeDamage.Invoke(other, damage);
            return damage;
        }

        public void UseItem(ItemModel itemModel)
        {
            onUseItem.Invoke(itemModel);
        }

        public void SetHeldItem(InventorySlotModel itemSlot)
        {
            heldItemSlot = itemSlot;
            onHeldItem.Invoke(heldItemSlot);
        }

        public void DropItem(InventorySlotModel itemSlot, int count = 1)
        {
            if (itemSlot.itemModel == null || itemSlot.itemModel.IsEmpty) return;
            ItemModel itemModel = ItemModelFactory.Create(itemSlot.itemModel.Data, new ItemModelState { count = itemSlot.itemModel.count });
            inventory.TakeItem(itemSlot, count);

            ItemModel dropItemModel = ItemModelFactory.Create(itemModel.Data, new ItemModelState { count = count });
            onDropItem.Invoke(dropItemModel);

            if (itemSlot == heldItemSlot)
                onHeldItem.Invoke(heldItemSlot);
        }

        public float CalculateDamage(AgentModel target, float damage)
        {
            return damage;
        }

        public void Dead(AgentModel target = null)
        {
            onDead.Invoke(target);
        }

        public void AddStatusEffect(IStatusEffect effect)
        {
            statusEffects.Add(effect);
            onApplyStatusEffect.Invoke(effect);
        }

        public void RemoveStatusEffect(IStatusEffect effect)
        {
            if (statusEffects.Remove(effect))
                onRemoveStatusEffect.Invoke(effect);
        }

        protected virtual void OnStatChanged()
        {
            onLife.Invoke(life);
            onLifeMax.Invoke(totalStat.lifeMax);
        }
    }
}