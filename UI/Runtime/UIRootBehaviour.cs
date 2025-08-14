using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace UI
{
    public class UIRootBehaviour<R> : UIMonoBehaviour where R : ViewBaseBehaviour
    {
        public bool autoAssign = false;

        [TargetViewBox]
        public R targetView;

        protected override void Awake()
        {
            base.Awake();
            if (autoAssign)
                AutoAssignTargetViewFromParents();
        }

        public override void Render()
        {

        }

        private void AutoAssignTargetViewFromParents()
        {
            Transform nowTransform = transform;
            while (nowTransform != null && targetView == null)
            {
                targetView = nowTransform.GetComponent<R>();
                nowTransform = nowTransform.parent;
            }
        }
    }
}
