using Core;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class DebugItemSpawner : MonoBehaviour
    {
        public ItemID itemID;
        public int count;

        public float force;

        private MT19937 rng;
        protected void Awake()
        {
            rng = GameRng.Game;
        }

        public void Spawn()
        {
            ItemModel itemModel = ItemModelFactory.Create(new ItemModelState { itemID = itemID, count = count });

            WorldItemController controller = ItemSystem.SpawnWorldItem(transform.position, itemModel, WorldItemMode.Drop);
            Rigidbody rigidbody = controller.GetComponent<Rigidbody>();

            Vector3 upVector = (Random.insideUnitSphere + Vector3.up * 5).normalized;
            float randomForce = rng.NextFloat(force, force * 1.5f);
            rigidbody.AddForce(upVector * randomForce, ForceMode.Impulse);
        }
    }
}