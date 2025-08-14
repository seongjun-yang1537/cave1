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

        #region View Event Callback
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