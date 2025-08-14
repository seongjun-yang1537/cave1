using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(PawnScope))]
    public class PawnView : AgentView
    {
        public readonly UnityEvent<PawnController, PawnPoseState> onPoseState = new();
        public readonly UnityEvent<PawnController, ItemModel> onHeldItem = new();
        public readonly UnityEvent<PawnController, ItemModel> onDropItem = new();

        public float dropForce = 100.0f;

        public Transform heldItemSocket;

        protected Vector3 deltaPosition;
        private Vector3 prevPosition;

        protected virtual void Update()
        {
            deltaPosition = transform.position - prevPosition;
            prevPosition = transform.position;
        }

        #region View Event Callback
        [AutoSubscribe(nameof(onDropItem))]
        protected virtual void OnDropItem(PawnController pawnController, ItemModel itemModel)
        {
            DropItemByForward(itemModel, transform.forward);
        }

        protected void DropItemByForward(ItemModel itemModel, Vector3 forward)
        {
            Vector3 spawnPositoin = transform.position + 1.5f * Vector3.up;
            DropItemController itemController = ItemSystem.SpawnDropItem(spawnPositoin, itemModel);

            GameObject go = itemController.gameObject;

            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            rigidbody.AddForce(forward * dropForce, ForceMode.Impulse);
        }

        [AutoSubscribe(nameof(onHeldItem))]
        protected virtual void OnHeldItem(PawnController pawnController, ItemModel itemModel)
        {
            heldItemSocket.DestroyAllChild();

            Vector3 spawnPosition = heldItemSocket.position;
            DropItemController itemController = ItemSystem.SpawnHeldItem(spawnPosition, itemModel);

            GameObject go = itemController.gameObject;
            Transform tr = go.transform;
            tr.SetParent(heldItemSocket);
            tr.ResetLocal();
        }

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