using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(PawnScope))]
    public class PawnView : AgentView
    {
        public readonly UnityEvent<PawnController, PawnPoseState> onPoseState = new();

        protected Vector3 deltaPosition;
        private Vector3 prevPosition;

        protected virtual void Update()
        {
            deltaPosition = transform.position - prevPosition;
            prevPosition = transform.position;
        }

        #region View Event Callback
        [AutoSubscribe(nameof(onAttack))]
        protected virtual void OnAttack(AgentController agentController, AgentController other, float damage)
        {
            PlayAnimation("Hand_Swing");
        }

        [AutoSubscribe(nameof(onUseItem))]
        protected virtual void OnUseItem(AgentController agentController, ItemModel itemModel)
        {
            PlayAnimation("Hand_Swing");
        }
        #endregion
    }
}