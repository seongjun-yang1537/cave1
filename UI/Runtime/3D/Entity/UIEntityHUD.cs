using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// World space UI for monster HUD following the entity head rig anchor.
    /// </summary>
    public class UIEntityHUD : MonoBehaviour
    {
        private UIEntityHUDLifeGauge lifeGauge;
        private AgentView targetView;
        private Transform followTarget;
        private EntityHUDPool pool;

        private void Awake()
        {
            lifeGauge = GetComponentInChildren<UIEntityHUDLifeGauge>();
        }

        private void LateUpdate()
        {
            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }
        }

        public void Bind(AgentController controller, EntityHUDPool ownerPool)
        {
            pool = ownerPool;
            if (controller == null)
                return;

            targetView = controller.agentView;
            followTarget = targetView?.rigAnchors?.head?.transform;

            if (targetView != null)
            {
                targetView.onLife.AddListener(OnLife);
                targetView.onLifeMax.AddListener(OnLifeMax);
                targetView.onTakeDamage.AddListener(OnTakeDamage);
                targetView.onDead.AddListener(OnDead);
                lifeGauge.SetRatio(controller.agentModel.totalStat.lifeRatio, false);
            }
        }

        public void Unbind()
        {
            if (targetView != null)
            {
                targetView.onLife.RemoveListener(OnLife);
                targetView.onLifeMax.RemoveListener(OnLifeMax);
                targetView.onTakeDamage.RemoveListener(OnTakeDamage);
                targetView.onDead.RemoveListener(OnDead);
            }

            targetView = null;
            followTarget = null;
            pool = null;
        }

        private void OnLife(AgentController controller, float life)
        {
            lifeGauge.SetRatio(controller.agentModel.totalStat.lifeRatio);
        }

        private void OnLifeMax(AgentController controller, float lifeMax)
        {
            lifeGauge.SetRatio(controller.agentModel.totalStat.lifeRatio);
        }

        private void OnTakeDamage(AgentController controller, AgentModel other, float damage)
        {
            lifeGauge.SetRatio(controller.agentModel.totalStat.lifeRatio);
            Vector3 pos = followTarget != null ? followTarget.position : transform.position;
            WorldUISystem.Instance.SpawnDamageIndicator(pos, damage);
        }

        private void OnDead(AgentController controller, AgentModel other)
        {
            pool?.Despawn(this);
        }
    }
}
