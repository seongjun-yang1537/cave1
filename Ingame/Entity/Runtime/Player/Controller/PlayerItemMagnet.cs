using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ingame
{
    public class PlayerItemMagnet
    {
        private readonly PlayerController playerController;
        private Vector3 centerPosition => playerController.transform.position + Vector3.up * 1.5f;

        private float magnetRange = 2.5f;
        private float magnetSpeed = 5f;
        private readonly List<WorldItemController> _collectingItems = new();

        public PlayerItemMagnet(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Update()
        {
            UpdateMagnet();
        }

        private void UpdateMagnet()
        {
            UpdateTrackingDroppedItems();
            UpdateFollowingCollectedItems();
        }

        private void UpdateTrackingDroppedItems()
        {
            var worldItemControllers = ItemSystem.Instance.ItemControllers
                .Where(ic => ic is WorldItemController)
                .Select(ic => ic as WorldItemController)
                .Where(wic => wic.CurrentMode == WorldItemController.Mode.Drop);

            foreach (var itemController in worldItemControllers)
            {
                if (!itemController.itemModel.isAcquireable)
                    continue;
                if (_collectingItems.Contains(itemController))
                    continue;
                if (Time.time - itemController.spawnTime < 2f)
                    continue;
                Vector3 diff = centerPosition - itemController.transform.position;
                if (diff.sqrMagnitude >= magnetRange * magnetRange)
                    continue;
                if (!RayTestDropItemController(itemController))
                    continue;

                var rigidbody = itemController.GetComponent<Rigidbody>();
                var collider = itemController.GetComponent<SphereCollider>();
                rigidbody.isKinematic = true;
                collider.enabled = false;
                _collectingItems.Add(itemController);
            }
        }

        private void UpdateFollowingCollectedItems()
        {
            Vector3 destination = centerPosition;
            for (int i = _collectingItems.Count - 1; i >= 0; i--)
            {
                var itemController = _collectingItems[i];
                if (itemController == null)
                {
                    _collectingItems.RemoveAt(i);
                    continue;
                }
                Vector3 from = itemController.transform.position;
                if ((from - destination).sqrMagnitude < 1f)
                {
                    playerController.AcquireItem(itemController.itemModel);
                    Object.Destroy(itemController.gameObject);
                    _collectingItems.RemoveAt(i);
                    continue;
                }
                Vector3 to = Vector3.Lerp(from, destination, 0.1f);
                Vector3 moveDir = (to - from).normalized;
                float moveDist = Mathf.Min(Vector3.Distance(from, to), magnetSpeed * Time.deltaTime);
                itemController.transform.position = from + moveDir * moveDist;
            }
        }

        private bool RayTestDropItemController(WorldItemController itemController)
        {
            Vector3 itemPosition = itemController.transform.position + Vector3.up;

            Vector3 origin = centerPosition;
            Vector3 dir = itemPosition - origin;
            float maxDist = Mathf.Min(magnetRange, Vector3.Distance(itemPosition, centerPosition));

            return !Physics.Raycast(origin, dir, maxDist, 1 << LayerMask.NameToLayer("Landscape"));
        }
    }
}