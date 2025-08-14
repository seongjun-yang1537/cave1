using UnityEngine;

namespace Ingame
{
    public class DebugEntitySpawner : MonoBehaviour
    {
        public EntityType entityType;

        public void Spawn()
        {
            EntityController entityController = EntitySystem.SpawnEntity(entityType);

            Transform tr = entityController.transform;
            tr.position = transform.position;

            entityController.SnapToNavMesh();
        }
    }
}