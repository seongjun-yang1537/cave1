using System.Collections.Generic;
using System.Linq;
using Core;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Ingame
{
    [RequireComponent(typeof(EntityServiceLocator))]
    public class EntitySystem : Singleton<EntitySystem>
    {
        private UnityEvent<PlayerController> onSpawnPlayer = new();
        public static UnityEvent<PlayerController> OnSpawnPlayer => Instance.onSpawnPlayer;

        private readonly Dictionary<int, EntityController> entityMapByID = new Dictionary<int, EntityController>();
        private Dictionary<EntityCategory, EntityPool> _pools;

        public IReadOnlyDictionary<EntityCategory, EntityPool> Pools
        {
            get
            {
                if (_pools == null || _pools.Count == 0)
                    InitializePools();
                return _pools;
            }
        }

        public UnityEvent<EntityController> onSpawn = new();

        private void Awake()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            _pools = new();

            RegisterPool<EnvironmentEntityPool>(EntityCategory.Environment);
            RegisterPool<PlayerEntityPool>(EntityCategory.Player);
            RegisterPool<ProjectileEntityPool>(EntityCategory.Projectile);
            RegisterPool<MonsterEntityPool>(EntityCategory.Monster);
            RegisterPool<OreEntityPool>(EntityCategory.Ore);
            RegisterPool<WorldItemEntityPool>(EntityCategory.WorldItem);
        }

        private void RegisterPool<T>(EntityCategory category) where T : EntityPool
        {
            var pool = transform.GetComponentInDirectChildren<T>();
            if (pool == null)
            {
                Debug.LogError($"Missing pool of type {typeof(T)} for category {category}", transform.gameObject);
                return;
            }
            _pools[category] = pool;
        }

        public static ICollection<EntityController> Entities => Instance.entityMapByID.Values;
        public static IEnumerable<PlayerController> Players =>
            Instance.Pools[EntityCategory.Player].transform.Cast<Transform>().Select(t => t.GetComponent<PlayerController>());

        public static void Register(EntityController controller)
        {
            if (controller == null || controller.entityModel == null) return;

            int id = controller.entityModel.entityID;
            if (!Instance.entityMapByID.ContainsKey(id))
            {
                Instance.entityMapByID.Add(id, controller);
            }

            if (controller is PlayerController)
                OnSpawnPlayer.Invoke(controller as PlayerController);
            Instance.onSpawn.Invoke(controller);
        }

        public static void Unregister(EntityController controller)
        {
            if (controller == null || controller.entityModel == null) return;

            int id = controller.entityModel.entityID;
            if (Instance.entityMapByID.ContainsKey(id))
            {
                Instance.entityMapByID.Remove(id);
            }
        }

        public static EntityController SpawnEntity(EntitySpawnContext context)
        {
            if (Instance.Pools.TryGetValue(context.Category, out var pool))
            {
                var controller = pool.Spawn(context);
                return controller;
            }
            Debug.LogWarning($"No pool for category {context.Category}");
            return null;
        }

        public static EntityController SpawnEntity(EntityType entityType)
        {
            var context = new SimpleSpawnContext(entityType.GetCategory(), entityType);
            return SpawnEntity(context);
        }

        public static EntityController FindControllerByID(int id)
        {
            if (Instance?.entityMapByID == null) return null;
            if (id == -1) return null;
            Instance.entityMapByID.TryGetValue(id, out var controller);
            return controller;
        }

        public static EntityController FindController(EntityModel entityModel)
            => FindControllerByID(entityModel.entityID);

        public static void KillAllEntities()
        {
            foreach (var (key, pool) in Instance.Pools)
            {
                if (key == EntityCategory.Player) continue;
                pool.KillAllEntities();
            }

            // TODO
            List<Transform> dropItems =
                Instance.Pools[EntityCategory.WorldItem].transform.Cast<Transform>().ToList();
            foreach (var dropItem in dropItems)
            {
                Despawn(dropItem.gameObject);
            }
        }

        public static void Despawn(GameObject gameObject)
        {
            if (gameObject != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    Object.DestroyImmediate(gameObject);
                else
#endif
                    Object.Destroy(gameObject);
            }
        }
    }
}