using UnityEngine;
using VContainer;

namespace Ingame
{
    public class PlayerAimtargetProvider : IAimTargetProvider
    {
        [Inject] private readonly Transform transform;

        public int GetAimtarget()
        {
            var cam = Camera.main;
            if (cam == null)
                return -1;

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f, 1 << LayerMask.NameToLayer("Entity")))
            {
                var target = hit.collider.GetComponentInParent<EntityController>();

                if (target != null && target.transform != transform)
                {
                    return target.entityModel.entityID;
                }
            }

            return -1;
        }
    }
}