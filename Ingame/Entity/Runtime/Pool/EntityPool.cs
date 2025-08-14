using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class EntityPool : MonoBehaviour
    {
        protected IEnumerable<EntityController> Entities
        {
            get => transform.GetComponentsInDirectChildren<EntityController>();
        }

        public virtual EntityController Spawn(EntitySpawnContext context)
        {
            GameObject prefab = EntityDB.LoadPrefab(context.entityType);
            EntityScope prefabScope = prefab.GetComponent<EntityScope>();
            {
                prefabScope.onCreateModel = () => EntityModelFactory.Create(context.entityType);
            }

            GameObject go = Instantiate(prefab);
            context.onAfterCreate?.Invoke(go);

            Transform tr = go.transform;
            {
                tr.SetParent(transform);
                tr.position = context.position;
                tr.rotation = context.rotation;
            }

            EntityController controller = go.GetComponent<EntityController>();
            {
                controller.SnapToNavMesh();
            }
            EntitySystem.Register(controller);

            return controller;
        }

        public virtual void Despawn(EntityController controller)
        {
            if (controller != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    Object.DestroyImmediate(controller.gameObject);
                else
#endif
                    Object.Destroy(controller.gameObject);
            }
        }

        public virtual void KillAllEntities()
        {
            List<EntityController> controllers = Entities.ToList();
            foreach (var controller in controllers)
                Despawn(controller);
        }
    }
}
