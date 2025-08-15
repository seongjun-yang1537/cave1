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
            float magnetRangeSqr = magnetRange * magnetRange;
            Vector3 center = centerPosition;

            foreach (var c in ItemSystem.Instance.ItemControllers.OfType<WorldItemController>())
            {
                if (!IsMagnetTarget(c, center, magnetRangeSqr)) continue;

                if (c.TryGetComponent<Rigidbody>(out var rb) &&
                    c.TryGetComponent<SphereCollider>(out var col))
                {
                    rb.isKinematic = true;
                    col.enabled = false;
                    _collectingItems.Add(c);
                }
            }
        }

        private bool IsMagnetTarget(WorldItemController c, Vector3 center, float magnetRangeSqr)
        {
            if (!c.itemModel.isAcquireable) return false;
            if (c.worldItemType != WorldItemType.DropItem) return false;
            if (_collectingItems.Contains(c)) return false;
            if (Time.time - c.spawnTime < 2f) return false;

            Vector3 diff = center - c.transform.position;
            if (diff.sqrMagnitude >= magnetRangeSqr) return false;

            if (!RayTestWorldItemController(c)) return false;

            return true;
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

        private bool RayTestWorldItemController(WorldItemController itemController)
        {
            Vector3 itemPosition = itemController.transform.position + Vector3.up;

            Vector3 origin = centerPosition;
            Vector3 dir = itemPosition - origin;
            float maxDist = Mathf.Min(magnetRange, Vector3.Distance(itemPosition, centerPosition));

            return !Physics.Raycast(origin, dir, maxDist, 1 << LayerMask.NameToLayer("Landscape"));
        }
    }
}