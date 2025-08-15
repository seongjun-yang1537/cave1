using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using VFX;

namespace Ingame
{
    [RequireComponent(typeof(AgentScope))]
    public class AgentView : EntityView
    {
        public readonly UnityEvent<AgentController, AgentController> onAnimationStartAttack = new();
        public readonly UnityEvent onAnimationEndAttack = new();

        public readonly UnityEvent<AgentController, float> onLife = new();
        public readonly UnityEvent<AgentController, float> onLifeMax = new();

        public readonly UnityEvent<AgentController, AgentModel, float> onTakeDamage = new();
        public readonly UnityEvent<AgentController, AgentController, float> onAttack = new();
        public readonly UnityEvent<AgentController, ItemModel> onUseItem = new();

        public readonly UnityEvent<AgentController, AgentModel> onDead = new();

        public readonly UnityEvent<AgentController, IStatusEffect> onApplyStatusEffect = new();
        public readonly UnityEvent<AgentController, IStatusEffect> onRemoveStatusEffect = new();

        public readonly UnityEvent<AgentController, ItemModel, WorldItemMode> onWorldItemChanged = new();

        public float dropForce = 100.0f;

        public Transform heldItemSocket;

        #region View Event Callback

        [AutoSubscribe(nameof(onWorldItemChanged))]
        protected virtual void OnWorldItemChanged(AgentController agentController, ItemModel itemModel, WorldItemMode mode)
        {
            if (mode == WorldItemMode.Drop)
            {
                DropItemByForward(itemModel, transform.forward);
            }
            else
            {
                heldItemSocket.DestroyAllChild();

                if (itemModel == null)
                    return;

                Vector3 spawnPosition = heldItemSocket.position;
                WorldItemController itemController = ItemSystem.SpawnWorldItem(spawnPosition, itemModel, WorldItemMode.Held);

                GameObject go = itemController.gameObject;
                Transform tr = go.transform;
                tr.SetParent(heldItemSocket);
                tr.ResetLocal();
            }
        }

        protected void DropItemByForward(ItemModel itemModel, Vector3 forward)
        {
            Vector3 spawnPositoin = transform.position + 1.5f * Vector3.up;
            WorldItemController itemController = ItemSystem.SpawnWorldItem(spawnPositoin, itemModel, WorldItemMode.Drop);

            GameObject go = itemController.gameObject;

            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            rigidbody.AddForce(forward * dropForce, ForceMode.Impulse);
        }

        [AutoSubscribe(nameof(onApplyStatusEffect))]
        protected virtual void OnApplyStatusEffect(AgentController agentController, IStatusEffect statusEffect)
        {
            if (statusEffect is StunStatusEffect)
            {
                Transform anchor = rigAnchors.body.transform;

                StunStatusEffect stunStatusEffect = statusEffect as StunStatusEffect;

                VFXSpawnContext context = new VFXSpawnContextBuilder()
                    .SetPosition(anchor.position)
                    .SetDuration(stunStatusEffect.durationSeconds)
                    .SetVFXID(VFXID.Stun)
                    .Build();

                VFXController vfxController = VFXSystem.SpawnVFX(context);
                vfxController.transform.SetParent(anchor);
            }
        }
        #endregion
    }
}